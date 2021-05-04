using System;
using System.Runtime.InteropServices;

namespace Crews.Utility.RotatoChip
{
    internal static class DisplayAPI
    {
        internal class NativeMethods
        {
#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments
            [DllImport("user32.dll", CharSet = CharSet.Ansi)]
            internal static extern DisplayChangeStatus ChangeDisplaySettingsEx(
                string lpszDeviceName, ref DeviceSettings lpDevMode, IntPtr hwnd,
                DisplaySettingsFlags dwflags, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Ansi)]
            internal static extern bool EnumDisplayDevices(
                string lpDevice, uint iDevNum, ref DisplayDevice lpDisplayDevice,
                uint dwFlags);

            [DllImport("user32.dll", CharSet = CharSet.Ansi)]
            internal static extern int EnumDisplaySettings(
                string lpszDeviceName, int iModeNum, ref DeviceSettings lpDevMode);
#pragma warning restore CA2101

            internal const int DMDO_DEFAULT = 0;
            internal const int DMDO_90 = 1;
            internal const int DMDO_180 = 2;
            internal const int DMDO_270 = 3;

            internal const int ENUM_CURRENT_SETTINGS = -1;

        }

        // See: https://msdn.microsoft.com/de-de/library/windows/desktop/dd162807(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        internal struct POINTL
        {
            internal readonly long X;
            internal readonly long Y;
        }

        internal enum DisplayChangeStatus : int
        {
            Successful = 0,
            Restart = 1,
            Failed = -1,
            BadMode = -2,
            NotUpdated = -3,
            BadFlags = -4,
            BadParam = -5,
            BadDualView = -6
        }

        // http://www.pinvoke.net/default.aspx/Enums/DisplayDeviceStateFlags.html
        [Flags()]
        internal enum DisplayDeviceStateFlags : int
        {
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            PrimaryDevice = 0x4,
            MirroringDriver = 0x8,
            VGACompatible = 0x10,
            Removable = 0x20,
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        // http://www.pinvoke.net/default.aspx/user32/ChangeDisplaySettingsFlags.html
        [Flags()]
        internal enum DisplaySettingsFlags : int
        {
            CDS_NONE = 0,
            CDS_UPDATEREGISTRY = 0x00000001,
            CDS_TEST = 0x00000002,
            CDS_FULLSCREEN = 0x00000004,
            CDS_GLOBAL = 0x00000008,
            CDS_SET_PRIMARY = 0x00000010,
            CDS_VIDEOPARAMETERS = 0x00000020,
            CDS_ENABLE_UNSAFE_MODES = 0x00000100,
            CDS_DISABLE_UNSAFE_MODES = 0x00000200,
            CDS_RESET = 0x40000000,
            CDS_RESET_EX = 0x20000000,
            CDS_NORESET = 0x10000000
        }

        [Flags()]
        internal enum DM : int
        {
            Orientation = 0x00000001,
            PaperSize = 0x00000002,
            PaperLength = 0x00000004,
            PaperWidth = 0x00000008,
            Scale = 0x00000010,
            Position = 0x00000020,
            NUP = 0x00000040,
            DisplayOrientation = 0x00000080,
            Copies = 0x00000100,
            DefaultSource = 0x00000200,
            PrintQuality = 0x00000400,
            Color = 0x00000800,
            Duplex = 0x00001000,
            YResolution = 0x00002000,
            TTOption = 0x00004000,
            Collate = 0x00008000,
            FormName = 0x00010000,
            LogPixels = 0x00020000,
            BitsPerPixel = 0x00040000,
            PelsWidth = 0x00080000,
            PelsHeight = 0x00100000,
            DisplayFlags = 0x00200000,
            DisplayFrequency = 0x00400000,
            ICMMethod = 0x00800000,
            ICMIntent = 0x01000000,
            MediaType = 0x02000000,
            DitherType = 0x04000000,
            PanningWidth = 0x08000000,
            PanningHeight = 0x10000000,
            DisplayFixedOutput = 0x20000000
        }
    }
}
