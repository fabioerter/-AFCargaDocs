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
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Http;
using System.Configuration;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using System.Text;
//using System.ServiceModel;
//using AFCargaDocs.AxServiceInterface;
using AFCargaDocs.Models.Entidades.Enum;
using AFCargaDocs.AxServiceInterface;

namespace AFCargaDocs.Controllers
{
    public class CargaDocsController : Controller
    {

        // GET: CargaDocs
        public ActionResult Index()
        {
            //?token=bWF0cmljdWxhPTAwMDU0OTY4MSZmbmRjPVBCVVBOSSZhaWR5PTIxMTEmYWlkcD1QUg==
            string encodedValues = Request.Params["token"];
            if (User != null && User.Identity.IsAuthenticated && string.IsNullOrEmpty(encodedValues))
            {
                Console.WriteLine($"(\"RecuperaAvance\")");
            }
            if (string.IsNullOrEmpty(encodedValues))
            {
                Console.WriteLine($"NoLogin");
                throw new HttpException(Convert.ToInt32(HttpStatusCode.Unauthorized),
                        "Debes iniciar sesión para ingresar a la aplicación!");
            }
            byte[] data = Convert.FromBase64String(encodedValues);
            string decodedValues = Encoding.UTF8.GetString(data);
            var parsedValues = HttpUtility.ParseQueryString(decodedValues);
            GlobalVariables.Matricula = parsedValues["matricula"];
            GlobalVariables.Fndc = parsedValues["fndc"];
            GlobalVariables.Aidy = parsedValues["aidy"];
            GlobalVariables.Aidp = parsedValues["aidp"];
            string token = encodedValues;
            ViewBag.Matricula = GlobalVariables.Matricula;
            ViewBag.fndc = GlobalVariables.Fndc;
            ViewBag.aidy = GlobalVariables.Aidy;
            ViewBag.aidp = GlobalVariables.Aidp;
            return View();
        }

        public String ObtenerDocumentos()
        {
            string result = "";
            try
            {
                result = JsonConvert.SerializeObject(CargaDocsService.ObtenerDocumentos(
                GlobalVariables.Matricula,
                GlobalVariables.Fndc,
                GlobalVariables.Aidy,
                GlobalVariables.Aidp));
            }
            catch (Exception ex)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, ex.Message);
            }
            return result;

        }

        public string teste(string clave)
        {
            AxServicesInterface axServicesInterface = new AxServicesInterface();
            string sessionTicket = axServicesInterface.Login("", "BANPROD", "DOCIDX", "di12345678!",
                                                                Convert.ToInt32(EAxType.AxFeature_Basic));
            string result = axServicesInterface.GetApplicationList(sessionTicket, "BANPROD");
            axServicesInterface.Logout(sessionTicket);
            return result;
        }
        public JsonResult guardarDocumento(string clave)
        {
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
            if (clave == null)
            {
                throw new HttpException(Convert.ToInt32(HttpStatusCode.BadRequest),
                        "No se puede encontrar su documento, espere un momento e intente nuevamente");
            }
            return Json(CargaDocsService.insertDocument(GlobalVariables.Matricula, clave, GlobalVariables.Fndc,
                GlobalVariables.Aidy, GlobalVariables.Aidp, file), JsonRequestBehavior.AllowGet);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult FileDisplay(string treqCode)
        {
            return View(CargaDocsService.DisplayFileFromServer(GlobalVariables.Matricula, treqCode, GlobalVariables.Fndc,
                GlobalVariables.Aidy, GlobalVariables.Aidp));
        }

        public ActionResult DownLoad()

        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/image11.png"));

            //File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "filename"+file.Type); 

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "test.xlsx");
        }

        [System.ComponentModel.ToolboxItem(false)]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ObtenerDatosDocumento()
        {
            return JsonConvert.SerializeObject(CargaDocsService.ObtenerDatosDocumento(
                GlobalVariables.Matricula, "AFCD", GlobalVariables.Fndc, GlobalVariables.Aidy, GlobalVariables.Aidp));
        }

        [System.ComponentModel.ToolboxItem(false)]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Guardar()
        {


            HttpContext Contexto = System.Web.HttpContext.Current;
            string claveDoc = Contexto.Request.Form.ToString().Substring(6);



            HttpFileCollection ColeccionArchivos = Contexto.Request.Files;// Context.Request.Files;
            string NombreArchivo = "";
            for (int ArchivoActual = 0; ArchivoActual < ColeccionArchivos.Count; ArchivoActual++)
            {
                NombreArchivo = ColeccionArchivos[ArchivoActual].FileName;
                string DatosArchivo = System.IO.Path.GetFileName(ColeccionArchivos[ArchivoActual].FileName);
                string CarpetaParaGuardar = Server.MapPath("Archivos") + "\\" + DatosArchivo;
                ColeccionArchivos[ArchivoActual].SaveAs(CarpetaParaGuardar);
                Contexto.Response.ContentType = "application/json";
                Contexto.Response.Write("{\"success\":true,\"msg\":\"" + NombreArchivo + "\"}");
                Contexto.Response.End();

            }

        }
        [HttpPost]
        public ActionResult Guardar2()
        {
            string dato = "algo";

            return View(dato);
        }
    }
}