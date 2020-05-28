using Newtonsoft.Json.Linq;
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

        public Kzrldoc(string matricula, string aidyCode, string aidpCode, string fndcCode, string treqCode)
        {
            DataBase dataBase = new DataBase();
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT KZRLDOC_PIDM,");
            sb.Append("       KZRLDOC_ID,");
            sb.Append("       KZRLDOC_AIDY_CODE,");
            sb.Append("       KZRLDOC_AIDP_CODE,");
            sb.Append("       KZRLDOC_FNDC_CODE,");
            sb.Append("       KZRLDOC_TREQ_CODE,");
            sb.Append("       KZRLDOC_TRST_CODE,");
            sb.Append("       KZRLDOC_TRST_DATE,");
            sb.Append("       KZRLDOC_FILE_NAME,");
            sb.Append("       KZRLDOC_FILE_TYPE,");
            sb.Append("       KZRLDOC_COMMENT");
            sb.Append(" FROM KZRLDOC");
            sb.Append(" WHERE KZRLDOC_PIDM = :pdim");
            sb.Append("  AND KZRLDOC_AIDY_CODE = :aidyCode");
            sb.Append("  AND KZRLDOC_AIDP_CODE = :aidpCode");
            sb.Append("  AND KZRLDOC_FNDC_CODE = :fndcCode");
            sb.Append("  AND KZRLDOC_TREQ_CODE = :treqCode");

            dataBase.AddFilter("pdim", GlobalVariables.getPdim(matricula));
            dataBase.AddFilter("aidyCode", aidyCode);
            dataBase.AddFilter("aidpCode", aidpCode);
            dataBase.AddFilter("fndcCode", fndcCode);
            dataBase.AddFilter("treqCode", treqCode);
            DataTable data;
            try
            {
                data = dataBase.ExecuteQuery(sb.ToString());
            }
            catch(Exception ex)
            {
                throw new HttpException(
                    (int)HttpStatusCode.InternalServerError, ex.Message);
            }

            if (data.Rows.Count > 1)
            {
                throw new HttpException(
                    (int)HttpStatusCode.InternalServerError,
                    "Mas de uno documento en KZRLDOC");
            }
            else if (data.Rows.Count == 0) throw new HttpException(
                    (int)HttpStatusCode.InternalServerError,
                    "Ninguno documento en KZRLDOC"); ;

            this.Matricula = matricula;
            this.Id = Convert.ToInt32(data.Rows[0][1].ToString());
            this.AidyCode = data.Rows[0][2].ToString();
            this.AidpCode = data.Rows[0][3].ToString();
            this.FndcCode = data.Rows[0][4].ToString();
            this.TreqCode = data.Rows[0][5].ToString();
            this.TrstCode = data.Rows[0][6].ToString();
            this.TrstDate = Convert.ToDateTime(data.Rows[0][7].ToString());
            this.FileName = data.Rows[0][8].ToString();
            this.FileType = data.Rows[0][9].ToString();
            this.Comment = data.Rows[0][10].ToString();
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

        public Kzrldoc Update()
        {
            DataBase dataBase = new DataBase();
            StringBuilder sb = new StringBuilder();

            sb.Append("UPDATE KZRLDOC ");
            sb.Append(" SET KZRLDOC_TRST_CODE     = :trstCode,");
            sb.Append("    KZRLDOC_TRST_DATE     = sysdate,");
            sb.Append("    KZRLDOC_FILE_NAME     = :fileName,");
            sb.Append("    KZRLDOC_FILE_TYPE     = :fileType,");
            sb.Append("    KZRLDOC_COMMENT       = :commentVar,");
            sb.Append("    KZRLDOC_ACTIVITY_DATE = sysdate");
            sb.Append(" WHERE KZRLDOC_PIDM = :pdim");
            sb.Append("  AND KZRLDOC_AIDY_CODE = :aidyCode");
            sb.Append("  AND KZRLDOC_AIDP_CODE = :aidpCode");
            sb.Append("  AND KZRLDOC_FNDC_CODE = :fndcCode");
            sb.Append("  AND KZRLDOC_TREQ_CODE = :treqCode");

            dataBase.AddFilter("trstCode", this.trstCode);
            dataBase.AddFilter("fileName", this.fileName);
            dataBase.AddFilter("fileType", this.fileType);
            dataBase.AddFilter("commentVar", this.comment);

            dataBase.AddFilter("pdim", GlobalVariables.getPdim(this.Matricula));
            dataBase.AddFilter("aidyCode", this.AidyCode);
            dataBase.AddFilter("aidpCode", this.AidpCode);
            dataBase.AddFilter("fndcCode", this.FndcCode);
            dataBase.AddFilter("treqCode", this.TreqCode);

            try
            {
                _ = dataBase.ExecuteQuery(sb.ToString());
            }
            catch (Exception ex)
            {
                throw new HttpException(
                    (int)HttpStatusCode.InternalServerError, ex.Message);
            }

            return new Kzrldoc(this.Matricula,this.AidyCode,
                        this.AidpCode,this.FndcCode,this.TreqCode);
        }
    }

}

