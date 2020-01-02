﻿using System;
using System.Collections.Generic;

namespace Project_HR.Models
{
    public partial class JobApplication
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public bool ContactAgreement { get; set; }
        public string CvUrl { get; set; }
        public int UserId { get; set; }

        public virtual JobOffer Offer { get; set; }
        public virtual User User { get; set; }
    }
}
