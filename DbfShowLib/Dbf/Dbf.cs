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

        public override int GetColumnIndex(string columnName)
        {
            string tmp;
            for (int c = 0; c <= countColumns - 1; c++)
            {
                tmp = new String(columns[c].name).ToUpper();
                if (tmp.IndexOf('\0') >= 0)
                {
                    if (tmp.Substring(0, tmp.IndexOf('\0')).Trim() == columnName.ToUpper())  // Поиск колонки в массиве                
                        return c;
                }
                else
                    if (tmp == columnName.ToUpper())  // Поиск колонки в массиве
                    return c;
            }
            return -1;
        }
        public override bool SetCodePage(byte CodePageID)
        {
            try
            {
                fileStreamDB?.Seek(29, SeekOrigin.Begin);
                fileStreamDB?.WriteByte(CodePageID);
                fileStreamDB?.Flush();
                header.codePage = CodePageID;
                codePage = CodePages.FindByCode(Convert.ToString(CodePageID));
                return true;
            }
            catch
            {
                return false;
            }
        }
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
        public override bool IsDeleted(int rowIndex)
        {
            long tempPostion = fileStreamDB.Position;
            long pos = header.headerSize + (long)rowIndex * (long)header.recordSize;
            fileStreamDB.Seek(pos, SeekOrigin.Begin);   //Размер заголовка+1 + нужная строка*размер записей+ смещение до нужной ячейки
            byte[] b = new byte[1];
            fileStreamDB.Read(b, 0, 1);
            if (b[0] == 32) return false;
            if (b[0] == 42) return true;
            fileStreamDB.Position = tempPostion;
            return false;
        }
        public override string GetValue(int columnIndex, int rowIndex)
        {
            if ((rowIndex > header.recordsCount) || (columnIndex > columns?.Count)) return "ERR";    //проверка на диапазон кол-ва столбцов            

            long pos = header.headerSize + 1 + rowIndex * (long)header.recordSize + columns[columnIndex].pos;
            fileStreamDB?.Seek(pos, SeekOrigin.Begin);   //Размер заголовка+1 + нужная строка*размер записей+ смещение до нужной ячейки
            byte[] buf = new byte[columns[columnIndex].sizeBin];
            fileStreamDB?.Read(buf, 0, columns[columnIndex].sizeBin);

            return ParseValue(columns[columnIndex].tip, buf);
        }
        public override string? SetValue(int columnIndex, int rowIndex, string value)
        {
            if ((columnIndex < 0) || (rowIndex < 0))
                return null;
            if (rowIndex > header.recordsCount) return null;

            if (value == null) value = "";

            long pos = header.headerSize + 1 + (long)rowIndex * (long)header.recordSize + columns[columnIndex].pos;
            fileStreamDB?.Seek(pos, SeekOrigin.Begin);   //Размер заголовка+1 + нужная строка*размер записей+ смещение до нужной ячейки                        

            byte[] buf = new byte[columns[columnIndex].sizeBin];
            byte[] buf2 = new byte[4];

            //Для числовых дробных значений, чтобы ноли были после точки
            string format = "0";
            int zp = columns[columnIndex].zpt;
            if (zp > 0)
            {
                format += ".";
                //format += separator;
                for (int x = 1; x <= zp; x++)
                    format += "0";
            }

            switch (GetColumnType(columnIndex))
            {
                case "DATETIME":
                    if (value != "")
                    {
                        //DateTime dd = Convert.Toda.ToDateTime(value);
                        DateTime dd;
                        if (!DateTime.TryParse(value, out dd))
                            return null;
                        buf = BitConverter.GetBytes(ToJulian(dd));
                        TimeSpan span = new TimeSpan(dd.Hour, dd.Minute, dd.Second);
                        int sec = Convert.ToInt32(span.TotalMilliseconds);
                        buf2 = BitConverter.GetBytes(Convert.ToInt32(sec));
                        buf[4] = buf2[0];
                        buf[5] = buf2[1];
                        buf[6] = buf2[2];
                        buf[7] = buf2[3];
                    }
                    else
                    {
                        if (value.Length > columns[columnIndex].sizeBin)
                            value = value.Substring(0, columns[columnIndex].sizeBin - 1);
                        string tempString1 = "";
                        if (value.Length < columns[columnIndex].sizeBin)
                        {
                            for (int x = 0; x <= columns[columnIndex].sizeBin - value.Length - 1; x++)
                                tempString1 += " ";
                        }
                        value += tempString1;
                        buf = encoding.GetBytes(value);
                    }
                    break;
                case "DATE":
                    if (value != "")
                    {
                        DateTime dateTime = Convert.ToDateTime(value);
                        string month = Convert.ToString(dateTime.Month);
                        string day = Convert.ToString(dateTime.Day);
                        if (month.Length == 1)
                            month = "0" + month;
                        if (day.Length == 1)
                            day = "0" + day;
                        buf = encoding.GetBytes(Convert.ToString(dateTime.Year) + month + day);
                    }
                    else
                    {
                        if (value.Length > columns[columnIndex].sizeBin)
                            value = value.Substring(0, columns[columnIndex].sizeBin - 1);
                        string tempString1 = "";
                        if (value.Length < columns[columnIndex].sizeBin)
                        {
                            for (int x = 0; x <= columns[columnIndex].sizeBin - value.Length - 1; x++)
                                tempString1 += " ";
                        }
                        value += tempString1;
                        buf = encoding.GetBytes(value);
                    }
                    break;
                case "BOOL": var val = value.ToUpper().Trim();
                        if ((val != "T") || (val != "F"))
                            return null;
                        buf = encoding.GetBytes(value);
                    break;
                case "DOUBLE":
                    buf = BitConverter.GetBytes(Math.Round(Convert.ToDouble(value), columns[columnIndex].zpt));
                    break;
                case "MEMO":
                    return null;
                    break;
                case "INTEGER":
                    buf = BitConverter.GetBytes(Convert.ToInt32(value));
                    break;
                case "CURRENCY":
                    buf = BitConverter.GetBytes(Convert.ToInt64(value));
                    break;
                case "NUMERIC":
                    string onePart = "";
                    string twoPart = "";

                    onePart = value;
                    var separator = ',';
                    if (value.IndexOf(separator) > 0)
                    {
                        onePart = value.Substring(0, value.IndexOf(separator));
                        twoPart = value.Substring(value.IndexOf(separator) + 1, value.Length - value.IndexOf(separator) - 1);
                    }

                    if (onePart.Length > columns[columnIndex].sizeBin - zp - 1)
                        onePart = onePart.Substring(0, columns[columnIndex].sizeBin - zp - 1);

                    if (value.Trim() != "")
                        value = onePart + separator + twoPart;
                    if (value.TrimEnd().TrimStart() != "")
                        value = Math.Round(Convert.ToDouble(value), columns[columnIndex].zpt).ToString(format);

                    string tempString2 = "";
                    if (value.Length < columns[columnIndex].sizeBin)
                    {
                        for (int x = 0; x <= columns[columnIndex].sizeBin - value.Length - 1; x++)
                            tempString2 += " ";
                    }
                    value = tempString2 + value;

                    value = value.Replace(',', '.');

                    buf = encoding.GetBytes(value);
                    break;
                case "FLOAT":

                    onePart = "";
                    twoPart = "";

                    separator = ',';
                    onePart = value;
                    if (value.IndexOf(separator) > 0)
                    {
                        onePart = value.Substring(0, value.IndexOf(separator));
                        twoPart = value.Substring(value.IndexOf(separator) + 1, value.Length - value.IndexOf(separator) - 1);
                    }

                    if (onePart.Length > columns[columnIndex].sizeBin - zp - 1)
                        onePart = onePart.Substring(0, columns[columnIndex].sizeBin - zp - 1);

                    value = onePart + separator + twoPart;
                    if (value.TrimEnd().TrimStart() != "")
                        value = Math.Round(Convert.ToDouble(value), columns[columnIndex].zpt).ToString(format);

                    tempString2 = "";
                    if (value.Length < columns[columnIndex].sizeBin)
                    {
                        for (int x = 0; x <= columns[columnIndex].sizeBin - value.Length - 1; x++)
                            tempString2 += " ";
                    }
                    value = tempString2 + value;
                    //!!!!!!!!!!
                    value = value.Replace(',', '.');
                    //!!!!!!!!!!
                    buf = encoding.GetBytes(value);
                    break;


                default:
                    if (value.Length > columns[columnIndex].sizeBin)
                        value = value.Substring(0, columns[columnIndex].sizeBin);
                    
                    string tempString21 = "";
                    if (value.Length < columns[columnIndex].sizeBin)
                    {
                        for (int x = 0; x <= columns[columnIndex].sizeBin - value.Length - 1; x++)
                            tempString21 += " ";
                    }
                    value += tempString21;

                    buf = encoding.GetBytes(value);
                    /*if (codePage_ == 65001)
                    {
                        int length = Encoding.GetEncoding(codePage_).GetBytes(Value).Length;
                        if (length > columns[Column].sizeBin)
                            Array.Resize(ref buf, columns[Column].sizeBin);
                    }*/
                    break;
            }
            fileStreamDB?.WriteAsync(buf, 0, buf.Length);
            fileStreamDB?.FlushAsync();
            var res = ParseValue(columns[columnIndex].tip, buf);
            return res;
        }
        public string ParseValue(char tip, byte[] buff)
        {
            char separator = Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
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
        void ReadColumns()
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
            ReadColumns();

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