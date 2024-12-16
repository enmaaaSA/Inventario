using SistemaInventario.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Implementaciones
{
    public class DashboardService : IDashboardService
    {
        public Task<Dictionary<string, int>> ProductosTopUltimaSemana()
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalCategorias()
        {
            throw new NotImplementedException();
        }

        public Task<string> TotalIngresosUltimaSemana()
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalProductos()
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalVentasUltimaSemana()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, int>> VentasUltimaSemana()
        {
            throw new NotImplementedException();
        }
    }
}
