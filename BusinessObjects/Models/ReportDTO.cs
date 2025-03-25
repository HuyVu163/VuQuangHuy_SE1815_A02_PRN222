using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class ReportDTO
    {
        public int totalNews { get; set; } = 0;
        public int totalCategory { get; set; } = 0;
        public int totalAuthor { get; set; } = 0;
        public int totalTag { get; set; } = 0;
        public String mostCategory { get; set; } = "";
    }
}
