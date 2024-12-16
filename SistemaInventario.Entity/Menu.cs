using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class Menu
{
    public int IdMenu { get; set; }

    public string? Nombre { get; set; }

    public string? Icono { get; set; }

    public string? Controlador { get; set; }

    public string? PagAccion { get; set; }

    public int? IdMenuPadre { get; set; }

    public bool Estado { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Menu IdMenuPadreNavigation { get; set; } = null!;

    public virtual ICollection<Menu> InverseIdMenuPadreNavigation { get; set; } = new List<Menu>();

    public virtual ICollection<RolMenu> RolMenu { get; set; } = new List<RolMenu>();
}
