namespace SistemaInventario.ApplicationWeb.Models.ViewModels
{
    public class VMNegocio
    {
        public int IdNegocio { get; set; }
        public string Documento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public string Impuesto { get; set; }
        public string SimboloMoneda { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public int Estado { get; set; }
    }
}
