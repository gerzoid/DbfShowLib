using DbfShowLib.DBF;
using DbfShowLib.Sorting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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

        public virtual bool SetCodePage(byte CodePageID)
        {
            codePage = CodePages.FindByCode(Convert.ToString(CodePageID));
            return true;
        }

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

        //Выдает текущую позицию строки в зависимости от фильтрации или сортировки.. 
        public virtual int GetCurrentRowPosition(int RowIndex)
        {
            if (filteredRecords != null)
            {
                return filteredRecords.Length > 0 ? filteredRecords[RowIndex] : RowIndex;
            }
            else
                return RowIndex;
        }

        public virtual void SortValue(string ColumnName, SortingType sortingType)
        {
            int indexColumnInDB = GetColumnIndex(ColumnName);

            string[] filteredRecordsValue;
            if (filteredRecords == null)
            {
                filteredRecords = new int[countRows];
                filteredRecordsValue = new string[countRows];
                for (int i = 0; i <= countRows - 1; i++)
                {
                    filteredRecordsValue[i] = GetValue(indexColumnInDB, i);
                    filteredRecords[i] = i;
                }
            }
            else
            {
                filteredRecordsValue = new string[filteredRecords.Length];
                for (int i = 0; i <= filteredRecords.Length - 1; i++)
                    filteredRecordsValue[i] = GetValue(indexColumnInDB, filteredRecords[i]);
            }
            Sort sortClass = new Sort();
            sortClass.SortArray(ref filteredRecordsValue, ref filteredRecords, ColumnName, GetColumnType(indexColumnInDB), sortingType);
            //SortingColumns.sortingColumns.Clear();
            //SortingColumns.sortingColumns.Add(new SortingColumn() { columnName = ColumnName, columnIndex = indexColumnInDB, type = sortingType });
            filteredRecordsValue = null;
        }

        public abstract string GetValueWithFilter(int columnIndex, int rowIndex);

        public abstract Task<string>? SetValue(int columnIndex, int rowIndex, string value);

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
