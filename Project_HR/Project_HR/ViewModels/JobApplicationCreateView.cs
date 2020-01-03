using System.Collections.Generic;

namespace Project_HR.Models
{
    public class JobApplicationCreateView : JobApplication
    {
        public IEnumerable<Company> Companies { get; set; }
    }
}
