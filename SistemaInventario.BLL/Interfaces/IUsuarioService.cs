using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> Lista();
        Task<Usuario> Crear(Usuario entidad, string urlPlantillaCorreo);
        Task<Usuario> Editar(Usuario entidad);
        Task<bool> Eliminar(int idUsuario);
        Task<Usuario> ObtenerCredenciales(string correo, string clave);
        Task<Usuario> ObtenerId(int idUsuario);
        Task<bool> GuardarPefil(Usuario entidad);
        Task<bool> CambiarClave(int idUsuario, string claveActual, string claveNueva);
        Task<bool> RestablecerClave(string correo, string urlPlantillaCorreo);
        Task<Usuario> ObtenerUsuarioActivoPorId(int idUsuario);
    }
}
