using Microsoft.EntityFrameworkCore;
using SistemaInventario.BLL.Interfaces;
using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Implementaciones
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _repositorio;
        private readonly ICorreoService _correoServicio;
        private readonly IUtilidadesService _utilidadesServicio;
        public UsuarioService(IGenericRepository<Usuario> repositorio,
            ICorreoService correoServicio,
            IUtilidadesService utilidadesServicio)
        {
            _repositorio = repositorio;
            _correoServicio = correoServicio;
            _utilidadesServicio = utilidadesServicio;
        }


        public async Task<List<Usuario>> Lista()
        {
            IQueryable<Usuario> query = await _repositorio.Consultar();
            return query.Include(r => r.IdRolNavigation).ToList();
        }

        public async Task<Usuario> Crear(Usuario entidad, string urlPlantillaCorreo)
        {
            if (!_utilidadesServicio.CorreoValido(entidad.Correo))
                throw new InvalidOperationException("El formato del correo electrónico no es válido.");

            Usuario userExitsEmail = await _repositorio.Obtener(u => u.Correo == entidad.Correo && u.Estado == true);
            Usuario userExitsNumber = await _repositorio.Obtener(u => u.NumeroDocumento == entidad.NumeroDocumento && u.Estado == true);

            if (userExitsEmail != null)
                throw new TaskCanceledException("El correo ya existe");
            if (userExitsNumber != null)
                throw new TaskCanceledException("El numero de documento ya existe");

            try
            {
                string passwordGenerated = _utilidadesServicio.GenerarClave();
                entidad.Clave = _utilidadesServicio.ConvertirSha256(passwordGenerated);

                Usuario userCreated = await _repositorio.Crear(entidad);
                if (userCreated.IdUsuario == 0)
                    throw new TaskCanceledException("No se pudo crear el Usuario");
                if (urlPlantillaCorreo != "")
                {
                    urlPlantillaCorreo = urlPlantillaCorreo.Replace("[correo]", userCreated.Correo).Replace("[clave]", passwordGenerated);
                    string htmlEmail = "";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlPlantillaCorreo);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            StreamReader readerStream = null;
                            if(response.CharacterSet == null)
                                readerStream = new StreamReader(dataStream);
                            else
                                readerStream = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));
                            htmlEmail = readerStream.ReadToEnd();
                            response.Close();
                            readerStream.Close();
                        }
                    }
                    if (htmlEmail != "")
                        await _correoServicio.EnviarCorreo(userCreated.Correo, "Cuenta creada", htmlEmail);
                }
                IQueryable<Usuario> query = await _repositorio.Consultar(u => u.IdUsuario == userCreated.IdUsuario);
                userCreated = query.Include(r => r.IdRolNavigation).First();
                return userCreated;
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public async Task<Usuario> Editar(Usuario entidad)
        {
            Usuario userExits = await _repositorio.Obtener(u => u.Correo == entidad.Correo && u.NumeroDocumento == entidad.NumeroDocumento && u.IdUsuario != entidad.IdUsuario && u.Estado == true);
            if (userExits != null)
                throw new InvalidOperationException("El correo y/o numero de documento no existe.");

            try
            {
                IQueryable<Usuario> queryUser = await _repositorio.Consultar(p => p.IdUsuario == entidad.IdUsuario);
                Usuario userOriginal = queryUser.First();

                Usuario userNew = new Usuario
                {
                    Nombre = entidad.Nombre,
                    Apellido = entidad.Apellido,
                    Documento = entidad.Documento,
                    NumeroDocumento = entidad.NumeroDocumento,
                    Correo = entidad.Correo,
                    IdRol = entidad.IdRol,
                    Clave = userOriginal.Clave,
                    Estado = entidad.Estado,
                };

                Usuario userCreate = await _repositorio.Crear(userNew);
                if (userCreate == null)
                    throw new TaskCanceledException("No se pudo editar el usuario.");

                userOriginal.Estado = false;
                bool statusFalse = await _repositorio.Editar(userOriginal);

                Usuario userCreated = queryUser.Include(r => r.IdRolNavigation).First();
                return userCreated;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<bool> Eliminar(int idUsuario)
        {
            throw new NotImplementedException();
        }

        public async Task<Usuario> ObtenerCredenciales(string correo, string clave)
        {
            string passwordEncrypted = _utilidadesServicio.ConvertirSha256(clave);
            Usuario userFound = await _repositorio.Obtener(u => u.Correo.Equals(correo) && u.Clave.Equals(passwordEncrypted));
            return userFound;
        }

        public async Task<Usuario> ObtenerId(int idUsuario)
        {
            IQueryable<Usuario> query = await _repositorio.Consultar(u => u.IdUsuario ==  idUsuario);
            Usuario result = query.Include(r => r.IdRolNavigation).FirstOrDefault();
            return result;
        }

        public async Task<bool> GuardarPefil(Usuario entidad)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CambiarClave(int idUsuario, string claveActual, string claveNueva)
        {
            try
            {
                Usuario userFound = await _repositorio.Obtener(u => u.IdUsuario == idUsuario);
                if (userFound == null)
                    throw new TaskCanceledException("El usuario no existe");
                if (userFound.Clave != _utilidadesServicio.ConvertirSha256(claveActual))
                    throw new TaskCanceledException("La contraseña ingresada como actual no es correcta");

                string pwNew = _utilidadesServicio.ConvertirSha256(claveNueva);

                IQueryable<Usuario> queryUser = await _repositorio.Consultar(p => p.IdUsuario == idUsuario);
                Usuario userOriginal = queryUser.First();

                Usuario userNew = new Usuario
                {
                    Nombre = userOriginal.Nombre,
                    Apellido = userOriginal.Apellido,
                    Documento = userOriginal.Documento,
                    NumeroDocumento = userOriginal.NumeroDocumento,
                    Correo = userOriginal.Correo,
                    IdRol = userOriginal.IdRol,
                    Clave = pwNew,
                    Estado = userOriginal.Estado,
                };

                Usuario userCreate = await _repositorio.Crear(userNew);
                if (userCreate == null)
                    throw new TaskCanceledException("No se pudo cambiar la contraseña.");

                userOriginal.Estado = false;
                bool statusFalse = await _repositorio.Editar(userOriginal);
                return statusFalse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> RestablecerClave(string correo, string urlPlantillaCorreo)
        {
            Usuario userFound = await _repositorio.Obtener(u => u.Correo == correo && u.Estado == true);
            if (userFound == null)
                throw new TaskCanceledException("No encontramos ningun usuario con este correo y/o esta inactivo.");
            try
            {
                IQueryable<Usuario> query = await _repositorio.Consultar(u => u.Correo == correo && u.Estado == true);
                Usuario userOriginal = query.First();

                string passwordGenerated = _utilidadesServicio.GenerarClave();
                string passwordHash = _utilidadesServicio.ConvertirSha256(passwordGenerated);

                Usuario userNew = new Usuario
                {
                    Nombre = userOriginal.Nombre,
                    Apellido = userOriginal.Apellido,
                    Documento = userOriginal.Documento,
                    NumeroDocumento = userOriginal.NumeroDocumento,
                    Correo = userOriginal.Correo,
                    IdRol = userOriginal.IdRol,
                    Clave = passwordHash,
                    Estado = userOriginal.Estado,
                };

                Usuario userCreated = await _repositorio.Crear(userNew);
                if (userCreated.IdUsuario == 0)
                    throw new TaskCanceledException("No se pudo crear el Usuario");

                urlPlantillaCorreo = urlPlantillaCorreo.Replace("[clave]", passwordGenerated);
                string htmlEmail = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlPlantillaCorreo);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader readerStream = null;
                        if (response.CharacterSet == null)
                            readerStream = new StreamReader(dataStream);
                        else
                            readerStream = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));
                        htmlEmail = readerStream.ReadToEnd();
                        response.Close();
                        readerStream.Close();
                    }
                }

                bool emailSent = false;
                if (htmlEmail != "")
                    emailSent = await _correoServicio.EnviarCorreo(correo, "Contraseña Restablecida", htmlEmail);
                if (!emailSent)
                    throw new TaskCanceledException("Tenemos problemas. Por favor inténtalo de nuevo más tarde");

                userOriginal.Estado = false;
                bool result = await _repositorio.Editar(userOriginal);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Usuario> ObtenerUsuarioActivoPorId(int idUsuario)
        {
            IQueryable<Usuario> query = await _repositorio.Consultar(u => u.IdUsuario == idUsuario && u.Estado == true);
            Usuario result = query.Include(r => r.IdRolNavigation).FirstOrDefault();
            return result;
        }
    }
}
