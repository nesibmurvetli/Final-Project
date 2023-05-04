using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace END_Project.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string PosName { get; set; }
        public string Responsibilities { get; set; }

        public bool IsDeactive { get; set; }

        public List<Employee> Employees { get; set; }

    }
}
