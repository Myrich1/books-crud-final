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
    public class PdfRepository : IPdfRepository
    {

        public bool Delete(int id)
        {
            using (IDbConnection db = new MySqlConnection(Helper.ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                // TOOD swap if works
                int result = db.Execute("delete from pdf_library where id=@id", new { id }, commandType: CommandType.Text);
                //executing delete operation  
                return result != 0;
            }
        }


        public List<PdfLibrary> GetAll
        {
            get
            {
                //connecting to data base
                using (IDbConnection db = new MySqlConnection(Helper.ConnectionString))
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();
                    //getting all the information from the data base
                    return db.Query<PdfLibrary>("select * from pdf_library", commandType: CommandType.Text).ToList();
                }
            }
        }

        public PdfLibrary GetById(int id)
        {
            
                //connecting to data base
                using (IDbConnection db = new MySqlConnection(Helper.ConnectionString))
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();
                    //getting all the information from the data base
                    return db.Query<PdfLibrary>(
                        "Select * from pdf_library where id=id",
                        commandType: CommandType.Text).First<PdfLibrary>();
                }
            
        }

        public int Insert(PdfLibrary obj)
        {
            using (IDbConnection db = new MySqlConnection(Helper.ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                   db.Open();
                DynamicParameters p = new DynamicParameters();  //creating a dapper parameter 
                p.Add("p_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.AddDynamicParams(new { p_content = obj.content, p_file_name= obj.file_name });

                db.Execute("sp_library_Insert", p, commandType: CommandType.StoredProcedure); //executing stored procedure
                int id = p.Get<int>("p_id");

                return id;
            }
        }
    }
}
