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
    public class TipoDocumentoCompraService : ITipoDocumentoCompraService
    {
        private readonly IGenericRepository<TipoDocumentoCompra> _repositorio;
        public TipoDocumentoCompraService(IGenericRepository<TipoDocumentoCompra> repositorio)
        {
            _repositorio = repositorio;
        }


        public async Task<List<TipoDocumentoCompra>> Lista()
        {
            IQueryable<TipoDocumentoCompra> query = await _repositorio.Consultar();
            return query.ToList();
        }
    }
}
