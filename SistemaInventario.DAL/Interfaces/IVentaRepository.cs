using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.DAL.Interfaces
{
    public interface IVentaRepository : IGenericRepository<Venta>
    {
        Task<Venta> Registrar(Venta entidad);
        Task<List<DetalleVenta>> Reporte(DateTime fechaInicio, DateTime fechaFin);
        string GenerarNumeroVenta();
        Task<Venta> ObtenerVentaPorId(int idVenta);
        Task<List<DetalleVenta>> ObtenerDetallesVenta(int idVenta);
        Task ActualizarEstadoVenta(int idVenta, bool nuevoEstado);
        Task ActualizarStockPorDevolucion(List<DetalleVenta> detalleVenta);
    }
}
