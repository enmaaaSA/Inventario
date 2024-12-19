using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.BLL.Implementaciones;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.ApplicationWeb.Models.ViewModels;
using SistemaInventario.ApplicationWeb.Utilidades.Response;

namespace SistemaInventario.ApplicationWeb.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardServicio;
        public DashboardController(IDashboardService dashboardServicio)
        {
            _dashboardServicio = dashboardServicio;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerResumen()
        {
            GenericResponse<VMDashboard> gResponse = new GenericResponse<VMDashboard>();
            try
            {
                VMDashboard vmDashBoard = new VMDashboard();

                vmDashBoard.TotalVentas = await _dashboardServicio.TotalVentasUltimaSemana();
                vmDashBoard.TotalIngresos = await _dashboardServicio.TotalIngresosUltimaSemana();
                vmDashBoard.TotalProductos = await _dashboardServicio.TotalProductos();
                vmDashBoard.TotalCategorias = await _dashboardServicio.TotalCategorias();

                List<VMVentasSemana> listaVentasSemana = new List<VMVentasSemana>();
                List<VMProductosSemana> listaProductosSemana = new List<VMProductosSemana>();

                foreach (KeyValuePair<string, int> item in await _dashboardServicio.VentasUltimaSemana())
                {
                    listaVentasSemana.Add(new VMVentasSemana()
                    {
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }

                foreach (KeyValuePair<string, int> item in await _dashboardServicio.ProductosTopUltimaSemana())
                {
                    listaProductosSemana.Add(new VMProductosSemana()
                    {
                        Producto = item.Key,
                        Cantidad = item.Value
                    });
                }

                vmDashBoard.VentasUltimaSemana = listaVentasSemana;
                vmDashBoard.ProductosTopUltimaSemana = listaProductosSemana;

                gResponse.Estado = true;
                gResponse.Objeto = vmDashBoard;

            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }


            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}
