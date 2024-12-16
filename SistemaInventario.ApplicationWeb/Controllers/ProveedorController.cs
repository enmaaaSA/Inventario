using AutoMapper;
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
    public class ProveedorController : Controller
    {
        private readonly IProveedorService _proveedorServicio;
        private readonly IMapper _mapper;
        public ProveedorController(IProveedorService proveedorServicio,
            IMapper mapper)
        {
            _proveedorServicio = proveedorServicio;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMProveedor> vmListBusiness = _mapper.Map<List<VMProveedor>>(await _proveedorServicio.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = vmListBusiness });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMProveedor modelo)
        {
            GenericResponse<VMProveedor> gResponse = new GenericResponse<VMProveedor>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                   .Select(c => c.Value)
                                                   .SingleOrDefault();
                modelo.IdUsuario = int.Parse(idUsuario);
                Proveedor businessCreated = await _proveedorServicio.Crear(_mapper.Map<Proveedor>(modelo));
                modelo = _mapper.Map<VMProveedor>(businessCreated);
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
        public async Task<IActionResult> Editar([FromBody] VMProveedor modelo)
        {
            GenericResponse<VMProveedor> gResponse = new GenericResponse<VMProveedor>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                   .Select(c => c.Value)
                                                   .SingleOrDefault();
                modelo.IdUsuario = int.Parse(idUsuario);
                Proveedor businessEdited = await _proveedorServicio.Editar(_mapper.Map<Proveedor>(modelo));
                modelo = _mapper.Map<VMProveedor>(businessEdited);
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
