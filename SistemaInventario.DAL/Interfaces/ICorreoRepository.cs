using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Interfaces
{
    public interface ICorreoRepository
    {
        Task<bool> EnviarCorreo(string correoDestino, string asunto, string mensaje);
    }
}
