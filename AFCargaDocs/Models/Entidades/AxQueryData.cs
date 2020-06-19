using AFCargaDocs.Models.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Web;

namespace AFCargaDocs.Models.Entidades
{
    public class AxQueryData
    {
        /// <summary>
        /// Indicates whether to search all
        /// document revisions
        /// </summary>
        bool allRevisions;
        /// <summary>
        /// Application ID; can be obtained from an
        /// AxApplication Object
        /// </summary>
        string appid;
        /// <summary>
        /// Sort order for document IDs; valid sort
        /// order types are listed in the SortOrder
        /// Enumeration section
        /// </summary>
        EAXSortOrder docid_sortorder;
        /// <summary>
        /// Full-text query criteria; an instance of
        /// the AxFullTextQueryData Object; can
        /// be left null or empty if you do not want
        /// to perform a full-text search
        /// </summary>
        string ftdata;
        /// <summary>
        /// Query ID; can be obtained from an
        /// AxApplicationQuery Object; when this
        /// parameter is supplied, the query
        /// criteria from the Fields member of this
        /// object will be ignored; the stored query
        /// identified by the id will be executed
        /// </summary>
        string id;
        /// <summary>
        /// Query name; can be obtained from an
        /// AxApplicationQuery Object
        /// </summary>
        string name;
        /// <summary>
        /// Type of data provider; valid data
        /// provider types are listed in the
        /// DataProvider Enumeration section
        /// </summary>
        EAXDataProvider provider;
        /// <summary>
        /// Query type; valid query types are listed
        /// in the QueryType Enumeration section;
        /// default value is Normal
        /// </summary>
        EAXQueryType qtype;
        /// <summary>
        /// Query retention type; valid types are
        /// listed in the QueryRetentionType
        /// Enumeration section
        /// </summary>
        EAXQueryRetentionType qrtype;
        /// <summary>
        /// Name of the sorting field; a special
        /// case for this parameter is DocID,
        /// case-insensitive, when you want the
        /// result set sorted by document ID
        /// </summary>
        string sortfldname;
        /// <summary>
        /// Sort order for a specified sorting field;
        /// valid sort order types are listed in the
        /// SortOrder Enumeration section; the
        /// default value is None
        /// </summary>
        EAXSortOrder sortorder;

        List<AxFieldValue> AXFields;

        public AxQueryData(bool allRevisions, string appid,
            EAXSortOrder docid_sortorder, string ftdata,
            string id, string name, EAXDataProvider provider,
            EAXQueryType qtype, EAXQueryRetentionType qrtype,
            string sortfldname, EAXSortOrder sortorder)
        {
            this.allRevisions = allRevisions;
            this.appid = appid;
            this.docid_sortorder = docid_sortorder;
            this.ftdata = ftdata;
            this.id = id;
            this.name = name;
            this.provider = provider;
            this.qtype = qtype;
            this.qrtype = qrtype;
            this.sortfldname = sortfldname;
            this.sortorder = sortorder;
            this.AXFields = new List<AxFieldValue>();
        }

        public AxQueryData()
        {
            this.AXFields = new List<AxFieldValue>();
        }

        /// <summary>
        /// Indicates whether to search all
        /// document revisions
        /// </summary>
        public bool AllRevisions { get => allRevisions; set => allRevisions = value; }
        /// <summary>
        /// Application ID; can be obtained from an
        /// AxApplication Object
        /// </summary>
        public string Appid { get => appid; set => appid = value; }
        /// <summary>
        /// Sort order for document IDs; valid sort
        /// order types are listed in the SortOrder
        /// Enumeration section
        /// </summary>
        public EAXSortOrder Docid_sortorder { get => docid_sortorder; set => docid_sortorder = value; }
        /// <summary>
        /// Full-text query criteria; an instance of
        /// the AxFullTextQueryData Object; can
        /// be left null or empty if you do not want
        /// to perform a full-text search
        /// </summary>
        public string Ftdata { get => ftdata; set => ftdata = value; }
        /// <summary>
        /// Query ID; can be obtained from an
        /// AxApplicationQuery Object; when this
        /// parameter is supplied, the query
        /// criteria from the Fields member of this
        /// object will be ignored; the stored query
        /// identified by the id will be executed
        /// </summary>
        public string Id { get => id; set => id = value; }
        /// <summary>
        /// Query name; can be obtained from an
        /// AxApplicationQuery Object
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// Type of data provider; valid data
        /// provider types are listed in the
        /// DataProvider Enumeration section
        /// </summary>
        public EAXDataProvider Provider { get => provider; set => provider = value; }
        /// <summary>
        /// Query type; valid query types are listed
        /// in the QueryType Enumeration section;
        /// default value is Normal
        /// </summary>
        public EAXQueryType Qtype { get => qtype; set => qtype = value; }
        /// <summary>
        /// Query retention type; valid types are
        /// listed in the QueryRetentionType
        /// Enumeration section
        /// </summary>
        public EAXQueryRetentionType Qrtype { get => qrtype; set => qrtype = value; }
        /// <summary>
        /// Name of the sorting field; a special
        /// case for this parameter is DocID,
        /// case-insensitive, when you want the
        /// result set sorted by document ID
        /// </summary>
        public string Sortfldname { get => sortfldname; set => sortfldname = value; }
        /// <summary>
        /// Sort order for a specified sorting field;
        /// valid sort order types are listed in the
        /// SortOrder Enumeration section; the
        /// default value is None
        /// </summary>
        public EAXSortOrder Sortorder { get => sortorder; set => sortorder = value; }


        public void addField(int id, bool isNull, string value = "")
        {
            AxFieldValue axFieldValueTemp = new AxFieldValue();

            axFieldValueTemp.Id = id;
            axFieldValueTemp.IsNull = isNull;
            axFieldValueTemp.Value = value;

            AXFields.Add(axFieldValueTemp);
        }

        override public string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<?xml version=\"1.0\" encoding=\"utf - 16\"?>  <ax:AxQueryData");
            sb.Append("	xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ");
            sb.Append("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" ");
            sb.Append("xmlns:ax=\"http://www.emc.com/ax\" ");
            //sb.Append(" provider=\"" + this.Provider + "\" ");
            //sb.Append("        	id=\"" + this.Id + "\"");
            //sb.Append("         name=\"" +this.Name+"\" ");
            sb.Append("        	appid=\"" + this.Appid + "\" qtype=\"" + this.Qtype + "\" ");
            //sb.Append("        	qrtype=\"" + this.Qrtype + "\" ");
            //sb.Append("        	docid_sortorder=\"" + this.Docid_sortorder + "\" ");
            //sb.Append("        	sortorder=\"" + this.Sortorder + "\" ");
            sb.Append(/*"        	allRevisions=\"" + this.AllRevisions.ToString().ToLower() + "\"*/">");
            sb.Append("            <ax:Fields>");

            foreach (var item in this.AXFields)
            {
                sb.Append(item.ToString());
            }

            sb.Append("        </ax:Fields>");
            //sb.Append("        <ax:ftdata exptype=\"AllWords\" ");
            //sb.Append("        	queryop=\"AND\" ");
            //sb.Append("        	criteria=\"\" ");
            //sb.Append("        	useThesaurus=\"false\" />  ");
            sb.Append("</ax:AxQueryData>");
            return sb.ToString();
        }
    }
}