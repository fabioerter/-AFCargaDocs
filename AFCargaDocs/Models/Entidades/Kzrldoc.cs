using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace AFCargaDocs.Models.Entidades
{
    public class Kzrldoc
    {
        string matricula;
        int id;
        string aidyCode;
        string aidpCode;
        string fndcCode;
        string treqCode;
        string trstCode;
        DateTime trstDate;
        string fileName;
        string fileType;
        string comment;
        string aplicacion;

        public Kzrldoc()
        {
        }

        public Kzrldoc(string matricula, string aidyCode, string aidpCode, string fndcCode, string treqCode)
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
                treqCode,
                OracleDbType.Varchar2, 32);

            //Out parameters
            dataBase.AddOutParameter("p_kzrldoc_tb",
                OracleDbType.RefCursor, 20);

            //Call of the function
            DataTable p_kzrldoc_tb = dataBase.ExecuteFunction("SZ_BFQ_CARGADOCSSAF.f_kzrldoc_tb",
                "salida",
                OracleDbType.Varchar2, 200).Tables["p_kzrldoc_tb"];
            DataRow row = p_kzrldoc_tb.Rows[0];


            this.TreqCode = row["KZRLDOC_TREQ_CODE"].ToString();
            this.AidpCode = row["KZRLDOC_AIDP_CODE"].ToString();
            this.AidyCode = row["KZRLDOC_AIDY_CODE"].ToString();
            this.Comment = row["KZRLDOC_COMMENT"].ToString();
            this.FileName = row["KZRLDOC_FILE_NAME"].ToString();
            this.FileType = row["KZRLDOC_FILE_TYPE"].ToString();
            this.FndcCode = row["KZRLDOC_FNDC_CODE"].ToString();
            this.Id = Convert.ToInt32(row["KZRLDOC_ID"].ToString());
            this.Matricula = GlobalVariables.Matricula;
            this.TrstCode = row["KZRLDOC_TRST_CODE"].ToString();
            this.TrstDate = Convert.ToDateTime(row["KZRLDOC_TRST_DATE"].ToString());
        }

        public string Matricula { get => matricula; set => matricula = value; }
        public int Id { get => id; set => id = value; }
        public string AidyCode { get => aidyCode; set => aidyCode = value; }
        public string AidpCode { get => aidpCode; set => aidpCode = value; }
        public string FndcCode { get => fndcCode; set => fndcCode = value; }
        public string TreqCode { get => treqCode; set => treqCode = value; }
        public string TrstCode { get => trstCode; set => trstCode = value; }
        public DateTime TrstDate { get => trstDate; set => trstDate = value; }
        public string FileName { get => fileName; set => fileName = value; }
        public string FileType { get => fileType; set => fileType = value; }
        public string Comment { get => comment; set => comment = value; }
        public string Aplicacion { get => aplicacion; set => aplicacion = value; }

        public Kzrldoc Update()
        {
            DataBase dataBase = new DataBase();

            //In parameters
            dataBase.AddParameter("p_pidm",
                GlobalVariables.getPdim(this.Matricula),
                OracleDbType.Int64, 22);
            dataBase.AddParameter("p_id",
                this.Id.ToString(),
                OracleDbType.Varchar2, 16);
            dataBase.AddParameter("p_aidy_code",
                this.AidyCode,
                OracleDbType.Varchar2, 16);
            dataBase.AddParameter("p_aidp_code",
                this.AidpCode,
                OracleDbType.Varchar2, 32);
            dataBase.AddParameter("p_fndc_code",
                this.FndcCode, 
                OracleDbType.Varchar2, 40);
            dataBase.AddParameter("p_apfr_code",
                this.Aplicacion,
                OracleDbType.Varchar2, 32);
            dataBase.AddParameter("p_treq_code",
                this.TreqCode,
                OracleDbType.Varchar2, 32);
            dataBase.AddParameter("p_trst_code",
                this.TrstCode,
                OracleDbType.Varchar2, 16);
            dataBase.AddParameter("p_filename",
                this.FileName,
                OracleDbType.Varchar2, 100);
            dataBase.AddParameter("p_filetype",
                this.FileType,
                OracleDbType.Varchar2, 200);
            dataBase.AddParameter("p_comment",
                this.Comment,
                OracleDbType.Varchar2, 200);
            dataBase.AddParameter("p_trans",
                "U",
                OracleDbType.Varchar2, 16);

            //Out parameters
            dataBase.AddOutParameter("p_Message",
                OracleDbType.Varchar2, 200);

            //Call of the function
            DataTable parameters = dataBase.ExecuteProcedure
                ("SZ_BFA_CARGADOCSSAF.P_TRANS_CARGADOCS").Tables["parameters"];
            var row = parameters.Rows[0]["p_Message"];

            return new Kzrldoc(this.Matricula,this.AidyCode,
                        this.AidpCode,this.FndcCode,this.TreqCode);
        }

        public Kzrldoc Insert()
        {
            DataBase dataBase = new DataBase();

            //In parameters
            dataBase.AddParameter("p_pidm",
                GlobalVariables.getPdim(this.Matricula),
                OracleDbType.Int64, 22);
            dataBase.AddParameter("p_id",
                this.Id.ToString(),
                OracleDbType.Varchar2, 16);
            dataBase.AddParameter("p_aidy_code",
                this.AidyCode,
                OracleDbType.Varchar2, 16);
            dataBase.AddParameter("p_aidp_code",
                this.AidpCode,
                OracleDbType.Varchar2, 32);
            dataBase.AddParameter("p_fndc_code",
                this.FndcCode,
                OracleDbType.Varchar2, 40);
            dataBase.AddParameter("p_apfr_code",
                this.Aplicacion,
                OracleDbType.Varchar2, 32);
            dataBase.AddParameter("p_treq_code",
                this.TreqCode,
                OracleDbType.Varchar2, 32);
            dataBase.AddParameter("p_trst_code",
                this.TrstCode,
                OracleDbType.Varchar2, 16);
            dataBase.AddParameter("p_filename",
                this.FileName,
                OracleDbType.Varchar2, 100);
            dataBase.AddParameter("p_filetype",
                this.FileType,
                OracleDbType.Varchar2, 200);
            dataBase.AddParameter("p_comment",
                this.Comment,
                OracleDbType.Varchar2, 200);
            dataBase.AddParameter("p_trans",
                "I",
                OracleDbType.Varchar2, 16);

            //Out parameters
            dataBase.AddOutParameter("p_Message",
                OracleDbType.Varchar2, 200);

            //Call of the function
            DataTable parameters = dataBase.ExecuteProcedure
                ("SZ_BFA_CARGADOCSSAF.P_TRANS_CARGADOCS").Tables["parameters"];
            var row = parameters.Rows[0]["p_Message"];

            return new Kzrldoc(this.Matricula, this.AidyCode,
                        this.AidpCode, this.FndcCode, this.TreqCode);
        }
    }

}

