using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DbfShowLib
{
    abstract class StandartBase: IDataBase
    {
        protected int[]? filteredRecords;
        protected string? currentFileName;                       //Имя файла
        protected Stream? fileStreamDB;                   //Файловый поток для чтения и записи информации в файл
        protected Stream? fileStreamTMPDB;               //Файловый поток для чтения и записи информации в файл
        protected FileAccessMode fileAccessMode;
        protected int countColumns = 0;
        protected int countRows = 0;

        protected CodePages CodePages = new CodePages();

        protected int codePage = 1251;
        protected int codePageID = 201;
        protected string codePageName = "Russian Windows";

        public void Close()
        {
            fileStreamDB.Close();
        }

        public virtual void OpenFile(string fileName)
        {
            try
            {
                fileStreamDB = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                fileAccessMode = FileAccessMode.ReadWrite;
            }
            catch (Exception E)
            {
                fileStreamDB = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                fileAccessMode = FileAccessMode.Read;
            }
            fileStreamDB.Position = 0;
            currentFileName = fileName;
        }
    }
}
