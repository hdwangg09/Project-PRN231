using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Modals;
namespace DataAccess.Repositories.AuthorRepo
{
    public interface IAuthorRepository
    {
        //get list author
        List<Author> GetAllAuthor();

        //get author by id
        Author GetAuthorById(int authorId);

        //add author
        bool AddAuthor(Author authorInfor);

        //update author
        bool UpdateAuthor(Author authorInfor);

        //delete author
        bool DeleteAuthor(int authorId);

        //List<Author> GetAuthorByBookId(int bookId);
    }
}
