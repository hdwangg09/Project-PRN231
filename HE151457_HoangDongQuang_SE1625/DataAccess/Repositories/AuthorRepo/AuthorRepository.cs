using BusinessObject.Modals;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.AuthorRepo
{
    public class AuthorRepository : IAuthorRepository
    {
        public List<Author> GetAllAuthor() => AuthorDAO.Instance.GetAllAuthor();

        public Author GetAuthorById(int authorId) => AuthorDAO.Instance.GetAuthorById(authorId);

        public bool AddAuthor(Author authorInfor) => AuthorDAO.Instance.AddAuthor(authorInfor);

        public bool UpdateAuthor(Author authorInfor) => AuthorDAO.Instance.EditAuthor(authorInfor);

        public bool DeleteAuthor(int authorId) => AuthorDAO.Instance.DeleteAuthor(authorId);


    }
}
