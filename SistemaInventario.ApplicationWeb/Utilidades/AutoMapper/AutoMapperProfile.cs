using AutoMapper;
using SistemaInventario.ApplicationWeb.Models.ViewModels;
using SistemaInventario.Entity;
using System.Globalization;

namespace SistemaInventario.ApplicationWeb.Utilidades.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol, VMRol>().ReverseMap();
            #endregion Rol

            #region Usuario
            CreateMap<Usuario, VMUsuario>()
                .ForMember(destino =>
                    destino.Estado,
                    opt => opt.MapFrom(origen => origen.Estado == true ? 1 : 0))
                .ForMember(destino =>
                    destino.Rol,
                    opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre));

            CreateMap<VMUsuario, Usuario>()
                .ForMember(destino =>
                    destino.Estado,
                    opt => opt.MapFrom(origen => origen.Estado == 1 ? true : false))
                .ForMember(destino =>
                    destino.IdRolNavigation,
                    opt => opt.Ignore());
            #endregion Usuario

            #region Negocio
            CreateMap<Negocio, VMNegocio>()
                .ForMember(destino =>
                    destino.Impuesto,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Impuesto.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.Usuario,
                    opt => opt.MapFrom(origen => origen.IdUsuarioNavigation.Nombre));

            CreateMap<VMNegocio, Negocio>()
                .ForMember(destino =>
                    destino.Impuesto,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Impuesto, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.IdUsuarioNavigation,
                    opt => opt.Ignore());
            #endregion Negocio

            #region Categoria
            CreateMap<Categoria, VMCategoria>()
                .ForMember(destino =>
                    destino.Estado,
                    opt => opt.MapFrom(origen => origen.Estado == true ? 1 : 0))
                .ForMember(destino =>
                    destino.Usuario,
                    opt => opt.MapFrom(origen => origen.IdUsuarioNavigation.Nombre));

            CreateMap<VMCategoria, Categoria>()
                .ForMember(destino =>
                    destino.Estado,
                    opt => opt.MapFrom(origen => origen.Estado == 1 ? true : false))
                .ForMember(destino =>
                    destino.IdUsuarioNavigation,
                    opt => opt.Ignore());
            #endregion Categoria

            #region Marca
            CreateMap<Marca, VMMarca>()
                .ForMember(destino =>
                    destino.Estado,
                    opt => opt.MapFrom(origen => origen.Estado == true ? 1 : 0))
                .ForMember(destino =>
                    destino.Usuario,
                    opt => opt.MapFrom(origen => origen.IdUsuarioNavigation.Nombre));

            CreateMap<VMMarca, Marca>()
                .ForMember(destino =>
                    destino.Estado,
                    opt => opt.MapFrom(origen => origen.Estado == 1 ? true : false))
                .ForMember(destino =>
                    destino.IdUsuarioNavigation,
                    opt => opt.Ignore());
            #endregion Marca

            #region Producto
            CreateMap<Producto, VMProducto>()
                .ForMember(destino =>
                    destino.Estado,
                    opt => opt.MapFrom(origen => origen.Estado == true ? 1 : 0))
                .ForMember(destino =>
                    destino.Categoria,
                    opt => opt.MapFrom(origen => origen.IdCategoriaNavigation.Nombre))
                .ForMember(destino =>
                    destino.Marca,
                    opt => opt.MapFrom(origen => origen.IdMarcaNavigation.Nombre))
                .ForMember(destino =>
                    destino.PrecioCompra,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.PrecioCompra.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.PrecioVenta,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.PrecioVenta.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.UsuarioNombre,
                    opt => opt.MapFrom(origen => origen.IdUsuarioNavigation.Nombre))
                .ForMember(destino =>
                    destino.UsuarioApellido,
                    opt => opt.MapFrom(origen => origen.IdUsuarioNavigation.Apellido));

            CreateMap<VMProducto, Producto>()
                .ForMember(destino =>
                    destino.Estado,
                    opt => opt.MapFrom(origen => origen.Estado == 1 ? true : false))
                .ForMember(destino =>
                    destino.IdCategoriaNavigation,
                    opt => opt.Ignore())
                .ForMember(destino =>
                    destino.IdMarcaNavigation,
                    opt => opt.Ignore())
                .ForMember(destino =>
                    destino.PrecioCompra,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioCompra, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.PrecioVenta,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioVenta, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.IdUsuarioNavigation,
                    opt => opt.Ignore());
            #endregion Producto

            #region TipoDocumentoCompra
            CreateMap<TipoDocumentoCompra, VMTipoDocumentoCompra>().ReverseMap();
            #endregion TipoDocumentoCompra

            #region TipoDocumentoVenta
            CreateMap<TipoDocumentoVenta, VMTipoDocumentoVenta>().ReverseMap();
            #endregion TipoDocumentoVenta

            #region Compra
            CreateMap<Compra, VMCompra>()
                .ForMember(destino =>
                    destino.Usuario,
                    opt => opt.MapFrom(origen => origen.IdUsuarioNavigation.Nombre))
                .ForMember(destino =>
                    destino.TipoDocumentoCompra,
                    opt => opt.MapFrom(origen => origen.IdTipoDocumentoCompraNavigation.Nombre))
                .ForMember(destino =>
                    destino.DocumentoProveedor,
                    opt => opt.MapFrom(origen => origen.IdProveedorNavigation.NumeroDocumento))
                .ForMember(destino =>
                    destino.NombreProveedor,
                    opt => opt.MapFrom(origen => origen.IdProveedorNavigation.Nombre))
                .ForMember(destino =>
                    destino.Proveedor,
                    opt => opt.MapFrom(origen => origen.IdProveedorNavigation.Nombre))
                .ForMember(destino =>
                    destino.SubTotal,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.SubTotal.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.ImpuestoTotal,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.ImpuestoTotal.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.Fecha,
                    opt => opt.MapFrom(origen => origen.Fecha.Value.ToString("dd/MM/yyyy")));

            CreateMap<VMCompra, Compra>()
                .ForMember(destino =>
                    destino.IdUsuarioNavigation,
                    opt => opt.Ignore())
                .ForMember(destino =>
                    destino.IdTipoDocumentoCompraNavigation,
                    opt => opt.Ignore())
                .ForMember(destino =>
                    destino.IdProveedorNavigation,
                    opt => opt.Ignore())
                .ForMember(destino =>
                    destino.SubTotal,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.SubTotal, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.ImpuestoTotal,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.ImpuestoTotal, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Total, new CultureInfo("es-PE"))));
            #endregion Compra

            #region Venta
            CreateMap<Venta, VMVenta>()
                .ForMember(destino =>
                    destino.Usuario,
                    opt => opt.MapFrom(origen => origen.IdUsuarioNavigation.Nombre))
                .ForMember(destino =>
                    destino.TipoDocumentoVenta,
                    opt => opt.MapFrom(origen => origen.IdTipoDocumentoVentaNavigation.Nombre))
                .ForMember(destino =>
                    destino.SubTotal,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.SubTotal.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.ImpuestoTotal,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.ImpuestoTotal.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-PE"))));

            CreateMap<VMVenta, Venta>()
                .ForMember(destino =>
                    destino.IdUsuarioNavigation,
                    opt => opt.Ignore())
                .ForMember(destino =>
                    destino.IdTipoDocumentoVentaNavigation,
                    opt => opt.Ignore())
                .ForMember(destino =>
                    destino.SubTotal,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.SubTotal, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.ImpuestoTotal,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.ImpuestoTotal, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Total, new CultureInfo("es-PE"))));
            #endregion Venta

            #region DetalleCompra
            CreateMap<DetalleCompra, VMDetalleCompra>()
                .ForMember(destino =>
                    destino.PrecioCompra,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.PrecioCompra.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-PE"))));

            CreateMap<VMDetalleCompra, DetalleCompra>()
                .ForMember(destino =>
                    destino.PrecioCompra,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioCompra, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Total, new CultureInfo("es-PE"))));

            CreateMap<DetalleCompra, VMReporteCompra>()
                .ForMember(destino =>
                    destino.Fecha,
                    opt => opt.MapFrom(origen => origen.IdCompraNavigation.Fecha.Value.ToString("dd/MM/yyyy")))
                .ForMember(destino =>
                    destino.NumeroCompra,
                    opt => opt.MapFrom(origen => origen.IdCompraNavigation.NumeroCompra))
                .ForMember(destino =>
                    destino.TipoDocumento,
                    opt => opt.MapFrom(origen => origen.IdCompraNavigation.IdTipoDocumentoCompraNavigation.Nombre))
                .ForMember(destino =>
                    destino.DocumentoProveedor,
                    opt => opt.MapFrom(origen => origen.IdCompraNavigation.IdProveedorNavigation.NumeroDocumento))
                .ForMember(destino =>
                    destino.NombreProveedor,
                    opt => opt.MapFrom(origen => origen.IdCompraNavigation.IdProveedorNavigation.Nombre))
                .ForMember(destino =>
                    destino.SubTotalCompra,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.IdCompraNavigation.SubTotal.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.ImpuestoTotalCompra,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.IdCompraNavigation.ImpuestoTotal.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.TotalCompra,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.IdCompraNavigation.Total.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.Producto,
                    opt => opt.MapFrom(origen => origen.NombreProducto))
                .ForMember(destino =>
                    destino.PrecioCompra,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.PrecioCompra.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-PE"))));
            #endregion DetalleCompra

            #region DetalleVenta
            CreateMap<DetalleVenta, VMDetalleVenta>()
                .ForMember(destino =>
                    destino.PrecioVenta,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.PrecioVenta.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-PE"))));

            CreateMap<VMDetalleVenta, DetalleVenta>()
                .ForMember(destino =>
                    destino.PrecioVenta,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioVenta, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Total, new CultureInfo("es-PE"))));

            CreateMap<DetalleVenta, VMReporteVenta>()
                .ForMember(destino =>
                    destino.Fecha,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.Fecha.Value.ToString("dd/MM/yyyy")))
                .ForMember(destino =>
                    destino.NumeroVenta,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.NumeroVenta))
                .ForMember(destino =>
                    destino.TipoDocumento,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.IdTipoDocumentoVentaNavigation.Nombre))
                .ForMember(destino =>
                    destino.DocumentoCliente,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.DocumentoCliente))
                .ForMember(destino =>
                    destino.NombreCliente,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.NombreCliente))
                .ForMember(destino =>
                    destino.SubTotalVenta,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.IdVentaNavigation.SubTotal.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.ImpuestoTotalVenta,
                   opt => opt.MapFrom(origen => Convert.ToString(origen.IdVentaNavigation.ImpuestoTotal.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.TotalVenta,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.IdVentaNavigation.Total.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.Producto,
                    opt => opt.MapFrom(origen => origen.NombreProducto))
                .ForMember(destino =>
                    destino.PrecioVenta,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.PrecioVenta.Value, new CultureInfo("es-PE"))))
                .ForMember(destino =>
                    destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-PE"))));
            #endregion DetalleVenta

            #region Proveedor
            CreateMap<Proveedor, VMProveedor>()
                .ForMember(destino =>
                    destino.Estado,
                    opt => opt.MapFrom(origen => origen.Estado == true ? 1 : 0))
                .ForMember(destino =>
                    destino.Usuario,
                    opt => opt.MapFrom(origen => origen.IdUsuarioNavigation.Nombre));

            CreateMap<VMProveedor, Proveedor>()
                .ForMember(destino =>
                    destino.Estado,
                    opt => opt.MapFrom(origen => origen.Estado == 1 ? true : false))
                .ForMember(destino =>
                    destino.IdUsuarioNavigation,
                    opt => opt.Ignore());
            #endregion Proveedor

            #region Menu
            CreateMap<Menu, VMMenu>()
                .ForMember(destino =>
                destino.SubMenu,
                opt => opt.MapFrom(origen => origen.InverseIdMenuPadreNavigation));
            #endregion

            #region Toma
            CreateMap<TomaInventario, VMToma>().ReverseMap();
            #endregion
        }
    }
}