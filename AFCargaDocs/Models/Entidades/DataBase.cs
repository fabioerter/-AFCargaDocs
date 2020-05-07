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

        Dictionary<String, String> filters;

        DataTable result;
        public DataBase()
        {
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
                try
                {
                    cnx.Open();
                    if (filters.Count() > 0)
                    {
                        foreach (String name in filters.Keys)
                        {
                            OracleParameter parameter = new OracleParameter(":" + name, filters[name]);
                            cmd.Parameters.Add(parameter);
                        }
                    }
                    OracleDataAdapter dataAdapter = new OracleDataAdapter(cmd);
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
                if (dataSet.Tables.Count == 0)
                {
                    return null;
                }
                return dataSet.Tables[0];


            }
        }
        public void ExecuteNonQuery(string query)
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
                                OracleParameter parameter = new OracleParameter(":" + name, filters[name]);
                                cmd.Parameters.Add(parameter);
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
                    result = dataSet.Tables[0];
                }

            }
        }
    }
}