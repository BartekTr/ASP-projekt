using System;
using System.Collections.Generic;

namespace Project_HR.Models
{
    public partial class Company
    {
        public Company()
        {
            JobOffer = new HashSet<JobOffer>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public virtual ICollection<JobOffer> JobOffer { get; set; }
    }
}
