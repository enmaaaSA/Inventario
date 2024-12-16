using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.DAL.Interfaces
{
    public interface IDevolucionRepository : IGenericRepository<Devolucion>
    {
        Task<bool> Registrar(Devolucion entidad, List<DetalleDevolucion> detalles);
        Task<List<DetalleDevolucion>> Reporte(DateTime fechaInicio, DateTime fechaFin);
        Task<Venta> ObtenerVentaPorNumero(string numeroVenta);
    }
}
