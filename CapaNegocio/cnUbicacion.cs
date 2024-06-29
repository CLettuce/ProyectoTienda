using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class cnUbicacion
    {
        private cUbicacion objCapaDato = new cUbicacion();


        public List<ceDepartamento> ObtenerDepartamento() {

            return objCapaDato.ObtenerDepartamento();
        }

        public List<ceMunicipio> ObtenerMunicipio(string iddepartamento) {

            return objCapaDato.ObtenerMunicipio(iddepartamento);
        }


    }
}
