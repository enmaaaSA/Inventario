using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class Producto
{
    public int IdProducto { get; set; }

    public int IdUsuario { get; set; }

    public string CodigoBarra { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int IdCategoria { get; set; }

    public int IdMarca { get; set; }

    public int Stock { get; set; }

    public decimal? PrecioCompra { get; set; }

    public decimal? PrecioVenta { get; set; }

    public bool Estado { get; set; }

    public DateTime Fecha { get; set; }

    public string? Razon { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompra { get; set; } = new List<DetalleCompra>();

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<HistorialProducto> HistorialProducto { get; set; } = new List<HistorialProducto>();

    public virtual ICollection<TomaInventario> TomaInventario { get; set; } = new List<TomaInventario>();

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual Marca IdMarcaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<DetalleDevolucion> DetalleDevolucion { get; set; } = new List<DetalleDevolucion>();
}
