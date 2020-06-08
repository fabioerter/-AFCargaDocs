using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;

namespace AFCargaDocs.Models.Entidades
{
    public class DataBase
    {

        List<OracleParameter> parameterList;

        Dictionary<String, String> filters;
        Dictionary<String, String> outParameters;
        Dictionary<String, OracleDbType> dbTypeList;
        Dictionary<String, int> sizeList;
        DataTable result;
        public DataBase()
        {
            filters = new Dictionary<string, string>();
            outParameters = new Dictionary<string, string>();
            dbTypeList = new Dictionary<string, OracleDbType>();
            sizeList = new Dictionary<string, int>();
            parameterList = new List<OracleParameter>();
        }

        public void AddFilter(String name, String value)
        {
            filters.Add(name, value);
        }
        public void AddParameter(string name, string value, OracleDbType dbType, int size)
        {
            filters.Add(name, value);
            this.dbTypeList.Add(name, dbType);
            this.sizeList.Add(name, size);

            OracleParameter parameter = new OracleParameter(name, dbType)
            {
                Value = value,
                Size = size
            };
            if (value == null)
            {
                parameter.Value = DBNull.Value;
            }
            parameterList.Add(parameter);
        }
        public void AddOutParameter(String name, OracleDbType dbType, int size)
        {
            this.outParameters.Add(name, "out");
            this.dbTypeList.Add(name, dbType);
            this.sizeList.Add(name, size);
            OracleParameter parameter = new OracleParameter(name, dbType)
            {
                Direction = System.Data.ParameterDirection.Output,
                Value = DBNull.Value
            };
            if (dbTypeList[name] != OracleDbType.RefCursor)
                                          parameter.Size = size;
            parameterList.Add(parameter);
        }

        public DataTable ExecuteQuery(String query)
        {
            using (OracleConnection cnx = new OracleConnection(ConfigurationManager.ConnectionStrings["Banner"].ConnectionString))
            {
                cnx.KeepAliveTime = 30000;
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
        public DataTable ExecuteProcedure(string query)
        {
            using (OracleConnection cnx = new OracleConnection(ConfigurationManager.ConnectionStrings["Banner"].ConnectionString))
            {
                DataSet dataSet = new DataSet();
                OracleCommand cmd = new OracleCommand(query, cnx);
                cmd.CommandText = query;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
                {
                    try
                    {
                        cnx.Open();
                        if (outParameters.Count() > 0)
                        {
                            foreach (String name in outParameters.Keys)
                            {
                                OracleParameter parameter = new OracleParameter(name, dbTypeList[name])
                                {
                                    Direction = System.Data.ParameterDirection.Output,
                                    Size = sizeList[name]
                                };
                                cmd.Parameters.Add(parameter);
                            }
                        }
                        if (filters.Count() > 0)
                        {
                            foreach (String name in filters.Keys)
                            {
                                OracleParameter parameter = new OracleParameter(name, dbTypeList[name])
                                {
                                    Value = filters[name],
                                    Size = sizeList[name]
                                };
                                if (filters[name] == null)
                                {
                                    parameter.Value = DBNull.Value;
                                }
                                cmd.Parameters.Add(parameter);
                            }
                        }
                        //OracleDataReader reader = cmd.ExecuteReader();
                        dataAdapter.SelectCommand = cmd;

                        DataTable dataTableTemp = new DataTable("outParameters", "");

                        if (outParameters.Count() > 0)
                        {

                            foreach (String name in outParameters.Keys)
                            {

                                dataTableTemp.Columns.Add(name);
                                dataTableTemp.Rows.Add(cmd.Parameters[name].Value);
                            }

                        }
                        dataSet.Tables.Add(dataTableTemp);
                        this.result = dataTableTemp;
                    }
                    catch (Exception ex)
                    {
                        throw new HttpException((int)HttpStatusCode.InternalServerError, ex.Message);
                    }
                    finally
                    {
                        cnx.Close();
                    }
                    return dataSet.Tables["outParameters"];
                }

            }
        }

        public DataSet ExecuteFunction(string query, string outName, OracleDbType returnType, int? returnSize = null)
        {

            using (OracleConnection cnx = new OracleConnection(ConfigurationManager.ConnectionStrings["Banner"].ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(query, cnx);
                DataSet dataSet = new DataSet();
                cmd.CommandText = query;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
                {
                    try
                    {
                        cnx.Open();
                        OracleParameter salida = new OracleParameter(outName,
                               returnType, DBNull.Value,
                               ParameterDirection.ReturnValue);

                        if (returnSize != null)
                            salida.Size = (int)returnSize;

                        cmd.Parameters.Add(salida);

                        foreach (OracleParameter parameter in parameterList)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                        dataAdapter.SelectCommand = cmd;

                        cmd.ExecuteNonQuery();

                        if (returnType == OracleDbType.RefCursor)
                        {
                            dataAdapter.AcceptChangesDuringFill = true;
                            dataAdapter.Fill(dataSet, outName, (OracleRefCursor)(cmd.Parameters[outName].Value));
                        }
                        else
                        {
                            dataSet.Tables.Add(outName).Columns.Add(outName);
                            dataSet.Tables[outName].Rows.Add(cmd.Parameters[outName].Value);
                        }



                        if (outParameters.Count() > 0)
                        {

                            foreach (String name in outParameters.Keys)
                            {
                                if (dbTypeList[name] == OracleDbType.RefCursor)
                                {
                                    dataAdapter.AcceptChangesDuringFill = true;
                                    dataAdapter.Fill(dataSet, name, (OracleRefCursor)(cmd.Parameters[name].Value));
                                }
                                else
                                {
                                    dataSet.Tables.Add(name).Rows.Add(cmd.Parameters[name].Value);
                                }
                            }

                        }
                        //dataSet.Tables.Add(dataTableTemp);
                        this.result = dataSet.Tables[outName];
                    }
                    catch (Exception ex)
                    {
                        throw new HttpException((int)HttpStatusCode.InternalServerError, ex.Message);
                    }
                    finally
                    {
                        cnx.Close();
                    }
                    return dataSet;
                }

            }
        }
        public string getOutParamater(string paramName)
        {
            return result.Rows[0][paramName].ToString();
        }
    }
}