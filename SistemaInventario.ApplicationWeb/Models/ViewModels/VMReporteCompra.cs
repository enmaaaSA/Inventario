namespace SistemaInventario.ApplicationWeb.Models.ViewModels
{
    public class VMReporteCompra
    {
        public string Fecha { get; set; }
        public string NumeroCompra { get; set; }
        public string TipoDocumento { get; set; }
        public string DocumentoProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string SubTotalCompra { get; set; }
        public string ImpuestoTotalCompra { get; set; }
        public string TotalCompra { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public string PrecioCompra { get; set; }
        public string Total { get; set; }
    }
}
