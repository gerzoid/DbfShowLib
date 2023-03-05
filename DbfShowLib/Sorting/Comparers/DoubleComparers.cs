using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbfShowLib.Sorting.Comparers
{
    public class DoubleComparerAsc : IComparer<double>
   {
        public int Compare(double x, double y)
        {
            if (x.CompareTo(y) == 0)
                return 0;
            else
                if (x.CompareTo(y) > 0)
                return 1;
            else
                return -1;
        }
    }
    public class DoubleComparerDesc : IComparer<double>
    {
        public int Compare(double x, double y)
        {
            if (x.CompareTo(y) == 0)
                return 0;
            else
                if (x.CompareTo(y) > 0)
                return -1;
            else
                return 1;
        }
    }
}
