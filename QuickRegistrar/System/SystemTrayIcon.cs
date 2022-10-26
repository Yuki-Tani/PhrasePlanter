using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices; // DllImport

namespace PhrasePlanter.QuickRegistrar
{
    // TODO: fix
    class SystemTrayIcon
    {
        // https://learn.microsoft.com/ja-jp/windows/win32/api/shellapi/ns-shellapi-notifyicondataw
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct NOTIFYICONDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uID;
            public int uFlags;
            public int uCallbackMessage;
            public IntPtr hIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x80)]
            public string szTip;
            public int dwState;
            public int dwStateMask;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)]
            public string szInfo;
            public int uTimeoutOrVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x40)]
            public string szInfoTitle;
            public int dwInfoFlags;
            public IntPtr guidItem;
            public IntPtr hBalloonIcon;
        }

        // https://learn.microsoft.com/ja-jp/windows/win32/api/shellapi/nf-shellapi-shell_notifyiconw
        [DllImport("shell32.dll")]
        private extern static bool Shell_NotifyIcon(UInt32 dwMessage, IntPtr lpData);

        // NOTIFYICONDATA uFlags
        private const int NIF_MESSAGE = 0x00000001;
        private const int NIF_ICON = 0x00000002;
        private const int NIF_TIP = 0x00000004;
        private const int NIF_STAGE = 0x00000008;
        private const int NIF_INFO = 0x00000010;
        private const int NIF_GUID = 0x00000020;
        private const int NIF_REALTIME = 0x00000040;
        private const int NIF_SHOWTIP = 0x00000080;

        private const UInt32 NIM_ADD = 0x00000000;

        private NOTIFYICONDATA notifyIcon;

        public SystemTrayIcon(IntPtr hwnd)
        {
            
            notifyIcon = new NOTIFYICONDATA()
            {
                cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA)),
                hWnd = hwnd,
                uID = 0,
                uFlags = NIF_MESSAGE | NIF_TIP,
                uCallbackMessage = 0,
                hIcon = IntPtr.Zero,
                szTip = "PhrasePlanter Quick Registrar",
                dwState = 0,
                dwStateMask = 0,
                szInfo = "",
                uTimeoutOrVersion = 0,
                szInfoTitle = "",
                dwInfoFlags = 0,
                guidItem = IntPtr.Zero,
                hBalloonIcon = IntPtr.Zero
            };

            IntPtr pNotifyIcon = IntPtr.Zero;
            Marshal.StructureToPtr(notifyIcon, pNotifyIcon, true);
            Shell_NotifyIcon(NIM_ADD, pNotifyIcon);
        }
    }
}
