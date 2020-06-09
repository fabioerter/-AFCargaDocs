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
using System.Xml;
using System.Data;

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
        public string teste2(string treqCode)
        {
            AxServicesInterface axServicesInterface = new AxServicesInterface();
            string sessionTicket = axServicesInterface.Login("", "BANPROD", /*"OTGMGR", "u_pick_it",*//*"BSASSBUSR1", "u_pick_it",*/ "DOCIDX", "di12345678!",
                                                        Convert.ToInt32(EAxType.AxFeature_FullTextSearch));
            AxDocumentIndexQueryData axQueryData = new AxDocumentIndexQueryData();
            axQueryData.addField(1, false, GlobalVariables.Matricula);
            axQueryData.addField(2, false, GlobalVariables.getPdim(
                                            GlobalVariables.Matricula));
            string result = "";

            try
            {
                result = axServicesInterface.QueryDocuments(sessionTicket, "BANPROD",
                                        axQueryData.ToString(), 0, 1, 1, false, false);
            }
            catch (Exception ex)
            {
                throw new HttpException(
                    (int)HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                axServicesInterface.Logout(sessionTicket);
            }
            return result;

        }
        public string teste(string treqCode)
        {
            AxServicesInterface axServicesInterface = new AxServicesInterface();
            string sessionTicket = axServicesInterface.Login("", "BANPROD", "OTGMGR", "u_pick_it",/*"BSASSBUSR1", "u_pick_it",*/ /*"DOCIDX", "di12345678!",*/
                                                        Convert.ToInt32(EAxType.AxFeature_FullTextSearch));
            AxQueryData axQueryData = new AxQueryData();
            axQueryData.Appid = "403";
            axQueryData.Qtype = EAXQueryType.Normal;
            axQueryData.Provider = EAXDataProvider.ApplicationXtender;
            axQueryData.Id = "1";
            axQueryData.Qrtype = EAXQueryRetentionType.AllIncludingRetention;
            axQueryData.Docid_sortorder = EAXSortOrder.None;
            axQueryData.Sortorder = EAXSortOrder.None;
            axQueryData.addField(1, false, GlobalVariables.Matricula);
            axQueryData.addField(2, false, GlobalVariables.getPdim(
                                            GlobalVariables.Matricula));
            //axQueryData.addField(3, false, GlobalVariables.Aidy);
            //axQueryData.addField(4, false, GlobalVariables.Aidp);
            //axQueryData.addField(5, false, treqCode);
            //axQueryData.addField(6, false, "");
            //axQueryData.addField(7, false, GlobalVariables.Fndc);
            //axQueryData.addField(8, false, GlobalVariables.Aplicacion);
            //axQueryData.addField(9, false, "");
            //axQueryData.addField(10, false, "");
            string result = "";
            try
            {
                result = axServicesInterface.QueryDocuments(sessionTicket, "BANPROD",
                                        axQueryData.ToString(), 0, 1, 1, false, false);
            }
            catch (Exception ex)
            {
                throw new HttpException(
                    (int)HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                axServicesInterface.Logout(sessionTicket);
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            doc.GetElementsByTagName("");
            return result;
            //DataBase dataBase = new DataBase();

            //dataBase.AddParameter("p_pidm",
            //    GlobalVariables.getPdim(GlobalVariables.Matricula),
            //    OracleDbType.Int16, 8);
            //dataBase.AddParameter("p_aidy_code",
            //    GlobalVariables.Aidy,
            //    OracleDbType.Varchar2, 20);
            //dataBase.AddParameter("p_aidp_code",
            //    GlobalVariables.Aidp,
            //    OracleDbType.Varchar2, 20);
            //dataBase.AddParameter("p_treq_code",
            //    clave, OracleDbType.Varchar2, 20);

            //return JsonConvert.SerializeObject(
            //    dataBase.ExecuteFunction("KV_APPLICANT_TRK_REQT.F_QUERY_ALL",
            //    "APPLICANT_TRK_REQT_rc",
            //    OracleDbType.RefCursor));
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

        public string guardarDocumento(string clave)
        {
            return JsonConvert.SerializeObject(CargaDocsService.teste2(clave));
            //HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];

            //Document document = new Document(GlobalVariables.Matricula, treqCode,
            //    GlobalVariables.Fndc, GlobalVariables.Aidy, GlobalVariables.Aidp);
            //byte[] fileContents = new byte[file.ContentLength];

            //string id = CargaDocsService.KzrldocInsert(document, file);


            //// Get the object used to communicate with the server.
            //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(GlobalVariables.Ftpip + "/" + id);

            //request.Method = WebRequestMethods.Ftp.UploadFile;

            //// This example assumes the FTP site uses anonymous logon.
            //request.Credentials = new NetworkCredential(GlobalVariables.FtpUser, GlobalVariables.FtpPassword);
            //request.UseBinary = true;
            //fileContents = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);
            //// Copy the contents of the file to the request stream.

            //request.ContentLength = fileContents.Length;

            //using (Stream requestStream = request.GetRequestStream())
            //{
            //    requestStream.Write(fileContents, 0, fileContents.Length);
            //}

            //using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            //{
            //    Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
            //}


            //AxServicesInterface axServicesInterface = new AxServicesInterface();
            //string sessionTicket = axServicesInterface.Login("", "BANPROD", /*"services.bdmapp", "W#7hdw!68dxZ",*//*"BSASSBUSR1", "u_pick_it",*/"DOCIDX", "di12345678!",
            //                                                        Convert.ToInt32(EAxType.AxFeature_Basic));
            //string result = null;
            //try
            //{

            //    AxDocumentCreationData newDocument = new AxDocumentCreationData(403, "BANPROD",
            //                    path/*result*/, EAxFileType.FT_UNKNOWN,
            //                    true, true, false, 0);

            //    AxDocumentIndex newDocumentIndex = new AxDocumentIndex("-1",
            //            GlobalVariables.Matricula,
            //            GlobalVariables.getPdim(GlobalVariables.Matricula),
            //            GlobalVariables.Aidy, GlobalVariables.Aidp,
            //            GlobalVariables.Fndc, treqCode,
            //            GlobalVariables.Aplicacion,
            //            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            //    result = axServicesInterface.CreateNewDocument(sessionTicket, newDocument.ToString(),
            //                                                           newDocumentIndex.ToString());
            //}
            //catch (Exception ex)
            //{
            //    throw new HttpException((int)HttpStatusCode.BadRequest, ex.Message);
            //}
            //finally
            //{
            //    axServicesInterface.Logout(sessionTicket);
            //}
            //return result;
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

        //public string guardarDocumento(string clave)
        //{
        //    HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
        //    if (clave == null)
        //    {
        //        throw new HttpException(Convert.ToInt32(HttpStatusCode.BadRequest),
        //                "No se puede encontrar su documento, espere un momento e intente nuevamente");
        //    }
        //    string result = "";
        //    try
        //    {
        //        result = JsonConvert.SerializeObject(CargaDocsService.insertDocument(GlobalVariables.Matricula, clave, GlobalVariables.Fndc,
        //        GlobalVariables.Aidy, GlobalVariables.Aidp, file));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new HttpException((int)HttpStatusCode.InternalServerError,
        //                                                                ex.Message);
        //    }

        //    return result;
        //}

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult FileDisplay(string treqCode)
        {
            return View(CargaDocsService.DisplayFileFromServer(GlobalVariables.Matricula, treqCode, GlobalVariables.Fndc,
                GlobalVariables.Aidy, GlobalVariables.Aidp));
        }

        public ActionResult DownLoad(string treqCode)

        {
            FileInfoFtp file = CargaDocsService.DisplayFileFromServer(
                GlobalVariables.Matricula, treqCode, GlobalVariables.Fndc, GlobalVariables.Aidy, GlobalVariables.Aidp);
            byte[] fileBytes = Convert.FromBase64String(file.FileContent);

            //File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "filename"+file.Type); 

            return File(fileBytes, file.FileType, file.FileName);
        }

        [System.ComponentModel.ToolboxItem(false)]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ObtenerDatosDocumento()
        {
            return JsonConvert.SerializeObject(CargaDocsService.ObtenerDatosDocumento(
                GlobalVariables.Matricula, "AFCD", GlobalVariables.Fndc, GlobalVariables.Aidy, GlobalVariables.Aidp));
        }

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