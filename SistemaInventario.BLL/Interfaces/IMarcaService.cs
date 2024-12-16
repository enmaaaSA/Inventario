using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Interfaces
{
    public interface IMarcaService
    {
        Task<List<Marca>> Lista();
        Task<Marca> Crear(Marca entidad);
        Task<Marca> Editar(Marca entidad);
        Task<bool> Eliminar(int idMarca);
    }
}
