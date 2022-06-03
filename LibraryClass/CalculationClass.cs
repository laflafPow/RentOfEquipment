using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClass
{
    public class CalculationClass
    {
        public static int DaysInRent(int NumberOfDayOfBegin, int NumberOfDayOfEnd)
        {
            return NumberOfDayOfEnd - NumberOfDayOfBegin + 1;
        }
        public static decimal CostOfRent(int DaysInRent, decimal Cost, int Count)
        {
            return (Cost * DaysInRent) * Count;
        }
        public static decimal Discount(int NumberOfDayOfBegin, int NumberOfDayOfEnd, decimal Cost, int NumberOfDayFactOfEnd)
        {
            return Cost * DaysInRent(NumberOfDayOfBegin, NumberOfDayFactOfEnd) - Cost * Convert.ToDecimal(0.005) * Convert.ToDecimal(DaysInRent(NumberOfDayFactOfEnd, NumberOfDayOfEnd));
        }

        public static decimal Penality(int NumberOfDayOfBegin, int NumberOfDayOfEnd, decimal Cost, int NumberOfDayFactOfEnd)
        {
            return Cost * DaysInRent(NumberOfDayOfBegin, NumberOfDayFactOfEnd) + Cost * Convert.ToDecimal(0.01) * Convert.ToDecimal(DaysInRent(NumberOfDayFactOfEnd, NumberOfDayOfEnd));
        }
    }
}
