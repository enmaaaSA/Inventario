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
    public class ProveedorService : IProveedorService
    {
        private readonly IGenericRepository<Proveedor> _repositorio;
        public ProveedorService(IGenericRepository<Proveedor> repositorio)
        {
            _repositorio = repositorio;
        }

        
        public async Task<List<Proveedor>> Lista()
        {
            IQueryable<Proveedor> query = await _repositorio.Consultar();
            return query.Include(u => u.IdUsuarioNavigation).ToList();
        }

        public async Task<Proveedor> Crear(Proveedor entidad)
        {
            Proveedor supplierExists = await _repositorio.Obtener(s => s.NumeroDocumento == entidad.NumeroDocumento && s.Estado == true);
            if (supplierExists != null)
                throw new TaskCanceledException("El proveedor ya existe.");
            try
            {
                Proveedor supplierCreated = await _repositorio.Crear(entidad);
                if (supplierCreated.IdProveedor == 0)
                    throw new TaskCanceledException("No se puede crear el proveedor.");
                IQueryable<Proveedor> query = await _repositorio.Consultar(s => s.IdProveedor == supplierCreated.IdProveedor);
                supplierCreated = query.Include(u => u.IdUsuarioNavigation).First();
                return supplierCreated;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Proveedor> Editar(Proveedor entidad)
        {
            Proveedor supplierExists = await _repositorio.Obtener(s => s.IdProveedor != entidad.IdProveedor &&
                                                                       s.NumeroDocumento == entidad.NumeroDocumento &&
                                                                       s.Estado == true);
            if (supplierExists != null)
                throw new TaskCanceledException("El proveedor ya existe.");
            try
            {
                IQueryable<Proveedor> querySupplier = await _repositorio.Consultar(s => s.IdProveedor == entidad.IdProveedor);
                Proveedor supplierOriginal = querySupplier.First();
                Proveedor supplierNew = new Proveedor
                {
                    TipoDocumento = entidad.TipoDocumento,
                    NumeroDocumento = entidad.NumeroDocumento,
                    Nombre = entidad.Nombre,
                    Correo = entidad.Correo,
                    Direccion = entidad.Direccion,
                    IdUsuario = entidad.IdUsuario,
                    Estado = entidad.Estado
                };
                Proveedor supplierCreate = await _repositorio.Crear(supplierNew);
                if (supplierCreate == null)
                    throw new TaskCanceledException("No se puede actualizar el proveedor.");
                supplierOriginal.Estado = false;
                bool statusFalse = await _repositorio.Editar(supplierOriginal);
                Proveedor supplierCreated = querySupplier.Include(u => u.IdUsuarioNavigation).First();
                return supplierCreated;
            }
            catch
            {
                throw;
            }
        }
    }
}
