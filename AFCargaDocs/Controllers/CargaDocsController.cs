using AFCargaDocs.Models;
using AFCargaDocs.Models.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
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

        public String ObtenerDocumentos(string nombre)
        {
            ViewBag.Nombre = nombre;
            return JsonConvert.SerializeObject(CargaDocsService.ObtenerDocumentos("000549681"));
        }
    }
}