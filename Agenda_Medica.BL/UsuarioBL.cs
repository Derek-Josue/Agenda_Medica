using System;
using System.Security.Cryptography;
using System.Text;
using Agenda_Medica.DAL;
using Agenda_Medica.Entities;

namespace Agenda_Medica.BL
{
    public class UsuarioBL
    {
        public static Usuario UsuarioActual;

        public bool Login(string usuario, string clave)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
                return false;

            string hash;
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(clave));
                hash = BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }

            UsuarioActual = new UsuarioDAL().Validar(usuario.Trim(), hash);
            return UsuarioActual != null;
        }
    }
}
