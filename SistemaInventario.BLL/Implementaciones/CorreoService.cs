using SistemaInventario.BLL.Interfaces;
using SistemaInventario.DAL.Interfaces;
using SistemaInventario.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Implementaciones
{
    public class CorreoService : ICorreoService
    {
        private readonly IGenericRepository<Configuracion> _repositorio;
        public CorreoService(IGenericRepository<Configuracion> repositorio)
        {
            _repositorio = repositorio;
        }


        public async Task<bool> EnviarCorreo(string correoDestino, string asunto, string mensaje)
        {
            try
            {
                IQueryable<Configuracion> query = await _repositorio.Consultar(c => c.Recurso.Equals("Servicio_Correo"));
                Dictionary<string, string> config = query.ToDictionary(keySelector: c => c.Propiedad, elementSelector: c => c.Valor);
                var credentials = new NetworkCredential(config["correo"], config["clave"]);
                var email = new MailMessage()
                {
                    From = new MailAddress(config["correo"], config["alias"]),
                    Subject = asunto,
                    Body = mensaje,
                    IsBodyHtml = true
                };
                email.To.Add(new MailAddress(correoDestino));
                var clientServer = new SmtpClient()
                {
                    Host = config["host"],
                    Port = int.Parse(config["puerto"]),
                    Credentials = credentials,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true
                };
                clientServer.Send(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
