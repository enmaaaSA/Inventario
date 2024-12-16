using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Interfaces
{
    public interface IVentaService
    {
        Task<List<Producto>> ObtenerProductos(string busqueda);
        Task<Venta> Registrar(Venta entidad);
        Task<List<Venta>> Historial(string numeroVenta, string fechaInicio, string fechaFin);
        Task<Venta> Detalle(string numeroVenta);
        Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin);
        Task<List<Venta>> ReporteExcelVenta();
        Task<List<DetalleVenta>> ReporteExcel();
        Task<List<Venta>> Historial1(string numeroVenta);
    }
}