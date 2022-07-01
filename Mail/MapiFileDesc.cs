using System;
using System.Runtime.InteropServices;

namespace YACUF.Mail
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class MapiFileDesc
    {
        public int reserved;
        public int flags;
        public int position;
        public string? path;
        public string? name;
        public IntPtr type;
    }
}
