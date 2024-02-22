using BusinessObject.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.PublisherRepo
{
    public interface IPublisherRepository
    {
        //get list Publisher
        List<Publisher> GetAllPublisher();

        //get publisher by id
        Publisher GetPublisherByID(int publisherID);

        //add publisher
        bool AddPublisher(Publisher publisherInfor);

        //update publisher
        bool UpdatePublisher(Publisher publisherInfor);

        //delete publisher
        bool DeletePublisher(int publisherID);

    }
}
