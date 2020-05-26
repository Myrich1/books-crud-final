using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Book.Business;
using Books.Business;
using Books.Core;
using Dapper; 

namespace Books
{
   
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        EntityState objState = EntityState.Unchanged;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                pdfLibraryBindingSource.DataSource = PdfLibraryService.GetAll();
                bookBindingSource.DataSource = BookService.GetAll();
                pContainer.Enabled = false; //not being able to write anything

            }
            //throwing an exception if not connected
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) //Adding new book
        {
            objState = EntityState.Added;
            pContainer.Enabled = true;
            bookBindingSource.Add(new Core.Book());
            bookBindingSource.MoveLast();  //new book is going at last place
            txtBookName.Focus();

        }

        private void btnEdit_Click(object sender, EventArgs e)  //Update
        {
            objState = EntityState.Changed;
            pContainer.Enabled = true;  //we can change information when container is enabled
            txtBookName.Focus();
        }
         
        private void metroGrid1_CellClick(object sender, DataGridViewCellEventArgs e) //when any part of cell is clicked
        {
        }

        private void btnDelete_Click(object sender, EventArgs e) //delete information
        {
            objState = EntityState.Deleted;
            if (MetroFramework.MetroMessageBox.Show(this, "Are you sure want to delete this record>", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //sending a "Are you sure message ?"
            {
                try
                {
                    Core.Book obj = bookBindingSource.Current as Core.Book;
                    if (obj != null)
                    {
                        bookBindingSource.List.Remove(obj);
                        BookService.Delete(obj.id);
                        metroGrid1.Refresh();
                    }
                }
                //throwing an exception if not connected
                catch (Exception ex)
                {
                    MetroFramework.MetroMessageBox.Show(this, ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)  //save the changes
        {
            try
            {
                // handle update existing record
                Core.Book bookToUpdate = bookBindingSource.Current as Core.Book;

                Core.Book newBook = new Core.Book  //getting the information from text boxes
                {
                    genre = metroTextBox1.Text,
                    book_name = txtBookName.Text,
                    book_author = txtBookAuthor.Text
                };

                if (bookToUpdate.id != 0)
                {
                    newBook.id = bookToUpdate.id;
                    BookService.Update(newBook, objState);
                    // updating the binding source
                    bookBindingSource.DataSource = BookService.GetAll(); 
                    return;
                }
               
                if (newBook != null)  //saving the new book
                {
                    newBook = BookService.Save(newBook, objState);
                    bookBindingSource.Add(newBook);
                    //updating the data base
                    metroGrid1.Refresh();
                    pContainer.Enabled = false;
                   
                }
                bookBindingSource.DataSource = BookService.GetAll(); // updating the binding source
            }
            //throwing an exception if not connected
            catch (Exception ex)
            {
                MetroFramework.MetroMessageBox.Show(this, ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void htmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void txtBookID_Click(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            byte[] fileContent = null;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
              //opening the directory
                openFileDialog.InitialDirectory = "d:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                         filePath = openFileDialog.FileName;
                         //Read the contents of the file into a stream
                         var fileStream = openFileDialog.OpenFile();

                         using (StreamReader reader = new StreamReader(fileStream))
                         {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                reader.BaseStream.CopyTo(ms);
                                fileContent = ms.ToArray();
                            }
                         }
               }
             }
            //file is going in the binding source
            PdfLibrary pdfLibrary = new PdfLibrary();
            pdfLibrary.file_name = filePath;
            pdfLibrary.content = fileContent;
            pdfLibraryBindingSource.Add(pdfLibrary);
            PdfLibraryService.Save(pdfLibrary, EntityState.Added);
        }

        private void metroGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void axAcroPDF1_OnError(object sender, EventArgs e)
        {

        }

        private void pdfLibraryBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void metroGrid_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {

        }

        private void metroGrid2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            const int loadIndex = 3;
            const int deleteIndex = 4;

            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                int index = e.RowIndex;
                PdfLibrary library = (PdfLibrary)pdfLibraryBindingSource.List[index];

                if (e.ColumnIndex == loadIndex)  //pdf loader 
                {
                    
                    if (library.file_name != null)
                    {
                        string filePath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
                        File.WriteAllBytes(filePath, library.content);

                        axAcroPDF1.LoadFile(filePath);
                        axAcroPDF1.src = filePath;

                        axAcroPDF1.setShowToolbar(false); //disable pdf toolbar.
                        axAcroPDF1.Enabled = true;
                        axAcroPDF1.Show();
                    }
                } else if(e.ColumnIndex == deleteIndex)  //delete button 
                {

                    //if()

                    pdfLibraryBindingSource.List.Remove(library);
                    PdfLibraryService.Delete(library.id);
                    metroGrid2.Refresh();

                }
               

            }
        }
    }
}
