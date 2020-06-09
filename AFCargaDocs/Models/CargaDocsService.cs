
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
        public static FileInfoFtp DisplayFileFromServer(string matricula, string clave,
                                                string fndcCode, string aidyCode, string aidpCode)
        {
            Document document = new Document(matricula, clave, fndcCode, aidyCode, aidpCode);
            if (document.status == "PS")
            {
                Console.WriteLine($"Su documento no esta en nuestro sistema!");
                throw new HttpException((int)HttpStatusCode.InternalServerError,
                                    "Su documento no esta en nuestro sistema!");
            }
            FileInfoFtp file = new FileInfoFtp(clave);
            WebClient request = new WebClient();


            request.Credentials = new NetworkCredential(GlobalVariables.FtpUser, GlobalVariables.FtpPassword);

            try
            {
                Stream streamTemp = null;
                byte[] newFileData = request.DownloadData(new Uri(GlobalVariables.Ftpip) + "/" + file.FileId);
                //if (file.FileType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                //{
                //    Converter toPdf = new Converter();
                //    toPdf.convert(newFileData,Format.DOCX, streamTemp, Format.PDF);
                //    file.FileContent = GlobalVariables.ConvertToBase64(streamTemp);
                //}
                file.FileContent = Convert.ToBase64String(newFileData);
            }
            catch (Exception ex)
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError, ex.Message);
            }

            if (file.FileContent == null)
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError, "El Documento no esta en nuestro servidor");
            }

            return file;
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

            Document document = new Document(GlobalVariables.Matricula, treqCode,
                GlobalVariables.Fndc, GlobalVariables.Aidy, GlobalVariables.Aidp);

            byte[] fileContents = new byte[file.ContentLength];

            Kzrldoc docIndex = new Kzrldoc(GlobalVariables.Matricula, document.aidyCode,
                    document.aidpCode, document.fndcCode, document.clave);

            docIndex.TrstCode = trstCode;
            docIndex.FileName = file.FileName;
            docIndex.FileType = file.ContentType;

            docIndex.Update();

            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(
                GlobalVariables.Ftpip + "/" + docIndex.Id);

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


            string result = null;
            try
            {
                AxQueryData axQueryData = new AxQueryData();
                axQueryData.Appid = "403";
                axQueryData.Qtype = EAXQueryType.Normal;
                axQueryData.addField(1, false, GlobalVariables.Matricula);
                axQueryData.addField(2, false, GlobalVariables.getPdim(
                                                GlobalVariables.Matricula));
                axQueryData.addField(3, false, GlobalVariables.Aidy);
                axQueryData.addField(4, false, GlobalVariables.Aidp);
                axQueryData.addField(5, false, treqCode);
                axQueryData.addField(6, true, "");
                axQueryData.addField(7, false, GlobalVariables.Fndc);
                axQueryData.addField(8, false, GlobalVariables.Aplicacion);
                axQueryData.addField(9, false, GlobalVariables.Matricula);
                axQueryData.addField(1, false, GlobalVariables.Matricula);
                axQueryData.addField(1, false, GlobalVariables.Matricula);
                axQueryData.addField(1, false, GlobalVariables.Matricula);
                axQueryData.addField(1, false, GlobalVariables.Matricula);
                axQueryData.addField(1, false, GlobalVariables.Matricula);
                axQueryData.addField(1, false, GlobalVariables.Matricula);



                axServicesInterface.QueryDocuments(sessionTicket, "BANPROD",
                    axQueryData.ToString(), 0, 1, 1, false, false);

                AxDocumentPointer axDocumentPointer = new AxDocumentPointer("BANPROD", 403, 1, EAxDocumentType.Document, 0);

                //axServicesInterface.DeleteDocument(sessionTicket,);


                AxDocumentCreationData newDocument = new AxDocumentCreationData(403, "BANPROD",
                                "//SRVBDMAPPDEVL/RepositorioAyudasFinancieras/TEST/" + docIndex.Id/*result*/, EAxFileType.FT_UNKNOWN,
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

        public static Document insertDocument(string matricula, string clave, string fndcCode,
                                        string aidyCode, string aidpCode, HttpPostedFile file)
        {

            Document document = new Document(matricula, clave, fndcCode, aidyCode, aidpCode);
            byte[] fileContents = new byte[file.ContentLength];
            if (document.status != "PS")
            {
                throw new HttpException(Convert.ToInt32(HttpStatusCode.BadRequest),
                    "El Documento ya esta en nuestro servidor");
            }

            string id = KzrldocInsert(document, file);


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

            //DataBase dataBase = new DataBase();

            //dataBase.AddParameter("p_pidm", matricula, OracleDbType.Int16, 8);
            //dataBase.AddParameter("p_aidy_code", document.aidyCode, OracleDbType.Varchar2, 20);
            //dataBase.AddParameter("p_aidp_code", document.aidpCode, OracleDbType.Varchar2, 20);
            //dataBase.AddParameter("p_treq_code", document.clave, OracleDbType.Varchar2, 20);
            //dataBase.AddParameter("p_trst_code", "NR", OracleDbType.Varchar2, 20);
            //dataBase.AddParameter("p_trst_date",
            //    DateTime.Now.ToString("dd-MMM-yyyy").Replace(".", "").ToUpper(),
            //    OracleDbType.Varchar2, 20);
            //dataBase.AddParameter("p_establish_date",
            //    DateTime.Now.ToString("dd-MMM-yyyy").Replace(".", "").ToUpper(),
            //    OracleDbType.Varchar2, 20);
            //dataBase.AddParameter("p_comment", "Upload by web service", OracleDbType.Varchar2, 9);
            //dataBase.AddParameter("p_data_origin", null, OracleDbType.Varchar2, 20);
            //dataBase.AddParameter("p_create_user_id", "cargaDocsWeb", OracleDbType.Varchar2, 20);
            //dataBase.AddParameter("p_create_date",
            //    DateTime.Now.ToString("dd-MMM-yyyy").Replace(".", "").ToUpper(),
            //    OracleDbType.Varchar2, 20);
            //dataBase.AddParameter("p_user_id", "cargaDocsWeb", OracleDbType.Varchar2, 20);
            //dataBase.AddOutParameter("p_rowid_out", OracleDbType.Varchar2, 18);

            //try
            //{
            //    _ = dataBase.ExecuteProcedure("KV_APPLICANT_TRK_REQT.P_CREATE");
            //}
            //catch (Exception ex)
            //{
            //    throw new HttpException((int)HttpStatusCode.InternalServerError, ex.Message);
            //}
            //return new Document(GlobalVariables.Matricula, document.clave,
            //                    document.fndcCode, document.aidyCode, document.aidpCode);

            // insert en tabla 
            using (OracleConnection cnx = new OracleConnection(ConfigurationManager.ConnectionStrings["Banner"].ConnectionString))
            {
                cnx.Open();
                OracleTransaction oracleTransaction = cnx.BeginTransaction();
                OracleCommand comando = new OracleCommand();
                comando.Connection = cnx;
                comando.CommandText = @"KV_APPLICANT_TRK_REQT.P_CREATE";
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Transaction = oracleTransaction;


                comando.Parameters.Add(new OracleParameter("p_pidm", OracleDbType.Int16)
                {
                    Value = GlobalVariables.getPdim(matricula),
                    Size = 8
                });
                comando.Parameters.Add(new OracleParameter("p_aidy_code", OracleDbType.Varchar2)
                {
                    Value = document.aidyCode,
                    Size = 20
                });
                comando.Parameters.Add(new OracleParameter("p_aidp_code", OracleDbType.Varchar2)
                {
                    Value = document.aidpCode,
                    Size = 20
                });
                comando.Parameters.Add(new OracleParameter("p_treq_code", OracleDbType.Varchar2)
                {
                    Value = document.clave,
                    Size = 20
                });
                comando.Parameters.Add(new OracleParameter("p_trst_code", OracleDbType.Varchar2)
                {
                    Value = "NR",
                    Size = 20
                });
                comando.Parameters.Add(new OracleParameter("p_trst_date", OracleDbType.Varchar2)
                {
                    Value = DateTime.Now.ToString("dd-MMM-yyyy").Replace(".", "").ToUpper(),
                    Size = 20
                });
                comando.Parameters.Add(new OracleParameter("p_establish_date", OracleDbType.Varchar2)
                {
                    Value = DateTime.Now.ToString("dd-MMM-yyyy").Replace(".", "").ToUpper(),
                    Size = 20
                });
                comando.Parameters.Add(new OracleParameter("p_comment", OracleDbType.Varchar2)
                {
                    Value = "teste",
                    Size = 9
                });
                comando.Parameters.Add(new OracleParameter("p_data_origin", OracleDbType.Varchar2)
                {
                    Value = null,
                    Size = 9
                });
                comando.Parameters.Add(new OracleParameter("p_create_user_id", OracleDbType.Varchar2)
                {
                    Value = "cargaDocsWeb",
                    Size = 20
                });
                comando.Parameters.Add(new OracleParameter("p_create_date", OracleDbType.Varchar2)
                {
                    Value = DateTime.Now.ToString("dd-MMM-yyyy").Replace(".", "").ToUpper(),
                    Size = 20
                });
                comando.Parameters.Add(new OracleParameter("p_user_id", OracleDbType.Varchar2)
                {
                    Value = "cargaDocsWeb",
                    Size = 20
                });
                comando.Parameters.Add(new OracleParameter("p_rowid_out", OracleDbType.Varchar2)
                {
                    Direction = System.Data.ParameterDirection.Output,
                    Size = 18
                });
                try
                {


                    comando.ExecuteNonQuery();
                    string lector = (comando.Parameters["p_rowid_out"].Value).ToString();
                    document = new Document(GlobalVariables.Matricula, document.clave,
                                document.fndcCode, document.aidyCode, document.aidpCode);
                    oracleTransaction.Commit();
                }
                catch (Exception ex)
                {
                    oracleTransaction.Rollback();
                    throw new HttpException(Convert.ToInt32(HttpStatusCode.InternalServerError), ex.Message);
                }
                finally
                {
                    cnx.Close();
                }


                return document;

            }

        }
        public static Document teste2(string treqCode)
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
                                "//SRVBDMAPPDEVL/RepositorioAyudasFinancieras/TEST/" + id/*result*/, EAxFileType.FT_UNKNOWN,
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


        public static string KzrldocInsert(Document document, HttpPostedFile file)
        {
            DataBase dataBase = new DataBase();
            dataBase.AddParameter("p_kzrldoc_max", "1", OracleDbType.Int32, 8);
            dataBase.ExecuteFunction("SZ_BFQ_CARGADOCSSAF.f_kzrldoc_max", "ID", OracleDbType.Int32, 20);
            String id = dataBase.getOutParamater("ID");


            dataBase = new DataBase();
            Kzrldoc kzrldoc = new Kzrldoc() { 
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
            }catch(Exception ex)
            {
                throw new HttpException(
                    (int)HttpStatusCode.InternalServerError, ex.Message);
            }
            return kzrldoc.Id.ToString();
        }
    }
}
