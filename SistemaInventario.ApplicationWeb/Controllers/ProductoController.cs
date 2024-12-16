using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaInventario.ApplicationWeb.Models.ViewModels;
using SistemaInventario.ApplicationWeb.Utilidades.Response;
using SistemaInventario.BLL.Implementaciones;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.Entity;
using System.Collections.Immutable;
using System.Security.Claims;

namespace SistemaInventario.ApplicationWeb.Controllers
{
    [Authorize]
    public class ProductoController : Controller
    {
        private readonly IProductoService _productoServicio;
        private readonly ICorreoService _correoServicio;
        private readonly IMapper _mapper;
        public ProductoController(IProductoService productoServicio,
            ICorreoService correoServicio,
            IMapper mapper)
        {
            _productoServicio = productoServicio;
            _correoServicio = correoServicio;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMProducto> vmListProduct = _mapper.Map<List<VMProducto>>(await _productoServicio.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = vmListProduct });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] string modelo)
        {
            GenericResponse<VMProducto> gResponse = new GenericResponse<VMProducto>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                   .Select(c => c.Value)
                                                   .SingleOrDefault();
                VMProducto vmProduct = JsonConvert.DeserializeObject<VMProducto>(modelo);
                vmProduct.IdUsuario = int.Parse(idUsuario);
                Producto productCreated = await _productoServicio.Crear(_mapper.Map<Producto>(vmProduct));
                vmProduct = _mapper.Map<VMProducto>(productCreated);
                gResponse.Estado = true;
                gResponse.Objeto = vmProduct;
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
            GenericResponse<VMProducto> gResponse = new GenericResponse<VMProducto>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                   .Select(c => c.Value)
                                                   .SingleOrDefault();
                VMProducto vmProduct = JsonConvert.DeserializeObject<VMProducto>(modelo);
                vmProduct.IdUsuario = int.Parse(idUsuario);
                Producto productEdited = await _productoServicio.Editar(_mapper.Map<Producto>(vmProduct));
                vmProduct = _mapper.Map<VMProducto>(productEdited);
                gResponse.Estado = true;
                gResponse.Objeto = vmProduct;
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
            List<VMProducto> products = _mapper.Map<List<VMProducto>>(await _productoServicio.Lista());
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Reporte Productos");
                worksheet.Cell(1, 1).Value = "IdProducto";
                worksheet.Cell(1, 2).Value = "Codigo de Barras";
                worksheet.Cell(1, 3).Value = "Nombre";
                worksheet.Cell(1, 4).Value = "Descripción";
                worksheet.Cell(1, 5).Value = "Categoria";
                worksheet.Cell(1, 6).Value = "Marca";
                worksheet.Cell(1, 7).Value = "Stock";
                worksheet.Cell(1, 8).Value = "Precio de Compra";
                worksheet.Cell(1, 9).Value = "Precio de Venta";
                worksheet.Cell(1, 10).Value = "Estado";
                worksheet.Cell(1, 11).Value = "Usuario";
                worksheet.Cell(1, 12).Value = "Razon";
                worksheet.Cell(1, 13).Value = "Fecha de Registro";
                worksheet.Row(1).Style.Font.Bold = true;
                for (int i = 0; i < products.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = products[i].IdProducto;
                    worksheet.Cell(i + 2, 2).Value = products[i].CodigoBarra;
                    worksheet.Cell(i + 2, 3).Value = products[i].Nombre;
                    worksheet.Cell(i + 2, 4).Value = products[i].Descripcion;
                    worksheet.Cell(i + 2, 5).Value = products[i].Categoria;
                    worksheet.Cell(i + 2, 6).Value = products[i].Marca;
                    worksheet.Cell(i + 2, 7).Value = products[i].Stock;
                    worksheet.Cell(i + 2, 8).Value = products[i].PrecioCompra;
                    worksheet.Cell(i + 2, 9).Value = products[i].PrecioVenta;
                    worksheet.Cell(i + 2, 10).Value = products[i].Estado == 1 ? "Activo" : "Inactivo";
                    worksheet.Cell(i + 2, 11).Value = $"{products[i].UsuarioNombre} {products[i].UsuarioApellido}";
                    worksheet.Cell(i + 2, 12).Value = products[i].Razon;
                    worksheet.Cell(i + 2, 13).Value = products[i].Fecha;
                }
                worksheet.Range(worksheet.FirstCellUsed(), worksheet.LastCellUsed()).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(worksheet.FirstCellUsed(), worksheet.LastCellUsed()).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    var fileName = "Reporte Productos_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> TomaInventarioExcel()
        {
            List<VMProducto> products = _mapper.Map<List<VMProducto>>(await _productoServicio.Lista())
                                               .Where(p => p.Estado == 1)
                                               .ToList();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Productos");
                worksheet.Cell(1, 1).Value = "Id Producto";
                worksheet.Cell(1, 2).Value = "Codigo de Barras";
                worksheet.Cell(1, 3).Value = "Nombre";
                worksheet.Cell(1, 4).Value = "Estado";
                worksheet.Cell(1, 5).Value = "Stock Digital";
                worksheet.Cell(1, 6).Value = "Stock Fisico";
                worksheet.Row(1).Style.Font.Bold = true;
                for (int i = 0; i < products.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = products[i].IdProducto;
                    worksheet.Cell(i + 2, 2).Value = products[i].CodigoBarra;
                    worksheet.Cell(i + 2, 3).Value = products[i].Nombre;
                    worksheet.Cell(i + 2, 4).Value = products[i].Estado == 1 ? "Activo" : "Inactivo";
                    worksheet.Cell(i + 2, 5).Value = products[i].Stock;
                }
                worksheet.Range(worksheet.FirstCellUsed(), worksheet.LastCellUsed()).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(worksheet.FirstCellUsed(), worksheet.LastCellUsed()).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    var fileName = "Toma de Inventario_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPut]
        public async Task<IActionResult> Toma([FromBody] VMProducto modelo)
        {
            GenericResponse<VMProducto> gResponse = new GenericResponse<VMProducto>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                   .Select(c => c.Value)
                                                   .SingleOrDefault();
                modelo.IdUsuario = int.Parse(idUsuario);
                Producto productEdited = await _productoServicio.TomaInventario(_mapper.Map<Producto>(modelo));
                modelo = _mapper.Map<VMProducto>(productEdited);
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
