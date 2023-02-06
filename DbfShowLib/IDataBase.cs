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

        public string GetColumnName(int  columnIndex);
        public string GetColumnType(int columnIndex);
        public int GetColumnSize(int columnName);
        //public ReadRow();
    }
}
