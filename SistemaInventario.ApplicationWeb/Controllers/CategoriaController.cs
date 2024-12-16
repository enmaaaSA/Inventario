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
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaServicio;
        private readonly IMapper _mapper;
        public CategoriaController(ICategoriaService categoriaServicio,
            IMapper mapper)
        {
            _categoriaServicio = categoriaServicio;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMCategoria> vmListCategory = _mapper.Map<List<VMCategoria>>(await _categoriaServicio.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = vmListCategory });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMCategoria modelo)
        {
            GenericResponse<VMCategoria> gResponse = new GenericResponse<VMCategoria>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                   .Select(c => c.Value)
                                                   .SingleOrDefault();
                modelo.IdUsuario = int.Parse(idUsuario);
                Categoria categotyCreated = await _categoriaServicio.Crear(_mapper.Map<Categoria>(modelo));
                modelo = _mapper.Map<VMCategoria>(categotyCreated);
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
        public async Task<IActionResult> Editar([FromBody] VMCategoria modelo)
        {
            GenericResponse<VMCategoria> gResponse = new GenericResponse<VMCategoria>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                   .Select(c => c.Value)
                                                   .SingleOrDefault();
                modelo.IdUsuario = int.Parse(idUsuario);
                Categoria categoryEdited = await _categoriaServicio.Editar(_mapper.Map<Categoria>(modelo));
                modelo = _mapper.Map<VMCategoria>(categoryEdited);
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
