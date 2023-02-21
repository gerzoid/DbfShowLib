using DbfShowLib.DBF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DbfShowLib
{
    public abstract class StandartBase: IDataBase
    {
        protected int[]? filteredRecords;
        protected string? currentFileName;                       //Имя файла
        protected Stream? fileStreamDB;                   //Файловый поток для чтения и записи информации в файл
        protected Stream? fileStreamTMPDB;               //Файловый поток для чтения и записи информации в файл
        protected FileAccessMode fileAccessMode;
        protected int countColumns = 0;
        protected int countRows = 0;

        protected Encoding encoding = Encoding.ASCII;

        protected CodePages CodePages = new CodePages();
        protected CodePage codePage = new CodePage() { code = "201", codePage = "1251", name = "Russian Windows" };
       
        public CodePage CodePage { get { return codePage; } }

        public int CountColumns {  get { return countColumns; } }
        public int CountRows { get {  return countRows; } }

        public virtual bool IsDeleted(int rowIndex)
        {
            return false;
        }

        public virtual string GetColumnName(int columnIndex)
        {
            return "Col"+columnIndex;
        }

        public abstract int GetColumnIndex(string columnName);


        public abstract string GetVersion();
        public virtual string GetColumnType(int columnIndex)
        {
            return "char";
        }
        public abstract int GetColumnSize(int columnIndex);

        public void Close()
        {
            fileStreamDB?.Close();
        }

        public abstract bool SetValue(int columnIndex, int rowIndex, string value);

        public virtual string GetValue(int columnIndex, int rowIndex)
        {
            return "";
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
                Console.WriteLine(E.Message);
            }
            fileStreamDB.Position = 0;
            currentFileName = fileName;
        }
    }
}
