using System;
using System.Collections.Generic;
using Autofac;
using Books.Core;
using Books.DataAccess;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Business
{
    public class PdfLibraryService
    {
        static IContainer _container;
        static PdfLibraryService()
        {
            ContainerBuilder builder = new ContainerBuilder(); //Initializes a new instance of the ContainerBuilder class.
            builder.RegisterType<PdfRepository>().As<IPdfRepository>();
            _container = builder.Build(); //Create a new container with the component registrations that have been made.
        }
        public static bool Delete(int id)
        {
            return _container.Resolve<IPdfRepository>().Delete(id); //retrieve a service from a context
        }
        public static List<PdfLibrary> GetAll()
        {
            return _container.Resolve<IPdfRepository>().GetAll; //retrieve a service from a context
        }
        public static PdfLibrary Save(PdfLibrary obj, EntityState state)
        {
            if (state == EntityState.Added)
                obj.id = _container.Resolve<IPdfRepository>().Insert(obj); //retrieve a service from a context
            else
                // TODO remove it later
                throw new NotImplementedException("This is not implemented");
            return obj;
        }

        public static PdfLibrary GetById(int id)
        {
            return _container.Resolve<IPdfRepository>().GetById(id); //retrieve a service from a context
        }
    }
}
