using BusinessObject.Modals;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.PublisherRepo
{
    public class PublisherRepository : IPublisherRepository
    {
        public List<Publisher> GetAllPublisher() => PublisherDAO.Instance.GetAllPublisher();

        public Publisher GetPublisherByID(int publisherID) => PublisherDAO.Instance.GetPublisherByID(publisherID);

        public bool AddPublisher(Publisher publisherInfor) => PublisherDAO.Instance.AddPublisher(publisherInfor);

        public bool UpdatePublisher(Publisher publisherInfor) => PublisherDAO.Instance.UpdatePublisher(publisherInfor);

        public bool DeletePublisher(int publisherID) => PublisherDAO.Instance.DeletePublisher(publisherID);

    }
}
