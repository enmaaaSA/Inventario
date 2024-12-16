namespace SistemaInventario.ApplicationWeb.Models.ViewModels
{
    public class VMProducto
    {
        public int IdProducto { get; set; }
        public int IdUsuario { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioApellido { get; set; }
        public string CodigoBarra { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
        public int IdMarca { get; set; }
        public string Marca { get; set; }
        public int Stock { get; set; }
        public string PrecioCompra { get; set; }
        public string PrecioVenta { get; set; }
        public int Estado { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Razon { get; set; }
    }
}
