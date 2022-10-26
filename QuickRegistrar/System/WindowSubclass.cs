using System;

using System.Runtime.InteropServices; // DllImport
using System.Diagnostics; // Debug

namespace PhrasePlanter.QuickRegistrar
{
    class WindowSubclass : IDisposable
    {
        public delegate int Delegate_SubclassWinProc(IntPtr hwnd, uint uMsg, IntPtr wParam, IntPtr lParam, UIntPtr uIdSubclass, UIntPtr dwRefData);

        // https://learn.microsoft.com/ja-jp/windows/win32/api/commctrl/nf-commctrl-defsubclassproc
        [DllImport("comctl32.dll")]
        public extern static int DefSubclassProc(IntPtr hwnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        // https://learn.microsoft.com/ja-jp/windows/win32/api/commctrl/nf-commctrl-setwindowsubclass
        [DllImport("comctl32.dll")]
        private extern static bool SetWindowSubclass(IntPtr hwnd, IntPtr pfnSubclass, UIntPtr uIdSubclass, UIntPtr dwRefData);

        // https://learn.microsoft.com/ja-jp/windows/win32/api/commctrl/nf-commctrl-removewindowsubclass
        [DllImport("comctl32.dll")]
        private extern static bool RemoveWindowSubclass(IntPtr hwnd, IntPtr pfnSubclass, nuint uIdSubclass);

        private static nuint serialId = 1;
        
        private nuint id;
        private IntPtr hwnd;
        private IntPtr pfnSubWinProc;

        public WindowSubclass(IntPtr hwnd, Delegate_SubclassWinProc subWinProc)
        {
            id = serialId++;
            this.hwnd = hwnd;

            pfnSubWinProc = Marshal.GetFunctionPointerForDelegate(subWinProc);
            Debug.WriteLine("set window subclass");
            SetWindowSubclass(hwnd, pfnSubWinProc, id, UIntPtr.Zero);
        }

        ~WindowSubclass()
        {
            Dispose();
        }

        public void Dispose()
        {
            Debug.WriteLine("remove window subclass");
            RemoveWindowSubclass(hwnd, pfnSubWinProc, id);
        }
    }
}
