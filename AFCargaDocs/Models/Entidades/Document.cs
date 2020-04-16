using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFCargaDocs.Models.Entidades
{
    /// <summary>
    /// Estructura que representa una petición de un token de sesión
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Clabe del archivo
        /// </summary>
        public string Clave { get; set; }
        /// <summary>
        /// Nome del archivo
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Status del archivo (NR = no revisado o PENDIENTE POR REVIAR)
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Fecha del archivo
        /// </summary>
        public string Fecha { get; set; }
    }
}