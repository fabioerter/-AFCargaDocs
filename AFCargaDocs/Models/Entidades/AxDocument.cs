using AFCargaDocs.Models.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AFCargaDocs.Models.Entidades
{
    public class AxDocument
    {
        ///// <summary>
        ///// Application identifier
        ///// </summary>
        //int appid;
        ///// <summary>
        ///// Application name
        ///// </summary>
        //string appName;
        /// <summary>
        /// Data source name
        /// </summary>
        string dsn;
        /// <summary>
        /// Full file path of an image file from which
        //  to create a new document
        /// </summary>
        string filepath;
        /// <summary>
        /// Valid types are listed in the AXTypes
        //  Object Enumerations section
        /// </summary>
        EAxFileType filetype;
        /// <summary>
        /// Indicates whether to ignore
        ///  document-level security checking while
        ///  saving index values
        /// </summary>
        bool ignoreDls;
        /// <summary>
        /// Indicates whether to ignore duplicated
        /// indexes while saving index values
        /// </summary>
        bool ignoreDupindex;
        /// <summary>
        /// Indicates whether to split multi-page
        ///image files such as PDF, TIFF, and text.
        ///The default value is true. Note that at
        ///this release, TIFF and text images will
        ///be split automatically even if this
        ///parameter is set to false. It is a well
        ///known behavior that if the filetype
        ///parameter is set to something other
        ///than
        ///AXTypes.FileType.FT_UNKNOWN,
        ///multiple PDF pages will be split at the
        ///server side disregarding the value of
        ///this parameter.
        /// </summary>
        bool splitimg;
        /// <summary>
        /// Number of sub-pages in the image file
        ///(if any)
        /// </summary>
        string subpages;

        public AxDocument()
        {
        }

        public AxDocument(string dsn, string filepath, EAxFileType filetype, bool ignoreDls, bool ignoreDupindex, bool splitimg, string subpages)
        {
            this.Dsn = dsn;
            this.Filepath = filepath;
            this.Filetype = filetype;
            this.IgnoreDls = ignoreDls;
            this.IgnoreDupindex = ignoreDupindex;
            this.Splitimg = splitimg;
            this.Subpages = subpages;
        }
        public string Dsn { get => dsn; set => dsn = value; }
        public string Filepath { get => filepath; set => filepath = value; }
        public EAxFileType Filetype { get; set; }
        public bool IgnoreDls { get => ignoreDls; set => ignoreDls = value; }
        public bool IgnoreDupindex { get => ignoreDupindex; set => ignoreDupindex = value; }
        public bool Splitimg { get => splitimg; set => splitimg = value; }
        public string Subpages { get => subpages; set => subpages = value; }
        override public string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
            stringBuilder.Append("<ax:AxDocCrtData xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
            stringBuilder.Append("	xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" dsn=\"" + this.Dsn + "\" appid=\"403\"");
            stringBuilder.Append("	filepath=\"" + this.Filepath + "\" ignore_dup_index=\"" + this.IgnoreDupindex + "\"");
            stringBuilder.Append("	ignore_dls=\"" + this.IgnoreDls + "\" splitimg=\""+this.Splitimg+"\" subpages=\""+this.Subpages +"\" filetype=\""+this.Filetype+"\"");
            stringBuilder.Append("	xmlns:ax=\"http://www.emc.com/ax\" />");
            return stringBuilder.ToString();
        }
    }
}