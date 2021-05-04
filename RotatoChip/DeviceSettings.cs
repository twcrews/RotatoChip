using System;
using System.Runtime.InteropServices;
using static Crews.Utility.RotatoChip.DisplayAPI;

namespace Crews.Utility.RotatoChip
{
    // See: https://msdn.microsoft.com/en-us/library/windows/desktop/dd183565(v=vs.85).aspx
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    internal struct DeviceSettings
    {
        internal bool IsPerpendicularTo(Orientation orientation) => (DisplayOrientation + (int)orientation) % 2 == 1;

        internal void SetOrientationTo(Orientation orientation)
        {
            DisplayOrientation = orientation switch
            {
                Orientation.Portrait => NativeMethods.DMDO_270,
                Orientation.UpsideDown => NativeMethods.DMDO_180,
                Orientation.PortraitInverted => NativeMethods.DMDO_90,
                Orientation.Landscape => NativeMethods.DMDO_DEFAULT,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(orientation), orientation, "Unsupported orientation."),
            };
        }

        internal const int CCHDEVICENAME = 32;
        internal const int CCHFORMNAME = 32;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
        [FieldOffset(0)]
        internal string DeviceName;
        [FieldOffset(32)]
        internal short SpecVersion;
        [FieldOffset(34)]
        internal short DriverVersion;
        [FieldOffset(36)]
        internal short Size;
        [FieldOffset(38)]
        internal short DriverExtra;
        [FieldOffset(40)]
        internal DM Fields;

        [FieldOffset(44)]
        private readonly short DeviceOrientation;
        [FieldOffset(46)]
        private readonly short PaperSize;
        [FieldOffset(48)]
        private readonly short PaperLength;
        [FieldOffset(50)]
        private readonly short PaperWidth;
        [FieldOffset(52)]
        private readonly short Scale;
        [FieldOffset(54)]
        private readonly short Copies;
        [FieldOffset(56)]
        private readonly short DefaultSource;
        [FieldOffset(58)]
        private readonly short PrintQuality;

        [FieldOffset(44)]
        internal POINTL Position;
        [FieldOffset(52)]
        internal int DisplayOrientation;
        [FieldOffset(56)]
        internal int DisplayFixedOutput;

        [FieldOffset(60)]
        internal short Color;
        [FieldOffset(62)]
        internal short Duplex;
        [FieldOffset(64)]
        internal short YResolution;
        [FieldOffset(66)]
        internal short TTOption;
        [FieldOffset(68)]
        internal short Collate;
        [FieldOffset(72)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
        internal string FormName;
        [FieldOffset(102)]
        internal short LogPixels;
        [FieldOffset(104)]
        internal int BitsPerPel;
        [FieldOffset(108)]
        internal int PelsWidth;
        [FieldOffset(112)]
        internal int PelsHeight;
        [FieldOffset(116)]
        internal int DisplayFlags;
        [FieldOffset(116)]
        internal int Nup;
        [FieldOffset(120)]
        internal int DisplayFrequency;

        internal void SwapDimensions()
        {
            int temp = PelsHeight;
            PelsHeight = PelsWidth;
            PelsWidth = temp;
        }
    }
}
