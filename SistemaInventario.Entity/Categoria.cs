using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class Categoria
{
    public int IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdUsuario { get; set; }

    public bool Estado { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Producto> Producto { get; set; } = new List<Producto>();
}
