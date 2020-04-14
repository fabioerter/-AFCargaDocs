
using Residencias.API.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace Residencias.API.Models
{
    /// <summary>
    /// Servicio para obtener y modificar información de contactos de emergencias de un alumno
    /// </summary>
    public static class CargaDocsService
    {

        /// <summary>
        /// Método que obtiene los contactos de una persona
        /// </summary>
        /// <param name="matricula">Matricula del alumno</param>
        /// <returns>Arreglo con los contactos que le corresponden a la matricula</returns>
        public static string ObtenerDocumentos(string matricula)
        {

            using (OracleConnection cnx = new OracleConnection(ConfigurationManager.ConnectionStrings["Banner"].ConnectionString))
            {
                cnx.Open();

                // Preparamos la consulta de los datos personales
                OracleCommand comando = new OracleCommand();
                comando.Connection = cnx;
                comando.CommandText = @"Select * from KVRTRFN";
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.ExecuteNonQuery();

                
                cnx.Close();
            }

            return null;
        }
    }
}