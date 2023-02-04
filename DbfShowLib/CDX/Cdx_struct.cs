using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DbfShowLib.CDX
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CDXHeader
    {
        public int pointerRoot;
        public int pointerFree;
        public int reserved;
        public short lengthKey;
        public byte options;
        public byte signature;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ExteriorNodeRecord
    {
        public short atributNode;
        public short keyCount;
        public int pointerLeftNode;
        public int pointerRightNode;
        public short freeSpace;
        public int maskNumberRecord;
        public byte duplicateCountMask;
        public byte trailingCountMask;
        public byte countBitsInNumberRecord;
        public byte countBitsInDuplicateCount;
        public byte countBitsInTrailingCount;
        public byte countBytesNumberRecordsDuplicateTrailingCount;
    }

}

