﻿using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Interfaces
{
    public interface IMenuService
    {
        Task<List<Menu>> ObtenerMenu(int idUsuario);
    }
}
