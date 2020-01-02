using System;
using System.Collections.Generic;

namespace Project_HR.Models
{
    public partial class JobOffer
    {
        public JobOffer()
        {
            JobApplication = new HashSet<JobApplication>();
        }

        public int Id { get; set; }
        public string JobTitle { get; set; }
        public int CompanyId { get; set; }
        public decimal? SalaryFrom { get; set; }
        public decimal? SalaryTo { get; set; }
        public DateTime Created { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime? ValidUntil { get; set; }
        public int? Hrid { get; set; }

        public virtual Company Company { get; set; }
        public virtual User Hr { get; set; }
        public virtual ICollection<JobApplication> JobApplication { get; set; }
    }
}
