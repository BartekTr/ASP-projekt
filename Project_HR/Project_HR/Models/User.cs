using System;
using System.Collections.Generic;

namespace Project_HR.Models
{
    public partial class User
    {
        public User()
        {
            JobApplication = new HashSet<JobApplication>();
            JobOffer = new HashSet<JobOffer>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAdress { get; set; }
        public int RoleId { get; set; }
        public string PhoneNumber { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<JobApplication> JobApplication { get; set; }
        public virtual ICollection<JobOffer> JobOffer { get; set; }
    }
}
