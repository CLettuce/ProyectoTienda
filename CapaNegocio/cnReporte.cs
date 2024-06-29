using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using CapaDatos;
using CapaEntidad;


namespace CapaNegocio
{
    public class cnReporte
    {
        private cReporte objCapaDato = new cReporte();

        public List<ceReporte> Ventas(string fechainicio, string fechafin, string idtransaccion) {
            return objCapaDato.Ventas(fechainicio,fechafin,idtransaccion);
        }


        public ceDashBoard VerDashBoard()
        {
            return objCapaDato.VerDashBoard();
        }

    }
}
