#pragma once

#include "MainWindow.g.h"

namespace winrt::PhrasePlanter::implementation
{
    struct MainWindow : MainWindowT<MainWindow>
    {
        MainWindow();

        int32_t MyProperty();
        void MyProperty(int32_t value);

        void OnClickTranslateButton(Windows::Foundation::IInspectable const & sender, Microsoft::UI::Xaml::RoutedEventArgs const & args);

    private:
        fire_and_forget OnClickTranslateButtonAsync();

        winrt::hstring displayText;
    };
}

namespace winrt::PhrasePlanter::factory_implementation
{
    struct MainWindow : MainWindowT<MainWindow, implementation::MainWindow>
    {
    };
}
