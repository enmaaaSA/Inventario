using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class TipoDocumentoCompra
{
    public int IdTipoDocumentoCompra { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdUsuario { get; set; }

    public bool Estado { get; set; }

    public DateTime Fecha { get; set; }

    public virtual ICollection<Compra> Compra { get; set; } = new List<Compra>();

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
