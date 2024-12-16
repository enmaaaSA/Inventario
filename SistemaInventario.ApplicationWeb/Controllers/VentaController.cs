using AutoMapper;
using ClosedXML.Excel;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.ApplicationWeb.Models.ViewModels;
using SistemaInventario.ApplicationWeb.Utilidades.Response;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System;
using System.Security.Claims;

namespace SistemaInventario.ApplicationWeb.Controllers
{
    [Authorize]
    public class VentaController : Controller
    {
        private readonly ITipoDocumentoVentaService _tipoDocumentoVentaServicio;
        private readonly IVentaService _ventaServicio;
        private readonly IMapper _mapper;
        private readonly IConverter _converter;
        public VentaController(ITipoDocumentoVentaService tipoDocumentoVentaServicio,
            IVentaService ventaServicio,
            IConverter converter,
            IMapper mapper)
        {
            _tipoDocumentoVentaServicio = tipoDocumentoVentaServicio;
            _ventaServicio = ventaServicio;
            _converter = converter;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            return View();
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
        public async Task<IActionResult> ListaTipoDocumentoVenta()
        {
            List<VMTipoDocumentoVenta> vmListaTipoDocumentos = _mapper.Map<List<VMTipoDocumentoVenta>>(await _tipoDocumentoVentaServicio.Lista());
            return StatusCode(StatusCodes.Status200OK, vmListaTipoDocumentos);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductos(string busqueda)
        {
            List<VMProducto> vmListProducts = _mapper.Map<List<VMProducto>>(await _ventaServicio.ObtenerProductos(busqueda));
            return StatusCode(StatusCodes.Status200OK, vmListProducts);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarVenta([FromBody] VMVenta modelo)
        {
            GenericResponse<VMVenta> gResponse = new GenericResponse<VMVenta>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                                   .Select(c => c.Value)
                                                   .SingleOrDefault();
                modelo.IdUsuario = int.Parse(idUsuario);
                Venta ventaGenerated = await _ventaServicio.Registrar(_mapper.Map<Venta>(modelo));
                modelo = _mapper.Map<VMVenta>(ventaGenerated);
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
        public async Task<IActionResult> HistorialVenta(string numeroVenta, string fechaInicio, string fechaFin)
        {
            List<VMVenta> vmHistorialVenta = _mapper.Map<List<VMVenta>>(await _ventaServicio.Historial(numeroVenta, fechaInicio, fechaFin));
            return StatusCode(StatusCodes.Status200OK, vmHistorialVenta);
        }

        public IActionResult MostrarPDFCompra(string numeroCompra)
        {
            string urlPlantillaVista = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/PDFCompra?numeroCompra={numeroCompra}";
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                },
                Objects = {
                    new ObjectSettings(){
                        Page = urlPlantillaVista
                    }
                }
            };
            var archivoPDF = _converter.Convert(pdf);
            return File(archivoPDF, "application/pdf");
        }

        public IActionResult MostrarPDFVenta(string numeroVenta)
        {
            string urlPlantillaVista = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/PDFVenta?numeroVenta={numeroVenta}";
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                },
                Objects = {
                    new ObjectSettings(){
                        Page = urlPlantillaVista
                    }
                }
            };
            var archivoPDF = _converter.Convert(pdf);
            return File(archivoPDF, "application/pdf");
        }

        [HttpGet]
        public async Task<IActionResult> ReporteExcel()
        {
            List<DetalleVenta> ventas = await _ventaServicio.ReporteExcel();
            List<Venta> venta = await _ventaServicio.ReporteExcelVenta();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Reporte Ventas");
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
                worksheet.Cell(1, 11).Value = "Nombre de Usuario";
                worksheet.Cell(1, 12).Value = "Apellido de Usuario";
                worksheet.Cell(1, 13).Value = "Fecha de Registro";
                worksheet.Cell(1, 14).Value = "Fecha de Registro";
                worksheet.Row(1).Style.Font.Bold = true;
                for (int i = 0; i < ventas.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = ventas[i].IdVenta;
                    worksheet.Cell(i + 2, 2).Value = ventas[i].IdVentaNavigation.NumeroVenta;
                    worksheet.Cell(i + 2, 6).Value = ventas[i].IdVentaNavigation.DocumentoCliente;
                    worksheet.Cell(i + 2, 7).Value = ventas[i].IdVentaNavigation.NombreCliente;
                    worksheet.Cell(i + 2, 8).Value = ventas[i].CodigoProducto;
                    worksheet.Cell(i + 2, 9).Value = ventas[i].NombreProducto;
                    worksheet.Cell(i + 2, 10).Value = ventas[i].Cantidad;
                    worksheet.Cell(i + 2, 11).Value = ventas[i].PrecioVenta;
                    worksheet.Cell(i + 2, 12).Value = ventas[i].IdVentaNavigation.SubTotal;
                    worksheet.Cell(i + 2, 13).Value = ventas[i].IdVentaNavigation.ImpuestoTotal;
                    worksheet.Cell(i + 2, 14).Value = ventas[i].IdVentaNavigation.Total;
                }
                for (int i = 0; i < venta.Count; i++)
                {
                    worksheet.Cell(i + 2, 3).Value = venta[i].IdTipoDocumentoVentaNavigation.Nombre;
                    worksheet.Cell(i + 2, 4).Value = venta[i].IdUsuarioNavigation.Nombre;
                    worksheet.Cell(i + 2, 5).Value = venta[i].IdUsuarioNavigation.Apellido;
                }
                worksheet.Range(worksheet.FirstCellUsed(), worksheet.LastCellUsed()).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(worksheet.FirstCellUsed(), worksheet.LastCellUsed()).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    var fileName = "Reporte Ventas_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerDetalleVenta([FromQuery] string numeroVenta)
        {
            GenericResponse<List<DetalleVenta>> gResponse = new GenericResponse<List<DetalleVenta>>();

            try
            {
                // Buscar la venta por el número de venta
                var venta = await _ventaServicio.Detalle(numeroVenta);

                if (venta == null)
                {
                    gResponse.Estado = false;
                    gResponse.Mensaje = "No se encontró una venta con el número proporcionado.";
                    return NotFound(gResponse);
                }

                // Mapear el detalle de la venta a un modelo para enviar al cliente
                List<DetalleVenta> detalleVenta = venta.DetalleVenta.Select(dv => new DetalleVenta
                {
                    IdProducto = dv.IdProducto,
                    NombreProducto = dv.IdProductoNavigation.Nombre,
                    Cantidad = dv.Cantidad,
                    PrecioVenta = dv.PrecioVenta
                }).ToList();

                gResponse.Estado = true;
                gResponse.Objeto = detalleVenta;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = $"Ocurrió un error: {ex.Message}";
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpGet]
        public async Task<IActionResult> HistorialVenta1(string numeroVenta)
        {
            List<VMVenta> vmHistorialVenta = _mapper.Map<List<VMVenta>>(await _ventaServicio.Historial1(numeroVenta));
            return StatusCode(StatusCodes.Status200OK, vmHistorialVenta);
        }

    }
}
