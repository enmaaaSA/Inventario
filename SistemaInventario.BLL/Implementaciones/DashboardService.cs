using SistemaInventario.BLL.Interfaces;
using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace SistemaInventario.BLL.Implementaciones
{
    public class DashboardService : IDashboardService
    {
        private readonly IVentaRepository _repositorioVenta;
        private readonly IGenericRepository<DetalleVenta> _repositorioDetalleVenta;
        private readonly IGenericRepository<Marca> _repositorioMarca;
        private readonly IGenericRepository<Categoria> _repositorioCategoria;
        private readonly IGenericRepository<Producto> _repositorioProducto;
        private DateTime FechaInicio = DateTime.Now;
        public DashboardService(IVentaRepository repositorioVenta,
            IGenericRepository<DetalleVenta> repositorioDetalleVenta,
             IGenericRepository<Categoria> repositorioCategoria,
             IGenericRepository<Marca> repositorioMarca,
              IGenericRepository<Producto> repositorioProducto
            )
        {
            _repositorioVenta = repositorioVenta;
            _repositorioDetalleVenta = repositorioDetalleVenta;
            _repositorioCategoria = repositorioCategoria;
            _repositorioProducto = repositorioProducto;
            FechaInicio = FechaInicio.AddDays(-7);
        }


        public async Task<int> TotalVentasUltimaSemana()
        {
            try
            {
                IQueryable<Venta> query = await _repositorioVenta.Consultar(v => v.Fecha.Value.Date >= FechaInicio.Date);
                int total = query.Count();
                return total;
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> TotalIngresosUltimaSemana()
        {
            try
            {
                IQueryable<Venta> query = await _repositorioVenta.Consultar(v => v.Fecha.Value.Date >= FechaInicio.Date);

                decimal resultado = query
                    .Select(v => v.Total)
                    .Sum(v => v.Value);

                return Convert.ToString(resultado, new CultureInfo("es-PE"));
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<int> TotalProductos()
        {
            try
            {
                IQueryable<Producto> query = await _repositorioProducto.Consultar();
                int total = query.Count();
                return total;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> TotalCategorias()
        {
            try
            {
                IQueryable<Categoria> query = await _repositorioCategoria.Consultar();
                int total = query.Where(b => b.Estado == true).Count();
                return total;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> TotalMarcas()
        {
            try
            {
                IQueryable<Marca> query = await _repositorioMarca.Consultar();
                int total = query.Where(b => b.Estado == true).Count();
                return total;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Dictionary<string, int>> VentasUltimaSemana()
        {
            try
            {

                IQueryable<Venta> query = await _repositorioVenta
                    .Consultar(v => v.Fecha.Value.Date >= FechaInicio.Date);


                Dictionary<string, int> resultado = query
                    .GroupBy(v => v.Fecha.Value.Date).OrderByDescending(g => g.Key)
                    .Select(dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() })
                    .ToDictionary(keySelector: r => r.fecha, elementSelector: r => r.total);

                return resultado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Dictionary<string, int>> ProductosTopUltimaSemana()
        {
            try
            {

                IQueryable<DetalleVenta> query = await _repositorioDetalleVenta.Consultar();


                Dictionary<string, int> resultado = query
                    .Include(v => v.IdVentaNavigation)
                    .Where(dv => dv.IdVentaNavigation.Fecha.Value.Date >= FechaInicio.Date)
                    .GroupBy(dv => dv.NombreProducto).OrderByDescending(g => g.Count())
                    .Select(dv => new { producto = dv.Key, total = dv.Count() }).Take(4)
                    .ToDictionary(keySelector: r => r.producto, elementSelector: r => r.total);
                return resultado;
            }
            catch
            {
                throw;
            }
        }
    }
}
