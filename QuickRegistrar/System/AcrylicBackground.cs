using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;
using Microsoft.UI.Composition.SystemBackdrops; // Acrylic background
using WinRT; // Window.As
using System.Runtime.InteropServices; // DllImport
using Microsoft.UI.Windowing; // titlebar
using Windows.UI; // Color
using Microsoft.UI; // Color

namespace PhrasePlanter.QuickRegistrar
{

    // TODO: Support Light/Dark mode
    internal class AcrylicBackground: IDisposable
    {
        [StructLayout(LayoutKind.Sequential)]
        struct DispatcherQueueOptions
        {
            internal int dwSize;
            internal int threadType;
            internal int apartmentType;
        }

        [DllImport("CoreMessaging.dll")]
        private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);

        public struct Style
        {
            public float tintOpacity;
            public float tintLuminosityOpacity;
            public Color tintColor;
            public Color fallbackColor;
        }

        private readonly Window window;
        private readonly DesktopAcrylicController acrylicController;
        private readonly SystemBackdropConfiguration backdropConfig;
        private object dispatcherQueueController;

        public AcrylicBackground(Window window, AppWindowTitleBar titleBar, Style acrylicStyle)
        {
            this.window = window;
            if (!DesktopAcrylicController.IsSupported()) return;
            EnsureWindowsSystemDispatcherQueueController();

            backdropConfig = new SystemBackdropConfiguration();
            
            backdropConfig.IsInputActive = true;
            SetBackdropConfigTheme();

            acrylicController = new DesktopAcrylicController();

            acrylicController.TintColor = acrylicStyle.tintColor;
            acrylicController.TintOpacity = acrylicStyle.tintOpacity;
            acrylicController.LuminosityOpacity = acrylicStyle.tintLuminosityOpacity;
            acrylicController.FallbackColor = acrylicStyle.fallbackColor;

            acrylicController.AddSystemBackdropTarget(window.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            acrylicController.SetSystemBackdropConfiguration(backdropConfig);

            ((FrameworkElement)window.Content).ActualThemeChanged += Window_ThemeChanged;

            // make title bar transparent
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        public void Dispose()
        {
            acrylicController.Dispose();
            ((FrameworkElement)window.Content).ActualThemeChanged -= Window_ThemeChanged;
        }

        private void Window_ThemeChanged(FrameworkElement sender, object args)
        {
            SetBackdropConfigTheme();
        }

        private void EnsureWindowsSystemDispatcherQueueController()
        {
            if (Windows.System.DispatcherQueue.GetForCurrentThread() != null)
            {
                // one already exists, so we'll just use it.
                return;
            }

            if (dispatcherQueueController == null)
            {
                DispatcherQueueOptions options;
                options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
                options.threadType = 2;    // DQTYPE_THREAD_CURRENT
                options.apartmentType = 2; // DQTAT_COM_STA

                CreateDispatcherQueueController(options, ref dispatcherQueueController);
            }
        }

        private void SetBackdropConfigTheme()
        {
            switch (((FrameworkElement)window.Content).ActualTheme)
            {
                case ElementTheme.Dark: backdropConfig.Theme = SystemBackdropTheme.Dark; break;
                case ElementTheme.Light: backdropConfig.Theme = SystemBackdropTheme.Light; break;
                case ElementTheme.Default: backdropConfig.Theme = SystemBackdropTheme.Default; break;
            }
        }
    }
}
