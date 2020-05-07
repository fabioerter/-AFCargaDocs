using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace AFCargaDocs.Models.Entidades
{
    /// <summary>
    /// Estructura que representa una petición de un token de sesión
    /// </summary>
    public class Document
    {
        public Document()
        {
        }

        public Document(string matricula, string clave, string fndcCode, string aidyCode, string aidpCode)
        {
            DataTable dataTable;
            StringBuilder query = new StringBuilder();

            query.Append(" SELECT KVRTRFN_FNDC_CODE,");
            query.Append("        KVRTRFN_AIDY_CODE,");
            query.Append("        KVRTRFN_AIDP_CODE,");
            query.Append("        KVRTRFN_TREQ_CODE,");
            query.Append("        KVVTREQ_DESC,");
            query.Append("        NVL((SELECT KVRAREQ_TRST_CODE");
            query.Append("             FROM KVRAREQ");
            query.Append("             WHERE KVRAREQ_PIDM = F_UDEM_STU_PIDM(:matricula)");
            query.Append("               AND KVRAREQ_AIDY_CODE = KVRTRFN_AIDY_CODE");
            query.Append("               AND KVRAREQ_AIDP_CODE = KVRTRFN_AIDP_CODE");
            query.Append("               AND KVRAREQ_TREQ_CODE = KVRTRFN_TREQ_CODE");
            query.Append("            ), 'PS')                  STATUS,");
            query.Append("        (SELECT KVRAREQ_TRST_DATE");
            query.Append("             FROM KVRAREQ");
            query.Append("             WHERE KVRAREQ_PIDM = F_UDEM_STU_PIDM(:matricula1)");
            query.Append("               AND KVRAREQ_AIDY_CODE = KVRTRFN_AIDY_CODE");
            query.Append("               AND KVRAREQ_AIDP_CODE = KVRTRFN_AIDP_CODE");
            query.Append("               AND KVRAREQ_TREQ_CODE = KVRTRFN_TREQ_CODE");
            query.Append("            ) ACTIVITY_DATE");
            query.Append(" FROM KVRTRFN,");
            query.Append("      KVVTREQ");
            query.Append(" WHERE KVRTRFN_FNDC_CODE = :fndcCode");
            query.Append("   AND KVRTRFN_AIDY_CODE = :aidyCode");
            query.Append("   AND KVRTRFN_AIDP_CODE = :aidpCode");
            query.Append("   AND KVRTRFN_TREQ_CODE = :treqCode");
            query.Append("   AND KVRTRFN_TREQ_CODE = KVVTREQ_CODE");
            query.Append("   AND KVRTRFN_TREQ_CODE NOT IN (SELECT KVRAREQ_TREQ_CODE");
            query.Append("                                 FROM KVRAREQ");
            query.Append("                                 WHERE KVRAREQ_PIDM = F_UDEM_STU_PIDM(:matricula2)");
            query.Append("                                   AND KVRAREQ_AIDY_CODE = KVRTRFN_AIDY_CODE");
            query.Append("                                   AND KVRAREQ_AIDP_CODE = KVRTRFN_AIDP_CODE");
            query.Append("                                   AND KVRAREQ_TRST_CODE = 'NQ'");
            query.Append(" )");
            query.Append("");

            try
            {
                DataBase dataBase = new DataBase();
                dataBase.AddFilter("matricula", matricula);
                dataBase.AddFilter("matricula1", matricula);
                dataBase.AddFilter("fndcCode", fndcCode);
                dataBase.AddFilter("aidyCode", aidyCode);
                dataBase.AddFilter("aidpCode", aidpCode);
                dataBase.AddFilter("treqCode", clave);
                dataBase.AddFilter("matricula2", matricula);


                dataTable = dataBase.ExecuteQuery(query.ToString());
            }
            catch (Exception ex)
            {
                throw new HttpException(ex.ToString(), Convert.ToInt32(HttpStatusCode.InternalServerError));
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            if (dataTable.Rows.Count > 1)
            {
                throw new HttpException("To many Rows", Convert.ToInt32(HttpStatusCode.InternalServerError));
            }
            DataRow dr = dataTable.Rows[0];
            this.fndcCode = dr[0].ToString();
            this.aidyCode = dr[1].ToString();
            this.aidpCode = dr[2].ToString();
            this.clave = dr[3].ToString();
            this.name = dr[4].ToString();
            this.status = dr[5].ToString();
            if (dr[6] == System.DBNull.Value)
            {
                this.fecha = new DateTime(1900, 1, 1)
                            .ToString("dd-MMM-yyyy").Replace(".", "").ToUpper();
            }
            else
            {
                this.fecha = Convert.ToDateTime(dr[6].ToString())
                            .ToString("dd-MMM-yyyy").Replace(".", "").ToUpper();
            }
        }


        /// <summary>
        /// Clabe del archivo (treqCod)
        /// </summary>
        public string clave { get; set; }
        /// <summary>
        /// Nome del archivo
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Status del archivo (NR = no revisado o PENDIENTE POR REVIAR)
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// Fecha del archivo
        /// </summary>
        public string fecha { get; set; }
        [JsonIgnore]
        public string fndcCode { get; set; }
        [JsonIgnore]
        public string aidyCode { get; set; }
        [JsonIgnore]
        public string aidpCode { get; set; }
    }
}