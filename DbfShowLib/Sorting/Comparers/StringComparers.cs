using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbfShowLib.Sorting.Comparers
{
    /// <summary>
    /// Специальный класс от Icomparer для корректной сортировки полей с типом DATE или DateTime
    /// </summary>
    public class StringComparerAsc : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if ((y.Length <= 0) && (x.Length <= 0)) return 0;

            if (x.CompareTo(y) == 0)
                return 0;
            else
                if (x.CompareTo(y) > 0)
                return 1;
            else
                return -1;
        }

    }
    public class StringComparerDesc : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if ((y.Length <= 0) && (x.Length <= 0)) return 0;

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
