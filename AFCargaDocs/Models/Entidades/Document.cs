using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Residencias.API.Models.Entidades
{
    /// <summary>
    /// Estructura que representa una petición de un token de sesión
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Nome del archivo
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Status del archivo (NR = no revisado o PENDIENTE POR REVIAR)
        /// </summary>
        public string Status { get; set; }
    }
}