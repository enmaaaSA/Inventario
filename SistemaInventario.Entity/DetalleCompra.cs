using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class DetalleCompra
{
    public int IdDetalleCompra { get; set; }

    public int IdCompra { get; set; }

    public int IdProducto { get; set; }

    public string CodigoProducto { get; set; } = null!;

    public string NombreProducto { get; set; } = null!;

    public string MarcaProducto { get; set; } = null!;

    public string CategoriaProducto { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal? PrecioCompra { get; set; }

    public decimal? Total { get; set; }

    public virtual Compra IdCompraNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
