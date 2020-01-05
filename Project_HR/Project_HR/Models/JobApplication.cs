using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_HR.Models
{
    public partial class JobApplication
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        [Required]
        public bool ContactAgreement { get; set; }
        public string CvUrl { get; set; }
        public int UserId { get; set; }
        public int StateId { get; set; }
        public virtual JobOffer Offer { get; set; }
        public virtual User User { get; set; }
        public virtual State State { get; set; }

    }
}
