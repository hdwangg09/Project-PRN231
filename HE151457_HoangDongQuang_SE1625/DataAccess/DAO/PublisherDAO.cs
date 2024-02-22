using BusinessObject.Modals;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    internal class PublisherDAO
    {
        private static PublisherDAO instance = null;
        private static readonly object instanceLock = new object();

        private PublisherDAO() { }

        public static PublisherDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                        instance = new PublisherDAO();
                }
                return instance;
            }
        }

        public List<Publisher> GetAllPublisher()
        {
            try
            {
                Asm2Context context = new Asm2Context();
                List<Publisher> listPub = context.Publishers.ToList();
                return listPub;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Publisher GetPublisherByID(int publisherID)
        {
            try
            {
                using (var context = new Asm2Context())
                {
                    Publisher pub = context.Publishers.SingleOrDefault(p => p.PibId == publisherID);
                    return pub;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool AddPublisher(Publisher publisherInfor)
        {
            try
            {
                using (var context = new Asm2Context())
                {
                    context.Publishers.Add(publisherInfor);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdatePublisher(Publisher publisherInfor)
        {
            try
            {
                using (var context = new Asm2Context())
                {
                    context.Entry(publisherInfor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeletePublisher(int publisherID)
        {
            try
            {
                using (var context = new Asm2Context())
                {
                    Publisher publisherToDelete = context.Publishers
                        .Include(p => p.Books)
                            .ThenInclude(b => b.BookAuthors)
                        .SingleOrDefault(p => p.PibId == publisherID);

                    if (publisherToDelete != null)
                    {
                        foreach (var book in publisherToDelete.Books)
                        {
                            context.BookAuthors.RemoveRange(book.BookAuthors);
                        }
                        context.Books.RemoveRange(publisherToDelete.Books);
                        context.Publishers.Remove(publisherToDelete);
                        context.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
