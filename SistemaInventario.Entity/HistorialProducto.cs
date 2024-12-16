using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class HistorialProducto
{
    public int IdHistorialProducto { get; set; }

    public int IdProducto { get; set; }

    public int IdUsuario { get; set; }

    public string ColumnaModificada { get; set; } = null!;

    public string ValorAnterior { get; set; } = null!;

    public string ValorNuevo { get; set; } = null!;

    public string? Razon { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
