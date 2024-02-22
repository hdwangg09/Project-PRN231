using BusinessObject.Modals;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.BookRepo
{
    public class BookRepository : IBookRepository
    {
        public List<Book> GetAllBook() => BookDAO.Instance.GetAllBook();

        public Book GetBookByID(int bookId) => BookDAO.Instance.GetBookByID(bookId);

        public bool AddBook(Book bookInfor) => BookDAO.Instance.AddBook(bookInfor);

        public bool UpdateBook(Book bookInfor) => BookDAO.Instance.UpdateBook(bookInfor);

        public bool DeleteBook(int bookId) => BookDAO.Instance.DeleteBook(bookId);


    }
}
