using Microsoft.EntityFrameworkCore;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.DAL.Implementaciones;
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
    public class VentaService : IVentaService
    {
        private readonly IGenericRepository<Producto> _repositorioProducto;
        private readonly IGenericRepository<DetalleVenta> _repositorioDetalleVenta;
        private readonly IVentaRepository _repositorioVenta;
        private readonly IUtilidadesService _utilidadesServicio;
        public VentaService(IGenericRepository<Producto> repositorioProducto,
            IGenericRepository<DetalleVenta> repositorioDetalleVenta,
            IVentaRepository repositorioVenta,
            IUtilidadesService utilidadesServicio)
        {
            _repositorioDetalleVenta = repositorioDetalleVenta;
            _repositorioProducto = repositorioProducto;
            _repositorioVenta = repositorioVenta;
            _utilidadesServicio = utilidadesServicio;
        }


        public async Task<List<Producto>> ObtenerProductos(string busqueda)
        {
            IQueryable<Producto> query = await _repositorioProducto.Consultar(p => p.Estado == true && p.Stock > 0
                && string.Concat(p.CodigoBarra, p.Nombre).Contains(busqueda));
            return query.Include(c => c.IdCategoriaNavigation)
                .Include(m => m.IdMarcaNavigation)
                .ToList();
        }

        public async Task<Venta> Registrar(Venta entidad)
        {
            try
            {
                entidad.NumeroVenta = _repositorioVenta.GenerarNumeroVenta();
                entidad.Estado = true;
                return await _repositorioVenta.Registrar(entidad);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Venta>> Historial(string numeroVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Venta> query = await _repositorioVenta.Consultar();
            fechaInicio = fechaInicio is null ? "" : fechaInicio;
            fechaFin = fechaFin is null ? "" : fechaFin;
            if (fechaInicio != "" && fechaFin != "")
            {
                DateTime dateStart = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                DateTime dateEnd = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));
                return query.Where(v => v.Fecha.Value.Date >= dateStart.Date && v.Fecha.Value.Date <= dateEnd.Date)
                    .Include(tdv => tdv.IdTipoDocumentoVentaNavigation)
                    .Include(u => u.IdUsuarioNavigation)
                    .Include(dv => dv.DetalleVenta)
                    .ToList();
            }
            else
            {
                return query.Where(c => c.NumeroVenta == numeroVenta)
                    .Include(tdc => tdc.IdTipoDocumentoVentaNavigation)
                    .Include(u => u.IdUsuarioNavigation)
                    .Include(dc => dc.DetalleVenta)
                    .ToList();
            }
        }

        public async Task<Venta> Detalle(string numeroVenta)
        {
            IQueryable<Venta> query = await _repositorioVenta.Consultar(c => c.NumeroVenta == numeroVenta);
            return query.Include(tdv => tdv.IdTipoDocumentoVentaNavigation)
                .Include(u => u.IdUsuarioNavigation)
                .Include(dv => dv.DetalleVenta)
                .First();
        }

        public async Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin)
        {
            DateTime dateStart = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
            DateTime dateEnd = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));
            List<DetalleVenta> list = await _repositorioVenta.Reporte(dateStart, dateEnd);
            return list;
        }

        public async Task<List<Venta>> ReporteExcelVenta()
        {
            IQueryable<Venta> query = await _repositorioVenta.Consultar();
            return query.Include(tdc => tdc.IdTipoDocumentoVentaNavigation)
                        .Include(u => u.IdUsuarioNavigation)
                        .ToList();
        }

        public async Task<List<DetalleVenta>> ReporteExcel()
        {
            IQueryable<DetalleVenta> query = await _repositorioDetalleVenta.Consultar();
            return query.Include(tdc => tdc.IdVentaNavigation)
                        .Include(u => u.IdProductoNavigation)
                        .ToList();
        }

        public async Task<List<Venta>> Historial1(string numeroVenta)
        {
            IQueryable<Venta> query = await _repositorioVenta.Consultar();
            return query.Where(c => c.NumeroVenta == numeroVenta)
                    .Include(tdc => tdc.IdTipoDocumentoVentaNavigation)
                    .Include(u => u.IdUsuarioNavigation)
                    .Include(dc => dc.DetalleVenta)
                    .ToList();
        }
    }
}
