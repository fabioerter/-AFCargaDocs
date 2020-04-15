using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace AFCargaDocs.Models.Entidades
{
    public static class DataBase
    {
        public static DataTable ExecuteQuery(String query)
        {
            using (OracleConnection cnx = new OracleConnection(ConfigurationManager.ConnectionStrings["Banner"].ConnectionString))
            {
                DataSet dataSet = new DataSet();

                OracleCommand cmd = new OracleCommand(query, cnx);

                using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
                {
                    try
                    {
                        cnx.Open();
                        dataAdapter.SelectCommand = cmd;
                        dataAdapter.Fill(dataSet);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        cnx.Close();
                    }
                    return dataSet.Tables[0];
                }

            }
        }
    }
}