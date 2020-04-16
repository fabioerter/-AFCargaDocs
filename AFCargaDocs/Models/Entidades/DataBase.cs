using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace AFCargaDocs.Models.Entidades
{
    public class DataBase
    {
        String query;

        Dictionary<String, String> filters;

        public DataBase()
        {
            this.query = query;
            filters = new Dictionary<string, string>();
        }

        public void AddFilter(String name, String value)
        {
            filters.Add(name, value);
        }

        public DataTable ExecuteQuery(String query)
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
                        if (filters.Count() > 0)
                        {
                            foreach (String name in filters.Keys)
                            {
                                cmd.Parameters.Add(":" + name, filters[name]);
                            }
                        }
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