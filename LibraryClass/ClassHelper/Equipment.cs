using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass.ClassHelper
{
    public partial class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int IdType { get; set; }
        public string PhotoPath { get; set; }
        public bool Status { get; set; }
        public byte ProductLive { get; set; }
        public bool IsDeleted { get; set; }
        public string Photo { get; set; }
    }
}
