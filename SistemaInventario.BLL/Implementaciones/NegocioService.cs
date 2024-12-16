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
    public class NegocioService : INegocioService
    {
        private readonly IGenericRepository<Negocio> _repositorio;
        private readonly ICorreoService _correoServicio;
        public NegocioService(IGenericRepository<Negocio> repositorio,
            ICorreoService correoServicio)
        {
            _repositorio = repositorio;
            _correoServicio = correoServicio;
        }


        public async Task<Negocio> Obtener()
        {
            try
            {
                Negocio businessFound = await _repositorio.Obtener(n => n.Estado == true);
                return businessFound;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Negocio> Editar(Negocio entidad)
        {
            try
            {
                IQueryable<Negocio> query = await _repositorio.Consultar(n => n.Estado == true);
                Negocio businessOriginal = query.First();

                Negocio businessNew = new Negocio
                {
                    Documento = entidad.Documento,
                    NumeroDocumento = entidad.NumeroDocumento,
                    Nombre = entidad.Nombre,
                    Correo = entidad.Correo,
                    Direccion = entidad.Direccion,
                    Impuesto = entidad.Impuesto,
                    SimboloMoneda = entidad.SimboloMoneda,
                    IdUsuario = businessOriginal.IdUsuario,
                    Estado = true,
                };

                Negocio businessCreated = await _repositorio.Crear(businessNew);
                if (businessNew == null)
                    throw new TaskCanceledException("No se pudo editar el Negocio.");

                businessOriginal.Estado = false;
                bool statusFalse = await _repositorio.Editar(businessOriginal);
                return businessCreated;
            }
            catch 
            {
                throw;
            }
        }
    }
}
