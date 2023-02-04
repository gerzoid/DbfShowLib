using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DbfShowLib.DBF
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct Header
    {
        public byte byte1;
        public byte yy, mm, dd;    //Дата последнего обновления ГГ ММ ДД
        public int recordsCount;   //Кол-во записей в таблице
        public short headerSize;
        public short recordSize;
        private byte reserved1_0;
        private byte reserved1_1;
        public byte openTransaction;    //14
        public byte crypt;
        private int reserved;  //(4 байта)
        private int reserved2;  //(4 байта)
        private int reserved3;  //(4 байта)
        public byte haveIndex;
        public byte codePage;
        public short reserved5;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct Header_DBASEIII
    {
        public byte byte1;
        public byte yy, mm, dd;    //Дата последнего обновления ГГ ММ ДД
        public int recordsCount;   //Кол-во записей в таблице
        public short headerSize;
        public short recordSize;
        private int reserved;  //(4 байта)
        private int reserved2;  //(4 байта)
        private int reserved3;  //(4 байта)
        private int reserved4;  //(4 байта)
        private int reserved5;  //(4 байта)
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct Column
    {
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 11)]
        public char[] name;     // 0- 10
        public char tip;        //11
        public int displacement;   //12 -15
        public byte sizeBin;    //16   Length of field (in bytes)
        public byte zpt;        //17  Number of decimal places
        public byte flags; //18 Field flags: 0x01   System Column (not visible to user) 0x02   Column can store null values 0x04   Binary column (for CHAR and MEMO only) 0x06   (0x02+0x04) When a field is NULL and binary (Integer, Currency, and Character/Memo fields) 0x0C   Column is autoincrementing
        public int nextValue;//19-22   Value of autoincrement Next value
        public byte step;         //23
                                  //[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 6)]        
                                  //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public int reserved3; //6 байт - 24-27
                              //public int reserved4; //2 байт - 29
                              //public byte mdx;        //1  - 30
        public int pos; //позиция //4    -34    
    }

}
