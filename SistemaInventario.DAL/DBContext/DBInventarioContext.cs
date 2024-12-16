using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SistemaInventario.Entity;

namespace SistemaInventario.Entity;

public partial class DBInventarioContext : DbContext
{
    public DBInventarioContext()
    {
    }

    public DBInventarioContext(DbContextOptions<DBInventarioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Compra> Compra { get; set; }

    public virtual DbSet<Configuracion> Configuracion { get; set; }

    public virtual DbSet<DetalleCompra> DetalleCompra { get; set; }

    public virtual DbSet<DetalleVenta> DetalleVenta { get; set; }

    public virtual DbSet<Marca> Marca { get; set; }

    public virtual DbSet<Menu> Menu { get; set; }

    public virtual DbSet<Negocio> Negocio { get; set; }

    public virtual DbSet<Producto> Producto { get; set; }

    public virtual DbSet<Proveedor> Proveedor { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<RolMenu> RolMenu { get; set; }

    public virtual DbSet<TipoDocumentoCompra> TipoDocumentoCompra { get; set; }

    public virtual DbSet<TipoDocumentoVenta> TipoDocumentoVenta { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    public virtual DbSet<Venta> Venta { get; set; }

    public virtual DbSet<HistorialProducto> HistorialProducto { get; set; }

    public virtual DbSet<TomaInventario> TomaInventario { get; set; }

    public virtual DbSet<Contador> Contador { get; set; }

    public virtual DbSet<DetalleDevolucion> DetalleDevolucion { get; set; }

    public virtual DbSet<Devolucion> Devolucion { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);

            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Categoria)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Categoria_Usuario");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra);

            entity.ToTable("Compra");

            entity.Property(e => e.IdCompra).HasColumnName("idCompra");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");
            entity.Property(e => e.IdTipoDocumentoCompra).HasColumnName("idTipoDocumentoCompra");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.ImpuestoTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("impuestoTotal");
            entity.Property(e => e.NumeroCompra)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numeroCompra");
            entity.Property(e => e.SubTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subTotal");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compra)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compra_Proveedor");

            entity.HasOne(d => d.IdTipoDocumentoCompraNavigation).WithMany(p => p.Compra)
                .HasForeignKey(d => d.IdTipoDocumentoCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compra_TipoDocumentoCompra");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Compra)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compra_Usuario");
        });

        modelBuilder.Entity<Configuracion>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Configuracion");

            entity.Property(e => e.Propiedad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("propiedad");
            entity.Property(e => e.Recurso)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("recurso");
            entity.Property(e => e.Valor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("valor");
        });

