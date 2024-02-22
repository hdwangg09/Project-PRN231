using BusinessObject.Modals;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class AuthorDAO
    {
        //Using Singleton Pattern
        private static AuthorDAO instance = null;
        private static readonly object instanceLock = new object();
        private AuthorDAO() { }

        public static AuthorDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                        instance = new AuthorDAO();
                }
                return instance;
            }
        }

        public List<Author> GetAllAuthor()
        {
            try
            {
                Asm2Context context = new Asm2Context();
                List<Author> listAuthor = context.Authors.ToList();
                return listAuthor;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Author GetAuthorById(int authorId)
        {

            try
            {
                using (var context = new Asm2Context())
                {
                    Author author = context.Authors.SingleOrDefault(a => a.AuthorId == authorId);
                    return author;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool AddAuthor(Author authorInfor)
        {
            try
            {
                using (var context = new Asm2Context())
                {
                    context.Authors.Add(authorInfor);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool EditAuthor(Author authorInfor)
        {
            try
            {
                using (var context = new Asm2Context())
                {
                    context.Entry(authorInfor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteAuthor(int authorId)
        {
            try
            {
                using (var context = new Asm2Context())
                {
                    var authorToDelete = context.Authors
                        .Include(a => a.BookAuthors)
                        .SingleOrDefault(a => a.AuthorId == authorId);

                    if (authorToDelete == null)
                    {
                        return false; 
                    }

                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (authorToDelete.BookAuthors != null && authorToDelete.BookAuthors.Any())
                            {
                                context.BookAuthors.RemoveRange(authorToDelete.BookAuthors);
                            }

                            context.Authors.Remove(authorToDelete);
                            context.SaveChanges();
                            transaction.Commit();
                            return true; 
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return false; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false; 
            }
        }

    }
}
