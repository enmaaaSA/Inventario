using System.ComponentModel.DataAnnotations;

namespace SistemaInventario.ApplicationWeb.Models.ViewModels
{
    public class VMUsuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Documento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Correo { get; set; }
        public int IdRol { get; set; }
        public string Rol { get; set; }
        public int Estado { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
