using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.ApplicationWeb.Models.ViewModels;
using SistemaInventario.BLL.Interfaces;

namespace SistemaInventario.ApplicationWeb.Controllers
{
    public class PlantillaController : Controller
    {
        private readonly ICompraService _compraServicio;
        private readonly IVentaService _ventaServicio;
        private readonly INegocioService _negocioServicio;
        private readonly IMapper _mapper;
        public PlantillaController(ICompraService compraServicio,
            IVentaService ventaServicio,
            INegocioService negocioServicio,
            IMapper mapper)
        {
            _compraServicio = compraServicio;
            _ventaServicio = ventaServicio;
            _negocioServicio = negocioServicio;
            _mapper = mapper;
        }


        public IActionResult EnviarClave(string correo, string clave)
        {
            ViewData["Correo"] = correo;
            ViewData["Clave"] = clave;
            ViewData["Url"] = $"{this.Request.Scheme}://{this.Request.Host}";
            return View();
        }
        
        public IActionResult RestablecerClave(string clave)
        {
            ViewData["Clave"] = clave;
            return View();
        }

        public IActionResult ClaveRestablacida()
        {
            return View();
        }

        public async Task<IActionResult> PDFVenta(string numeroVenta)
        {
            VMVenta vmVenta = _mapper.Map<VMVenta>(await _ventaServicio.Detalle(numeroVenta));
            VMNegocio vmNegocio = _mapper.Map<VMNegocio>(await _negocioServicio.Obtener());
            VMPDFVenta modelo = new VMPDFVenta();
            modelo.Negocio = vmNegocio;
            modelo.Venta = vmVenta;
            return View(modelo);
        }

        public async Task<IActionResult> PDFCompra(string numeroCompra)
        {
            VMCompra vmCompra = _mapper.Map<VMCompra>(await _ventaServicio.Detalle(numeroCompra));
            VMNegocio vmNegocio = _mapper.Map<VMNegocio>(await _negocioServicio.Obtener());
            VMPDFCompra modelo = new VMPDFCompra();
            modelo.Negocio = vmNegocio;
            modelo.Compra = vmCompra;
            return View(modelo);
        }
    }
}
