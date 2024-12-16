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
    public class TomaInventarioController : Controller
    {
        private readonly IProductoService _productoServicio;
        private readonly IMapper _mapper;
        private readonly ITomaInventarioService _tomaInventarioServicio;
        public TomaInventarioController(IProductoService productoServicio,
                                        ITomaInventarioService tomaInventarioServicio,
                                        IMapper mapper)
        {
            _productoServicio = productoServicio;
            _mapper = mapper;
            _tomaInventarioServicio = tomaInventarioServicio;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> TomaInventarioStock([FromBody] VMTomaInventario modelo)
        {
            GenericResponse<VMTomaInventario> gResponse = new GenericResponse<VMTomaInventario>();
            try
            {
                Producto product = await _productoServicio.TomaInventario(_mapper.Map<Producto>(modelo));
                modelo = _mapper.Map<VMTomaInventario>(product);
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

        [HttpPost]
        public async Task<IActionResult> StockTomaInventario([FromBody] VMTomaInventario modelo)
        {
            GenericResponse<VMTomaInventario> gResponse = new GenericResponse<VMTomaInventario>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();
                modelo.IdUsuario = int.Parse(idUsuario);
                Producto productEdited = await _productoServicio.TomaInventario(_mapper.Map<Producto>(modelo));
                modelo = _mapper.Map<VMTomaInventario>(productEdited);
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<VMToma> modelos)
        {
            GenericResponse<List<VMToma>> gResponse = new GenericResponse<List<VMToma>>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                   .Select(c => c.Value)
                                                   .SingleOrDefault();


                int IdUser = int.Parse(idUsuario);

                List<TomaInventario> tomas = modelos.Select(m => {
                    m.IdUsuario = IdUser;
                    return _mapper.Map<TomaInventario>(m);
                }).ToList();

                List<TomaInventario> tomasRegistradas = new List<TomaInventario>();

                foreach (var toma in tomas)
                {
                    TomaInventario creada = await _tomaInventarioServicio.Crear(toma);
                    tomasRegistradas.Add(creada);
                }

                gResponse.Estado = true;
                gResponse.Objeto = tomasRegistradas.Select(t => _mapper.Map<VMToma>(t)).ToList();
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
