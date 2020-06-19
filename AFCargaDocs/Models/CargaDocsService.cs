
using Newtonsoft.Json;
using AFCargaDocs.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Web.Http;
using System.Net.Http;
using System.Data;
using System.Text;
using System.IO;
using WebGrease.Activities;
using System.Web.Http.Results;
using AFCargaDocs.AxServiceInterface;
using AFCargaDocs.Models.Entidades.Enum;
using System.Xml;
using AFCargaDocs.Models.Entidades.AxXML;

namespace AFCargaDocs.Models
{
    /// <summary>
    /// Servicio para obtener y modificar información de contactos de emergencias de un alumno
    /// </summary>
    public static class CargaDocsService
    {

        /// <summary>
        /// Método que obtiene los dopcumentos de una 
        /// </summary>
        /// <param name="matricula">Matricula del alumno</param>
        /// <returns>Arreglo con los contactos que le corresponden a la matricula</returns>
        public static Document[] ObtenerDocumentos(
            string matricula, string fndcCode, string aidyCode, string aidpCode)
        {
            DocumentList documentList = new DocumentList(matricula, fndcCode, aidyCode, aidpCode);
            return documentList.GetDocumentList().ToArray();
        }
        public static Document ObtenerDatosDocumento(string matricula, string clave,
                                                string fndcCode, string aidyCode, string aidpCode)
        {
            return new Document(matricula, clave, fndcCode, aidyCode, aidpCode);

        }
        public static FileInfoFtp DisplayFileFromServer(string matricula, string treqCode,
                                                string fndcCode, string aidyCode, string aidpCode)
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
            newDocument.addField(5, false, treqCode);
            newDocument.addField(6, false, "");
            newDocument.addField(7, false, GlobalVariables.Fndc);
            newDocument.addField(8, false, GlobalVariables.Aplicacion);
            newDocument.addField(9, false, "");
            newDocument.addField(10, false, "");
            string documents;
            try
            {
                documents = axServicesInterface.QueryApplicationIndexesByAppId(
                    sessionTicket, "BANPROD", 403, false, true, newDocument.ToString(), 0, 1, 20);
            }
            catch (Exception ex)
            {
                throw new HttpException(
                    (int)HttpStatusCode.InternalServerError, ex.Message);
            }
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(documents);
            //string teste = xml.GetElementsByTagName("ax:Row")[0].ChildNodes[0].LastChild.Attributes[1].Value;
            string xmlString = xml.GetElementsByTagName(
                    "ax:Rows")[0].InnerXml.
                    Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "").
                    Replace("xsi:", "");
            AxRow row =
                Serialization<AxRow>
                .DeserializeFromXmlFile(xmlString);


            AxStreamResult axStreamResult = new AxStreamResult();
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

                AxStringArray stringArray = Serialization<AxStringArray>
                    .DeserializeFromXmlFile(axServicesInterface.GetExportDocumentPagesResult
                    (sessionTicket, GlobalVariables.DataSource, exportId, false));

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

                axStreamResult = Serialization<AxStreamResult>.
                    DeserializeFromXmlFile(axServicesInterface.DownloadImageStream(
                    sessionTicket, GlobalVariables.DataSource,
                    stringArray.itemString[0], data.ToString()));

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

            Kzrldoc kzrldoc = new Kzrldoc(
                GlobalVariables.Matricula,
                GlobalVariables.Aidy, GlobalVariables.Aidp,
                GlobalVariables.Fndc, treqCode);

