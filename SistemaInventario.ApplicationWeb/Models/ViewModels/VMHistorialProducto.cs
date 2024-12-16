namespace SistemaInventario.ApplicationWeb.Models.ViewModels
{
    public class VMHistorialProducto
    {
        public int IdProducto { get; set; }
        public string Columna { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
        public string Razon { get; set; }
        public int IdUsuario { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
