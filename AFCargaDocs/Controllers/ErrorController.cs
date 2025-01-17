﻿using Microsoft.Ajax.Utilities;
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
                    ViewBag.Title = "Surgió una excepcion.";
                    if (message == "")
                    {
                        ViewBag.Description = "Algo salio mal";
                    }
                    else
                    {
                        ViewBag.Description = error + " : " + message;
                    }
                    break;
            }

            return View();
        }
        public JsonResult AjaxError(int error = 0, string message = "")
        {
            return Json(new {errMsg = message, errNum =  error});
        }
    }
}