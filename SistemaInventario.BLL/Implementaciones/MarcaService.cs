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
    public class MarcaService : IMarcaService
    {
        private readonly IGenericRepository<Marca> _repositorio;
        public MarcaService(IGenericRepository<Marca> repositorio)
        {
            _repositorio = repositorio;
        }


        public async Task<List<Marca>> Lista()
        {
            IQueryable<Marca> query = await _repositorio.Consultar();
            return query.Include(u => u.IdUsuarioNavigation)
                .ToList();
        }

        public async Task<Marca> Crear(Marca entidad)
        {
            Marca brandExists = await _repositorio.Obtener(b => b.Nombre == entidad.Nombre && b.Estado == true);
            if (brandExists != null)
                throw new TaskCanceledException("La marca ya existe.");

            try
            {
                Marca brandCreated = await _repositorio.Crear(entidad);
                if (brandCreated.IdMarca == 0)
                    throw new TaskCanceledException("No se puede crear la marca.");
                IQueryable<Marca> query = await _repositorio.Consultar(b => b.IdMarca == brandCreated.IdMarca);
                brandCreated = query.Include(u => u.IdUsuarioNavigation).First();
                return brandCreated;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Marca> Editar(Marca entidad)
        {
            Marca brandExists = await _repositorio.Obtener(c => c.IdMarca != entidad.IdMarca &&
                                                                c.Nombre == entidad.Nombre &&
                                                                c.Estado == true);
            if (brandExists != null)
                throw new TaskCanceledException("La marca ya existe.");
            try
            {
                IQueryable<Marca> queryBrand = await _repositorio.Consultar(c => c.IdMarca == entidad.IdMarca);
                Marca brandOriginal = queryBrand.First();
                Marca brandNew = new Marca
                {
                    Nombre = entidad.Nombre,
                    IdUsuario = entidad.IdUsuario,
                    Estado = entidad.Estado,
                };

                Marca brandCreate = await _repositorio.Crear(brandNew);
                if (brandCreate == null)
                    throw new TaskCanceledException("No se pudo actualizar la marca.");

                brandOriginal.Estado = false;
                bool statusFalse = await _repositorio.Editar(brandOriginal);
                Marca brandCreated = queryBrand.Include(u => u.IdUsuarioNavigation).First();

                return brandCreated;
            }
            catch
            {
                throw;
            }
        }

        public Task<bool> Eliminar(int idMarca)
        {
            throw new NotImplementedException();
        }
    }
}
