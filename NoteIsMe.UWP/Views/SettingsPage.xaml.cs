using Microsoft.Toolkit.Uwp.Notifications;
using NoteIsMe.UWP.Views.HomeViews;
using System.Collections.Generic;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public Dictionary<string, string> languageList { get; } = new Dictionary<string, string>()
            {
            {"en-Us", "English" },
            {"pt-PT", "Portuguese" },
            {"it-IT", "Italian" },
            {"af-ZA", "afrikaans" }
            };

        public List<string> themeList { get; } = new List<string>()
            {"System Default", "Light", "Dark"};

        public SettingsPage()
        {
            this.InitializeComponent();
        }


        private void themeDropdown_Loaded(object sender, RoutedEventArgs e)
        {
            object themeVal = ApplicationData.Current.LocalSettings.Values["themeSetting"];
            if (themeVal != null)
            {
                if ((int) themeVal == 0)
                {
                    themeDropdown.SelectedIndex = 1;
                }
                else if ((int)themeVal == 1)
                {
                    themeDropdown.SelectedIndex = 2;
                }
                else
                {
                    themeDropdown.SelectedIndex = 0;
                }
            }
            else
            {
                themeDropdown.SelectedIndex = 0;
            }
        }

        private void themeDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedTheme = themeDropdown.SelectedValue as string;
            if (selectedTheme == "Light")
            {
                Frame.CacheSize = 0;
                ((Frame)Window.Current.Content).RequestedTheme = ElementTheme.Light;
                ApplicationData.Current.LocalSettings.Values["themeSetting"] = 0;
            }
            else if (selectedTheme == "Dark")
            {
                Frame.CacheSize = 0;
                ((Frame)Window.Current.Content).RequestedTheme = ElementTheme.Dark;
                ApplicationData.Current.LocalSettings.Values["themeSetting"] = 1;
            }else
            {
                Frame.CacheSize = 0;
                ((Frame)Window.Current.Content).RequestedTheme = ElementTheme.Default;
                ApplicationData.Current.LocalSettings.Values["themeSetting"] = null;
            }
            
        }

        private void languageDropdown_Loaded(object sender, RoutedEventArgs e)
        {
            object langVal = ApplicationData.Current.LocalSettings.Values["language"];
            if (langVal != null)
            {
                if ((string)langVal == "en-US")
                {
                    languageDropdown.SelectedIndex = 0;
                }
                else if ((string)langVal == "pt-PT")
                {
                    languageDropdown.SelectedIndex = 1;
                }
                else if ((string)langVal == "it-IT")
                {
                    languageDropdown.SelectedIndex = 2;
                }
                else if ((string)langVal == "af-ZA")
                {
                    languageDropdown.SelectedIndex = 3;
                }
                else
                {
                    languageDropdown.SelectedIndex = 0;
                }
            }
            else
            {
                languageDropdown.SelectedIndex = 0;
            }
        }

        private void languageDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedLang = languageDropdown.SelectedValue as string;
            if (selectedLang == "en-US")
            {
                ApplicationData.Current.LocalSettings.Values["language"] = selectedLang;
                ApplicationLanguages.PrimaryLanguageOverride = selectedLang;

            }
            else if (selectedLang == "pt-PT")
            {
                ApplicationData.Current.LocalSettings.Values["language"] = selectedLang;

                ApplicationLanguages.PrimaryLanguageOverride = selectedLang;


            }
            else if (selectedLang == "af-ZA")
            {
                ApplicationData.Current.LocalSettings.Values["language"] = selectedLang;
                ApplicationLanguages.PrimaryLanguageOverride = selectedLang;


            }
            else if (selectedLang == "it-IT")
            {
                ApplicationData.Current.LocalSettings.Values["language"] = selectedLang;
                ApplicationLanguages.PrimaryLanguageOverride = selectedLang;


            }
            else
            {
                ApplicationData.Current.LocalSettings.Values["language"] = "en-US";
                ApplicationLanguages.PrimaryLanguageOverride = "en-US";


            }
        }

        private void reloadPage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));

            var content = new ToastContentBuilder()
                    .AddText("Please restart the app for full language change in UI.")
                    .GetToastContent();

            var toaster = new ToastNotification(content.GetXml());

            ToastNotificationManager.CreateToastNotifier().Show(toaster);

        }
    }
}
