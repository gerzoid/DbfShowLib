using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DbfShowLib.MEM
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MEMRecord
    {
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 11)]
        public char[] name;     // 0- 10
        public char tip;        //11
        public int reserved;   //12 -15
        public byte sizeBin;    //16   Length of field (in bytes)
        public byte zpt;        //17  Number of decimal places
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 14)]
        public char[] reserved2;     // 0- 10
        public byte plus;
    }
    public struct MEMColumn
    {
        public string name;
        public char tip;
        public byte sizeBin;    //16   Length of field (in bytes)
        public byte zpt;        //17  Number of decimal places
        public long pos;
        public string value;
        public List<MEMColumn> array;
    }
}
