using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

using Microsoft.UI.Windowing;
using System.Diagnostics; // Debug
using Windows.Graphics; // SizeInt32, PointInt32

namespace PhrasePlanter.QuickRegistrar
{
    public sealed partial class MainWindow : Window
    {
        private const int width = 640; // NOTE: different from XAML unit
        private const int height = 640; // NOTE: different from XAML unit
        //private static readonly AcrylicBackground.Style acrylicStyle = new AcrylicBackground.Style()
        //{
        //    tintOpacity = 0.6f,
        //    tintLuminosityOpacity = 0.6f,
        //    tintColor = new Windows.UI.Color() { A = 255, R = 0, G = 0, B = 0 },
        //    fallbackColor = new Windows.UI.Color() { A = 255, R = 10, G = 10, B = 30 }
        //};

    public readonly IntPtr hwnd;
        private readonly AppWindow appWindow;
        // private AcrylicBackground acrylicBackground;

        public MainWindow()
        {
            this.InitializeComponent();
            hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            appWindow = AppWindow.GetFromWindowId(windowId);

            appWindow.Title = "PhrasePlanter Quick Registrar";

            appWindow.SetPresenter(AppWindowPresenterKind.CompactOverlay);
            
            // remove title bar
            var titlebar = appWindow.TitleBar;
            titlebar.ExtendsContentIntoTitleBar = true;

            // acrylicBackground = new AcrylicBackground(this, appWindow.TitleBar, acrylicStyle);
        }

        public void ToggleVisibility()
        {
            if (appWindow.IsVisible)
            {
                appWindow.Hide();
                // acrylicBackground.Dispose();
                return;
            }

            // size
            appWindow.Resize(new SizeInt32(width, height));
            // position
            var workArea = SystemParameters.GetWorkArea();
            Debug.WriteLine($"left:{workArea.left} Top:{workArea.top} right:{workArea.right} bottom:{workArea.bottom}");
            appWindow.Move(new PointInt32(workArea.right - width, workArea.bottom - height));

            // acrylicBackground.Dispose();
            // acrylicBackground = new AcrylicBackground(this, appWindow.TitleBar, acrylicStyle);

            appWindow.Show();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs args)
        {
            var db = new PhrasePlanterDataBase();
            var answer = db.TestAccess();
            SaveButton.Content = answer;
        }
    }
}
