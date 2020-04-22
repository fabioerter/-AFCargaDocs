
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
        public static Document[] ObtenerDocumentos(string matricula)
        {
            DataTable dataTable;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT KVRTRFN_TREQ_CODE             REQUISITO,");
            query.Append("        KVVTREQ_DESC                  DESCRIPCION,");
            query.Append("        NVL((SELECT KVRAREQ_TRST_CODE");
            query.Append("             FROM KVRAREQ --En esta tabla se van registrando los requisitos de la solicitud del alumno");
            query.Append("             WHERE KVRAREQ_PIDM = F_UDEM_STU_PIDM('000549681')--P_PIDM");
            query.Append("               AND KVRAREQ_AIDY_CODE = KVRTRFN_AIDY_CODE");
            query.Append("               AND KVRAREQ_AIDP_CODE = KVRTRFN_AIDP_CODE");
            query.Append("               AND KVRAREQ_TREQ_CODE = KVRTRFN_TREQ_CODE");
            query.Append("            ), 'PS')                  STATUS,");
            query.Append("        NVL((SELECT KVRAREQ_TRST_DATE");
            query.Append("             FROM KVRAREQ --En esta tabla se van registrando los requisitos de la solicitud del alumno");
            query.Append("             WHERE KVRAREQ_PIDM = F_UDEM_STU_PIDM('000549681')--P_PIDM");
            query.Append("               AND KVRAREQ_AIDY_CODE = KVRTRFN_AIDY_CODE");
            query.Append("               AND KVRAREQ_AIDP_CODE = KVRTRFN_AIDP_CODE");
            query.Append("               AND KVRAREQ_TREQ_CODE = KVRTRFN_TREQ_CODE");
            query.Append("            ), to_date('01-01-1900')) ACTIVITY_DATE,");
            query.Append("        KVRTRFN_FNDC_CODE             FONDO,");
            query.Append("        KVRTRFN_AIDY_CODE             AÑO_AYUDA,");
            query.Append("        KVRTRFN_AIDP_CODE             PERIODO_AYUDA");
            query.Append(" FROM KVRTRFN,--Tabla en donde estan configurados todos los requisitos de la Solicitud");
            query.Append("      KVVTREQ");
            query.Append(" WHERE KVRTRFN_FNDC_CODE = 'PBUPNI' --V_FNDC");
            query.Append("   AND KVRTRFN_AIDY_CODE = '2111'   --V_AIDY");
            query.Append("   AND KVRTRFN_AIDP_CODE = 'PR'     --V_AIDP");
            query.Append("   AND KVRTRFN_TREQ_CODE = KVVTREQ_CODE");
            query.Append("   AND KVRTRFN_TREQ_CODE NOT IN (SELECT KVRAREQ_TREQ_CODE");
            query.Append("                                 FROM KVRAREQ --En esta tabla se van registrando los requisitos de la solicitud del alumno");
            query.Append("                                 WHERE KVRAREQ_PIDM = F_UDEM_STU_PIDM('000549681')--P_PIDM");
            query.Append("                                   AND KVRAREQ_AIDY_CODE = KVRTRFN_AIDY_CODE");
            query.Append("                                   AND KVRAREQ_AIDP_CODE = KVRTRFN_AIDP_CODE");
            query.Append("                                   AND KVRAREQ_TRST_CODE = 'NQ'");
            query.Append(" );");
            query.Append("");

            try
            {
                DataBase dataBase = new DataBase();
                dataBase.AddFilter("matricula", matricula);
                dataBase.AddFilter("fndcCode", "PBUPNI");
                dataBase.AddFilter("aidyCode", "2111");
                dataBase.AddFilter("aidpCode", "PR");


                dataTable = dataBase.ExecuteQuery(query.ToString());
            }
            catch (Exception ex)
            {
                throw new HttpException(ex.ToString(), Convert.ToInt32(HttpStatusCode.BadRequest));
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            List<Document> documentList = new List<Document>();
            foreach (DataRow dr in dataTable.Rows)
            {
                Document document = new Document()
                {
                    Clave = dr[0].ToString(),
                    Name = dr[1].ToString(),
                    Fecha = Convert.ToDateTime(dr[2].ToString()).ToShortDateString(),
                    Status = dr[3].ToString()
                };
                documentList.Add(document);
            }
            return documentList.ToArray();
            //using (OracleConnection cnx = new OracleConnection(ConfigurationManager.ConnectionStrings["Banner"].ConnectionString))
            //{
            //    cnx.Open();

            //    // Preparamos la consulta
            //    OracleDataReader reader = new OracleCommand("SELECT * FROM KVRTRFN", cnx).ExecuteReader();
            //    List<Document> documentList = new List<Document>();
            //    try
            //    {
            //        while (reader.Read())
            //        {
            //            Console.WriteLine(reader.GetString(0) + ", " + reader.GetString(1));
            //            String teste = reader.GetString(1);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new HttpException(ex.ToString(), Convert.ToInt32(HttpStatusCode.BadRequest));
            //        /*throw new HttpResponseException(HttpStatusCode.BadRequest);*/
            //    }
            //    return documentList;
            //}
        }
    }






}
