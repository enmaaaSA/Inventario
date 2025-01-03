﻿namespace SistemaInventario.ApplicationWeb.Models.ViewModels
{
    public class VMCompra
    {
        public int IdCompra { get; set; }
        public string NumeroCompra { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public int IdProveedor { get; set; }
        public string Proveedor { get; set; }
        public string DocumentoProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public int IdTipoDocumentoCompra { get; set; }
        public string TipoDocumentoCompra { get; set; }
        public string SubTotal { get; set; }
        public string ImpuestoTotal { get; set; }
        public string Total { get; set; }
        public string Fecha { get; set; }
        public virtual ICollection<VMDetalleCompra> DetalleCompra { get; set; }
    }
}
