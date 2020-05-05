using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace AFCargaDocs.Models.Entidades
{
    public class FileInfoFtp
    {
        public FileInfoFtp(string treqCode)
        {
            StringBuilder query = new StringBuilder();
            DataBase dataBase= new DataBase();
            query.Append(" SELECT KZRLDOC_FILE_NAME, KZRLDOC_FILE_TYPE, KZRLDOC_ID ");
            query.Append(" FROM KZRLDOC ");
            query.Append(" WHERE KZRLDOC_PIDM = F_UDEM_STU_PIDM(:matricula) ");
            query.Append("   AND KZRLDOC_AIDY_CODE = :aidyCode ");
            query.Append("   AND KZRLDOC_AIDP_CODE = :aidpCode ");
            query.Append("   AND KZRLDOC_FNDC_CODE = :fndcCode ");
            query.Append("   AND KZRLDOC_TREQ_CODE = :treqCode ");
            dataBase.AddFilter("matricula",GlobalVariables.Matricula);
            dataBase.AddFilter("aidyCode", GlobalVariables.Aidy);
            dataBase.AddFilter("aidpCode", GlobalVariables.Aidp);
            dataBase.AddFilter("fndcCode", GlobalVariables.Fndc);
            dataBase.AddFilter("treqCode", treqCode);
            try
            {
                DataRow datarow = dataBase.ExecuteQuery(query.ToString()).Rows[0];
                FileName = datarow[0].ToString();
                FileType = datarow[1].ToString();
                FileId = datarow[2].ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{1}", ex.Message);
            }
        }
        
        public string FileName { get; internal set; }
        public string FileType { get; internal set; }
        [JsonIgnore]
        public string FileId { get; }
        public string FileContent { get; set; }

    }
}