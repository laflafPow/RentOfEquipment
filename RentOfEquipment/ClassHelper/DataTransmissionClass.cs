using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentOfEquipment.ClassHelper
{
    public class DataTransmissionClass
    {
        public static EF.Client GetClient { get; set; }
        public static EF.Employee GetEmployee { get; set; }
        public static EF.Equipment GetEquipment { get; set; }
    }
}
