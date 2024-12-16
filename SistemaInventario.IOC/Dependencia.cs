using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaInventario.BLL.Implementaciones;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.DAL.Implementaciones;
using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencia(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DBInventarioContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CadenaSQL"));
            });
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICompraRepository, CompraRepository>();
            services.AddScoped<IVentaRepository, VentaRepository>();
            services.AddScoped<ICorreoRepository, CorreoRepository>();
            services.AddScoped<IDevolucionRepository, DevolucionRepository>();

            services.AddScoped<IUtilidadesService, UtilidadesService>();
            services.AddScoped<ICorreoService, CorreoService>();

            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IMarcaService, MarcaService>();
            services.AddScoped<IProductoService, ProductoService>();

            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService, UsuarioService>();

            services.AddScoped<INegocioService, NegocioService>();
            services.AddScoped<IProveedorService, ProveedorService>();

            services.AddScoped<ITipoDocumentoCompraService, TipoDocumentoCompraService>();
            services.AddScoped<ITipoDocumentoVentaService, TipoDocumentoVentaService>();
            services.AddScoped<ICompraService, CompraService>();
            services.AddScoped<IVentaService, VentaService>();
            
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<ITomaInventarioService, TomaInventarioService>();
            services.AddScoped<IDevolucionService, DevolucionService>();
        }
    }
}
