using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Core
{
    public interface IPdfRepository  //for more flexibility
    {
        List<PdfLibrary> GetAll { get; }
        int Insert(PdfLibrary obj);
      //  bool Update(PdfLibrary obj);
        bool Delete(int id);

        PdfLibrary GetById(int id);
    }
}
