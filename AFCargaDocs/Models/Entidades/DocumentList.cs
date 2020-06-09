using Oracle.ManagedDataAccess.Client;
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
            DataBase dataBase = new DataBase();

            dataBase.AddParameter("p_pidm",
                GlobalVariables.getPdim(matricula),
                OracleDbType.Int64, 22);
            dataBase.AddParameter("p_aidy_code",
                aidyCode,
                OracleDbType.Varchar2, 16);
            dataBase.AddParameter("p_aidp_code",
                aidpCode,
                OracleDbType.Varchar2, 32);
            dataBase.AddParameter("p_fndc_code",
                fndcCode, OracleDbType.Varchar2, 40);

            dataBase.AddOutParameter("p_obdocs_all",
                 OracleDbType.RefCursor, 20);
            DataTable pObdocsAll = dataBase.ExecuteFunction("SZ_BFQ_CARGADOCSSAF.f_obdocs_ps",
                                    "salida",OracleDbType.Varchar2, 200).Tables["p_obdocs_all"];


            foreach (DataRow row in pObdocsAll.Rows)
            {
                Document temp = new Document()
                {
                    clave = row["REQUISITO"].ToString(),
                    status = row["STATUS"].ToString(),
                    name = row["DESCRIPCION"].ToString(),
                    fecha = row["ACTIVITY_DATE"].ToString(),
                    comment = row["COMMENTS"].ToString()
                };
                if (row["ACTIVITY_DATE"] == System.DBNull.Value)
                {
                    temp.fecha = new DateTime(1900, 1, 1)
                                .ToString("dd-MMM-yyyy").Replace(".", "").ToUpper();
                }
                else
                {
                    temp.fecha = Convert.ToDateTime(row["ACTIVITY_DATE"].ToString())
                                    .ToString("dd-MMM-yyyy").Replace(".", "").ToUpper();
                }
                this.documents.Add(temp);
            }
            return this.documents;
        }
    }
}