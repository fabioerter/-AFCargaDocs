using AFCargaDocs.Models.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFCargaDocs.Models.Entidades
{
    public class AxDocumentCreationData
    {
        /// <summary>
        /// Application identifier
        /// </summary>
        int appId;
        /// <summary>
        /// Data source name
        /// </summary>
        string dsn;
        /// <summary>
        /// Full file path of an image file from which
        /// to create a new document
        /// </summary>
        string filePath;
        /// <summary>
        /// Valid types are listed in the AXTypes
        /// Object Enumerations section
        /// </summary>
        EAxFileType fileType;
        /// <summary>
        /// Indicates whether to ignore
        /// document-level security checking while
        /// saving index values
        /// </summary>
        bool ignoreDls;
        /// <summary>
        /// Indicates whether to ignore duplicated
        /// indexes while saving index values
        /// </summary>
        bool ignoreDupIndex;
        /// <summary>
        /// Indicates whether to split multi-page
        /// image files such as PDF, TIFF, and text.
        /// The default value is true. Note that at
        /// this release, TIFF and text images will
        /// be split automatically even if this
        /// parameter is set to false. It is a well
        /// known behavior that if the filetype
        /// parameter is set to something other
        /// than
        /// AXTypes.FileType.FT_UNKNOWN,
        /// multiple PDF pages will be split at the
        /// server side disregarding the value of
        /// this parameter.
        /// </summary>
        bool splitimg;
        /// <summary>
        /// Number of sub-pages in the image file
        /// (if any)
        /// </summary>
        int subpages;

        public AxDocumentCreationData()
        {
        }

        public AxDocumentCreationData(int appId, string dsn,
            string filePath, EAxFileType fileType, bool ignoreDls,
            bool ignoreDupIndex, bool splitimg, int subpages)
        {
            this.appId = appId;
            this.dsn = dsn;
            this.filePath = filePath;
            this.fileType = fileType;
            this.ignoreDls = ignoreDls;
            this.ignoreDupIndex = ignoreDupIndex;
            this.splitimg = splitimg;
            this.subpages = subpages;
        }

        public int AppId { get => appId; set => appId = value; }
        public string Dsn { get => dsn; set => dsn = value; }
        public string FilePath { get => filePath; set => filePath = value; }
        public EAxFileType FileType { get => fileType; set => fileType = value; }
        public bool IgnoreDls { get => ignoreDls; set => ignoreDls = value; }
        public bool IgnoreDupIndex { get => ignoreDupIndex; set => ignoreDupIndex = value; }
        public bool Splitimg { get => splitimg; set => splitimg = value; }
        public int Subpages { get => subpages; set => subpages = value; }
        override public string ToString()
        {
            return "<?xml version=\"1.0\" encoding=\"utf - 16\"?> " +
                    "<ax:AxDocCrtData xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                    "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" dsn = \"" +
                    this.Dsn + "\" appid = \"" + this.AppId + "\"" +
                    " filepath = \"" + this.FilePath + "\" ignore_dup_index = \"" +
                    this.IgnoreDupIndex.ToString().ToLower() + "\"" +
                    " ignore_dls = \"" + this.IgnoreDls.ToString().ToLower() +
                    "\" splitimg = \"" + this.Splitimg.ToString().ToLower() +
                    "\" subpages = \"" + this.Subpages + "\" "/*filetype = \"" + this.FileType + "\""*/ +
                    " xmlns:ax = \"http://www.emc.com/ax\" /> ";
        }
    }


}