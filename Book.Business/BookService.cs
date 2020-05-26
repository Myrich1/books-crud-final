using System;
using System.Collections.Generic;
using Autofac;
using Books.Core;
using Books.DataAccess;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Books.Business
{
    public class BookService  //for easy change of the dataAccess to any ORM or to any data base
    {
        static IContainer _container;
        static BookService()
        {
            ContainerBuilder builder = new ContainerBuilder(); //Initializes a new instance of the ContainerBuilder class.
            builder.RegisterType<BookRepository>().As<IBookRepository>();
            _container=builder.Build(); //Create a new container with the component registrations that have been made.
        }
        public static bool Delete(int bookID)
        {
            return _container.Resolve<IBookRepository>().Delete(bookID); //retrieve a service from a context
        }
        public static List<Core.Book> GetAll()
        {
            return _container.Resolve<IBookRepository>().GetAll; //retrieve a service from a context
        }
        public static Core.Book Save(Core.Book obj, EntityState state)  //save
        {
            _container.Resolve<IBookRepository>().Insert(obj); //retrieve a service from a context

            return obj;
        }

        public static bool Update(Core.Book obj, EntityState state)  //edit
        {
           return _container.Resolve<IBookRepository>().Update(obj);  
        }
    }
}
