using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DbfShowLib
{
    interface IDataBase
    {
        public void OpenFile(string fileName);
        public void Close();
        public string GetVersion();
        public int GetColumnIndex(string columnName);
        public string GetColumnName(int  columnIndex);
        public string GetColumnType(int columnIndex);
        public int GetColumnSize(int columnName);
        public string GetValue(int columnIndex, int rowIndex);
        public Task<string>? SetValue(int columnIndex, int rowIndex, string value);
        public bool IsDeleted(int rowIndex);
        public bool SetCodePage(byte CodePageID);
    }
}
