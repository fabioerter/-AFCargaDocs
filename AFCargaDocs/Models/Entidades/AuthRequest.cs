using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Residencias.API.Models.Entidades
{
    /// <summary>
    /// Estructura que representa una petición de un token de sesión
    /// </summary>
    public class AuthRequest
    {
        /// <summary>
        /// Matricula del alumno que se desea autenticar
        /// </summary>
        public string Matricula { get; set; }
        /// <summary>
        /// PIN del usuario en el AD del alumno que se desea autenticar
        /// </summary>
        public string PIN { get; set; }
    }
}