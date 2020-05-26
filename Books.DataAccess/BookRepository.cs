using Books.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Books.DataAccess
{
    public class BookRepository : IBookRepository
    {
        public bool Delete(int id)
        {
            using (IDbConnection db = new MySqlConnection(Helper.ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                // TOOD swap if works
                int result = db.Execute("delete from books.books_managment where id = @id", new { id }, commandType: CommandType.Text);

                //executing delete operation  
                return result != 0;
            }
        }

        public List<Book> GetAll
        {
            get
            {
                //connecting to data base
                using (IDbConnection db = new MySqlConnection(Helper.ConnectionString))
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();
                    //getting all the information from the data base
                    return db.Query<Book>("Select * from books.books_managment", commandType: CommandType.Text).ToList();
                }
            }
        }

        public int Insert(Book obj)
        {
            using (IDbConnection db = new MySqlConnection(Helper.ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                DynamicParameters p = new DynamicParameters();  //creating a dapper parameter 
                p.Add("p_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.AddDynamicParams(new { p_book_name = obj.book_name, p_book_author = obj.book_author, p_genre=obj.genre});

                db.Execute("sp_books_Insert", p, commandType: CommandType.StoredProcedure); //executing stored procedure
                int id = p.Get<int>("p_id");

                return id;
            }
        }

        public bool Update(Book obj)
        {
            using (IDbConnection db = new MySqlConnection(Helper.ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                //executing update operation
                int result = db.Execute("update books.books_managment set book_name = @book_name, book_author = @book_author, genre = @genre where id = @id;", 
                    new {
                        book_name = obj.book_name,
                        book_author = obj.book_author,
                        genre = obj.genre, 
                        id = obj.id
                    });
                return result != 0;
            }
        }
    }
}
