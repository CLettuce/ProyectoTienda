using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;


using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            ceUsuario oUsuario = new ceUsuario();

            oUsuario = new cnUsuarios().Listar().Where(u => u.Correo == correo && u.Clave == cnRecursos.ConvertirSha256(clave)).FirstOrDefault();


            if (oUsuario == null)
            {

                ViewBag.Error = "Correo o contraseña no correcta";
                return View();
            }
            else {

                if (oUsuario.Reestablecer) {

                    TempData["IdUsuario"] = oUsuario.IdUsuario;
                    return RedirectToAction("CambiarClave");

                }


                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);

                ViewBag.Error = null;

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idusuario, string claveactual, string nuevaclave, string confirmarclave)
        {

            ceUsuario oUsuario = new ceUsuario();

            oUsuario = new cnUsuarios().Listar().Where(u => u.IdUsuario == int.Parse(idusuario)).FirstOrDefault();

            if (oUsuario.Clave != cnRecursos.ConvertirSha256(claveactual))
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if (nuevaclave != confirmarclave) {

                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["vclave"] = "";


            nuevaclave = cnRecursos.ConvertirSha256(nuevaclave);


            string mensaje = string.Empty;

            bool respuesta = new cnUsuarios().CambiarClave(int.Parse(idusuario), nuevaclave, out mensaje);

            if (respuesta)
            {

                return RedirectToAction("Index");
            }
            else {

                TempData["IdUsuario"] = idusuario;

                ViewBag.Error = mensaje;
                return View();
            }

        }


        [HttpPost]
        public ActionResult Reestablecer(string correo) {

            ceUsuario ousurio = new ceUsuario();

            ousurio = new cnUsuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (ousurio == null) {


                ViewBag.Error = "No se encontro un usuario relacionado a ese correo";
                return View();
            }


            string mensaje = string.Empty;
            bool respuesta = new cnUsuarios().ReestablecerClave(ousurio.IdUsuario, correo, out mensaje);

            if (respuesta)
            {

                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");

            }
            else {

                ViewBag.Error = mensaje;
                return View();
            }

        
        
        }

        public ActionResult CerrarSesion() {

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");

        }



    }
}