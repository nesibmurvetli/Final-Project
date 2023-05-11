using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace END_Project.Models
{
    public class EmailMessage
    {
        public int Id { get; set; }
        public string? To { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}
