using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaInventario.ApplicationWeb.Models.ViewModels;
using SistemaInventario.ApplicationWeb.Utilidades.Response;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.Entity;
using System.Security.Claims;

namespace SistemaInventario.ApplicationWeb.Controllers
{
    [Authorize]
    public class MarcaController : Controller
    {
        private readonly IMarcaService _marcaServicio;
        private readonly IMapper _mapper;
        public MarcaController(IMarcaService marcaServicio,
            IMapper mapper)
        {
            _marcaServicio = marcaServicio;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMMarca> vmListBrand = _mapper.Map<List<VMMarca>>(await _marcaServicio.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = vmListBrand });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMMarca modelo)
        {
            GenericResponse<VMMarca> gResponse = new GenericResponse<VMMarca>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                   .Select(c => c.Value)
                                                   .SingleOrDefault();
                modelo.IdUsuario = int.Parse(idUsuario);
                Marca brandCreated = await _marcaServicio.Crear(_mapper.Map<Marca>(modelo));
                modelo = _mapper.Map<VMMarca>(brandCreated);
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

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] VMMarca modelo)
        {
            GenericResponse<VMMarca> gResponse = new GenericResponse<VMMarca>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                   .Select(c => c.Value)
                                                   .SingleOrDefault();
                modelo.IdUsuario = int.Parse(idUsuario);
                Marca brandEdited = await _marcaServicio.Editar(_mapper.Map<Marca>(modelo));
                modelo = _mapper.Map<VMMarca>(brandEdited);
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
    }
}
