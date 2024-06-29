using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(ceCliente objeto)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(objeto.Nombres) ? "" : objeto.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(objeto.Apellidos) ? "" : objeto.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;

            if (objeto.Clave != objeto.ConfirmarClave) {

                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }


            resultado = new cnCliente().Registrar(objeto, out mensaje);

            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else {
                ViewBag.Error = mensaje;
                return View();
            }

        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            ceCliente oCliente = null;

            oCliente = new cnCliente().Listar().Where(item => item.Correo == correo && item.Clave == cnRecursos.ConvertirSha256(clave)).FirstOrDefault();


            if (oCliente == null)
            {
                ViewBag.Error = "Correo o contraseña no son correctas";
                return View();

            }
            else {

                if (oCliente.Reestablecer)
                {
                    TempData["IdCliente"] = oCliente.IdCliente;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else {

                    FormsAuthentication.SetAuthCookie(oCliente.Correo, false);

                    Session["Cliente"] = oCliente;

                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");


                }

            }
        }


        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {


            ceCliente cliente = new ceCliente();

            cliente = new cnCliente().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "No se encontro un cliente relacionado a ese correo";
                return View();
            }


            string mensaje = string.Empty;
            bool respuesta = new cnCliente().ReestablecerClave(cliente.IdCliente, correo, out mensaje);

            if (respuesta)
            {

                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");

            }
            else
            {

                ViewBag.Error = mensaje;
                return View();
            }


        }

        [HttpPost]
        public ActionResult CambiarClave(string idcliente, string claveactual, string nuevaclave, string confirmaclave)
        {

            ceCliente oCliente = new ceCliente();

            oCliente = new cnCliente().Listar().Where(u => u.IdCliente == int.Parse(idcliente)).FirstOrDefault();

            if (oCliente.Clave != cnRecursos.ConvertirSha256(claveactual))
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vclave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if (nuevaclave != confirmaclave)
            {

                TempData["IdCliente"] = idcliente;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["vclave"] = "";


            nuevaclave = cnRecursos.ConvertirSha256(nuevaclave);


            string mensaje = string.Empty;

            bool respuesta = new cnCliente().CambiarClave(int.Parse(idcliente), nuevaclave, out mensaje);

            if (respuesta)
            {

                return RedirectToAction("Index");
            }
            else
            {

                TempData["IdCliente"] = idcliente;

                ViewBag.Error = mensaje;
                return View();
            }

        }



        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");

        }
    }
}