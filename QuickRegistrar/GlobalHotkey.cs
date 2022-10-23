using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices; // DllImport
using System.Diagnostics; // Debug
using Windows.System;

namespace PhrasePlanter.QuickRegistrar
{
    public class GlobalHotkey: IDisposable
    {
        // https://learn.microsoft.com/ja-jp/windows/win32/api/winuser/nf-winuser-registerhotkey
        [DllImport("user32.dll")]
        extern static int RegisterHotKey(IntPtr hwnd, int id, uint fsModifiers, uint vk);

        // https://learn.microsoft.com/ja-jp/windows/win32/api/winuser/nf-winuser-unregisterhotkey
        [DllImport("user32.dll")]
        extern static int UnregisterHotKey(IntPtr hwnd, int id);

        // https://learn.microsoft.com/ja-jp/windows/win32/api/commctrl/nf-commctrl-setwindowsubclass
        [DllImport("comctl32.dll")]
        extern static bool SetWindowSubclass(IntPtr hwnd, IntPtr pfnSubclass, UIntPtr uIdSubclass, UIntPtr dwRefData);

        // https://learn.microsoft.com/ja-jp/windows/win32/api/commctrl/nf-commctrl-removewindowsubclass
        [DllImport("comctl32.dll")]
        extern static bool RemoveWindowSubclass(IntPtr hwnd, IntPtr pfnSubclass, UIntPtr uIdSubclass);

        // https://learn.microsoft.com/ja-jp/windows/win32/api/commctrl/nf-commctrl-defsubclassproc
        [DllImport("comctl32.dll")]
        extern static int DefSubclassProc(IntPtr hwnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        private delegate int Delegate_SubclassWinProc(IntPtr hwnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, UIntPtr dwRefData);
        private const uint WM_HOTKEY = 0x0312;

        public static class ModifierKey
        {
            public const uint MOD_ALT = 0x0001;
            public const uint MOD_CONTROL = 0x0002;
            public const uint MOD_SHIFT = 0x0004;
            public const uint MOD_WIN = 0x0008;
        }

        private IntPtr hwnd;
        private IntPtr pfnSubWinProc;
        private Dictionary<int, Action> actionStore;

        public GlobalHotkey(IntPtr hwnd)
        {
            this.hwnd = hwnd;
            actionStore = new Dictionary<int, Action>();
        }

        ~GlobalHotkey()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (pfnSubWinProc != IntPtr.Zero)
            {
                RemoveWindowSubclass(hwnd, pfnSubWinProc, (UIntPtr)1);
                pfnSubWinProc = IntPtr.Zero;
            }
            foreach (int key in actionStore.Keys)
            {
                UnregisterHotKey(hwnd, key);
            }
        }

        public void Register(uint modifierKeys, VirtualKey vkey, Action action)
        {
            if (pfnSubWinProc == IntPtr.Zero)
            {
                Delegate_SubclassWinProc subWinProcDelegate = SubclassWinProc;
                pfnSubWinProc = Marshal.GetFunctionPointerForDelegate(subWinProcDelegate);
                SetWindowSubclass(hwnd, pfnSubWinProc, (UIntPtr)1, UIntPtr.Zero);
            }

            var id = actionStore.Count;
            actionStore.Add(id, action);
            RegisterHotKey(hwnd, id, modifierKeys, (uint)vkey);
        }

        // TODO: investigate randomly happened ExecutionEngineException
        private int SubclassWinProc(IntPtr hwnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, UIntPtr dwRefData)
        {
            Debug.WriteLine("message: " + uMsg);
            switch (uMsg)
            {
                case WM_HOTKEY:
                    Debug.WriteLine("hotkey detected: " + wParam);
                    var action = actionStore[(int)wParam]; // TODO: safe cast
                    action.Invoke();
                    return 0;
            }

            return DefSubclassProc(hwnd, uMsg, wParam, lParam);
        }
    }
}
