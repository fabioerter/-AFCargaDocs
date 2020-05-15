using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AFCargaDocs.Models.Entidades
{
    public class AxStreamData
    {
        //string encryption;
        string imageBytes;
        string key;
        string origFile;
        //string startbyte;

        public AxStreamData(string imageBytes, string key, string origFile)
        {
            this.imageBytes = imageBytes;
            this.key = key;
            this.origFile = origFile;
        }

        public string ImageBytes { get => imageBytes; set => imageBytes = value; }
        public string Key { get => key; set => key = value; }
        public string OrigFile { get => origFile; set => origFile = value; }

        override public string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<?xml version=\"1.0\" encoding=\"utf - 16\"?>  ");
            sb.Append("<ax:AxStreamData xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"  ");
            sb.Append("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" encryption=\"false\"  ");
            sb.Append("ImageBytes=\"" + this.ImageBytes + "\" key=\"" + this.Key + "\" ");
            sb.Append("origFile = \"" + this.OrigFile + "\"  ");
            sb.Append("startbyte = \"0\" ");
            sb.Append("xmlns:ax=\"http://www.emc.com/ax\">  ");
            sb.Append("</ax:AxStreamData>  ");

            return sb.ToString();
        }

    }
}