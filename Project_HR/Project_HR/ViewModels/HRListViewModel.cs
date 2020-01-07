using System.Collections.Generic;

namespace Project_HR.Models
{
    public class HRListViewModel
    {
        public IEnumerable<User> Users{ get; set; }
        public JobOffer Offer { get; set; }
    }
}