        modelBuilder.Entity<DetalleCompra>(entity =>
        {
            entity.HasKey(e => e.IdDetalleCompra);

            entity.ToTable("DetalleCompra");

            entity.Property(e => e.IdDetalleCompra).HasColumnName("idDetalleCompra");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CategoriaProducto)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("categoriaProducto");
            entity.Property(e => e.CodigoProducto)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codigoProducto");
            entity.Property(e => e.IdCompra).HasColumnName("idCompra");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.MarcaProducto)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("marcaProducto");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreProducto");
            entity.Property(e => e.PrecioCompra)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precioCompra");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DetalleCompra)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleCompra_Compra");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleCompra)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleCompra_Producto");
        });

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.HasKey(e => e.IdDetalleVenta);

            entity.Property(e => e.IdDetalleVenta).HasColumnName("idDetalleVenta");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CategoriaProducto)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("categoriaProducto");
            entity.Property(e => e.CodigoProducto)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codigoProducto");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdVenta).HasColumnName("idVenta");
            entity.Property(e => e.MarcaProducto)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("marcaProducto");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreProducto");
            entity.Property(e => e.PrecioVenta)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precioVenta");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVenta_Producto");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVenta_Venta");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.IdMarca);

            entity.ToTable("Marca");

            entity.Property(e => e.IdMarca).HasColumnName("idMarca");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Marca)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Marca_Usuario");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.IdMenu);

            entity.ToTable("Menu");

            entity.Property(e => e.IdMenu).HasColumnName("idMenu");
            entity.Property(e => e.Controlador)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("controlador");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Icono)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("icono");
            entity.Property(e => e.IdMenuPadre).HasColumnName("idMenuPadre");
            entity.Property(e => e.Nombre)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PagAccion)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("pagAccion");

            entity.HasOne(d => d.IdMenuPadreNavigation).WithMany(p => p.InverseIdMenuPadreNavigation)
                .HasForeignKey(d => d.IdMenuPadre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Menu_Menu");
        });

        modelBuilder.Entity<Negocio>(entity =>
        {
            entity.HasKey(e => e.IdNegocio);

            entity.ToTable("Negocio");

            entity.Property(e => e.IdNegocio).HasColumnName("idNegocio");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Documento)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("documento");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Impuesto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("impuesto");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numeroDocumento");
            entity.Property(e => e.SimboloMoneda)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("simboloMoneda");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Negocio)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Negocio_Usuario");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto);

            entity.ToTable("Producto");

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.CodigoBarra)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codigoBarra");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Razon)
                .HasColumnType("text")
                .HasColumnName("razon");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.IdMarca).HasColumnName("idMarca");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PrecioCompra)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precioCompra");
            entity.Property(e => e.PrecioVenta)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precioVenta");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Producto)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Categoria");

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Producto)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Marca");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Producto)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Usuario");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor);

            entity.ToTable("Proveedor");

            entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numeroDocumento");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tipoDocumento");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Proveedor)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Proveedor_Usuario");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.ToTable("Rol");

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<RolMenu>(entity =>
        {
            entity.HasKey(e => e.IdRolMenu);

            entity.ToTable("RolMenu");

            entity.Property(e => e.IdRolMenu).HasColumnName("idRolMenu");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdMenu).HasColumnName("idMenu");
            entity.Property(e => e.IdRol).HasColumnName("idRol");

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.RolMenu)
                .HasForeignKey(d => d.IdMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolMenu_Menu");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolMenu)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolMenu_Rol");
        });

        modelBuilder.Entity<TipoDocumentoCompra>(entity =>
        {
            entity.HasKey(e => e.IdTipoDocumentoCompra);

            entity.ToTable("TipoDocumentoCompra");

            entity.Property(e => e.IdTipoDocumentoCompra).HasColumnName("idTipoDocumentoCompra");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TipoDocumentoCompra)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TipoDocumentoCompra_Usuario");
        });

        modelBuilder.Entity<TipoDocumentoVenta>(entity =>
        {
            entity.HasKey(e => e.IdTipoDocumentoVenta);

            entity.Property(e => e.IdTipoDocumentoVenta).HasColumnName("idTipoDocumentoVenta");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TipoDocumentoVenta)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TipoDocumentoVenta_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("clave");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Documento)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("documento");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numeroDocumento");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Rol");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta);

            entity.Property(e => e.IdVenta).HasColumnName("idVenta");
            entity.Property(e => e.DocumentoCliente)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("documentoCliente");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdTipoDocumentoVenta).HasColumnName("idTipoDocumentoVenta");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.ImpuestoTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("impuestoTotal");
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreCliente");
            entity.Property(e => e.NumeroDocumentoCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numeroDocumentoCliente");
            entity.Property(e => e.NumeroVenta)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numeroVenta");
            entity.Property(e => e.SubTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subTotal");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdTipoDocumentoVentaNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdTipoDocumentoVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Venta_TipoDocumentoVenta");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Venta_Usuario");
        });

        modelBuilder.Entity<HistorialProducto>(entity =>
        {
            entity.HasKey(e => e.IdHistorialProducto);

            entity.ToTable("HistorialProducto");

            entity.Property(e => e.IdHistorialProducto).HasColumnName("idHistorialProducto");
            entity.Property(e => e.ColumnaModificada)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("columnaModificada");
            entity.Property(e => e.ValorAnterior)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("valorAnterior");
            entity.Property(e => e.ValorNuevo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("valorNuevo");
            entity.Property(e => e.Razon)
                .HasColumnType("text")
                .HasColumnName("razon");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(h => h.HistorialProducto)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialProducto_Producto");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(h => h.HistorialProducto)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialProducto_Usuario");
        });

        modelBuilder.Entity<TomaInventario>(entity =>
        {
            entity.HasKey(e => e.IdTomaInventario);

            entity.ToTable("TomaInventario");

            entity.Property(e => e.IdTomaInventario).HasColumnName("idTomaInventario");
            entity.Property(e => e.DocumentoEncargado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("documentoEncargado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.FechaToma).HasColumnName("fechaToma");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.NombreEncargado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreEncargado");
            entity.Property(e => e.NumeroDocumentoEncargado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numeroDocumentoEncargado");
            entity.Property(e => e.Razon)
                .HasColumnType("text")
                .HasColumnName("razon");
            entity.Property(e => e.StockAnterior).HasColumnName("stockAnterior");
            entity.Property(e => e.StockNuevo).HasColumnName("stockNuevo");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.TomaInventario)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TomaInventario_Producto");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TomaInventario)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TomaInventario_Usuario");
        });

        modelBuilder.Entity<Contador>(entity =>
        {
            entity.HasKey(e => e.IdContador).HasName("PK_Contador_1");

            entity.ToTable("Contador");

            entity.Property(e => e.IdContador).HasColumnName("idContador");
            entity.Property(e => e.Contador1).HasColumnName("contador");
            entity.Property(e => e.Fecha)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("fecha");
        });

        modelBuilder.Entity<Devolucion>(entity =>
        {
            entity.HasKey(e => e.IdDevolucion);

            entity.ToTable("Devolucion");

            entity.Property(e => e.IdDevolucion)
                .ValueGeneratedNever()
                .HasColumnName("idDevolucion");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.IdVenta).HasColumnName("idVenta");
            entity.Property(e => e.MontoDevuelto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("MontoDevuelto");
            entity.Property(e => e.Motivo)
                .HasColumnType("text")
                .HasColumnName("motivo");
            entity.Property(e => e.NumeroVenta)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numeroVenta");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Devolucion)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Devolucion_Usuario");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.Devolucion)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Devolucion_Venta");
        });

        modelBuilder.Entity<DetalleDevolucion>(entity =>
        {
            entity.HasKey(e => e.IdDetalleDevolucion);

            entity.ToTable("DetalleDevolucion");

            entity.Property(e => e.IdDetalleDevolucion)
                .ValueGeneratedNever()
                .HasColumnName("idDetalleDevolucion");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdDevolucion).HasColumnName("idDevolucion");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");

            entity.HasOne(d => d.IdDevolucionNavigation).WithMany(p => p.DetalleDevolucion)
                .HasForeignKey(d => d.IdDevolucion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleDevolucion_Devolucion");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleDevolucion)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleDevolucion_Producto");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
