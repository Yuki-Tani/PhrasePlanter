using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices; // DllImport

namespace PhrasePlanter.QuickRegistrar
{
    class SystemParameters
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public Int32 left;
            public Int32 top;
            public Int32 right;
            public Int32 bottom;
        }

        [DllImport("User32.dll")]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

        private const uint SPI_GETWORKAREA = 0x0030;

        // primary window size without taskbar
        public static RECT GetWorkArea()
        {
            RECT rectProto = new RECT();
            IntPtr pRect = Marshal.AllocHGlobal(Marshal.SizeOf(rectProto));
            RECT workAreaRect;
            try
            {
                // https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.interopservices.marshal.structuretoptr?view=net-6.0
                Marshal.StructureToPtr(rectProto, pRect, true);
                SystemParametersInfo(SPI_GETWORKAREA, 0, pRect, 0);
                workAreaRect = (RECT)Marshal.PtrToStructure(pRect, typeof(RECT));
            }
            finally
            {
                Marshal.FreeHGlobal(pRect);
            }

            return workAreaRect;
        }
    }
}
