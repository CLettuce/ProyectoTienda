using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ceDetalleVenta
    {

        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public ceProducto oProducto { get; set; }

        public int Cantidad { get; set; }
        public decimal Total { get; set; }

        public string IdTransaccion { get; set; }


    }
}
