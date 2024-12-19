using Microsoft.EntityFrameworkCore;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Implementaciones
{
    public class CompraService : ICompraService
    {
        private readonly IGenericRepository<Producto> _repositorioProducto;
        private readonly ICompraRepository _repositorioCompra;
        public CompraService(IGenericRepository<Producto> repositorioProducto,
            ICompraRepository repositorioCompra)
        {
            _repositorioProducto = repositorioProducto;
            _repositorioCompra = repositorioCompra;
        }


        public async Task<List<Producto>> ObtenerProductos(string busqueda)
        {
            IQueryable<Producto> query = await _repositorioProducto.Consultar(p => p.Estado == true 
                && string.Concat(p.CodigoBarra, p.Nombre).Contains(busqueda));
            return query.Include(c => c.IdCategoriaNavigation)
                .Include(m => m.IdMarcaNavigation)
                .ToList();
        }

        public async Task<Compra> Registrar(Compra entidad)
        {
            try
            {
                if (entidad.NumeroCompra == null || entidad.NumeroCompra == "")
                    throw new TaskCanceledException("Debe colocar numbero de Compra");
                return await _repositorioCompra.Registrar(entidad);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Compra>> Historial(string numeroCompra, string fechaInicio, string fechaFin)
        {
            IQueryable<Compra> query = await _repositorioCompra.Consultar();
            fechaInicio = fechaInicio is null ? "" : fechaInicio;
            fechaFin = fechaFin is null ? "" : fechaFin;
            if (fechaInicio != "" && fechaFin != "")
            {
                DateTime dateStart = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                DateTime dateEnd = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));
                return query.Where(v => v.Fecha.Value.Date >= dateStart.Date && v.Fecha.Value.Date <= dateEnd.Date)
                    .Include(tdc => tdc.IdTipoDocumentoCompraNavigation)
                    .Include(u => u.IdUsuarioNavigation)
                    .Include(dc => dc.DetalleCompra)
                    .Include(p => p.IdProveedorNavigation)
                    .ToList();
            }
            else
            {
                return query.Where(c => c.NumeroCompra == numeroCompra)
                    .Include(tdc => tdc.IdTipoDocumentoCompraNavigation)
                    .Include(u => u.IdUsuarioNavigation)
                    .Include(dc => dc.DetalleCompra)
                    .Include(p => p.IdProveedorNavigation)
                    .ToList();
            }
        }

        public async Task<Compra> Detalle(string numeroCompra)
        {
            IQueryable<Compra> query = await _repositorioCompra.Consultar(c => c.NumeroCompra == numeroCompra);
            return query.Include(tdc => tdc.IdTipoDocumentoCompraNavigation)
                .Include(u => u.IdUsuarioNavigation)
                .Include(dc => dc.DetalleCompra)
                .Include(p => p.IdProveedorNavigation)
                .First();
        }

        public async Task<List<DetalleCompra>> Reporte(string fechaInicio, string fechaFin)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
            DateTime dateEnd = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));
            List<DetalleCompra> list = await _repositorioCompra.Reporte(dateStart, dateEnd);
            return list;
        }
    }
}
