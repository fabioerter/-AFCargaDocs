using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        private static string aplicacion;


        public static string Matricula { get => matricula; set => matricula = value; }
        public static string Fndc { get => fndc; set => fndc = value; }
        public static string Aidy { get => aidy; set => aidy = value; }
        public static string Aidp { get => aidp; set => aidp = value; }
        public static string Aplicacion { get => aplicacion; set => aplicacion = value; }
        public static string getPdim( string matricula)
        {
            using (OracleConnection cnx = new OracleConnection(ConfigurationManager.ConnectionStrings["Banner"].ConnectionString))
            {
                cnx.Open();

                OracleCommand comando = new OracleCommand();
                comando.Connection = cnx;
                comando.CommandText = @"F_UDEM_STU_PIDM";
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.Add(new OracleParameter("salida", OracleDbType.Varchar2)
                {
                    Size = 200,
                    Direction = System.Data.ParameterDirection.ReturnValue
                });
                comando.Parameters.Add(new OracleParameter(matricula, OracleDbType.Varchar2)
                {
                    Value = GlobalVariables.Matricula,
                    Size = 9
                });
                comando.ExecuteNonQuery();
                return (comando.Parameters["salida"].Value).ToString();
            }
        }
    }
}