using System.Collections.Generic;

namespace Project_HR.Models
{
    public class JobApplicationCreateView : JobApplication
    {
        public IEnumerable<User> Users { get; set; }
    }
}
