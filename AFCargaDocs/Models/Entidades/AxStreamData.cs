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
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" <?xml version=\"1.0\" encoding=\"utf - 16\"?> ");
            stringBuilder.Append(" <ax:AxStreamResult xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ");
            stringBuilder.Append(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" key=\"0\" ");
            stringBuilder.Append(" origFile=\"nomeArchivo\" byteAmount=\"145\" iscontinue=\"false\" ");
            stringBuilder.Append(" xmlns:ax=\"http://www.emc.com/ax\"> ");
            stringBuilder.Append(" 	<ax:ImageBytes>"+ this.ImageBytes + "</ax:ImageBytes> ");
            stringBuilder.Append(" </ax:AxStreamResult> ");
            return stringBuilder.ToString();
        }

    }
}