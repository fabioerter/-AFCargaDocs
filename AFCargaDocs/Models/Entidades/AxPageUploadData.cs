using AFCargaDocs.Models.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AFCargaDocs.Models.Entidades
{
    public class AxPageUploadData
    {
        /// <summary>
        /// Action type; valid page upload action types
        /// are listed in the PageUploadAction
        /// Enumeration section
        /// </summary>
        EAxPageUploadAction act;
        /// <summary>
        /// Full file path of an image file to upload as a
        /// page
        /// </summary>
        string filepath;
        /// <summary>
        /// Type of the uploaded image file; valid file
        /// types are listed in the FileType Enumeration
        /// section; you do not have to assign a value to
        /// this property if you do not know exactly the
        /// type of the image uploaded; the server will
        /// figure out the image type by itself
        /// </summary>
        EAxFileType filetype;
        /// <summary>
        /// 1-based page position in a document you
        /// either want to replace with the new page or
        /// next to which you want to insert a new page
        /// </summary>
        int pos;
        /// <summary>
        /// ndicates whether to split a multi-page image
        /// such as PDF, TIFF, or text.The default value
        /// is True.Note that at this release, TIFF and
        /// text images will be split automatically even if
        /// this parameter is set to False.If the FileType
        /// parameter is set to something other than
        /// AXTypes.FileType.FT_UNKNOWN, multiple
        /// PDF pages will be split at the server side,
        /// disregarding the value of this parameter.
        /// </summary>
        bool splitimg;
        /// <summary>
        /// Number of subpages in the uploaded image
        /// file; you do not have to assign this property if
        /// you do not know the number of subpages in
        /// the uploaded image file; the server will
        /// calculate the subpage count
        /// </summary>
        int subpages;

        public AxPageUploadData()
        {

        }

        public AxPageUploadData(EAxPageUploadAction act, string filepath, EAxFileType filetype, int pos, bool splitimg, int subpages)
        {
            this.act = act;
            this.filepath = filepath;
            this.filetype = filetype;
            this.pos = pos;
            this.splitimg = splitimg;
            this.subpages = subpages;
        }

        public EAxPageUploadAction Act { get => act; set => act = value; }
        public string Filepath { get => filepath; set => filepath = value; }
        public EAxFileType Filetype { get => filetype; set => filetype = value; }
        public int Pos { get => pos; set => pos = value; }
        public bool Splitimg { get => splitimg; set => splitimg = value; }
        public int Subpages { get => subpages; set => subpages = value; }

        override public string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
            sb.Append("<AxPageUploadData file_path=\"" + this.Filepath + "\" ");
            sb.Append("action=\"" + this.Act + "\" position=\"" + this.Pos + "\" ");
            sb.Append("subpages=\"" + this.Subpages + "\" file_type=\"" + this.Subpages + "\" />");
            return sb.ToString();
        }
    }
}