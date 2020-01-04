using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_HR.Models
{
    public partial class JobOffer
    {
        public JobOffer()
        {
            JobApplication = new HashSet<JobApplication>();
        }

        public int Id { get; set; }
        [Display(Name = "Job title")]
        [Required]
        public string JobTitle { get; set; }
        public int CompanyId { get; set; }
        [Display(Name = "Salary from")]
        public decimal? SalaryFrom { get; set; }
        [Display(Name = "Salary to")]
        public decimal? SalaryTo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime Created { get; set; }
        public string Location { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime? ValidUntil { get; set; }
        public int? Hrid { get; set; }

        public virtual Company Company { get; set; }
        public virtual User Hr { get; set; }
        public virtual ICollection<JobApplication> JobApplication { get; set; }
    }
}
