using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class TomaInventario
{
    public int IdTomaInventario { get; set; }

    public int IdProducto { get; set; }

    public int IdUsuario { get; set; }

    public int StockAnterior { get; set; }

    public int StockNuevo { get; set; }

    public string DocumentoEncargado { get; set; } = null!;

    public string NumeroDocumentoEncargado { get; set; } = null!;

    public string NombreEncargado { get; set; } = null!;

    public string Razon { get; set; } = null!;

    public DateOnly FechaToma { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
