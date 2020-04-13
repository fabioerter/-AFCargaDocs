using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using Residencias.API.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Residencias.API.Models
{
    /// <summary>
    /// Servicio para obtener y modificar información de contactos de emergencias de un alumno
    /// </summary>
    public static class ContactosService
    {

        /// <summary>
        /// Método que obtiene los contactos de una persona
        /// </summary>
        /// <param name="matricula">Matricula del alumno</param>
        /// <returns>Arreglo con los contactos que le corresponden a la matricula</returns>
        public static Contacto[] ObtenerConctactos(string matricula)
        {
            List<Contacto> contactos = null;

            using (OracleConnection cnx = new OracleConnection(ConfigurationManager.ConnectionStrings["Banner"].ConnectionString))
            {
                cnx.Open();

                // Preparamos la consulta de los datos personales
                OracleCommand comando = new OracleCommand();
                comando.Connection = cnx;
                comando.CommandText = @"HWSKRESD.f_obtiene_contactos";
                comando.CommandType = System.Data.CommandType.StoredProcedure;

                comando.Parameters.Add(new OracleParameter("salida", OracleDbType.Varchar2)
                {
                    Size = 200,
                    Direction = System.Data.ParameterDirection.ReturnValue
                });

                comando.Parameters.Add(new OracleParameter("p_matricula", OracleDbType.Varchar2)
                {
                    Value = matricula,
                    Size = 9
                });

                comando.Parameters.Add(new OracleParameter("p_contactos", OracleDbType.RefCursor)
                {
                    Direction = System.Data.ParameterDirection.Output
                });

                comando.ExecuteNonQuery();

                // Revisamos si se pudo ejecutar la consulta
                if (comando.Parameters["salida"].Value?.ToString() == "OP_EXITOSA")
                {
                    // Inicializamos la variable de salida
                    contactos = new List<Contacto>();

                    OracleDataReader lector = ((OracleRefCursor)comando.Parameters["p_contactos"].Value).GetDataReader();

                    // Revisamos cada contacto
                    while (lector.Read())
                    {
                        Contacto persona = new Contacto()
                        {
                            Secuencia = lector["secuencia"]?.ToString(),
                            Relacion = new Relacion() {
                                Clave = lector["rel_code"]?.ToString(),
                                Descripcion = lector["relacion"]?.ToString()
                            },
                            Nombre = lector["nombre"]?.ToString(),
                            Apellidos = lector["apellidos"]?.ToString(),
                            Direccion = new Domicilio()
                            {
                                Calle = lector["calle"]?.ToString(),
                                Colonia = lector["colonia"]?.ToString(),
                                Ciudad = lector["ciudad"]?.ToString(),
                                Estado = new Estado() {
                                    Clave = lector["est_code"]?.ToString(),
                                    Nombre = lector["estado"]?.ToString()
                                },
                                Pais = new Estado() {
                                    Clave = lector["pais_code"]?.ToString(),
                                    Nombre = lector["pais"]?.ToString()
                                },
                                Area = lector["area"]?.ToString(),
                                Telefono = lector["telefono"]?.ToString()
                            }
                        };

                        contactos.Add(persona);
                        
                    }
                }

                cnx.Close();
            }

            return contactos.ToArray();
        }



        /// <summary>
        /// Método que obtiene los contactos adicionales de una persona
        /// </summary>
        /// <param name="matricula">Matricula del alumno</param>
        /// <returns>Arreglo con los contactos que le corresponden a la matricula</returns>
        public static Telefonos[] ObtenerConctactosAdicionales(string matricula)
        {
            List<Telefonos> telefonos = null;

            using (OracleConnection cnx = new OracleConnection(ConfigurationManager.ConnectionStrings["Banner"].ConnectionString))
            {
                cnx.Open();

                // Preparamos la consulta de los datos personales
                OracleCommand comando = new OracleCommand();
                comando.Connection = cnx;
                comando.CommandText = @"HWSKRESD.f_obtiene_contactos_adicional";
                comando.CommandType = System.Data.CommandType.StoredProcedure;

                comando.Parameters.Add(new OracleParameter("salida", OracleDbType.Varchar2)
                {
                    Size = 200,
                    Direction = System.Data.ParameterDirection.ReturnValue
                });

                comando.Parameters.Add(new OracleParameter("p_matricula", OracleDbType.Varchar2)
                {
                    Value = matricula,
                    Size = 9
                });

                comando.Parameters.Add(new OracleParameter("p_contactos", OracleDbType.RefCursor)
                {
                    Direction = System.Data.ParameterDirection.Output
                });

                comando.ExecuteNonQuery();

                // Revisamos si se pudo ejecutar la consulta
                if (comando.Parameters["salida"].Value?.ToString() == "OP_EXITOSA")
                {
                    // Inicializamos la variable de salida
                    telefonos = new List<Telefonos>();

                    OracleDataReader lector = ((OracleRefCursor)comando.Parameters["p_contactos"].Value).GetDataReader();

                    // Revisamos cada contacto
                    while (lector.Read())
                    {
                        Telefonos numero = new Telefonos()
                        {
                            Secuencia = lector["secuencia"]?.ToString(),
                            Descripcion = lector["descripcion"]?.ToString(),
                            Numero = lector["telefono"]?.ToString()
                        };

                        telefonos.Add(numero);

                    }
                }

                cnx.Close();
            }

            return telefonos.ToArray();
        }


        /// <summary>
        /// Función que actualiza el telefono de un contacto de emergencia
        /// </summary>
        /// <param name="matricula">Matricula del alumno que tiene la persona relacionada</param>
        /// <param name="datos">Datos de la persona relacioanda al alumno</param>
        /// <returns>Verdadero o falso, dependiendo de si se pudo realizar la actualización</returns>
        public static bool ActualizaContacto(string matricula, Contacto datos)
        {
            bool resultado = false;

            using (OracleConnection cnx = new OracleConnection(ConfigurationManager.ConnectionStrings["Banner"].ConnectionString))
            {
                cnx.Open();

                // Si es una estructura de borrado, ajustamos los datos de código de estado para que proceda correctamente el borrado
                if (datos.Nombre == null && datos.Apellidos == null && datos.Direccion.Calle == null &&
                        datos.Direccion.Colonia == null && datos.Direccion.Ciudad == null && datos.Direccion.Estado.Nombre == null &&
                        datos.Direccion.Pais.Clave == null && datos.Direccion.Area == null && datos.Direccion.Telefono == null)
                {
                    datos.Direccion.Estado.Clave = null;
                }

                // Preparamos la consulta de los datos personales
                OracleCommand comando = new OracleCommand();
                comando.Connection = cnx;
                comando.CommandText = @"HWSKRESD.f_actualiza_contacto";
                comando.CommandType = System.Data.CommandType.StoredProcedure;

                comando.Parameters.Add(new OracleParameter("salida", OracleDbType.Varchar2)
                {
                    Size = 500,
                    Direction = System.Data.ParameterDirection.ReturnValue
                });

                comando.Parameters.Add(new OracleParameter("p_matricula", OracleDbType.Varchar2)
                {
                    Value = matricula,
                    Size = 9
                });

                comando.Parameters.Add(new OracleParameter("p_relacion", OracleDbType.Varchar2)
                {
                    Value = datos.Relacion.Clave,
                    Size = 1
                });

                comando.Parameters.Add(new OracleParameter("p_nombre", OracleDbType.Varchar2)
                {
                    Value = datos.Nombre,
                    Size = 50
                });

                comando.Parameters.Add(new OracleParameter("p_apellido", OracleDbType.Varchar2)
                {
                    Value = datos.Apellidos,
                    Size = 50
                });

                comando.Parameters.Add(new OracleParameter("p_calle", OracleDbType.Varchar2)
                {
                    Value = datos.Direccion.Calle,
                    Size = 50
                });

                comando.Parameters.Add(new OracleParameter("p_colonia", OracleDbType.Varchar2)
                {
                    Value = datos.Direccion.Colonia,
                    Size = 50
                });

                comando.Parameters.Add(new OracleParameter("p_ciudad", OracleDbType.Varchar2)
                {
                    Value = datos.Direccion.Ciudad,
                    Size = 50
                });

                comando.Parameters.Add(new OracleParameter("p_estado", OracleDbType.Varchar2)
                {
                    Value = datos.Direccion.Estado.Clave,
                    Size = 5
                });

                comando.Parameters.Add(new OracleParameter("p_estado_otro", OracleDbType.Varchar2)
                {
                    Value = datos.Direccion.Estado.Nombre,
                    Size = 75
                });

                comando.Parameters.Add(new OracleParameter("p_pais", OracleDbType.Varchar2)
                {
                    Value = datos.Direccion.Pais.Clave,
                    Size = 3
                });

                comando.Parameters.Add(new OracleParameter("p_area", OracleDbType.Varchar2)
                {
                    Value = datos.Direccion.Area,
                    Size = 6
                });

                comando.Parameters.Add(new OracleParameter("p_telefono", OracleDbType.Varchar2)
                {
                    Value = datos.Direccion.Telefono,
                    Size = 12
                });

                comando.Parameters.Add(new OracleParameter("p_secuencia", OracleDbType.Varchar2)
                {
                    Direction = System.Data.ParameterDirection.InputOutput,
                    Value = datos.Secuencia,
                    Size = 1
                });

                comando.ExecuteNonQuery();

                // Revisamos si se pudo ejecutar la actualización
                if (comando.Parameters["salida"].Value?.ToString() == "OP_EXITOSA")
                {
                    resultado = true;
                }

                cnx.Close();
            }

            return resultado;
        }
    }
}