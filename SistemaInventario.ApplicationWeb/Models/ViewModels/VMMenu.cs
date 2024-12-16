using SistemaInventario.Entity;

namespace SistemaInventario.ApplicationWeb.Models.ViewModels
{
    public class VMMenu
    {
        public string Nombre { get; set; }
        public string Icono { get; set; }
        public string Controlador { get; set; }
        public string PagAccion { get; set; }
        public virtual ICollection<VMMenu> SubMenu { get; set; }
    }
}
