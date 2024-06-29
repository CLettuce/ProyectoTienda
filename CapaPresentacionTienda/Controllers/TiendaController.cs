using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.IO;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using CapaPresentacionTienda.Filter;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetalleProducto(int idproducto = 0)
        {
            ceProducto oProducto = new ceProducto();

            oProducto = new cnProducto().Listar().Where(p => p.IdProducto == idproducto).FirstOrDefault();

            if (oProducto != null && oProducto.Imagen != null)
            {
                oProducto.Base64 = Convert.ToBase64String(oProducto.Imagen);
                oProducto.Extension = "png"; // O el formato de la imagen que estés usando
            }

            return View(oProducto);
        }

        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<ceCategoria> lista = new List<ceCategoria>();
            lista = new cnCategoria().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarMarcaporCategoria(int idcategoria)
        {
            List<ceMarca> lista = new List<ceMarca>();
            lista = new cnMarca().ListarMarcaporCategoria(idcategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca, int nroPagina)
        {
            List<ceProducto> lista = new List<ceProducto>();

            int _TotalRegistros;
            int _TotalPaginas;

            lista = new cnProducto().ObtenerProductos(idmarca, idcategoria, nroPagina, 8, out _TotalRegistros, out _TotalPaginas).Select(p => new ceProducto()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca = p.oMarca,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                Base64 = p.Imagen != null ? Convert.ToBase64String(p.Imagen) : null,
                Extension = "png", // O el formato de la imagen que estés usando
                Activo = p.Activo
            }).ToList();

            var jsonresult = Json(new { data = lista, totalRegistros = _TotalRegistros, totalPaginas = _TotalPaginas }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }

        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {
            int idcliente = ((ceCliente)Session["Cliente"]).IdCliente;
            bool existe = new cnCarrito().ExisteCarrito(idcliente, idproducto);
            bool respuesta = false;
            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = new cnCarrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idcliente = ((ceCliente)Session["Cliente"]).IdCliente;
            int cantidad = new cnCarrito().CantidadEnCarrito(idcliente);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProductosCarrito()
        {
            int idcliente = ((ceCliente)Session["Cliente"]).IdCliente;
            List<ceCarrito> oLista = new List<ceCarrito>();

            oLista = new cnCarrito().ListarProducto(idcliente).Select(oc => new ceCarrito()
            {
                oProducto = new ceProducto()
                {
                    IdProducto = oc.oProducto.IdProducto,
                    Nombre = oc.oProducto.Nombre,
                    oMarca = oc.oProducto.oMarca,
                    Precio = oc.oProducto.Precio,
                    Base64 = oc.oProducto.Imagen != null ? Convert.ToBase64String(oc.oProducto.Imagen) : null,
                    Extension = "png" // O el formato de la imagen que estés usando
                },
                Cantidad = oc.Cantidad
            }).ToList();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {
            int idcliente = ((ceCliente)Session["Cliente"]).IdCliente;
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new cnCarrito().OperacionCarrito(idcliente, idproducto, sumar, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idcliente = ((ceCliente)Session["Cliente"]).IdCliente;
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new cnCarrito().EliminarCarrito(idcliente, idproducto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDepartamento()
        {
            List<ceDepartamento> oLista = new List<ceDepartamento>();
            oLista = new cnUbicacion().ObtenerDepartamento();
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerMunicipio(string IdDepartamento)
        {
            List<ceMunicipio> oLista = new List<ceMunicipio>();
            oLista = new cnUbicacion().ObtenerMunicipio(IdDepartamento);
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }


        [ValidarSession]
        [Authorize]
        public ActionResult Carrito()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<ceCarrito> oListaCarrito, ceVenta oVenta)
        {
            decimal total = 0;
            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = new CultureInfo("es-PE");
            detalle_venta.Columns.Add("IdProducto", typeof(string));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Total", typeof(decimal));

            foreach (ceCarrito oCarrito in oListaCarrito)
            {
                decimal subtotal = Convert.ToDecimal(oCarrito.Cantidad.ToString()) * oCarrito.oProducto.Precio;
                total += subtotal;

                detalle_venta.Rows.Add(new object[]
                {
                    oCarrito.oProducto.IdProducto,
                    oCarrito.Cantidad,
                    subtotal
                });
            }

            oVenta.MontoTotal = total;
            oVenta.IdCliente = ((ceCliente)Session["Cliente"]).IdCliente;

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;

            // Simulación del proceso de pago
            await Task.Delay(1000); // Simula un retraso en el proceso de pago

            return Json(new { success = true, message = "Pago procesado exitosamente." }, JsonRequestBehavior.AllowGet);
        }

        [ValidarSession]
        [Authorize]
        public async Task<ActionResult> PagoEfectuado()
        {
            // Simulación de la aprobación del pago
            await Task.Delay(1000); // Simula un retraso en la aprobación del pago

            ceVenta oVenta = (ceVenta)TempData["Venta"];
            DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];
            oVenta.IdTransaccion = Guid.NewGuid().ToString(); // Simulación de una ID de transacción

            string mensaje = string.Empty;
            bool respuesta = new cnVenta().Registrar(oVenta, detalle_venta, out mensaje);

            ViewData["Status"] = respuesta;
            ViewData["IdTransaccion"] = oVenta.IdTransaccion;

            return View();
        }

        [ValidarSession]
        [Authorize]
        public ActionResult MisCompras()
        {
            int idcliente = ((ceCliente)Session["Cliente"]).IdCliente;
            List<ceDetalleVenta> oLista = new List<ceDetalleVenta>();

            oLista = new cnVenta().ListarCompras(idcliente).Select(oc => new ceDetalleVenta()
            {
                oProducto = new ceProducto()
                {
                    Nombre = oc.oProducto.Nombre,
                    Precio = oc.oProducto.Precio,
                    Base64 = oc.oProducto.Imagen != null ? Convert.ToBase64String(oc.oProducto.Imagen) : null,
                    Extension = "png" // O el formato de la imagen que estés usando
                },
                Cantidad = oc.Cantidad,
                Total = oc.Total,
                IdTransaccion = oc.IdTransaccion
            }).ToList();

            return View(oLista);
        }
    }
}
