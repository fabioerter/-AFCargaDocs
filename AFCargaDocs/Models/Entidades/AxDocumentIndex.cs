using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AFCargaDocs.Models.Entidades
{
    public class AxDocumentIndex
    {

        override public string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" <ax:QueryItem xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ");
            stringBuilder.Append(" 	xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" id=\"-1\" ");
            stringBuilder.Append(" 	xmlns:ax=\"http://www.emc.com/ax\"> ");
            stringBuilder.Append(" 	<ax:Attributes /> ");
            stringBuilder.Append(" 	<ax:Fields> ");
            stringBuilder.Append(" 		<ax:Field id=\"1\" value=\"901\" isNull=\"false\" /> ");
            stringBuilder.Append(" 		<ax:Field id=\"2\" value=\"Index for delete(1)\" isNull=\"false\" /> ");
            stringBuilder.Append(" 		<ax:Field id=\"3\" value=\"02-16-2006\" isNull=\"false\" /> ");
            stringBuilder.Append(" 		<ax:Field id=\"4\" value=\"TIF\" isNull=\"false\" /> ");
            stringBuilder.Append(" 	</ax:Fields> ");
            stringBuilder.Append(" </ax:QueryItem> ");
            return stringBuilder.ToString();
        }
    }
}