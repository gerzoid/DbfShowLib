using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbfShowLib.Sorting.Comparers
{

    public class NumericComparerAsc : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if ((y.Length <= 0) && (x.Length <= 0)) return 0;
            if (x.Length <= 0) return -1;
            if (y.Length <= 0) return 1;

            if (Convert.ToDouble(x).CompareTo(Convert.ToDouble(y)) == 0)
                return 0;
            else
                if (Convert.ToDouble(x).CompareTo(Convert.ToDouble(y)) > 0)
                return 1;
            else
                return -1;
        }
    }
   public class NumericComparerDesc : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if ((y.Length <= 0) && (x.Length <= 0)) return 0;
            if (x.Length <= 0) return 1;
            if (y.Length <= 0) return -1;

            if (Convert.ToDouble(x).CompareTo(Convert.ToDouble(y)) == 0)
                return 0;
            else
                if (Convert.ToDouble(x).CompareTo(Convert.ToDouble(y)) > 0)
                return -1;
            else
                return 1;
        }
    }
}
