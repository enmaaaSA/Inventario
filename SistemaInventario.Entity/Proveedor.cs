using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class Proveedor
{
    public int IdProveedor { get; set; }

    public string TipoDocumento { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Correo { get; set; }

    public string? Direccion { get; set; }

    public bool Estado { get; set; }

    public DateTime Fecha { get; set; }

    public int IdUsuario { get; set; }

    public virtual ICollection<Compra> Compra { get; set; } = new List<Compra>();

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
