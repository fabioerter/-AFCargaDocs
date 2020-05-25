using AFCargaDocs.Models.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AFCargaDocs.Models.Entidades
{
    public class AxDocumentPointer
    {
        /// <summary>
        /// Data source name -- BANDEV
        /// </summary>
        string dsn;
        /// <summary>
        /// Document identifier; for a regular document this is
        ///the document ID, for an AppXtender Reports
        ///Mgmt report this is the report ID, and for a batch
        ///document this is the batch ID
        /// </summary>
        int id;
        /// <summary>
        /// 1-based page identifier
        /// </summary>
        int page;
        /// <summary>
        /// Document type; valid types are listed in the
        /// DocumentType Enumeration section
        /// </summary>
        EAxDocumentType type;
        /// <summary>
        /// 1-based version identifier
        /// </summary>
        int ver;

        public AxDocumentPointer(string dsn, int id, int page, EAxDocumentType type, int ver)
        {
            this.dsn = dsn;
            this.id = id;
            this.page = page;
            this.type = type;
            this.ver = ver;
        }

        override public string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
            stringBuilder.Append("<AxDocumentPointer type=\"" + this.type + "\" dsn=\"" + this.dsn +
                      "\" app=\"403\" id=\"" + this.id + "\" page=\"" + this.page + "\" ver=\"" + 
                      this.ver + "\" />");
            return stringBuilder.ToString();
        }
    }
}