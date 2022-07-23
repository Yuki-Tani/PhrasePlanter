#include "pch.h"
#include "MainWindow.xaml.h"
#if __has_include("MainWindow.g.cpp")
#include "MainWindow.g.cpp"
#endif

#include <regex>;

using namespace winrt;
using namespace Microsoft::UI::Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace winrt::PhrasePlanter::implementation
{
    MainWindow::MainWindow()
    {
        InitializeComponent();
        // Set title bar
        this->ExtendsContentIntoTitleBar(true);
        this->SetTitleBar(AppTitleBar());
    }

    int32_t MainWindow::MyProperty()
    {
        throw hresult_not_implemented();
    }

    void MainWindow::MyProperty(int32_t /* value */)
    {
        throw hresult_not_implemented();
    }

    void MainWindow::OnClickTranslateButton(IInspectable const &, RoutedEventArgs const &)
    {
        OnClickTranslateButtonAsync();
    }

    fire_and_forget MainWindow::OnClickTranslateButtonAsync()
    {
        winrt::apartment_context ui_thread;
        co_await resume_background();

        Windows::Web::Http::HttpClient httpClient;
        auto headers{ httpClient.DefaultRequestHeaders() };
        Windows::Foundation::Uri uri{ L"https://eikaiwa.dmm.com/uknow/search/?keyword=%E3%83%86%E3%82%B9%E3%83%88" };

        Windows::Web::Http::HttpResponseMessage res;
        winrt::hstring resBody;
        bool isSuccess;

        try
        {
            res = httpClient.GetAsync(uri).get();
            res.EnsureSuccessStatusCode();
            resBody = res.Content().ReadAsStringAsync().get();
            isSuccess = true;
        }
        catch (winrt::hresult_error const& ex)
        {
            resBody = ex.message();
            isSuccess = false;
        }

        if (!isSuccess)
        {
            co_await ui_thread;
            TranslationTextBlock().Text(L"Error!\n" + resBody);
            co_return;
        }

        // auto bodyStr = to_string(resBody);

        std::wregex rgx{ LR"(<h2>\s*<a href="/uknow/questions/(\d+)/">\s*(.*)って英語でなんて言うの？\s*</a>\s*</h2>)" };
        std::wstring wbody { resBody };

        for (
            std::wsregex_iterator itr{ wbody.cbegin(), wbody.cend(), rgx}, end;
            itr != end;
            ++itr
        )
        {
            displayText = displayText + (*itr).format(L"$1 : $2") + L"\n";
        }

        co_await ui_thread;
        TranslationTextBlock().Text(displayText);
    }
}
