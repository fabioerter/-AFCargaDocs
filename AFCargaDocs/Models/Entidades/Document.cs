 using Newtonsoft.Json;
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
            DataBase dataBase = new DataBase();

            //In parameters
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
            dataBase.AddParameter("p_treq_code",
                clave,
                OracleDbType.Varchar2, 32);

            //Out parameters
            dataBase.AddOutParameter("p_obdocs_one",
                OracleDbType.RefCursor, 20);

            //Call of the function
            DataTable p_obdocs_one = dataBase.ExecuteFunction("SZ_BFQ_CARGADOCSSAF.f_obdocs_one",
                "salida",
                OracleDbType.Varchar2, 200).Tables["p_obdocs_one"];
        
            if (p_obdocs_one.Rows.Count == 0)
            {
                throw new HttpException(
                    (int)(HttpStatusCode.InternalServerError),
                    "Incapaz de recuperar su documento");
            }
            if (p_obdocs_one.Rows.Count > 1)
            {
                throw new HttpException(
                    (int)(HttpStatusCode.InternalServerError)
                    ,"To many Rows" );
            }

            DataRow row = p_obdocs_one.Rows[0];

            this.clave = row["KVRTRFN_TREQ_CODE"].ToString();
            this.status = row["STATUS"].ToString();
            this.name = row["KVVTREQ_DESC"].ToString();
            this.fecha = row["ACTIVITY_DATE"].ToString();
            this.comment = row["COMMENTS"].ToString();
            this.aidpCode = row["KVRTRFN_AIDP_CODE"].ToString();
            this.aidyCode = row["KVRTRFN_AIDY_CODE"].ToString();
            this.fndcCode = row["KVRTRFN_FNDC_CODE"].ToString();
            if (row["ACTIVITY_DATE"] == System.DBNull.Value)
            {
                this.fecha = new DateTime(1900, 1, 1)
                            .ToString("dd-MMM-yyyy").Replace(".", "").ToUpper();
            }
            else
            {
                this.fecha = Convert.ToDateTime(row["ACTIVITY_DATE"].ToString())
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
        /// <summary>
        /// comentario del archivo
        /// </summary>
        public string comment { get; set; }
        [JsonIgnore]
        public string fndcCode { get; set; }
        [JsonIgnore]
        public string aidyCode { get; set; }
        [JsonIgnore]
        public string aidpCode { get; set; }
    }
}