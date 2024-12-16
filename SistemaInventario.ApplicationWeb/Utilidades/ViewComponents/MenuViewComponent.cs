using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.ApplicationWeb.Models.ViewModels;
using SistemaInventario.BLL.Interfaces;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace SistemaInventario.ApplicationWeb.Utilidades.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuService _menuServicio;
        private readonly IMapper _mapper;
        public MenuViewComponent(IMenuService menuServicio,
            IMapper mapper)
        {
            _menuServicio = menuServicio;
            _mapper = mapper;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            List<VMMenu> listaMenu;
            if (claimUser.Identity.IsAuthenticated)
            {
                string idUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();
                listaMenu = _mapper.Map<List<VMMenu>>(await _menuServicio.ObtenerMenu(int.Parse(idUsuario)));
            }
            else
                listaMenu = new List<VMMenu> { };
            return View(listaMenu);
        }
    }
}
