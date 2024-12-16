using Microsoft.EntityFrameworkCore;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Implementaciones
{
    public class DevolucionService : IDevolucionService
    {
        private readonly IGenericRepository<Venta> _repositorioVenta;
        private readonly IDevolucionRepository _devolucionRepositorio;
        public DevolucionService(IGenericRepository<Venta> repositorioVenta, IDevolucionRepository devolucionRepositorio)
        {
            _repositorioVenta = repositorioVenta;
            _devolucionRepositorio = devolucionRepositorio;
        }


        public async Task<Venta> ObtenerVentaPorNumero(string numeroVenta)
        {
            return await _devolucionRepositorio.ObtenerVentaPorNumero(numeroVenta);
        }

        public async Task<bool> RegistrarDevolucion(string numeroVenta, string motivo, int idUsuario)
        {
            var venta = await _devolucionRepositorio.ObtenerVentaPorNumero(numeroVenta);
            if (venta == null) throw new Exception("La venta no existe o ya fue devuelta.");

            var devolucion = new Devolucion
            {
                IdVenta = venta.IdVenta,
                NumeroVenta = venta.NumeroVenta,
                IdUsuario = idUsuario,
                Motivo = motivo,
                MontoDevuelto = venta.Total,
            };

            var detalles = venta.DetalleVenta.Select(d => new DetalleDevolucion
            {
                IdProducto = d.IdProducto,
                Cantidad = d.Cantidad
            }).ToList();

            return await _devolucionRepositorio.Registrar(devolucion, detalles);
        }

    }
}
