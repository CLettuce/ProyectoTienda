using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class cnCategoria
    {
        private cCategoria objCapaDato = new cCategoria();
        public List<ceCategoria> Listar()
        {
            return objCapaDato.Listar();
        }


        public int Registrar(ceCategoria obj, out string Mensaje)
        {

            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripcion de la categoria no puede ser vacio";
            }
            


            if (string.IsNullOrEmpty(Mensaje))
            {

               return objCapaDato.Registrar(obj, out Mensaje);

            }
            else
            {

                return 0;
            }



        }

        public bool Editar(ceCategoria obj, out string Mensaje)
        {

            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripcion de la categoria no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }



        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }



    }
}
