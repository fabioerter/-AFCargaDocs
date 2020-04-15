
using Newtonsoft.Json;
using AFCargaDocs.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Web.Http;
using System.Net.Http;
using System.Data;

namespace AFCargaDocs.Models
{
    /// <summary>
    /// Servicio para obtener y modificar información de contactos de emergencias de un alumno
    /// </summary>
    public static class CargaDocsService
    {

        /// <summary>
        /// Método que obtiene los dopcumentos de una 
        /// </summary>
        /// <param name="matricula">Matricula del alumno</param>
        /// <returns>Arreglo con los contactos que le corresponden a la matricula</returns>
        public static List<Document> ObtenerDocumentos(string matricula)
        {
            DataTable dataTable;
            try
            {
                dataTable = DataBase.ExecuteQuery("SELECT * FROM KVRTRFN");
            }
            catch (Exception ex)
            {
                throw new HttpException(ex.ToString(), Convert.ToInt32(HttpStatusCode.BadRequest));
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            List<Document> documentList = new List<Document>();
            foreach (DataRow dr in dataTable.Rows)
            {
                Document document = new Document()
                {
                    Name = dr[0].ToString(),
                    Status = dr[1].ToString()
                };
                documentList.Add(document);
            }
            return documentList;
            //using (OracleConnection cnx = new OracleConnection(ConfigurationManager.ConnectionStrings["Banner"].ConnectionString))
            //{
            //    cnx.Open();

            //    // Preparamos la consulta
            //    OracleDataReader reader = new OracleCommand("SELECT * FROM KVRTRFN", cnx).ExecuteReader();
            //    List<Document> documentList = new List<Document>();
            //    try
            //    {
            //        while (reader.Read())
            //        {
            //            Console.WriteLine(reader.GetString(0) + ", " + reader.GetString(1));
            //            String teste = reader.GetString(1);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new HttpException(ex.ToString(), Convert.ToInt32(HttpStatusCode.BadRequest));
            //        /*throw new HttpResponseException(HttpStatusCode.BadRequest);*/
            //    }
            //    return documentList;
            //}
        }
    }






}
