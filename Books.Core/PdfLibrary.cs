using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Core
{
    public class PdfLibrary // Referance to pdf_library MySql table
    {
        //Refference to id in mySQL
        public int id { get; set; }

        //Refference to content in mySQL
        public byte[] content { get; set; }

        //Refference to file_name in mySQL
        public string file_name { get; set; }
    }
}
