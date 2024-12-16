using Microsoft.EntityFrameworkCore;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.DAL.Implementaciones
{
    public class DevolucionRepository : GenericRepository<Devolucion>, IDevolucionRepository
    {
        private readonly DBInventarioContext _context;
        public DevolucionRepository(DBInventarioContext context) : base(context)
        {
            _context = context;
        }


        // Registrar Devolución
        public async Task<bool> Registrar(Devolucion devolucion, List<DetalleDevolucion> detalles)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Cambiar estado de la venta
                    var venta = await _context.Venta.FindAsync(devolucion.IdVenta);
                    if (venta == null) throw new Exception("Venta no encontrada.");
                    venta.Estado = false;
                    _context.Venta.Update(venta);

                    // Registrar la devolución
                    await _context.Devolucion.AddAsync(devolucion);
                    await _context.SaveChangesAsync();

                    // Registrar los detalles de la devolución
                    detalles.ForEach(detalle => detalle.IdDevolucion = devolucion.IdDevolucion);
                    await _context.DetalleDevolucion.AddRangeAsync(detalles);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        // Obtener Venta por Número
        public async Task<Venta> ObtenerVentaPorNumero(string numeroVenta)
        {
            return await _context.Venta
                .Include(v => v.DetalleVenta)
                .ThenInclude(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(v => v.NumeroVenta == numeroVenta && v.Estado == true);
        }

        public Task<List<DetalleDevolucion>> Reporte(DateTime fechaInicio, DateTime fechaFin)
        {
            throw new NotImplementedException();
        }
    }
}