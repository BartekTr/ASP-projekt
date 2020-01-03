using System.Collections.Generic;

namespace Project_HR.Models
{
    public class JobOfferCreateView : JobOffer
    {
        public IEnumerable<Company> Companies { get; set; }
    }
}
