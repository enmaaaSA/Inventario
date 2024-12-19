using Microsoft.EntityFrameworkCore;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.DAL.Implementaciones;
using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Implementaciones
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<Categoria> _repositorio;
        public CategoriaService(IGenericRepository<Categoria> repositorio)
        {
            _repositorio = repositorio;
        }


        public async Task<List<Categoria>> Lista()
        {
            IQueryable<Categoria> query = await _repositorio.Consultar();
            return query.Include(u => u.IdUsuarioNavigation)
                .ToList();
        }

        public async Task<Categoria> Crear(Categoria entidad)
        {
            Categoria categoryExists = await _repositorio.Obtener(c => c.Nombre == entidad.Nombre && c.Estado == true);
            if (categoryExists != null)
                throw new TaskCanceledException("La categoria ya existe.");

            try
            {
                Categoria categoryCreated = await _repositorio.Crear(entidad);
                if (categoryCreated.IdCategoria == 0)
                    throw new TaskCanceledException("No se puede crear la categoria.");
                IQueryable<Categoria> query = await _repositorio.Consultar(c => c.IdCategoria == categoryCreated.IdCategoria);
                categoryCreated = query.Include(u => u.IdUsuarioNavigation).First();
                return categoryCreated;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Categoria> Editar(Categoria entidad)
        {
            Categoria categoryExists = await _repositorio.Obtener(c => c.IdCategoria != entidad.IdCategoria &&
                                                                       c.Nombre == entidad.Nombre &&
                                                                       c.Estado == true);
            if (categoryExists != null)
                throw new TaskCanceledException("La categoria ya existe.");

            try
            {
                IQueryable<Categoria> queryCategory = await _repositorio.Consultar(c => c.IdCategoria == entidad.IdCategoria);
                Categoria categoryOriginal = queryCategory.First();

                Categoria categoryNew = new Categoria
                {
                    Nombre = entidad.Nombre,
                    IdUsuario = entidad.IdUsuario,
                    Estado = entidad.Estado,
                };

                Categoria categoryCreate = await _repositorio.Crear(categoryNew);
                if (categoryCreate == null)
                    throw new TaskCanceledException("No se pudo actualizar la categoria.");

                categoryOriginal.Estado = false;
                bool statusFalse = await _repositorio.Editar(categoryOriginal);
                Categoria categoryCreated = queryCategory.Include(u => u.IdUsuarioNavigation).First();

                return categoryCreated;
            }
            catch
            {
                throw;
            }
        }

        public Task<bool> Eliminar(int idCategoria)
        {
            throw new NotImplementedException();
        }
    }
}
