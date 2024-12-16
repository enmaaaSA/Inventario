using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaInventario.ApplicationWeb.Models.ViewModels;
using SistemaInventario.ApplicationWeb.Utilidades.Response;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.Entity;

namespace SistemaInventario.ApplicationWeb.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioServicio;
        private readonly IRolService _rolServicio;
        private readonly IMapper _mapper;
        public UsuarioController(IUsuarioService usuarioServicio,
            IRolService rolServicio,
            IMapper mapper)
        {
            _usuarioServicio = usuarioServicio;
            _rolServicio = rolServicio;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaRol()
        {
            List<VMRol> vmListRol = _mapper.Map<List<VMRol>>(await _rolServicio.Lista());
            return StatusCode(StatusCodes.Status200OK, vmListRol);
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMUsuario> vmListUser = _mapper.Map<List<VMUsuario>>(await _usuarioServicio.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = vmListUser });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] string modelo)
        {
            GenericResponse<VMUsuario> gResponse = new GenericResponse<VMUsuario>();

            try
            {
                VMUsuario vmUser = JsonConvert.DeserializeObject<VMUsuario>(modelo);
                string urlPlantillaCorreo = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/EnviarClave?correo=[correo]&clave=[clave]";
                Usuario userCreated = await _usuarioServicio.Crear(_mapper.Map<Usuario>(vmUser), urlPlantillaCorreo);
                vmUser = _mapper.Map<VMUsuario>(userCreated);
                gResponse.Estado = true;
                gResponse.Objeto = vmUser;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromForm] string modelo)
        {
            GenericResponse<VMUsuario> gResponse = new GenericResponse<VMUsuario>();

            try
            {
                VMUsuario vmUser = JsonConvert.DeserializeObject<VMUsuario>(modelo);
                Usuario userEdited = await _usuarioServicio.Editar(_mapper.Map<Usuario>(vmUser));

                vmUser = _mapper.Map<VMUsuario>(userEdited);

                gResponse.Estado = true;
                gResponse.Objeto = vmUser;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int idUsuario)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Estado = await _usuarioServicio.Eliminar(idUsuario);
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpGet]
        public async Task<IActionResult> ReporteExcel()
        {
            List<VMUsuario> users = _mapper.Map<List<VMUsuario>>(await _usuarioServicio.Lista());
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Reporte Productos");
                worksheet.Cell(1, 1).Value = "IdUsuario";
                worksheet.Cell(1, 2).Value = "Tipo de Documento";
                worksheet.Cell(1, 3).Value = "Número de Documento";
                worksheet.Cell(1, 4).Value = "Nombre";
                worksheet.Cell(1, 5).Value = "Apellido";
                worksheet.Cell(1, 6).Value = "Correo Electronico";
                worksheet.Cell(1, 7).Value = "Rol";
                worksheet.Cell(1, 8).Value = "Estado";
                worksheet.Cell(1, 9).Value = "Fecha de Registro";
                worksheet.Row(1).Style.Font.Bold = true;
                for (int i = 0; i < users.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = users[i].IdUsuario;
                    worksheet.Cell(i + 2, 2).Value = users[i].Documento;
                    worksheet.Cell(i + 2, 3).Value = users[i].NumeroDocumento;
                    worksheet.Cell(i + 2, 4).Value = users[i].Nombre;
                    worksheet.Cell(i + 2, 5).Value = users[i].Apellido;
                    worksheet.Cell(i + 2, 6).Value = users[i].Correo;
                    worksheet.Cell(i + 2, 7).Value = users[i].Rol;
                    worksheet.Cell(i + 2, 8).Value = users[i].Estado == 1 ? "Activo" : "Inactivo";
                    worksheet.Cell(i + 2, 9).Value = users[i].Fecha;
                }
                worksheet.Range(worksheet.FirstCellUsed(), worksheet.LastCellUsed()).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(worksheet.FirstCellUsed(), worksheet.LastCellUsed()).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    var fileName = "Reporte Usuarios_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
