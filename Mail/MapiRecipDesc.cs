using System;
using System.Runtime.InteropServices;

namespace YACUF.Mail
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class MapiRecipDesc
    {
        public int reserved;
        public int recipClass;
        public string? name;
        public string? address;
        public int eIDSize;
        public IntPtr entryID;
    }
}
