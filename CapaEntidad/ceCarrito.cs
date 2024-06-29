using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ceCarrito
    {

        public int IdCarrito { get; set; }
        public ceCliente oCliente { get; set; }
        public ceProducto oProducto { get; set; }
        public int Cantidad { get; set; }
        public byte[] Imagen { get; set; }

    }
}
