using SistemaInventario.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SistemaInventario.BLL.Implementaciones
{
    public class UtilidadesService : IUtilidadesService
    {
        private static Dictionary<string, int> _contadorDiario = new Dictionary<string, int>();
        private static readonly object _lock = new object();


        public string GenerarClave()
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] clave = new char[12];
            for (int i = 0; i < clave.Length; i++)
            {
                clave[i] = characters[random.Next(characters.Length)];
            }
            return new string(clave);
        }

        public string ConvertirSha256(string texto)
        {
            using (SHA256 hash256 = SHA256.Create())
            {
                byte[] bytes = hash256.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder result = new StringBuilder();
                foreach (byte b in bytes)
                {
                    result.Append(b.ToString("x2"));
                }
                return result.ToString();
            }
        }

        public string RamdomString(int length)
        {
            const string characters = "ABCDEFGHJKLMNPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] generated = new char[length];
            for (int i = 0; i < generated.Length; i++)
                generated[i] = characters[random.Next(characters.Length)];
            return new string(generated);
        }

        public bool CorreoValido(string correo)
        {
            string patronCorreo = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(correo, patronCorreo);
        }

        public string GenerarNumeroVenta()
        {
            string fecha = DateTime.Now.ToString("yyyyMMdd");
            int contador;
            lock (_lock)
            {
                if (!_contadorDiario.ContainsKey(fecha))
                    _contadorDiario[fecha] = 0;
                _contadorDiario[fecha]++;
                contador = _contadorDiario[fecha];
            }
            return $"{fecha}-{contador:D4}"; // Ejemplo: 20241125-0001
        }
    }
}
