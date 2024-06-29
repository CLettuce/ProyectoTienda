using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class cnVenta
    {
        private cVenta objCapaDato = new cVenta();
        public bool Registrar(ceVenta obj, DataTable DetalleVenta, out string Mensaje) {

            return objCapaDato.Registrar(obj, DetalleVenta, out Mensaje);
        }

        public List<ceDetalleVenta> ListarCompras(int idcliente) {
            return objCapaDato.ListarCompras(idcliente);
        }
    }
}
