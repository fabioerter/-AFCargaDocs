using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AFCargaDocs.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(int error = 0, string message="")
        {
            switch (error)
            {
                case 505:
                    ViewBag.Title = "Ocurrio un error inesperado";
                    ViewBag.Description = "Esto es muy vergonzoso, esperemos que no vuelva a pasar ..";
                    break;

                case 404:
                    ViewBag.Title = "Página no encontrada";
                    ViewBag.Description = "La URL que está intentando ingresar no existe";
                    break;
                case 401:
                    ViewBag.Title = "Sin permiso!";
                    ViewBag.Description = error + " : " + message;
                    break;
                default:
                    ViewBag.Title = "Se produjo un error inesperado";
                    if (message == "")
                    {
                        ViewBag.Description = "Algo salio muy mal";
                    }
                    else
                    {
                        ViewBag.Description = error + " : " + message;
                    }
                    break;
            }

            return View();
        }
    }
}