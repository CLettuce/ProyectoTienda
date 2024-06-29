using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class cnCliente
    {
        private cCliente objCapaDato = new cCliente();


        public int Registrar(ceCliente obj, out string Mensaje)
        {

            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del cliente no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                obj.Clave = cnRecursos.ConvertirSha256(obj.Clave);
                return objCapaDato.Registrar(obj, out Mensaje);

            }
            else
            {

                return 0;
            }



        }


        public List<ceCliente> Listar()
        {
            return objCapaDato.Listar();
        }



        public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
        {

            return objCapaDato.CambiarClave(idcliente, nuevaclave, out Mensaje);
        }


        public bool ReestablecerClave(int idcliente, string correo, out string Mensaje)
        {

            Mensaje = string.Empty;
            string nuevaclave = cnRecursos.GenerarClave();
            bool resultado = objCapaDato.ReestablecerClave(idcliente, cnRecursos.ConvertirSha256(nuevaclave), out Mensaje);

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
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña";

                return false;
            }


        }


    }
}
