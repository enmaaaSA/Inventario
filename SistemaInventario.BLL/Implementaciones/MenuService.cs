﻿using SistemaInventario.BLL.Interfaces;
using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Implementaciones
{
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository<Menu> _repositorioMenu;
        private readonly IGenericRepository<RolMenu> _repositorioRolMenu;
        private readonly IGenericRepository<Usuario> _repositorioUsuario;
        public MenuService(IGenericRepository<Menu> repositorioMenu,
            IGenericRepository<RolMenu> repositorioRolMenu,
            IGenericRepository<Usuario> repositorioUsuario)
        {
            _repositorioMenu = repositorioMenu;
            _repositorioRolMenu = repositorioRolMenu;
            _repositorioUsuario = repositorioUsuario;
        }


        public async Task<List<Menu>> ObtenerMenu(int idUsuario)
        {
            IQueryable<Usuario> tbUsuario = await _repositorioUsuario.Consultar(u => u.IdUsuario == idUsuario);
            IQueryable<RolMenu> tbRolMenu = await _repositorioRolMenu.Consultar();
            IQueryable<Menu> tbMenu = await _repositorioMenu.Consultar();

            IQueryable<Menu> MenuPadre = (from u in tbUsuario
                                          join rm in tbRolMenu on u.IdRol equals rm.IdRol
                                          join m in tbMenu on rm.IdMenu equals m.IdMenu
                                          join mpadre in tbMenu on m.IdMenuPadre equals mpadre.IdMenu
                                          select mpadre).Distinct().AsQueryable();

            IQueryable<Menu> MenuHijos = (from u in tbUsuario
                                          join rm in tbRolMenu on u.IdRol equals rm.IdRol
                                          join m in tbMenu on rm.IdMenu equals m.IdMenu
                                          where m.IdMenu != m.IdMenuPadre
                                          select m).Distinct().AsQueryable();

            List<Menu> listaMenu = (from mpadre in MenuPadre
                                    select new Menu()
                                    {
                                        Nombre = mpadre.Nombre,
                                        Icono = mpadre.Icono,
                                        Controlador = mpadre.Controlador,
                                        PagAccion = mpadre.PagAccion,
                                        InverseIdMenuPadreNavigation = (from mhijo in MenuHijos
                                                                        where mhijo.IdMenuPadre == mpadre.IdMenu
                                                                        select mhijo).ToList()
                                    }).ToList();

            return listaMenu;
        }
    }
}
