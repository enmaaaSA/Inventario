using Microsoft.EntityFrameworkCore;
using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.DAL.Implementaciones
{
    public class CompraRepository : GenericRepository<Compra>, ICompraRepository
    {
        private readonly DBInventarioContext _context;
        public CompraRepository(DBInventarioContext context) : base(context)
        {
            _context = context;
        }


        public async Task<Compra> Registrar(Compra entidad)
        {
            Compra compraGenerated = new Compra();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (DetalleCompra dc in entidad.DetalleCompra)
                    {
                        Producto productFound = _context.Producto.Where(p => p.IdProducto == dc.IdProducto).First();
                        productFound.Stock = productFound.Stock + dc.Cantidad;
                        _context.Producto.Update(productFound);
                    }
                    await _context.Compra.AddAsync(entidad);
                    await _context.SaveChangesAsync();
                    compraGenerated = entidad;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return compraGenerated;
        }

        public async Task<List<DetalleCompra>> Reporte(DateTime fechaInicio, DateTime fechaFin)
        {
            List<DetalleCompra> listResumen = await _context.DetalleCompra
                .Include(c => c.IdCompraNavigation)
                .ThenInclude(u => u.IdUsuarioNavigation)
                .Include(c => c.IdCompraNavigation)
                .ThenInclude(tdc => tdc.IdTipoDocumentoCompraNavigation)
                .Where(dc => dc.IdCompraNavigation.Fecha.Value.Date >= fechaInicio.Date 
                    && dc.IdCompraNavigation.Fecha.Value.Date <= fechaFin.Date).ToListAsync();
            return listResumen;
        }
    }
}