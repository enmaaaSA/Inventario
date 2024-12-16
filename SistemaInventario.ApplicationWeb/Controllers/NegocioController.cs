using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaInventario.ApplicationWeb.Models.ViewModels;
using SistemaInventario.ApplicationWeb.Utilidades.Response;
using SistemaInventario.BLL.Implementaciones;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.Entity;
using System.Security.Claims;

namespace SistemaInventario.ApplicationWeb.Controllers
{
    [Authorize]
    public class NegocioController : Controller
    {
        private readonly INegocioService _negocioServicio;
        private readonly IMapper _mapper;
        public NegocioController(INegocioService negocioServicio,
            IMapper mapper)
        {
            _negocioServicio = negocioServicio;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            GenericResponse<VMNegocio> gResponse = new GenericResponse<VMNegocio>();
            try
            {
                VMNegocio vmNegocio = _mapper.Map<VMNegocio>(await _negocioServicio.Obtener());
                gResponse.Estado = true;
                gResponse.Objeto = vmNegocio;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarCambio([FromForm] string modelo)
        {
            GenericResponse<VMNegocio> gResponse = new GenericResponse<VMNegocio>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                   .Select(c => c.Value)
                                                   .SingleOrDefault();
                VMNegocio vmBusiness = JsonConvert.DeserializeObject<VMNegocio>(modelo);
                vmBusiness.IdUsuario = int.Parse(idUsuario);
                Negocio businessEdited = await _negocioServicio.Editar(_mapper.Map<Negocio>(vmBusiness));
                vmBusiness = _mapper.Map<VMNegocio>(businessEdited);
                gResponse.Estado = true;
                gResponse.Objeto = vmBusiness;
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
