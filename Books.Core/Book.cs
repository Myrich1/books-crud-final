using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Core
{
    public class Book //reference to mySql data base
    {
        public int id { get; set; }
        //Refference to id in mySQL
        public string book_name{ get; set; }
        //Refference to book_name in mySQL
        public string book_author { get; set; }
        //Refference to book_author in mySQL
        public string genre { get; set; }
        //Refference to book_author in mySQL
    }
}
