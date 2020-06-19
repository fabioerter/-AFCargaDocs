using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;

namespace AFCargaDocs.Models.Entidades
{
    [XmlRoot("QueryResults")]
    public class AxQueryResults
    {
        [XmlRoot("attributes")]
        public class attributes
        {
            [XmlRoot("column")]
            public class column
            {
                [XmlAttribute]
                public string id { get; set; }
                [XmlAttribute]
                public string type { get; set; }
                [XmlAttribute]
                public string fieldid { get; set; }
                [XmlAttribute]
                public string name { get; set; }
                [XmlAttribute]
                public string order { get; set; }
                [XmlAttribute]
                public string visible { get; set; }
                [XmlAttribute]
                public string sortable { get; set; }
                [XmlAttribute]
                public string sortorder { get; set; }

            }
            [XmlArray("columns")]
            [XmlArrayItem("column")]
            public column[] columns { get; set; }

            
            [Serializable, XmlRoot("row")]
            public class row
            {

                [XmlAttribute("xsi:type")]
                public string type { get; set; }
                [XmlAttribute("id")]
                public string id { get; set; }

                [XmlRoot("item")]
                public class item
                {
                    [XmlAttribute]
                    public string id { get; set; }
                    [XmlAttribute]
                    public string value { get; set; }
                }

                [XmlArray("attributes")]
                [XmlArrayItem("item")]
                public item[] attributes { get; set; }

                [XmlRoot("field")]
                class field
                {
                    [XmlAttribute]
                    public string id { get; set; }
                    [XmlAttribute]
                    public string value { get; set; }
                    [XmlAttribute]
                    public string isnull { get; set; }
                }
                [XmlArray("fields")]
                [XmlArrayItem("field")]
                public item[] fields { get; set; }
            }
            [XmlArray("rows")]
            [XmlArrayItem("row")]
            public row[] rows { get; set; }
        }
    }

}


