
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


            request.Credentials = new NetworkCredential("ftpDevlAyudaFN", "Rep0AyudaFnD3vl$");

            try
            {
                byte[] newFileData = request.DownloadData(new Uri("ftp://148.238.49.217/") + file.FileId + file.FileName);
                file.FileContent = Convert.ToBase64String(newFileData);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{1}", e.Message);
            }

            return file;
        }
        public static Document insertDocument(string matricula, string clave, string fndcCode,
                                            string aidyCode, string aidpCode, HttpPostedFile file)
        {
            
            Document document = new Document(matricula, clave, fndcCode, aidyCode, aidpCode);
            byte[] fileContents = new byte[file.ContentLength];
            if (document.status != "PS")
            {
                throw new HttpException(Convert.ToInt32(HttpStatusCode.BadRequest), "El Documento ya esta en nuestro servidor");
            }

            string id = KzrldocInsert(document, file);


            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://148.238.49.217/"  + id + file.FileName);

            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential("ftpDevlAyudaFN", "Rep0AyudaFnD3vl$");
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


        private static string KzrldocInsert(Document document, HttpPostedFile file)
        {

            DataBase dataBase = new DataBase();
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT NVL((SELECT MAX(KZRLDOC_ID) + 1 ");
            query.Append("             FROM KZRLDOC), 1) ID ");
            query.Append(" FROM DUAL ");
            String id = (dataBase.ExecuteQuery(query.ToString()).Rows[0][0]).ToString();

            query.Clear();
            query.Append(" INSERT INTO KZRLDOC (KZRLDOC_PIDM, KZRLDOC_ID, KZRLDOC_AIDY_CODE, KZRLDOC_AIDP_CODE, KZRLDOC_FNDC_CODE,");
            query.Append("                      KZRLDOC_TREQ_CODE, KZRLDOC_TRST_CODE, KZRLDOC_TRST_DATE, KZRLDOC_FILE_NAME, KZRLDOC_FILE_TYPE,");
            query.Append("                      KZRLDOC_COMMENT, KZRLDOC_ACTIVITY_DATE, KZRLDOC_SURROGATE_ID, KZRLDOC_VERSION, KZRLDOC_USER_ID,");
            query.Append("                      KZRLDOC_VPDI_CODE)");
            query.Append(" VALUES (F_UDEM_STU_PIDM(:matricula),");
            query.Append("         (NVL((SELECT MAX(KZRLDOC_ID) + 1");
            query.Append("          FROM KZRLDOC),1)),");
            query.Append("         :aidyCode, :aidpCod, :fndcCode,");
            query.Append("         :treqCode, :trstCode,");
            query.Append("         SYSDATE, :fileName, :fileType, :comments,");
            query.Append("         SYSDATE, null, null, 'BANINST1', null) ");

            dataBase = new DataBase();
            dataBase.AddFilter("matricula", GlobalVariables.Matricula);
            dataBase.AddFilter("aidyCode", GlobalVariables.Aidy);
            dataBase.AddFilter("aidpCod", GlobalVariables.Aidp);
            dataBase.AddFilter("fndcCode", document.fndcCode);
            dataBase.AddFilter("treqCode", document.clave);
            dataBase.AddFilter("trstCode", document.status);
            dataBase.AddFilter("fileName", file.FileName);
            dataBase.AddFilter("fileType", file.ContentType);
            dataBase.AddFilter("comments", "Posted by WebApp");
            dataBase.ExecuteQuery(query.ToString());
            return id;
        }
    }
}
