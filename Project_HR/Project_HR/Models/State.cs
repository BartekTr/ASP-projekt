using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_HR.Models
{
    public partial class State
    {
        public State()
        {
            JobApplication = new HashSet<JobApplication>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<JobApplication> JobApplication { get; set; }

    }
}
