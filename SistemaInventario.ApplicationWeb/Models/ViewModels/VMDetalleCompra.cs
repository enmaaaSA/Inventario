namespace SistemaInventario.ApplicationWeb.Models.ViewModels
{
    public class VMDetalleCompra
    {
        public int IdProducto { get; set; }
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string MarcaProducto { get; set; }
        public string CategoriaProducto { get; set; }
        public int Cantidad { get; set; }
        public string PrecioCompra { get; set; }
        public string Total { get; set; }
    }
}
