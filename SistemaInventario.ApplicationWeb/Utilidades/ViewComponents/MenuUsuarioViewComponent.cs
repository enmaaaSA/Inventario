using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SistemaInventario.ApplicationWeb.Utilidades.ViewComponents
{
    public class MenuUsuarioViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            string userName = "";
            string lastName = "";
            if (claimUser.Identity.IsAuthenticated)
            {
                userName = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                           .Select(c => c.Value)
                                           .SingleOrDefault();
                lastName = claimUser.Claims.Where(c => c.Type == "Last")
                                           .Select(c => c.Value)
                                           .SingleOrDefault();
            }
            ViewData["nombreUsuario"] = $"{userName} {lastName}";
            return View();
        }
    }
}
