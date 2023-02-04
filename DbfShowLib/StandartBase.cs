using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DbfShowLib
{
    public abstract class StandartBase
    {
        protected int[]? filteredRecords;
        protected string? fileName;                       //Имя файла
        protected Stream? fileStreamDB;                   //Файловый поток для чтения и записи информации в файл
        protected Stream? fileStreamTMPDB;               //Файловый поток для чтения и записи информации в файл
        protected int countColumns = 0;
        protected int countRows = 0;

        protected CodePages CodePages = new CodePages();

        protected int codePage = 1251;
        protected int codePageID = 201;
        protected string codePageName = "Russian Windows";

    }
}
