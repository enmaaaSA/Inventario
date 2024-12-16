using Microsoft.EntityFrameworkCore;
using SistemaInventario.Entity;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Implementaciones
{
    public class TomaInventarioService : ITomaInventarioService
    {
        private readonly IGenericRepository<TomaInventario> _repositorio;
        private readonly IGenericRepository<Producto> _repositorioProducto;
        private readonly IProductoService _productoServicio;
        public TomaInventarioService(IGenericRepository<TomaInventario> repositorio,
                                     IGenericRepository<Producto> repositorioProducto,
                                     IProductoService productoServicio)
        {
            _repositorio = repositorio;
            _productoServicio = productoServicio;
            _repositorioProducto = repositorioProducto;
        }


        public async Task<TomaInventario> Crear(TomaInventario entidad)
        {
            if (entidad.IdProducto == null)
                throw new TaskCanceledException("No existe producto");
            try
            {
                TomaInventario tomaInventarioCreated = await _repositorio.Crear(entidad);
                if (tomaInventarioCreated.IdTomaInventario == null)
                    throw new TaskCanceledException("No se pudo crear la toma de inventario");
                IQueryable<Producto> query = await _repositorioProducto.Consultar(p => p.IdProducto == entidad.IdProducto);
                Producto product = query.First();
                product.Stock = entidad.StockNuevo;
                await _productoServicio.TomaInventario(product);
                return tomaInventarioCreated;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
