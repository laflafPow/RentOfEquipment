using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass
{
    public class RentMethodsClass
    {
        public static double RentSalary(DateTime StartRentDate, DateTime EndRentDate, ClassHelper.Equipment equipment, int Qty)
        {
            int days = (StartRentDate - EndRentDate).Days;

            double finishSalary = Qty * (Convert.ToDouble(equipment.Price) * 0.25 * days);

            return finishSalary;
        }
    }
}
