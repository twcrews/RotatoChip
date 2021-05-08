using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static Crews.Utility.RotatoChip.DisplayAPI;

namespace Crews.Utility.RotatoChip
{
    public class Display
    {
        public string Name => $"Display {DisplayIndex + 1}";
        public string DeviceName => Device.DeviceName;

        public void Rotate(Orientation orientation)
        {
            if (Settings.IsPerpendicularTo(orientation))
            {
                Settings.SwapDimensions();
            }

            Settings.SetOrientationTo(orientation);
            ChangeDisplaySettings(Device, Settings);
        }

        public static Display GetDisplay(uint displayIndex)
        {
            DisplayDevice device = new();
            device.cb = Marshal.SizeOf(device);

            if (!NativeMethods.EnumDisplayDevices(null, displayIndex, ref device, 0))
                throw new ArgumentOutOfRangeException(nameof(displayIndex),
                    displayIndex, "Not a valid display index.");

            return new()
            {
                DisplayIndex = displayIndex,
                Device = device, 
                Settings = GetSettings(device) 
            };
        }

        public static List<Display> GetAllDisplays()
        {
            List<Display> displays = new();
            try
            {
                for (uint i = 0; i <= 64; i++)
                {
                    displays.Add(GetDisplay(i));
                }
            }
            catch (InvalidOperationException) { }
            return displays;
        }

        private Display() { }

        private uint DisplayIndex;
        private DisplayDevice Device;
        private DeviceSettings Settings;

        private static DeviceSettings GetSettings(DisplayDevice device)
        {
            DeviceSettings deviceSettings = new();
            if (NativeMethods.EnumDisplaySettings(device.DeviceName,
                NativeMethods.ENUM_CURRENT_SETTINGS, ref deviceSettings) == 0)
            {
                throw new InvalidOperationException($"Failed to load settings for device: {device.DeviceName}");
            }
            return deviceSettings;
        }

        private static void ChangeDisplaySettings(DisplayDevice device, DeviceSettings settings)
        {
            DisplayChangeStatus result = NativeMethods.ChangeDisplaySettingsEx(device.DeviceName, ref settings, IntPtr.Zero,
                DisplaySettingsFlags.CDS_UPDATEREGISTRY, IntPtr.Zero);
            if (result != DisplayChangeStatus.Successful)
            {
                throw new InvalidOperationException($"Failed to rotate with error {result}");
            }
        }

    }
    
    public enum Orientation
    {
        Landscape = 0,
        Portrait = 1,
        UpsideDown = 2,
        PortraitInverted = 3
    }
}