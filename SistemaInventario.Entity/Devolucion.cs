using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class Devolucion
{
    public int IdDevolucion { get; set; }

    public int IdVenta { get; set; }

    public string NumeroVenta { get; set; } = null!;

    public int IdUsuario { get; set; }

    public string Motivo { get; set; } = null!;

    public decimal? MontoDevuelto { get; set; }

    public DateTime Fecha { get; set; }

    public virtual ICollection<DetalleDevolucion> DetalleDevolucion { get; set; } = new List<DetalleDevolucion>();

    public virtual Venta IdVentaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
