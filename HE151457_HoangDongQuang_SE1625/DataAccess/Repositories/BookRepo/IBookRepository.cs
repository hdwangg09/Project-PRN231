using BusinessObject.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.BookRepo
{
    public interface IBookRepository
    {
        //get list book
        List<Book> GetAllBook();

        //get book by id
        Book GetBookByID(int bookId);   

        //add book
        bool AddBook(Book bookInfor);

        //update book
        bool UpdateBook(Book bookInfor);

        //delete book
        bool DeleteBook(int bookId);
    }
}
