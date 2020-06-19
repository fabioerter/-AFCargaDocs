using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace AFCargaDocs.Models.Entidades.AxXML
{
    [XmlRoot(Namespace = "http://www.emc.com/ax")]
    public class AxStreamResult
    {
        //attributes
        [XmlAttribute]
        public int byteAmount { get; set; }
        [XmlAttribute]
        public bool iscontinue { get; set; }
        [XmlAttribute]
        public long fullsize { get; set; }
        [XmlAttribute]
        public long startbyte { get; set; }

        //elements
        [XmlElement]
        public string ImageBytes { get; set; }
        [XmlElement]
        public string NextStreamRequest { get; set; }


    }
}