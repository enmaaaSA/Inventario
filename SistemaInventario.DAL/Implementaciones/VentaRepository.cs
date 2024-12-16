using Microsoft.EntityFrameworkCore;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.DAL.Implementaciones
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly DBInventarioContext _context;
        private readonly ICorreoRepository _correoRepositorio;
        private static readonly object _lock = new object();
        public VentaRepository(DBInventarioContext context, ICorreoRepository correoRepositorio) : base(context)
        {
            _context = context;
            _correoRepositorio = correoRepositorio;
        }


        public async Task<Venta> Registrar(Venta entidad)
        {
            Venta ventaGenerated = new Venta();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (DetalleVenta dv in entidad.DetalleVenta)
                    {
                        Producto productFound = _context.Producto.Where(p => p.IdProducto == dv.IdProducto).First();
                        productFound.Stock = productFound.Stock - dv.Cantidad;
                        _context.Producto.Update(productFound);
                        if (productFound.Stock <= 10)
                        {
                            await _correoRepositorio.EnviarCorreo("one.12.tlv@gmail.com", $"Alerta: Stock bajo para {productFound.Nombre}",
                            $"El producto {productFound.Nombre} tiene un stock actual de {productFound.Stock}.");
                        }
                    }
                    await _context.Venta.AddAsync(entidad);
                    await _context.SaveChangesAsync();
                    ventaGenerated = entidad;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return ventaGenerated;
        }

        public async Task<List<DetalleVenta>> Reporte(DateTime fechaInicio, DateTime fechaFin)
        {
            List<DetalleVenta> listResumen = await _context.DetalleVenta
                .Include(v => v.IdVentaNavigation)
                .ThenInclude(u => u.IdUsuarioNavigation)
                .Include(v => v.IdVentaNavigation)
                .ThenInclude(tdv => tdv.IdTipoDocumentoVentaNavigation)
                .Where(dv => dv.IdVentaNavigation.Fecha.Value.Date >= fechaInicio.Date
                    && dv.IdVentaNavigation.Fecha.Value.Date <= fechaFin.Date).ToListAsync();
            return listResumen;
        }

        public string GenerarNumeroVenta()
        {
            string fecha = DateTime.Now.ToString("yyyyMMdd");
            int contador;

            lock (_lock)
            {
                // Consultar el último contador para la fecha actual
                var ultimoContador = _context.Contador
                    .Where(c => c.Fecha == fecha)
                    .OrderByDescending(c => c.Contador1)
                    .FirstOrDefault();

                if (ultimoContador == null)
                    // Si no existe un registro para la fecha, inicializar el contador en 1
                    contador = 1;
                else
                    // Incrementar el contador del último registro
                    contador = ultimoContador.Contador1 + 1;

                // Crear un nuevo registro en la tabla Contador
                var nuevoContador = new Contador
                {
                    Fecha = fecha,
                    Contador1 = contador
                };

                _context.Contador.Add(nuevoContador);
                _context.SaveChanges();
            }

            // Retornar el número de venta en el formato deseado
            return $"{fecha}-{contador:D4}"; // Ejemplo: 20241203-0001
        }

        public async Task<Venta> ObtenerVentaPorId(int idVenta)
        {
            return await _context.Venta
                .Include(v => v.DetalleVenta)
                .FirstOrDefaultAsync(v => v.IdVenta == idVenta);
        }

        public async Task<List<DetalleVenta>> ObtenerDetallesVenta(int idVenta)
        {
            return await _context.DetalleVenta
                .Where(dv => dv.IdVenta == idVenta)
                .ToListAsync();
        }

        public async Task ActualizarEstadoVenta(int idVenta, bool nuevoEstado)
        {
            var venta = await _context.Venta.FindAsync(idVenta);
            if (venta != null)
            {
                venta.Estado = nuevoEstado;
                _context.Venta.Update(venta);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ActualizarStockPorDevolucion(List<DetalleVenta> detalleVenta)
        {
            foreach (var detalle in detalleVenta)
            {
                var producto = await _context.Producto.FindAsync(detalle.IdProducto);
                if (producto != null)
                {
                    producto.Stock += detalle.Cantidad;
                    _context.Producto.Update(producto);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}