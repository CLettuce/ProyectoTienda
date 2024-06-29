using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {
        

        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }


        // ++++++++++++++++ CATEGORIA ++++++++++++++++++++

        #region CATEGORIA
        [HttpGet]
        public JsonResult ListarCategorias()
        {

            List<ceCategoria> oLista = new List<ceCategoria>();
            oLista = new cnCategoria().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult GuardarCategoria(ceCategoria objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdCategoria == 0)
            {

                resultado = new cnCategoria().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new cnCategoria().Editar(objeto, out mensaje);

            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new cnCategoria().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        // ++++++++++++++++ MARCA ++++++++++++++++++++

        #region MARCA
        [HttpGet]
        public JsonResult ListarMarca()
        {
            List<ceMarca> oLista = new List<ceMarca>();
            oLista = new cnMarca().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(ceMarca objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdMarca == 0)
            {
                resultado = new cnMarca().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new cnMarca().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new cnMarca().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        // ++++++++++++++++ PRODUCTO ++++++++++++++++++++
        #region PRODUCTO
        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<ceProducto> oLista = new List<ceProducto>();
            oLista = new cnProducto().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public async Task<JsonResult> GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            string mensaje = string.Empty;
            bool operacion_exitosa = true;

            ceProducto oProducto = JsonConvert.DeserializeObject<ceProducto>(objeto);

            if (decimal.TryParse(oProducto.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out decimal precio))
            {
                oProducto.Precio = precio;
            }
            else
            {
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);
            }

            if (archivoImagen != null)
            {
                using (var binaryReader = new BinaryReader(archivoImagen.InputStream))
                {
                    oProducto.Imagen = binaryReader.ReadBytes(archivoImagen.ContentLength);
                }
            }

            if (oProducto.IdProducto == 0)
            {
                int idProductoGenerado = new cnProducto().Registrar(oProducto, out mensaje);
                if (idProductoGenerado != 0)
                {
                    oProducto.IdProducto = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                operacion_exitosa = new cnProducto().Editar(oProducto, out mensaje);
            }

            return Json(new { operacionExitosa = operacion_exitosa, idGenerado = oProducto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ImagenProducto(int id) 
        {

            ceProducto oproducto = new cnProducto().Listar().Where(p => p.IdProducto == id).FirstOrDefault();

            if (oproducto != null && oproducto.Imagen != null)
            {
                string base64String = Convert.ToBase64String(oproducto.Imagen);
                return Json(new { conversion = true, extension = "jpg", textobase64 = base64String }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { conversion = false, ruta = oproducto?.RutaImagen }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new cnProducto().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}