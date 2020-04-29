
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
        public static bool DisplayFileFromServer(Uri serverUri)
        {
            // The serverUri parameter should start with the ftp:// scheme.
            if (serverUri.Scheme != Uri.UriSchemeFtp)
            {
                return false;
            }
            // Get the object used to communicate with the server.
            WebClient request = new WebClient();

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential("anonymous", "janeDoe@contoso.com");
            try
            {
                byte[] newFileData = request.DownloadData(serverUri.ToString());
                string fileString = System.Text.Encoding.UTF8.GetString(newFileData);
                Console.WriteLine(fileString);
            }
            catch (WebException e)
            {
                Console.WriteLine(e.ToString());
            }
            return true;
        }
        public static Document insertDocument(string matricula, string clave, string fndcCode,
                                            string aidyCode, string aidpCode, HttpPostedFile file)
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://148.238.49.217/" + file.FileName);

            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential("ftpDevlAyudaFN", "Rep0AyudaFnD3vl$");

            // Copy the contents of the file to the request stream.
            string fileStream = new StreamReader( file.InputStream).ReadToEnd();
            byte[] fileContents = Encoding.UTF8.GetBytes(fileStream);

            request.ContentLength = fileContents.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
            }



            Document document = new Document(matricula, clave, fndcCode, aidyCode, aidpCode);
            if (document.status != "PS")
            {
                throw new HttpException(Convert.ToInt32(HttpStatusCode.BadRequest), "El Documento ya esta en nuestro servidor");
            }
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
                }); ;
                comando.Parameters.Add(new OracleParameter("p_user_id", OracleDbType.Varchar2)
                {
                    Value = "cargaDocsWeb",
                    Size = 20
                });
                comando.Parameters.Add(new OracleParameter("p_rowid_out", OracleDbType.Varchar2)
                {
                    Direction = System.Data.ParameterDirection.Output,
                    Size = 18
                }); ;
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
    }
}
