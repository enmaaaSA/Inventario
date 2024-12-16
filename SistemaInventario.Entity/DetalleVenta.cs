using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class DetalleVenta
{
    public int IdDetalleVenta { get; set; }

    public int IdVenta { get; set; }

    public int IdProducto { get; set; }

    public string CodigoProducto { get; set; } = null!;

    public string NombreProducto { get; set; } = null!;

    public string MarcaProducto { get; set; } = null!;

    public string CategoriaProducto { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal? PrecioVenta { get; set; }

    public decimal? Total { get; set; }

    public bool Estado { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Venta IdVentaNavigation { get; set; } = null!;
}
