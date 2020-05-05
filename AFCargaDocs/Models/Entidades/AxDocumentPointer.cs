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
        /// Data source name
        /// </summary>
        string dsn;
        /// <summary>
        /// Returns a document reference
        /// </summary>
        string GetDocRef;
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



        override public string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
            stringBuilder.Append("<AxDocumentPointer type=\"Document\" dsn=\"VBS\" app=\"2\" id=\"16\" page=\"0\" ver=\"0\" />");
            return stringBuilder.ToString();
        }
    }
}