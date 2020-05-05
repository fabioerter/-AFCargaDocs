using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFCargaDocs.Models.Entidades.Enum
{
    /// <summary>
    /// This object defines common types and constants used across many of
    /// the data transfer objects.
    /// </summary>
    public enum EAxType
    {
        /// <summary>
        /// Basic feature set that has everything except
        /// full-text search and AppXtender Reports Mgmt
        /// document viewing capabilities
        /// </summary>
        AxFeature_Basic,
        /// <summary>
        /// Basic feature set that also includes AppXtender 
        /// Reports Mgmt document viewing capability
        /// </summary>
        AxFeature_ERMXDocuments,
        /// <summary>
        /// Basic feature set that also includes full-text search capability
        /// </summary>
        AxFeature_FullTextSearch,
        /// <summary>
        /// Application name for overlay forms
        /// </summary>
        FormsApp,
        /// <summary>
        /// Application name for rubber stamps
        /// </summary>
        RubberStampsApp
    }

    /// <summary>
    /// This enumeration provides all file types supported by ApplicationXtender.
    /// </summary>
    public enum EAxFileType
    {
        /// <summary>
        /// File type is unknown
        /// </summary>
        FT_UNKNOWN = 0,
        /// <summary>
        /// Text file type
        /// </summary>
        FT_Text = 1,
        /// <summary>
        /// Compressed file type
        /// </summary>
        FT_CompressedText = 2,
        /// <summary>
        /// Foreign file type
        /// </summary>
        FT_ForeignFile = 3,
        /// <summary>
        /// OLE file type
        /// </summary>
        FT_OLE = 4,
        /// <summary>
        /// Rich text format file type
        /// </summary>
        FT_RTF = 5,
        /// <summary>
        /// HTML file type
        /// </summary>
        FT_HTML = 6,
        /// <summary>
        /// PDF file type
        /// </summary>
        FT_PDF = 7,
        /// <summary>
        /// Image file type
        /// </summary>
        FT_IMAGE = 8,
        /// <summary>
        /// Annotation file type
        /// </summary>
        FT_Annotation = 255
    }

    /// <summary>
    /// This enumeration provides two document types for
    /// ApplicationXtender.
    /// </summary>
    public enum EAxDocumentType
    {
        /// <summary>
        /// Normal document
        /// </summary>
        Document = 0,
        /// <summary>
        /// AppXtender Reports Mgmt report document
        /// </summary>
        Report = 1
    }
}