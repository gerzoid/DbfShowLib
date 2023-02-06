using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DbfShowLib
{
    interface IDataBase
    {
        public void OpenFile(string fileName);
        public void Close();

        public string GetColumnName(int index);
        //public ReadRow();
    }
}
