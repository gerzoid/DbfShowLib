using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DbfShowLib.DBF
{
    public partial class Dbf : StandartBase, IDataBase
    {
        byte[] headerDBF;
        byte[] columnDBF;

        private Header header;
        private Column column;
        private List<Column> columns;

        public void ReadHeader()
        {
            long positionTemp = fileStreamDB.Position;
            fileStreamDB.Read(headerDBF, 0, 32);
            GCHandle pHandle = GCHandle.Alloc(headerDBF, GCHandleType.Pinned);
            header = (Header)Marshal.PtrToStructure(pHandle.AddrOfPinnedObject(), typeof(Header));
            pHandle.Free();
            //codePage_ = header.codePage;

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

        public void Close()
        {
            throw new NotImplementedException();
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
