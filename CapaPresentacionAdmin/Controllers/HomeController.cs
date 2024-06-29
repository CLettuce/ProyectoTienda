using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;



using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;

namespace CapaPresentacionAdmin.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {


            List<ceUsuario> oLista = new List<ceUsuario>();

            oLista = new cnUsuarios().Listar();


            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult GuardarUsuario(ceUsuario objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdUsuario == 0)
            {

                resultado = new cnUsuarios().Registrar(objeto, out mensaje);
            }
            else {
                resultado = new cnUsuarios().Editar(objeto, out mensaje);
            
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarUsuario(int id) {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new cnUsuarios().Eliminar(id,out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }





        [HttpGet]
        public JsonResult ListaReporte(string fechainicio,string fechafin,string idtransaccion)
        {
            List<ceReporte> oLista = new List<ceReporte>();

            oLista = new cnReporte().Ventas(fechainicio,fechafin,idtransaccion);

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }





        [HttpGet]
        public JsonResult VistaDashBoard() {
            ceDashBoard objeto = new cnReporte().VerDashBoard();

            return Json(new { resultado = objeto}, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public FileResult ExportarVenta(string fechainicio, string fechafin, string idtransaccion) {

            List<ceReporte> oLista = new List<ceReporte>();
            oLista = new cnReporte().Ventas(fechainicio, fechafin, idtransaccion);

            DataTable dt = new DataTable();

            dt.Locale = new System.Globalization.CultureInfo("es-PE");
            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTransaccion", typeof(string));


            foreach (ceReporte rp in oLista) {
                dt.Rows.Add(new object[] {
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                });
            
            }

            dt.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook()) {

                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream()) {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta" + DateTime.Now.ToString() + ".xlsx");
                
                }
            }



        }



    }
}