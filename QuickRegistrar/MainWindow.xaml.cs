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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PhrasePlanter.QuickRegistrar
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public readonly IntPtr hwnd;
        private readonly AppWindow appWindow;

        private const int width = 640;
        private const int height = 640;

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
        }

        public void ToggleVisibility()
        {
            if (appWindow.IsVisible)
            {
                appWindow.Hide();
                return;
            }

            // size
            appWindow.Resize(new SizeInt32(width, height));
            // position
            var workArea = SystemParameters.GetWorkArea();
            Debug.WriteLine($"left:{workArea.left} Top:{workArea.top} right:{workArea.right} bottom:{workArea.bottom}");
            appWindow.Move(new PointInt32(workArea.right - width, workArea.bottom - height));

            appWindow.Show();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
            
        }
    }
}
