using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.ApplicationWeb.Models.ViewModels;
using SistemaInventario.BLL.Implementaciones;
using SistemaInventario.BLL.Interfaces;

namespace SistemaInventario.ApplicationWeb.Controllers
{
    [Authorize]
    public class ReporteController : Controller
    {
        private readonly ICompraService _compraServicio;
        private readonly IVentaService _ventaServicio;
        private readonly IMapper _mapper;
        public ReporteController(ICompraService compraServicio,
            IVentaService ventaServicio,
            IMapper mapper)
        {
            _compraServicio = compraServicio;
            _ventaServicio  = ventaServicio;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Compra()
        {
            return View();
        }

        public IActionResult Venta()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ReporteCompra(string fechaInicio, string fechaFin)
        {
            List<VMReporteCompra> vmLista = _mapper.Map<List<VMReporteCompra>>(await _compraServicio.Reporte(fechaInicio, fechaFin));
            return StatusCode(StatusCodes.Status200OK, new { data = vmLista });
        }

        [HttpGet]
        public async Task<IActionResult> ReporteVenta(string fechaInicio, string fechaFin)
        {
            List<VMReporteVenta> vmLista = _mapper.Map<List<VMReporteVenta>>(await _ventaServicio.Reporte(fechaInicio, fechaFin));
            return StatusCode(StatusCodes.Status200OK, new { data = vmLista });
        }
    }
}
