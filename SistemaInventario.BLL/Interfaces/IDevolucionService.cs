using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Interfaces
{
    public interface IDevolucionService
    {
        Task<Venta> ObtenerVentaPorNumero(string numeroVenta);
        Task<bool> RegistrarDevolucion(string numeroVenta, string motivo, int idUsuario);
    }
}
