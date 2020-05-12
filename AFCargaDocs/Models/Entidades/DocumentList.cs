using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace AFCargaDocs.Models.Entidades
{
    public class DocumentList
    {
        List<Document> documents;
        string matricula;
        string fndcCode;
        string aidyCode;
        string aidpCode;

        public DocumentList(string matricula, string fndcCode, string aidyCode, string aidpCode)
        {
            documents = new List<Document>();
            this.matricula = matricula;
            this.fndcCode = fndcCode;
            this.aidyCode = aidyCode;
            this.aidpCode = aidpCode;
        }

        public List<Document> GetDocumentList()
        {
            DataTable dataTable;
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT KVRTRFN_TREQ_CODE             REQUISITO,");
            query.Append("        KVVTREQ_DESC                  DESCRIPCION,");
            query.Append("        NVL((SELECT KVRAREQ_TRST_CODE");
            query.Append("             FROM KVRAREQ ");
            query.Append("             WHERE KVRAREQ_PIDM = F_UDEM_STU_PIDM(:matricula2)");
            query.Append("               AND KVRAREQ_AIDY_CODE = KVRTRFN_AIDY_CODE");
            query.Append("               AND KVRAREQ_AIDP_CODE = KVRTRFN_AIDP_CODE");
            query.Append("               AND KVRAREQ_TREQ_CODE = KVRTRFN_TREQ_CODE");
            query.Append("            ), 'PS')                  STATUS,");
            query.Append("        (SELECT KVRAREQ_TRST_DATE");
            query.Append("             FROM KVRAREQ ");
            query.Append("             WHERE KVRAREQ_PIDM = F_UDEM_STU_PIDM(:matricula1)");
            query.Append("               AND KVRAREQ_AIDY_CODE = KVRTRFN_AIDY_CODE");
            query.Append("               AND KVRAREQ_AIDP_CODE = KVRTRFN_AIDP_CODE");
            query.Append("               AND KVRAREQ_TREQ_CODE = KVRTRFN_TREQ_CODE");
            query.Append("            ) ACTIVITY_DATE");
            query.Append(" FROM KVRTRFN,");
            query.Append("      KVVTREQ");
            query.Append(" WHERE KVRTRFN_AIDY_CODE = :aidyCode");
            query.Append("   AND KVRTRFN_AIDP_CODE = :aidpCode");
            query.Append("   AND KVRTRFN_FNDC_CODE = :fndcCode");
            query.Append("   AND KVRTRFN_TREQ_CODE = KVVTREQ_CODE");
            query.Append("   AND KVRTRFN_TREQ_CODE NOT IN (SELECT KVRAREQ_TREQ_CODE");
            query.Append("                                 FROM KVRAREQ ");
            query.Append("                                 WHERE KVRAREQ_PIDM = F_UDEM_STU_PIDM(:matricula)");
            query.Append("                                   AND KVRAREQ_AIDY_CODE = KVRTRFN_AIDY_CODE");
            query.Append("                                   AND KVRAREQ_AIDP_CODE = KVRTRFN_AIDP_CODE");
            query.Append("                                   AND KVRAREQ_TRST_CODE = 'NQ')");
            query.Append(" ORDER BY KVVTREQ_DESC");
            query.Append("");

            try
            {
                DataBase dataBase = new DataBase();
                dataBase.AddFilter("matricula2", matricula);
                dataBase.AddFilter("matricula1", matricula);
                dataBase.AddFilter("aidyCode", aidyCode);
                dataBase.AddFilter("aidpCode", aidpCode);
                dataBase.AddFilter("fndcCode", fndcCode);
                dataBase.AddFilter("matricula", matricula);

                dataTable = dataBase.ExecuteQuery(query.ToString());
            }
            catch (Exception ex)
            {
                throw new HttpException(ex.ToString(), Convert.ToInt32(HttpStatusCode.BadRequest));
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    Document document = new Document()
                    {
                        clave = dr[0].ToString(),
                        name = dr[1].ToString(),
                        status = dr[2].ToString()
                    };
                    if (dr[3] == System.DBNull.Value)
                    {
                        document.fecha = new DateTime(1900, 1, 1)
                                    .ToString("dd-MMM-yyyy").Replace(".", "").ToUpper();
                    }
                    else
                    {
                        document.fecha = Convert.ToDateTime(dr[3].ToString())
                                    .ToString("dd-MMM-yyyy").Replace(".", "").ToUpper();
                    }
                    this.documents.Add(document);
                }
            }
            return this.documents;
        }
    }
}