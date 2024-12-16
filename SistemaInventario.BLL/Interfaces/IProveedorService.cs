using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Interfaces
{
    public interface IProveedorService
    {
        Task<List<Proveedor>> Lista();
        Task<Proveedor> Crear(Proveedor entidad);
        Task<Proveedor> Editar(Proveedor entidad);
    }
}
