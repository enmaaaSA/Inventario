using Microsoft.EntityFrameworkCore;
using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.DAL.Implementaciones
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DBInventarioContext _context;
        public GenericRepository(DBInventarioContext context)
        {
            _context = context;
        }


        public async Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro)
        {
            try
            {
                TEntity entidad = await _context.Set<TEntity>().FirstOrDefaultAsync(filtro);
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<TEntity> Crear(TEntity entidad)
        {
            try
            {
                _context.Set<TEntity>().Add(entidad);
                await _context.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(TEntity entidad)
        {
            try
            {
                _context.Update(entidad);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(TEntity entidad)
        {
            try
            {
                _context.Remove(entidad);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>> filtro = null)
        {
            IQueryable<TEntity> queryEntidad = filtro == null ? _context.Set<TEntity>() : _context.Set<TEntity>().Where(filtro);
            return queryEntidad;
        }

    }
}
