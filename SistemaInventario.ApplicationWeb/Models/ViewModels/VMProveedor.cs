namespace SistemaInventario.ApplicationWeb.Models.ViewModels
{
    public class VMProveedor
    {
        public int IdProveedor { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public int Estado { get; set; }
    }
}