            return new FileInfoFtp()
            {
                FileContent = axStreamResult.ImageBytes,
                FileName = kzrldoc.FileName,
                FileType = kzrldoc.FileType
            };

        }
        public static string ObtenerDocumentos()
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
            //Document document = new Document(matricula, clave, fndcCode, aidyCode, aidpCode);
            //if (document.status == "PS")
            //{
            //    Console.WriteLine($"Su documento no esta en nuestro sistema!");
            //    throw new HttpException((int)HttpStatusCode.InternalServerError,
            //                        "Su documento no esta en nuestro sistema!");
            //}
            //FileInfoFtp file = new FileInfoFtp(clave);
            //WebClient request = new WebClient();


            //request.Credentials = new NetworkCredential(GlobalVariables.FtpUser, GlobalVariables.FtpPassword);

            //try
            //{
            //    Stream streamTemp = null;
            //    byte[] newFileData = request.DownloadData(new Uri(GlobalVariables.Ftpip) + "/" + file.FileId);
            //    //if (file.FileType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            //    //{
            //    //    Converter toPdf = new Converter();
            //    //    toPdf.convert(newFileData,Format.DOCX, streamTemp, Format.PDF);
            //    //    file.FileContent = GlobalVariables.ConvertToBase64(streamTemp);
            //    //}
            //    file.FileContent = Convert.ToBase64String(newFileData);
            //}
            //catch (Exception ex)
            //{
            //    throw new HttpException((int)HttpStatusCode.InternalServerError, ex.Message);
            //}

            //if (file.FileContent == null)
            //{
            //    throw new HttpException((int)HttpStatusCode.InternalServerError, "El Documento no esta en nuestro servidor");
            //}

            //return file;
        }

        public static bool validaCarga(string matricula, string treqCode, string fndcCode,
                                        string aidyCode, string aidpCode)
        {
            bool isOnServer = false;
            Document document = new Document(matricula, treqCode, fndcCode, aidyCode, aidpCode);
            switch (document.status)
            {
                case "PS":
                    isOnServer = false;
                    break;
                case "NR":
                    if (DateTime.Now.Subtract(DateTime.Parse(document.fecha))
                        .TotalMilliseconds < 2628000000)
                    {
                        isOnServer = true;
                    }
                    else
                    {
                        throw new HttpException((int)HttpStatusCode.BadRequest,
                      "No es posible cargar un documento con mas de un mes");
                    }
                    break;
                case "IV":
                    isOnServer = true;
                    break;
                case "CM":
                    throw new HttpException((int)HttpStatusCode.BadRequest,
                        "No se pode cargar un documento concluido.");
                    break;
            }
            return isOnServer;

        }

        public static Document updateDocument(string treqCode, HttpPostedFile file,
                    string trstCode)
        {

            AxServicesInterface axServicesInterface = new AxServicesInterface();

            string sessionTicket = axServicesInterface.Login("", "BANPROD", /*"services.bdmapp", "W#7hdw!68dxZ",*//*"BSASSBUSR1", "u_pick_it",*/"DOCIDX", "di12345678!",
                                                        Convert.ToInt32(EAxType.AxFeature_Basic));

            if (!validaCarga(GlobalVariables.Matricula, treqCode,
                GlobalVariables.Fndc, GlobalVariables.Aidy, GlobalVariables.Aidp))
            {
                throw new HttpException((int)HttpStatusCode.BadRequest,
                    "Documento no esta en nuestro servidor, volva a intentar.");
            }
            try
            {
                AxDocument.DeleteDocument(treqCode);
            }
            catch (Exception ex)
            {
                throw new HttpException(
                    (int)HttpStatusCode.InternalServerError
                    ,"Su documento no esta en Xtender (" + ex.Message + ")");
            }

            Document document = new Document(GlobalVariables.Matricula, treqCode,
                GlobalVariables.Fndc, GlobalVariables.Aidy, GlobalVariables.Aidp);

            byte[] fileContents = new byte[file.ContentLength];

            Kzrldoc docIndex = new Kzrldoc(GlobalVariables.Matricula, document.aidyCode,
                    document.aidpCode, document.fndcCode, document.clave);

            docIndex.TrstCode = trstCode;
            docIndex.FileName = file.FileName;
            docIndex.FileType = file.ContentType;

            docIndex.Update();

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(
                GlobalVariables.Ftpip + "/" + docIndex.Id);

            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential(GlobalVariables.FtpUser, GlobalVariables.FtpPassword);
            request.UseBinary = true;
            fileContents = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);

            request.ContentLength = fileContents.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
            }


            string result = null;
            try
            {
                AxDocumentCreationData newDocument = new AxDocumentCreationData(403, "BANPROD",
                                "//SRVBDMAPPDEVL/RepositorioAyudasFinancieras/DEVL/" + docIndex.Id/*result*/, EAxFileType.FT_UNKNOWN,
                                true, true, false, 0);

                AxDocumentIndex newDocumentIndex = new AxDocumentIndex("-1",
                        GlobalVariables.Matricula,
                        GlobalVariables.getPdim(GlobalVariables.Matricula),
                        GlobalVariables.Aidy, GlobalVariables.Aidp,
                        GlobalVariables.Fndc, treqCode,
                        GlobalVariables.Aplicacion,
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

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


            return new Document(GlobalVariables.Matricula, treqCode,
                GlobalVariables.Fndc, GlobalVariables.Aidy, GlobalVariables.Aidp);
        }
        public static Document GuardarDocumento(string treqCode)
        {
            HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];

            Document document = new Document(GlobalVariables.Matricula, treqCode,
                GlobalVariables.Fndc, GlobalVariables.Aidy, GlobalVariables.Aidp);
            byte[] fileContents = new byte[file.ContentLength];

            string id = CargaDocsService.KzrldocInsert(document, file);


            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(GlobalVariables.Ftpip + "/" + id);

            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(GlobalVariables.FtpUser, GlobalVariables.FtpPassword);
            request.UseBinary = true;
            fileContents = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);
            // Copy the contents of the file to the request stream.

            request.ContentLength = fileContents.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
            }


            AxServicesInterface axServicesInterface = new AxServicesInterface();
            string sessionTicket = axServicesInterface.Login("", "BANPROD", /*"services.bdmapp", "W#7hdw!68dxZ",*//*"BSASSBUSR1", "u_pick_it",*/"DOCIDX", "di12345678!",
                                                                    Convert.ToInt32(EAxType.AxFeature_Basic));
            string result = null;
            try
            {

                AxDocumentCreationData newDocument = new AxDocumentCreationData(403, "BANPROD",
                                "//SRVBDMAPPDEVL/RepositorioAyudasFinancieras/DEVL/" + id/*result*/, EAxFileType.FT_UNKNOWN,
                                true, true, false, 0);

                AxDocumentIndex newDocumentIndex = new AxDocumentIndex("-1",
                        GlobalVariables.Matricula,
                        GlobalVariables.getPdim(GlobalVariables.Matricula),
                        GlobalVariables.Aidy, GlobalVariables.Aidp,
                        GlobalVariables.Fndc, treqCode,
                        GlobalVariables.Aplicacion,
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                result = axServicesInterface.CreateNewDocument(sessionTicket, newDocument.ToString(),
                                                                       newDocumentIndex.ToString());
                var axDocument = AxDocument.SearchDocument(treqCode);
                axServicesInterface.CloseDocumentByRef(sessionTicket, axDocument.attributes[2].value, false, "");
            }
            catch (Exception ex)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, ex.Message);
            }
            finally
            {
                axServicesInterface.Logout(sessionTicket);
            }
            return new Document(GlobalVariables.Matricula, treqCode,
                GlobalVariables.Fndc, GlobalVariables.Aidy, GlobalVariables.Aidp);
        }


        public static string KzrldocInsert(Document document, HttpPostedFile file)
        {
            DataBase dataBase = new DataBase();
            dataBase.AddParameter("p_kzrldoc_max", "1", OracleDbType.Int32, 8);
            dataBase.ExecuteFunction("SZ_BFQ_CARGADOCSSAF.f_kzrldoc_max", "ID", OracleDbType.Int32, 20);
            String id = dataBase.getOutParamater("ID");


            dataBase = new DataBase();
            Kzrldoc kzrldoc = new Kzrldoc()
            {
                AidpCode = GlobalVariables.Aidp,
                AidyCode = GlobalVariables.Aidy,
                Aplicacion = GlobalVariables.Aplicacion,
                Comment = "Posted by WebApp",
                FileName = file.FileName,
                FileType = file.ContentType,
                FndcCode = document.fndcCode,
                Id = Convert.ToInt32(id),
                Matricula = GlobalVariables.Matricula,
                TreqCode = document.clave,
                TrstCode = document.status
            };
            try
            {
                kzrldoc = kzrldoc.Insert();
            }
            catch (Exception ex)
            {
                throw new HttpException(
                    (int)HttpStatusCode.InternalServerError, ex.Message);
            }
            return kzrldoc.Id.ToString();
        }
    }
}
