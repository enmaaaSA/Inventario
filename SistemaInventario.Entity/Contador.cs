using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class Contador
{
    public int IdContador { get; set; }

    public string Fecha { get; set; } = null!;

    public int Contador1 { get; set; }
}
