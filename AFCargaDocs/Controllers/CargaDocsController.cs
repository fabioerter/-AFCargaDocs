using AFCargaDocs.M;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AFCargaDocs.Controllers
{
    public class CargaDocsController : Controller
    {
        // GET: CargaDocs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CargaDocs(string nombre)
        {
            
            ViewBag.Nombre = nombre;
            return View();
        }
    }
}