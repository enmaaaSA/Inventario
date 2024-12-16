using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.DAL.Interfaces
{
    public interface ICompraRepository : IGenericRepository<Compra>
    {
        Task<Compra> Registrar(Compra entidad);
        Task<List<DetalleCompra>> Reporte(DateTime fechaInicio, DateTime fechaFin);
    }
}
