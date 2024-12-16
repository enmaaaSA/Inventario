using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Interfaces
{
    public interface IUtilidadesService
    {
        string GenerarClave();
        string ConvertirSha256(string texto);
        string RamdomString(int length);
        bool CorreoValido(string correo);
        string GenerarNumeroVenta();
    }
}
