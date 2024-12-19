using AutoMapper;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.ApplicationWeb.Models.ViewModels;
using SistemaInventario.ApplicationWeb.Utilidades.Response;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.Entity;
using System.Security.Claims;

namespace SistemaInventario.ApplicationWeb.Controllers
{
    [Authorize]
    public class CompraController : Controller
    {
        private readonly ITipoDocumentoCompraService _tipoDocumentoCompraServicio;
        private readonly ICompraService _compraServicio;
        private readonly IMapper _mapper;
        public CompraController(ITipoDocumentoCompraService tipoDocumentoCompraServicio,
            ICompraService compraServicio,
            IMapper mapper)
        {
            _tipoDocumentoCompraServicio = tipoDocumentoCompraServicio;
            _compraServicio = compraServicio;
            _mapper = mapper;
        }


        public IActionResult Registrar()
        {
            return View();
        }

        public IActionResult Historial()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaTipoDocumentoCompra()
        {
            List<VMTipoDocumentoCompra> vmListaTipoDocumentos = _mapper.Map<List<VMTipoDocumentoCompra>>(await _tipoDocumentoCompraServicio.Lista());
            return StatusCode(StatusCodes.Status200OK, vmListaTipoDocumentos);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductos(string busqueda)
        {
            List<VMProducto> vmListProducts = _mapper.Map<List<VMProducto>>(await _compraServicio.ObtenerProductos(busqueda));
            return StatusCode(StatusCodes.Status200OK, vmListProducts);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarCompra([FromBody] VMCompra modelo)
        {
            GenericResponse<VMCompra> gResponse = new GenericResponse<VMCompra>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                   .Select(c => c.Value)
                                                   .SingleOrDefault();
                modelo.IdUsuario = int.Parse(idUsuario);
                Compra compraGenerated = await _compraServicio.Registrar(_mapper.Map<Compra>(modelo));
                modelo = _mapper.Map<VMCompra>(compraGenerated);
                gResponse.Estado = true;
                gResponse.Objeto = modelo;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpGet]
        public async Task<IActionResult> HistorialCompra(string numeroCompra, string fechaInicio, string fechaFin)
        {
            List<VMCompra> vmHistorialCompra = _mapper.Map<List<VMCompra>>(await _compraServicio.Historial(numeroCompra, fechaInicio, fechaFin));
            return StatusCode(StatusCodes.Status200OK, vmHistorialCompra);
        }
    }
}
