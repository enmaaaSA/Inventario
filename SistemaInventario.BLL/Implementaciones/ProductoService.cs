using Microsoft.EntityFrameworkCore;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemaInventario.BLL.Implementaciones
{
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> _repositorio;
        private readonly IGenericRepository<HistorialProducto> _repositorioHistorial;
        private readonly ICorreoService _correoServicio;
        public ProductoService(IGenericRepository<Producto> repositorio,
            IGenericRepository<HistorialProducto> repositorioHistorial,
            ICorreoService correoServicio)
        {
            _repositorio = repositorio;
            _repositorioHistorial = repositorioHistorial;
            _correoServicio = correoServicio;
        }


        public async Task<List<Producto>> Lista()
        {
            IQueryable<Producto> query = await _repositorio.Consultar();
            return query.Include(c => c.IdCategoriaNavigation)
                .Include(m => m.IdMarcaNavigation)
                .Include(u => u.IdUsuarioNavigation)
                .ToList();
        }

        public async Task<Producto> Crear(Producto entidad)
        {
            Producto productExists = await _repositorio.Obtener(p => p.CodigoBarra == entidad.CodigoBarra && p.Estado == true);
            if (productExists != null)
                throw new TaskCanceledException("El codigo de barras ya existe. CREAR");

            try
            {
                if (entidad.PrecioCompra >= entidad.PrecioVenta)
                    throw new TaskCanceledException("El Precio de Compra no puede ser mayor al Precio de Venta. CREAR");

                Producto productCreated = await _repositorio.Crear(entidad);

                if (productCreated.IdProducto == 0)
                    throw new TaskCanceledException("No se pudo crear el producto. CREAR");

                IQueryable<Producto> query = await _repositorio.Consultar(p => p.IdProducto == productCreated.IdProducto);
                productCreated = query.Include(c => c.IdCategoriaNavigation)
                    .Include(m => m.IdMarcaNavigation)
                    .Include(u => u.IdUsuarioNavigation)
                    .First();
                #region A
                IQueryable<Producto> queryProduct = await _repositorio.Consultar(p => p.IdProducto == productCreated.IdProducto);
                Producto productRazon = queryProduct.First();
                productRazon.Razon = $"Producto creado por {productCreated.IdUsuarioNavigation.Nombre} {productCreated.IdUsuarioNavigation.Apellido}.";
                bool test = await _repositorio.Editar(productRazon);
                #endregion
                return productCreated;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Producto> Editar(Producto entidad)
        {
            Producto productExists = await _repositorio.Obtener(p => p.CodigoBarra == entidad.CodigoBarra && 
                                                                     p.IdProducto != entidad.IdProducto && 
                                                                     p.Estado == true);

            if (productExists != null)
                throw new InvalidOperationException("El codigo de barra no existe. EDICION");

            try
            {
                IQueryable<Producto> queryProduct = await _repositorio.Consultar(p => p.IdProducto == entidad.IdProducto);
                Producto productOriginal = queryProduct.First();

                if (entidad.PrecioCompra >= entidad.PrecioVenta)
                    throw new TaskCanceledException("El precio de compra no puede ser mayor que el precio de venta. EDICION");

                Producto productNew = new Producto
                {
                    IdUsuario = entidad.IdUsuario,
                    CodigoBarra = entidad.CodigoBarra,
                    Nombre = entidad.Nombre,
                    Descripcion = entidad.Descripcion,
                    IdCategoria = entidad.IdCategoria,
                    IdMarca = entidad.IdMarca,
                    Stock = entidad.Stock,
                    PrecioCompra = entidad.PrecioCompra,
                    PrecioVenta = entidad.PrecioVenta,
                    Estado = entidad.Estado,
                };

                Producto productCreate = await _repositorio.Crear(productNew);
                if (productCreate == null)
                    throw new TaskCanceledException("No se pudo actualizar el producto. EDICION");

                productOriginal.Estado = false;
                bool statusFalse = await _repositorio.Editar(productOriginal);

                Producto productCreated = queryProduct.Include(c => c.IdCategoriaNavigation)
                    .Include(m => m.IdMarcaNavigation)
                    .Include(u => u.IdUsuarioNavigation)
                    .First();

                productOriginal.Razon = $"{productOriginal.Razon} Se desactivo por el usuario {productCreated.IdUsuarioNavigation.Nombre} {productCreated.IdUsuarioNavigation.Apellido}.";
                bool statusFalse1 = await _repositorio.Editar(productOriginal);

                IQueryable<Producto> query = await _repositorio.Consultar(p => p.IdProducto == productCreate.IdProducto);
                Producto productRazon = query.First();
                productRazon.Razon = $"Producto creado por {productCreate.IdUsuarioNavigation.Nombre}{productCreate.IdUsuarioNavigation.Apellido}.";
                bool test = await _repositorio.Editar(productRazon);

                #region HistorialCompra
                if (productNew.Nombre != productOriginal.Nombre)
                {
                    HistorialProducto historial = new HistorialProducto
                    {
                        IdProducto = productOriginal.IdProducto,
                        ColumnaModificada = "Nombre",
                        ValorAnterior = productOriginal.Nombre,
                        ValorNuevo = productNew.Nombre,
                        Razon = $"Producto editado por cambio de Nombre. Realizado por {productCreated.IdUsuarioNavigation.Nombre} {productCreated.IdUsuarioNavigation.Apellido}.",
                        IdUsuario = productNew.IdUsuario,
                    };
                    HistorialProducto historialCreated = await _repositorioHistorial.Crear(historial);
                }

                if (productNew.Descripcion != productOriginal.Descripcion)
                {
                    HistorialProducto historial = new HistorialProducto
                    {
                        IdProducto = productOriginal.IdProducto,
                        ColumnaModificada = "Descripción",
                        ValorAnterior = productOriginal.Descripcion,
                        ValorNuevo = productNew.Descripcion,
                        Razon = $"Producto editado por cambio de Descripcion. Realizado por {productCreated.IdUsuarioNavigation.Nombre} {productCreated.IdUsuarioNavigation.Apellido}.",
                        IdUsuario = productNew.IdUsuario,
                    };
                    HistorialProducto historialCreated = await _repositorioHistorial.Crear(historial);
                }

                if (productNew.IdCategoria != productOriginal.IdCategoria)
                {
                    HistorialProducto historial = new HistorialProducto
                    {
                        IdProducto = productOriginal.IdProducto,
                        ColumnaModificada = "Categoria",
                        ValorAnterior = productOriginal.IdCategoria.ToString(),
                        ValorNuevo = productNew.IdCategoria.ToString(),
                        Razon = $"Producto editado por cambio de Categoria. Realizado por {productCreated.IdUsuarioNavigation.Nombre} {productCreated.IdUsuarioNavigation.Apellido}.",
                        IdUsuario = productNew.IdUsuario,
                    };
                    HistorialProducto historialCreated = await _repositorioHistorial.Crear(historial);
                }

                if (productNew.IdMarca != productOriginal.IdMarca)
                {
                    HistorialProducto historial = new HistorialProducto
                    {
                        IdProducto = productOriginal.IdProducto,
                        ColumnaModificada = "Marca",
                        ValorAnterior = productOriginal.IdMarca.ToString(),
                        ValorNuevo = productNew.IdMarca.ToString(),
                        Razon = $"Producto editado por cambio de Marca. Realizado por {productCreated.IdUsuarioNavigation.Nombre} {productCreated.IdUsuarioNavigation.Apellido}.",
                        IdUsuario = productNew.IdUsuario,
                    };
                    HistorialProducto historialCreated = await _repositorioHistorial.Crear(historial);
                }

                if (productNew.PrecioCompra != productOriginal.PrecioCompra)
                {
                    HistorialProducto historial = new HistorialProducto
                    {
                        IdProducto = productOriginal.IdProducto,
                        ColumnaModificada = "Precio de Compra",
                        ValorAnterior = productOriginal.PrecioCompra.ToString(),
                        ValorNuevo = productNew.PrecioCompra.ToString(),
                        Razon = $"Producto editado por cambio de Precio de Compra. Realizado por {productCreated.IdUsuarioNavigation.Nombre} {productCreated.IdUsuarioNavigation.Apellido}.",
                        IdUsuario = productNew.IdUsuario,
                    };
                    HistorialProducto historialCreated = await _repositorioHistorial.Crear(historial);
                }

                if (productNew.PrecioVenta != productOriginal.PrecioVenta)
                {
                    HistorialProducto historial = new HistorialProducto
                    {
                        IdProducto = productOriginal.IdProducto,
                        ColumnaModificada = "Precio de Venta",
                        ValorAnterior = productOriginal.PrecioVenta.ToString(),
                        ValorNuevo = productNew.PrecioVenta.ToString(),
                        Razon = $"Producto editado por cambio de Precio de Venta. Realizado por {productCreated.IdUsuarioNavigation.Nombre} {productCreated.IdUsuarioNavigation.Apellido}.",
                        IdUsuario = productNew.IdUsuario,
                    };
                    HistorialProducto historialCreated = await _repositorioHistorial.Crear(historial);
                }

                if (productNew.Estado != productOriginal.Estado)
                {
                    HistorialProducto historial = new HistorialProducto
                    {
                        IdProducto = productOriginal.IdProducto,
                        ColumnaModificada = "Estado",
                        ValorAnterior = productOriginal.Estado.ToString(),
                        ValorNuevo = productNew.Estado.ToString(),
                        Razon = $"Producto editado por cambio de Estado. Realizado por {productCreated.IdUsuarioNavigation.Nombre} {productCreated.IdUsuarioNavigation.Apellido}.",
                        IdUsuario = productNew.IdUsuario,
                    };
                    HistorialProducto historialCreated = await _repositorioHistorial.Crear(historial);
                }
                #endregion

                return productCreated;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idProducto)
        {
            Producto product = await _repositorio.Obtener(p => p.IdProducto == idProducto);

            if (product == null)
                throw new TaskCanceledException("No existe");
            if (product.Stock != 0 && product.Estado == true)
                throw new TaskCanceledException("No se puede eliminar, unidades restantes. Producto Activo");

            try
            {
                Producto productFound = await _repositorio.Obtener(p => p.IdProducto == idProducto);

                if (productFound == null)
                    throw new TaskCanceledException("El producto no existe");

                bool respuesta = await _repositorio.Eliminar(productFound);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Producto> TomaInventario(Producto entidad)
        {
            try
            {
                IQueryable<Producto> query = await _repositorio.Consultar(p => p.IdProducto == entidad.IdProducto);
                Producto productOriginal = await query.AsNoTracking().FirstAsync();
                if (productOriginal == null)
                    throw new Exception("El producto no existe");
                Producto productEdite = await query.FirstAsync();


                productEdite.Stock = entidad.Stock;
                productEdite.Razon = $"{productEdite.Razon} Producto editado por Toma de Inventario.";

                HistorialProducto historial = new HistorialProducto
                {
                    IdProducto = productEdite.IdProducto,
                    ColumnaModificada = "Stock",
                    ValorAnterior = productOriginal.Stock.ToString(),
                    ValorNuevo = entidad.Stock.ToString(),
                    Razon = "Producto editado por Toma de Inventario",
                    IdUsuario = entidad.IdUsuario,
                };
                HistorialProducto historialCreated = await _repositorioHistorial.Crear(historial);
                if (historialCreated.IdHistorialProducto == 0)
                    throw new TaskCanceledException("No se puedo realizar el cambio de Stock");
                bool respuesta = await _repositorio.Editar(productEdite);
                Producto productEdited = query.Include(c => c.IdCategoriaNavigation)
                                              .Include(m => m.IdMarcaNavigation)
                                              .Include(u => u.IdUsuarioNavigation)
                                              .First();
                return productEdited;
            }
            catch
            {
                throw;
            }
        }
    }
}
