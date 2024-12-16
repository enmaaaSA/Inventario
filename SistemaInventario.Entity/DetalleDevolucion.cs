using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class DetalleDevolucion
{
    public int IdDetalleDevolucion { get; set; }

    public int IdDevolucion { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public virtual Devolucion IdDevolucionNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
