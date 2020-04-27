
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
        public static Document ObtenerDatosDocumento(string matricula, string clave, string fndcCode, string aidyCode, string aidpCode)
        {
            return new Document(matricula, clave, fndcCode, aidyCode, aidpCode);

        }
        public static Document insertDocument(string matricula, string clave, string fndcCode, string aidyCode, string aidpCode)
        {
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
                    Value = Convert.ToDateTime(document.fecha).ToString("dd-MMM-yyyy")
                                                                .Replace(".", "").ToUpper(),
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

                }
                catch (Exception ex)
                {
                    throw new HttpException(Convert.ToInt32(HttpStatusCode.InternalServerError), ex.Message);
                }
                finally
                {
                    oracleTransaction.Rollback();
                    cnx.Close();
                }
                return document;

            }

        }
    }
}
