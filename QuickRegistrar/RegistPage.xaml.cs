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
    public sealed partial class RegistPage : Page
    {
        PhrasePlanterDataBase db;

        public RegistPage()
        {
            this.InitializeComponent();
            Debug.WriteLine("RegistPage created");
            db = new PhrasePlanterDataBase(
                Scecrets.TestAccountStatics.userId,
                Scecrets.TestAccountStatics.password
            );
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs args)
        {
            if (PhraseTextBox.Text.Trim() == "" || PhraseMeaningTextBox.Text.Trim() == "")
            {
                return;
            }

            var isSuccess = db.InsertPhrase(PhraseTextBox.Text, PhraseMeaningTextBox.Text);
            if (isSuccess)
            {
                PhraseTextBox.Text = "";
                PhraseMeaningTextBox.Text = "";
            }
        }

        private void OnAccountButtonClick(object sender, RoutedEventArgs args)
        {
            var values = db.WritePhrases();

            if (values.Count() < 2)
            {
                PhraseTextBox.Text = "Error";
                PhraseMeaningTextBox.Text = values[0];
                return;
            }

            PhraseTextBox.Text = values[0];
            PhraseMeaningTextBox.Text = values[1];
        }
    }
}
