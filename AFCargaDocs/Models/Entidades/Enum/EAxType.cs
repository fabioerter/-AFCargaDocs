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
    public enum EAxPageUploadAction
    {
        /// <summary>
        /// Insert the uploaded page file before a specified page position
        /// </summary>
        InsertBefore,
        /// <summary>
        /// Insert the uploaded page file after a specified page position
        /// </summary>
        InsertAfter,
        /// <summary>
        /// Replace a page at a specified position with the uploaded page
        /// file
        /// </summary>
        Replace
    }
    public enum EAXSortOrder
    {
        /// <summary>
        /// Sort in ascending order
        /// </summary>
        Ascending,
        /// <summary>
        /// Sort in descending order
        /// </summary>
        Descending,
        /// <summary>
        /// No sort
        /// </summary>
        None
    }
    public enum EAXDataProvider
    {
        /// <summary>
        /// Data from ApplicationXtender
        /// </summary>
        ApplicationXtender,
        /// <summary>
        /// Data from Records Manager for ApplicationXtender
        /// </summary>
        RecordsManager
    }
    public enum EAXQueryType
    {
        /// <summary>
        /// Normal query
        /// </summary>
        Normal,
        /// <summary>
        /// Cross-application query
        /// </summary>
        CAQ,
        /// <summary>
        /// AppXtender Reports Mgmt report query
        /// </summary>
        Report
    }
    /// <summary>
    /// Query retention type; valid types are
    /// listed in the QueryRetentionType
    /// Enumeration section
    /// </summary>
    public enum EAXQueryRetentionType
    {
        /// <summary>
        /// Include docs under retention in search
        /// </summary>
        AllIncludingRetention,
        /// <summary>
        /// Exclude docs under retention in search
        /// </summary>
        AllExcludingRetention,
        /// <summary>
        /// Only search docs under retention
        /// </summary>
        OnlyUnderRetention,
        /// <summary>
        /// Only search docs on retention hold
        /// </summary>
        OnlyOnRetentionHold,
        /// <summary>
        /// Only search docs under retention exclude docs
        /// on hold
        /// </summary>
        OnlyUnderRetentionNotOnHold
    }

    //public enum AxImageExportFormatData
    //{
    //    /// <summary>
    //    /// Indicates whether to use the native archive
    //    ///format for export
    //    /// </summary>
    //    ArchiveFormat,
    //    /// <summary>
    //    /// Black and white images; possible values are:
    //    /// • BMP
    //    /// • TIFF
    //    /// • TIFF_Compressed
    //    /// </summary>
    //    BlackWhiteImages,
    //    /// <summary>
    //    /// 4- or 8-bit color images; possible values are:
    //    /// • BMP
    //    /// • BMP_Compressed
    //    /// • GIF
    //    /// • TIFF
    //    /// • TIFF_Compressed
    //    /// </summary>
    //    ColorImages4or8Bit,
    //    /// <summary>
    //    /// Indicates whether to export text pages as
    //    /// images or as text files
    //    /// </summary>
    //    ExportTextAsImage,
    //    /// <summary>
    //    /// JPEG quality factor
    //    /// </summary>
    //    JpegCompression,
    //    /// <summary>
    //    /// Indicates whether to use multi-page image
    //    /// files for export
    //    /// </summary>
    //    MultiPage,
    //    /// <summary>
    //    /// True color image; possible values are:
    //    /// • BMP
    //    /// • GIF
    //    /// • JPEG
    //    /// • TIFF
    //    /// • TIFF_Compressed
    //    /// </summary>
    //    TrueColorImages,

    //}
    public enum AxFormTypes
    {
        None,
        Image,
        Text
    }
    public enum AxImageExportFormatData
    {
        PDF,
        TIFF,
        XPS,
        IMAGE
    }
}