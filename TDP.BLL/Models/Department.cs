using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDP.BLL.Models
{
    public class Department
    {
        public int ID { get; set; }
        public Guid IDGuid { get; set; }
        public string NameEN { get; set; }
        public string NameAR { get; set; }
    }
}

