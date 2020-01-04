using System.Collections.Generic;

namespace Project_HR.Models
{
    public class PagingViewModel
    {
        public IEnumerable<JobOffer> JobOffers { get; set; }
        public int TotalPage { get; set; }

    }
}
