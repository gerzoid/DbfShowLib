using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DbfShowLib
{
    public interface IDataBase
    {
        public void OpenFile();
        public void Close();
        //public ReadRow();
    }
}
