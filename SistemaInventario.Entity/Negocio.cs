using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class Negocio
{
    public int IdNegocio { get; set; }

    public string Documento { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string? Direccion { get; set; }

    public decimal? Impuesto { get; set; }

    public string SimboloMoneda { get; set; } = null!;

    public int IdUsuario { get; set; }
    
    public bool Estado { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
