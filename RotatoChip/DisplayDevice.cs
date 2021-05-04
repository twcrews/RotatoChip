using System.Runtime.InteropServices;
using static Crews.Utility.RotatoChip.DisplayAPI;

namespace Crews.Utility.RotatoChip
{
    // See: https://msdn.microsoft.com/en-us/library/windows/desktop/dd183569(v=vs.85).aspx
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct DisplayDevice
    {
        [MarshalAs(UnmanagedType.U4)]
        internal int cb;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        internal string DeviceName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        internal string DeviceString;
        [MarshalAs(UnmanagedType.U4)]
        internal DisplayDeviceStateFlags StateFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        internal string DeviceID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        internal string DeviceKey;
    }
}
