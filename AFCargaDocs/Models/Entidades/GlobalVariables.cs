using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace AFCargaDocs.Models.Entidades
{
    public static class GlobalVariables
    {
        private static string matricula;
        private static string fndc;
        private static string aidy;
        private static string aidp;
        private static string ftpip = "ftp://148.238.49.217";
        private static string aplicacion;
        private static string ftpUser = "ftpTstAyudaFN";
        private static string ftpPassword = "Rep0AyudaFnT3t$";



        public static string Matricula { get => matricula; set => matricula = value; }
        public static string Fndc { get => fndc; set => fndc = value; }
        public static string Aidy { get => aidy; set => aidy = value; }
        public static string Aidp { get => aidp; set => aidp = value; }
        public static string Aplicacion { get => aplicacion; set => aplicacion = value; }
        public static string Ftpip { get => ftpip; set => ftpip = value; }
        public static string FtpUser { get => ftpUser; set => ftpUser = value; }
        public static string FtpPassword { get => ftpPassword; set => ftpPassword = value; }

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            if (request["X-Requested-With"] == "XMLHttpRequest")
                return true;
            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }

        public static string ConvertToBase64(this Stream stream)
        {
            var bytes = new Byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);

            return Convert.ToBase64String(bytes);
        }

        public static string getPdim(string matricula)
        {
            DataBase dataBase = new DataBase();

            dataBase.AddParameter("CMATRICULA", matricula, OracleDbType.Varchar2, 9);

            dataBase.ExecuteFunction("F_UDEM_STU_PIDM", "salida", OracleDbType.Varchar2, 9);
            return dataBase.getOutParamater("salida");
        }
    }
}