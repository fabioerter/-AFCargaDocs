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
using System.IO;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

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
                throw new HttpException(Convert.ToInt32(HttpStatusCode.Unauthorized),
                        "Se passo un error en token.");
            }
            if (string.IsNullOrEmpty(encodedValues))
            {
                Console.WriteLine($"NoLogin");
                throw new HttpException(Convert.ToInt32(HttpStatusCode.Unauthorized),
                        "Debes iniciar sesión para ingresar a la aplicación.");
            }

            byte[] data = Convert.FromBase64String(encodedValues);
            string decodedValues = Encoding.UTF8.GetString(data);
            var parsedValues = HttpUtility.ParseQueryString(decodedValues);
            GlobalVariables.Matricula = parsedValues["matricula"];
            GlobalVariables.Fndc = parsedValues["fndc"];
            GlobalVariables.Aidy = parsedValues["aidy"];
            GlobalVariables.Aidp = parsedValues["aidp"];
            GlobalVariables.Aplicacion = parsedValues["p_apfr_code"];
            string token = encodedValues;
            ViewBag.Matricula = GlobalVariables.Matricula;
            ViewBag.fndc = GlobalVariables.Fndc;
            ViewBag.aidy = GlobalVariables.Aidy;
            ViewBag.aidp = GlobalVariables.Aidp;
            ViewBag.Aplicacion = GlobalVariables.Aplicacion;
            return View();
        }

        public string teste(string clave)
        {
            DataBase dataBase = new DataBase();

            dataBase.AddParameter("p_pidm",
                GlobalVariables.getPdim(GlobalVariables.Matricula),
                OracleDbType.Int16, 8);
            dataBase.AddParameter("p_aidy_code",
                GlobalVariables.Aidy,
                OracleDbType.Varchar2, 20);
            dataBase.AddParameter("p_aidp_code",
                GlobalVariables.Aidp,
                OracleDbType.Varchar2, 20);
            dataBase.AddParameter("p_treq_code",
                clave, OracleDbType.Varchar2, 20);

            return JsonConvert.SerializeObject(
                dataBase.ExecuteFunction("KV_APPLICANT_TRK_REQT.F_QUERY_ALL",
                "APPLICANT_TRK_REQT_rc",
                OracleDbType.RefCursor));
        }
        public string ObtenerDocumentos()
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

        public string teste2(String treqCode)
        {
            AxServicesInterface axServicesInterface = new AxServicesInterface();
            string sessionTicket = axServicesInterface.Login("", "BANPROD", "DOCIDX", "di12345678!",
                                                                    Convert.ToInt32(EAxType.AxFeature_Basic));
            string result = null;
            try
            {
                //axServicesInterface.GetApplicationList(sessionTicket, "BANPROD");
                //HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
                //byte[] fileContents = new byte[file.ContentLength];
                //fileContents = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);



                //result = axServicesInterface.UploadImageStream(sessionTicket, "BANPROD", new AxStreamData(
                //    Convert.ToBase64String(fileContents),"",file.FileName).ToString());



                AxDocumentCreationData newDocument = new AxDocumentCreationData(403, "BANPROD",
                                "E:/RepositorioAyudasFinancieras/DEVL/1"/*result*/, EAxFileType.FT_UNKNOWN,
                                true, true, false, 0);

                AxDocumentIndex newDocumentIndex = new AxDocumentIndex("0",
                    Convert.ToInt32(GlobalVariables.Matricula),
                        GlobalVariables.getPdim(GlobalVariables.Matricula),
                        GlobalVariables.Aidy, GlobalVariables.Aidp,
                        GlobalVariables.Fndc, treqCode,
                        GlobalVariables.Aplicacion,
                        DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

                //result = axServicesInterface.UploadDocumentPageByRef(sessionTicket,
                //        new AxDocumentPointer("BANPROD", 1, 1, EAxDocumentType.Document, 1).ToString(),
                //        new AxPageUploadData()
                //        {
                //            Act = EAxPageUploadAction.Replace,
                //            Filepath = result,
                //            Filetype = EAxFileType.FT_UNKNOWN,
                //            Pos = 1,
                //            Splitimg = true,
                //            Subpages = 0
                //        }.ToString());

                //result = axServicesInterface.ApplyAutoIndexByAppId(sessionTicket, "BANPROD",
                //    403, newDocumentIndex.ToString(), newDocumentIndex.ToString());


                result = axServicesInterface.CreateNewDocument(sessionTicket, newDocument.ToString(),
                                                                       newDocumentIndex.ToString());
            }
            catch (Exception ex)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, ex.Message);
            }
            finally
            {
                axServicesInterface.Logout(sessionTicket);
            }
            return result;
        }

        public string validaCarga(string treqCode)
        {
            return new JObject(new JProperty("isOnServer",
                 CargaDocsService.validaCarga(GlobalVariables.Matricula, treqCode, GlobalVariables.Fndc,
                 GlobalVariables.Aidy, GlobalVariables.Aidp))).ToString();
        }
        public string updateDocumento(string clave)
        {
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
            return JsonConvert.SerializeObject(
                CargaDocsService.updateDocument(clave, file, "NR"));
        }

        public string guardarDocumento(string clave)
        {
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
            if (clave == null)
            {
                throw new HttpException(Convert.ToInt32(HttpStatusCode.BadRequest),
                        "No se puede encontrar su documento, espere un momento e intente nuevamente");
            }
            string result = "";
            try
            {
                result = JsonConvert.SerializeObject(CargaDocsService.insertDocument(GlobalVariables.Matricula, clave, GlobalVariables.Fndc,
                GlobalVariables.Aidy, GlobalVariables.Aidp, file));
            }
            catch (Exception ex)
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError,
                                                                        ex.Message);
            }

            return result;
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