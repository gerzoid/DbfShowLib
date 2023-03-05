using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbfShowLib.Sorting.Comparers
{
    public class DateComparerAsc : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if ((y.Length < 8) && (x.Length < 8)) return 0;
            if (x.Length < 8) return -1;
            if (y.Length < 8) return 1;

            DateTime d1 = DateTime.Parse(x);
            DateTime d2 = DateTime.Parse(y);
            if (d1 < d2)
                return -1;
            else
                if (d1 > d2)
                return 1;
            else
                return 0;
        }
    }

    public class DateComparerDesc : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if ((y.Length < 8) && (x.Length < 8)) return 0;
            if (x.Length < 8) return -1;
            if (y.Length < 8) return 1;

            DateTime d1 = DateTime.Parse(x);
            DateTime d2 = DateTime.Parse(y);
            if (d1 < d2)
                return 1;
            else
                if (d1 > d2)
                return -1;
            else
                return 0;
        }

    }
}

