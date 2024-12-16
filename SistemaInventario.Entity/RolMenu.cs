using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class RolMenu
{
    public int IdRolMenu { get; set; }

    public int IdRol { get; set; }

    public int IdMenu { get; set; }

    public bool Estado { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Menu IdMenuNavigation { get; set; } = null!;

    public virtual Rol IdRolNavigation { get; set; } = null!;
}
