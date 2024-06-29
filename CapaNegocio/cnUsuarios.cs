using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class cnUsuarios
    {
        private cUsuarios objCapaDato = new cUsuarios();

        public List<ceUsuario> Listar() {
            return objCapaDato.Listar();
        }

        public int Registrar(ceUsuario obj, out string Mensaje) {

            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres)) {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {

                string clave = cnRecursos.GenerarClave();

                string asunto = "Creacion de Cuenta";
                string mensaje_correo = "<h3>Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", clave);


                bool respuesta = cnRecursos.EnviarCorreo(obj.Correo, asunto, mensaje_correo);

                if (respuesta)
                {

                    obj.Clave = cnRecursos.ConvertirSha256(clave);
                    return objCapaDato.Registrar(obj, out Mensaje);
                }
                else {
                    Mensaje = "No se puede enviar el correo";
                    return 0;
                }


            }
            else {

                return 0;
            }


            
        }

        public bool Editar(ceUsuario obj, out string Mensaje) {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Editar(obj, out Mensaje);
            }
            else {
                return false;
            }
        }


        public bool Eliminar(int id, out string Mensaje) {

            return objCapaDato.Eliminar(id, out Mensaje);
        }


        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {

            return objCapaDato.CambiarClave(idusuario,nuevaclave, out Mensaje);
        }


        public bool ReestablecerClave(int idusuario,string correo, out string Mensaje)
        {

            Mensaje = string.Empty;
            string nuevaclave = cnRecursos.GenerarClave();
            bool resultado = objCapaDato.ReestablecerClave(idusuario,cnRecursos.ConvertirSha256(nuevaclave), out Mensaje);

            if (resultado)
            {

                string asunto = "Contraseña Reestablecida";
                string mensaje_correo = "<h3>Su cuenta fue reestablecida correctamente</h3></br><p>Su contraseña para acceder ahora es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaclave);


                bool respuesta = cnRecursos.EnviarCorreo(correo, asunto, mensaje_correo);

                if (respuesta)
                {

                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return false;
                }

            }
            else {
                Mensaje = "No se pudo reestablecer la contraseña";

                return false;
            }


        }
    }
}
