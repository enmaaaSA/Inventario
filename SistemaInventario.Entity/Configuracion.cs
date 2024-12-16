using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class Configuracion
{
    public string Recurso { get; set; } = null!;

    public string Propiedad { get; set; } = null!;

    public string Valor { get; set; } = null!;
}
