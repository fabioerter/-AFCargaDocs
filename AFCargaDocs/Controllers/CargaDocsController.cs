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
using AFCargaDocs.Models.Entidades.AxXML;

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
        public string teste(string clave)
        {


            AxServicesInterface axServicesInterface = new AxServicesInterface();
            string sessionTicket = axServicesInterface.Login("", "BANPROD", /*"OTGMGR", "u_pick_it",*//*"BSASSBUSR1", "u_pick_it",*/ "DOCIDX", "di12345678!",
                                                        Convert.ToInt32(EAxType.AxFeature_FullTextSearch));

            AxDocumentIndexQueryData newDocument = new AxDocumentIndexQueryData();

            newDocument.addField(1, false, GlobalVariables.Matricula);
            newDocument.addField(2, false, GlobalVariables.getPdim(
                                            GlobalVariables.Matricula));
            newDocument.addField(3, false, GlobalVariables.Aidy);
            newDocument.addField(4, false, GlobalVariables.Aidp);
            newDocument.addField(5, false, clave);
            newDocument.addField(6, false, "");
            newDocument.addField(7, false, GlobalVariables.Fndc);
            newDocument.addField(8, false, GlobalVariables.Aplicacion);
            newDocument.addField(9, false, "");
            newDocument.addField(10, false, "");

            string result = "";
            try
            {
                result = axServicesInterface.QueryApplicationIndexesByAppId(
                    sessionTicket, "BANPROD", 403, false, true, newDocument.ToString(), 0, 1, 20);
            }
            catch (Exception ex)
            {
                throw new HttpException(
                    (int)HttpStatusCode.InternalServerError, ex.Message);
            }
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);
            //string teste = xml.GetElementsByTagName("ax:Row")[0].ChildNodes[0].LastChild.Attributes[1].Value;
            string teste22 = xml.GetElementsByTagName(
                    "ax:Rows")[0].InnerXml.
                    Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "").
                    Replace("xsi:", "");
            AxRow row =
                Serialization<AxRow>
                .DeserializeFromXmlFile(teste22);
            try
            {

                string exportId = axServicesInterface.ExportDocumentPagesByRef(sessionTicket, row.attributes[2].value,
                    new AxDocumentExportData()
                    {
                        Format = AxImageExportFormatData.TIFF,
                        Formtype = AxFormTypes.None,
                        HideAnnotations = true,
                        SinglePDFFile = false
                    }.ToString());
                result = axServicesInterface.GetExportDocumentPagesResult(sessionTicket, GlobalVariables.DataSource, exportId, false);

                AxStringArray stringArray = Serialization<AxStringArray>.DeserializeFromXmlFile(result);
                while (stringArray.itemString == null)
                {
                    string arrayTemp = axServicesInterface.GetExportDocumentPagesResult(
                               sessionTicket, GlobalVariables.DataSource, exportId, false);
                    stringArray = Serialization<AxStringArray>.DeserializeFromXmlFile(arrayTemp);
                    System.Threading.Thread.Sleep(50);

                }

                AxImageStreamData data = new AxImageStreamData()
                {
                    Encryption = false,
                    Maxbytes = 5000000,
                    Startbyte = 0
                };

                AxStreamResult axStreamResult = Serialization<AxStreamResult>.
                    DeserializeFromXmlFile(axServicesInterface.DownloadImageStream(
                    sessionTicket, GlobalVariables.DataSource,
                    stringArray.itemString[0], data.ToString()));
                result = axStreamResult.ImageBytes;
                //axServicesInterface.LockDocumentByRef(sessionTicket, row.attributes[2].value);
                //result = axServicesInterface.DeleteDocumentByRef(sessionTicket, row.attributes[2].value);

            }
            catch (Exception ex)
            {
                throw new HttpException(
                    (int)HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                axServicesInterface.CloseDocumentByRef(sessionTicket, row.attributes[2].value, false, "");
                axServicesInterface.Logout(sessionTicket);
            }



            return result;
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
            return JsonConvert.SerializeObject(CargaDocsService.GuardarDocumento(clave));
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

        [System.ComponentModel.ToolboxItem(false)]
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ObtenerDatosDocumento()
        {
            return JsonConvert.SerializeObject(CargaDocsService.ObtenerDatosDocumento(
                GlobalVariables.Matricula, "AFCD", GlobalVariables.Fndc, GlobalVariables.Aidy, GlobalVariables.Aidp));
        }
    }
}