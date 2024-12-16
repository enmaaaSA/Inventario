using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.ApplicationWeb.Models.ViewModels;
using SistemaInventario.ApplicationWeb.Utilidades.Response;
using SistemaInventario.BLL.Implementaciones;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System.Security.Claims;

namespace SistemaInventario.ApplicationWeb.Controllers
{
    public class MantenimientoController : Controller
    {
        private readonly IDevolucionRepository _devolucionRepositorio;
        private readonly IDevolucionService _devolucionServicio;
        private readonly IMapper _mapper;
        public MantenimientoController(IDevolucionRepository devolucionRepositorio,
            IDevolucionService devolucionServicio,
            IMapper mapper)
        {
            _devolucionRepositorio = devolucionRepositorio;
            _devolucionServicio = devolucionServicio;
            _mapper = mapper;
        }


        public IActionResult TomaInventario()
        {
            return View();
        }

        public IActionResult Devolucion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarDevolucion([FromBody] RegistrarDevolucionDTO modelo)
        {
            try
            {
                // Obtener el ID del usuario autenticado desde los Claims
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims
                                            .Where(c => c.Type == ClaimTypes.NameIdentifier)
                                            .Select(c => c.Value)
                                            .FirstOrDefault();

                if (string.IsNullOrEmpty(idUsuario))
                {
                    return Unauthorized(new { mensaje = "Usuario no autenticado." });
                }

                // Registrar devolución con el ID del usuario autenticado
                var resultado = await _devolucionServicio.RegistrarDevolucion(modelo.NumeroVenta, modelo.Motivo, int.Parse(idUsuario));

                if (resultado)
                {
                    return Ok(new { mensaje = "Devolución registrada correctamente." });
                }

                return BadRequest(new { mensaje = "No se pudo registrar la devolución." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        public class RegistrarDevolucionDTO
        {
            public string NumeroVenta { get; set; }
            public string Motivo { get; set; }
            public int IdUsuario { get; set; }
        }
    }
}
