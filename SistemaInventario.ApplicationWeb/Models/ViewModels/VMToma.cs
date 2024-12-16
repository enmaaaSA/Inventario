using SistemaInventario.Entity;

namespace SistemaInventario.ApplicationWeb.Models.ViewModels
{
    public class VMToma
    {
        public int IdTomaInventario { get; set; }

        public int IdProducto { get; set; }

        public int IdUsuario { get; set; }

        public int StockAnterior { get; set; }

        public int StockNuevo { get; set; }

        public string DocumentoEncargado { get; set; }

        public string NumeroDocumentoEncargado { get; set; }

        public string NombreEncargado { get; set; }

        public string Razon { get; set; }

        public DateOnly FechaToma { get; set; }
    }
}
