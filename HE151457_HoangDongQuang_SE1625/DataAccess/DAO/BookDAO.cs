using BusinessObject.Modals;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
	public class BookDAO
	{
		//Using Singleton Pattern
		private static BookDAO instance = null;
		private static readonly object instanceLock = new object();

		private BookDAO() { }

		public static BookDAO Instance
		{
			get
			{
				lock (instanceLock)
				{
					if (instance == null)
						instance = new BookDAO();
				}
				return instance;
			}
		}
		// Get list of books
		public List<Book> GetAllBook()
		{
			List<Book> listBook = new List<Book>();
			try
			{
				var _context = new Asm2Context();
				listBook = _context.Books.ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"{ex.Message}");
			}
			return listBook;
		}

		//get book by id
		public Book GetBookByID(int bookId)
		{
			Book book = null;
			try
			{
				var _context = new Asm2Context();
				book = _context.Books
					.Include(b => b.Pub)
					.Include(b => b.BookAuthors)
					.ThenInclude(ba => ba.Author)
					.FirstOrDefault(b => b.BookId == bookId);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"{ex.Message}");
			}
			return book;
		}

		//add book
		public bool AddBook(Book bookInfor)
		{
			Asm2Context _context = new Asm2Context();

			try
			{
				using (var transaction = _context.Database.BeginTransaction())
				{
					try
					{
						_context.Books.Add(bookInfor);
						foreach (var ba in bookInfor.BookAuthors)
						{
							_context.BookAuthors.Add(ba);
						}
						_context.SaveChanges();
						transaction.Commit();
						return true;
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						Console.WriteLine($"{ex.Message}");
						return false;
					}
				}
			}
			finally
			{
				_context.Dispose();
			}
		}


		//update book
		public bool UpdateBook(Book bookInfor)
		{
			Asm2Context _context = new Asm2Context();

			try
			{
				List<BookAuthor> authorListToDelete = _context.BookAuthors.Where(ba => ba.BookId == bookInfor.BookId).ToList();

				using (var transaction = _context.Database.BeginTransaction())
				{
					try
					{
						foreach (var ba in authorListToDelete)
						{
							_context.BookAuthors.Remove(ba);
						}

						foreach (var ba in bookInfor.BookAuthors)
						{
							_context.BookAuthors.Add(ba);
						}

						_context.Books.Update(bookInfor);
						_context.SaveChanges();
						transaction.Commit();
						return true;
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						Console.WriteLine($"{ex.Message}");
						return false;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"{ex.Message}");
				return false;
			}
			finally
			{
				_context.Dispose();
			}
		}

		//delete book
		public bool DeleteBook(int bookId)
		{
			if (bookId <= 0)
			{
				return false;
			}

			using (var _context = new Asm2Context())
			{
				try
				{
					var existedBook = _context.Books
						.Include(b => b.BookAuthors)
						.SingleOrDefault(b => b.BookId == bookId);

					if (existedBook == null)
					{
						return false;
					}

					using (var transaction = _context.Database.BeginTransaction())
					{
						try
						{
							_context.BookAuthors.RemoveRange(existedBook.BookAuthors);
							_context.Books.Remove(existedBook);
							_context.SaveChanges();
							transaction.Commit();
							return true;
						}
						catch (Exception ex)
						{
							transaction.Rollback();
							Console.WriteLine($"Lỗi khi xóa sách: {ex.Message}");
							return false;
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Lỗi khi xóa sách: {ex.Message}");
					return false;
				}
			}
		}
	}
}
