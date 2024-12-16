using System;
using System.Collections.Generic;

namespace SistemaInventario.Entity;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Documento { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public int IdRol { get; set; }

    public string Clave { get; set; } = null!;

    public bool Estado { get; set; }

    public DateTime Fecha { get; set; }

    public virtual ICollection<Categoria> Categoria { get; set; } = new List<Categoria>();

    public virtual ICollection<Compra> Compra { get; set; } = new List<Compra>();

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Marca> Marca { get; set; } = new List<Marca>();

    public virtual ICollection<Negocio> Negocio { get; set; } = new List<Negocio>();

    public virtual ICollection<Producto> Producto { get; set; } = new List<Producto>();

    public virtual ICollection<Proveedor> Proveedor { get; set; } = new List<Proveedor>();

    public virtual ICollection<TipoDocumentoCompra> TipoDocumentoCompra { get; set; } = new List<TipoDocumentoCompra>();

    public virtual ICollection<TipoDocumentoVenta> TipoDocumentoVenta { get; set; } = new List<TipoDocumentoVenta>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();

    public virtual ICollection<HistorialProducto> HistorialProducto { get; set; } = new List<HistorialProducto>();

    public virtual ICollection<TomaInventario> TomaInventario { get; set; } = new List<TomaInventario>();

    public virtual ICollection<Devolucion> Devolucion { get; set; } = new List<Devolucion>();
}
