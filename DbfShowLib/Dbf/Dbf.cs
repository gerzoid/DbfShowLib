using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DbfShowLib.DBF
{
    public partial class Dbf : StandartBase, IDataBase
    {
        byte[]? headerDBF;
        byte[]? columnDBF;

        private Header header;
        private Column column;
        private List<Column>? columns;

        public override string GetColumnName(int columnIndex)
        {
            return new string(columns[columnIndex].name).TrimEnd('\0');
        }
        public override int GetColumnSize(int columnIndex)
        {
            return columns[columnIndex].sizeBin;
        }

        public override string GetColumnType(int columnIndex)
        {
            if ((columnIndex > columns?.Count - 1) || (columnIndex < 0))
                return "ERR";

            switch (columns?[columnIndex].tip)
            {
                case 'C': return "CHAR";
                case 'D': return "DATE";
                case 'N': return "NUMERIC";
                case 'M': return "MEMO";
                case 'L': return "BOOL";
                case 'F': return "FLOAT";
                case 'T': return "DATETIME";
                case 'I': return "INTEGER";
                case 'Y': return "CURRENCY";
                case 'O': return "DOUBLE";
                case 'B': return "DOUBLE";
                default: return "UNKNOWN";
            }
        }

        public override string GetVersion()
        {
            if (headerDBF != null)
                switch (headerDBF?[0])
                {
                    case 2: return "FoxBASE";
                    case 3: return "FoxPro, FoxBASE+, dBASE III PLUS, dBASE IV (без memo)";
                    case 48: return "Visual FoxPro";
                    case 49: return "Visual FoxPro";
                    case 67: return "dBASE IV SQL файлы  (без memo)";
                    case 99: return "dBASE IV SQL system file  (без memo)";
                    case 131: return "FoxBASE+, dBASE III PLUS  (с memo)";
                    case 139: return "dBASE IV  (с memo)";
                    case 203: return "dBASE IV ASQL table file  (с memo)";
                    case 245: return "FoxPro 2.x  (или более ранних версий)  (с memo)";
                    case 251: return "FoxBASE";
                }
            return "Unknown";
        }
        public override string GetValue(int columnIndex, int rowIndex)
        {
            if ((rowIndex > header.recordsCount) || (columnIndex > columns?.Count)) return "ERR";    //проверка на диапазон кол-ва столбцов            

            long pos = header.headerSize + 1 + rowIndex * (long)header.recordSize + columns[columnIndex].pos;
            fileStreamDB?.Seek(pos, SeekOrigin.Begin);   //Размер заголовка+1 + нужная строка*размер записей+ смещение до нужной ячейки
            byte[] buf = new byte[columns[columnIndex].sizeBin];
            fileStreamDB?.Read(buf, 0, columns[columnIndex].sizeBin);

            return ParseValue(columns[columnIndex].tip, buf, columns[columnIndex].pos);
        }

        public string ParseValue(char tip, byte[] buff, int columnPos)
        {
            switch (tip)
            {
                case 'N':
                    string t = encoding.GetString(buff).Trim();
                    if (t.Trim() == "")
                        return "";
                    return t;
                case 'F':
                    t = encoding.GetString(buff).Trim();
                    if (t.Trim() == "")
                        return "";
                    return t;
                case 'L':

                    if ((Encoding.ASCII.GetString(buff, 0, 1) == "Y") || (Encoding.ASCII.GetString(buff, 0, 1) == "y") || (Encoding.ASCII.GetString(buff, 0, 1) == "T") || (Encoding.ASCII.GetString(buff, 0, 1) == "t"))
                        return "T";
                    else
                        return "F";
                case 'D':
                    string year = Encoding.ASCII.GetString(buff, 0, 4);
                    string month = Encoding.ASCII.GetString(buff, 4, 2);
                    string day = Encoding.ASCII.GetString(buff, 6, 2);
                    if ((year.Replace('\0', ' ').Trim() == "") || (month.Replace('\0', ' ').Trim() == "") || (day.Replace('\0', ' ').Trim() == ""))
                        return "";
                    else
                    {
                        if (day == "00") day = "01";
                        try
                        {
                            return new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(day)).ToShortDateString();
                        }
                        catch (Exception E)
                        {
                            Console.WriteLine(E.Message);
                            return "Ошибка";
                        }
                    }
                case 'T':

                    long lDate = System.BitConverter.ToInt32(buff, 0);
                    long lTime = System.BitConverter.ToInt32(buff, 4) * 10000L;
                    return JulianToDateTime(lDate).AddTicks(lTime).ToString();
                case 'I':

                    return Convert.ToString((BitConverter.ToInt32(buff, 0)));

                case 'Y':
                    return Convert.ToString(BitConverter.ToInt64(buff, 0));

                case 'O':
                    return Convert.ToString(Math.Round(BitConverter.ToDouble(buff, 0), 2));
                case 'B':
                    return Convert.ToString(Math.Round(BitConverter.ToDouble(buff, 0), 2));
                case 'M':
                    return "Not supported";
                default:
                    return encoding.GetString(buff).TrimEnd();
            }

        }


        public void ReadHeader()
        {
            long positionTemp = fileStreamDB.Position;
            fileStreamDB.Read(headerDBF, 0, 32);
            GCHandle pHandle = GCHandle.Alloc(headerDBF, GCHandleType.Pinned);
            header = (Header)Marshal.PtrToStructure(pHandle.AddrOfPinnedObject(), typeof(Header));
            pHandle.Free();

            CodePages cp = new CodePages();
            codePage = cp.FindByCode(Convert.ToString(header.codePage));
            encoding = Encoding.GetEncoding(Convert.ToInt32(codePage.codePage));

            double d = (header.headerSize - 33) / 32;
            if ((header.byte1 == 48) || (header.byte1 == 49) || (header.byte1 == 50))     //Если это VFP то true
                d = (header.headerSize - 33 - 263) / 32;

            countColumns = Convert.ToInt32(Math.Round(d));
            countRows = header.recordsCount;

            fileStreamDB.Position = positionTemp;
        }

        void ReadColumn()
        {
            fileStreamDB.Position = 32;
            GCHandle pHandle = new GCHandle();
            columns = new List<Column>();
            int pos = 0;
            for (int i = 1; i <= countColumns; i++)
            {
                fileStreamDB.Read(columnDBF, 0, 32);
                pHandle = GCHandle.Alloc(columnDBF, GCHandleType.Pinned);
                column = (Column)Marshal.PtrToStructure(pHandle.AddrOfPinnedObject(), typeof(Column));

                column.pos = pos;
                pos += column.sizeBin;

                if (column.tip != 0)
                    columns.Add(column);
            }
            countColumns = columns.Count;
            if (pHandle.IsAllocated)
                pHandle.Free();
        }
        public override void OpenFile(string fileName)
        {
            base.OpenFile(fileName);

            header = new Header();
            headerDBF = new byte[32];
            columnDBF = new byte[32];
            column = new Column();

            ReadHeader();
            ReadColumn();

        }

        public static byte[] StructToBuff<T>(T value) where T : struct
        {
            byte[] arr = new byte[Marshal.SizeOf(value)]; // создать массив
            GCHandle gch = GCHandle.Alloc(arr, GCHandleType.Pinned); // зафиксировать в памяти
            IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0); // и взять его адрес
            Marshal.StructureToPtr(value, ptr, true); // копировать в массив
            gch.Free(); // снять фиксацию
            return arr;

        }

    }
}