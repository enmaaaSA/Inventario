using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Interfaces
{
    public interface ICompraService
    {
        Task<List<Producto>> ObtenerProductos(string busqueda);
        Task<Compra> Registrar(Compra entidad);
        Task<List<Compra>> Historial(string numeroCompra, string fechaInicio, string fechaFin);
        Task<Compra> Detalle(string numeroCompra);
        Task<List<DetalleCompra>> Reporte(string fechaInicio, string fechaFin);
    }
}
