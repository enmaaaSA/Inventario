using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class Rol
{
    public int IdRol { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Estado { get; set; }

    public DateTime Fecha { get; set; }

    public virtual ICollection<RolMenu> RolMenu { get; set; } = new List<RolMenu>();

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
