using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class Compra
{
    public int IdCompra { get; set; }

    public string NumeroCompra { get; set; } = null!;

    public int IdUsuario { get; set; }

    public int IdProveedor { get; set; }

    public int IdTipoDocumentoCompra { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal? ImpuestoTotal { get; set; }

    public decimal? Total { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompra { get; set; } = new List<DetalleCompra>();

    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;

    public virtual TipoDocumentoCompra IdTipoDocumentoCompraNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
