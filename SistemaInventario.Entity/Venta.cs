using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class Venta
{
    public int IdVenta { get; set; }

    public string NumeroVenta { get; set; } = null!;

    public int IdUsuario { get; set; }

    public string? DocumentoCliente { get; set; }

    public string? NumeroDocumentoCliente { get; set; }

    public string? NombreCliente { get; set; }

    public int IdTipoDocumentoVenta { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal? ImpuestoTotal { get; set; }

    public decimal? Total { get; set; }

    public DateTime? Fecha { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual TipoDocumentoVenta IdTipoDocumentoVentaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Devolucion> Devolucion { get; set; } = new List<Devolucion>();
}
