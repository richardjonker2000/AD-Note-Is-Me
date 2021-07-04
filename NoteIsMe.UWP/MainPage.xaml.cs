using NoteIsMe.Domain.Models;
using NoteIsMe.UWP.ViewModels;
using NoteIsMe.UWP.Views;
using NoteIsMe.UWP.Views.AdminViews;
using NoteIsMe.UWP.Views.FolderViews;
using NoteIsMe.UWP.Views.HomeViews;
using NoteIsMe.UWP.Views.LogInOutViews;
using NoteIsMe.UWP.Views.NotebookViews;
using NoteIsMe.UWP.Views.NoteViews;
using NoteIsMe.UWP.Views.ProfileViews;
using NoteIsMe.UWP.Views.SketchViews;
using NoteIsMe.UWP.Views.TagViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409


namespace NoteIsMe.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static bool manageroradmin { get; set; }

        private void Manage_LogInOut_Buttons()
        {
            if (App.userViewModel.IsAuthenticated())
            {
                login.Visibility = Visibility.Collapsed;
                logout.Visibility = Visibility.Visible;

                ProfileMenuItem.Visibility = Visibility.Visible;
            }
            else
            {
                login.Visibility = Visibility.Visible;
                logout.Visibility = Visibility.Collapsed;

                ProfileMenuItem.Visibility = Visibility.Collapsed;
                AdminMenuItem.Visibility = Visibility.Collapsed;
            }
        }

        private void manage_admin_button()
        {
            if (App.userViewModel.IsAuthenticated())
            {

                int role = App.userViewModel.CurrentUser.Role;

                if (role > 1)
                {
                    AdminMenuItem.Visibility = Visibility.Visible;
                }
                else
                {
                    AdminMenuItem.Visibility = Visibility.Collapsed;
                }

                

            }

        }


        public MainPage()
        {
            
            this.InitializeComponent();
            
            Loaded += async (sender, args) =>
            {
                
                if (App.userViewModel.IsAuthenticated())
                {
                    int usrID = (int)ApplicationData.Current.LocalSettings.Values["currentUserID"];
                    if (usrID != 0)
                    {

                        Task<User> cur = App.UnitOfWork.UserRepository.FindByIdAsync(usrID);
                        App.userViewModel.CurrentUser = await cur;
                    }


                    

                }
                Manage_LogInOut_Buttons();
                if (App.userViewModel.IsAuthenticated())
                {
                    
                    ContentFrame.Navigate(typeof(HomeMainPage));
                    MainMenu.SelectedItem = HomeMenuItem;
                }
                else
                {
                    ContentFrame.Navigate(typeof(LoginPage));
                }

            };

            manage_admin_button();

        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }



        private void MainMenu_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {

            Manage_LogInOut_Buttons();
            manage_admin_button();
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                if (App.userViewModel.IsAuthenticated())
                {
                    var item = args.InvokedItemContainer as Microsoft.UI.Xaml.Controls.NavigationViewItem;

                    switch (item.Tag)
                    {
                        case "home":
                            ContentFrame.Navigate(typeof(HomeMainPage));
                            break;

                        case "newnote":
                            ContentFrame.Navigate(typeof(NoteForm));
                            break;

                        case "note":
                            ContentFrame.Navigate(typeof(NoteMainPage));
                            break;

                        case "sketch":
                            ContentFrame.Navigate(typeof(SketchMainPage));
                            break;

                        case "notebook":
                            ContentFrame.Navigate(typeof(NotebookMainPage));
                            break;

                        case "tag":
                            ContentFrame.Navigate(typeof(TagMainPage));
                            break;

                        case "folder":
                            ContentFrame.Navigate(typeof(FolderMainPage));
                            break;

                        case "profile":
                            ContentFrame.Navigate(typeof(ProfileMainPage));
                            break;

                        case "admin":
                            ContentFrame.Navigate(typeof(AdminMainPage));
                            break;



                    }
                }
                else
                {
                    ContentFrame.Navigate(typeof(LoginPage));
                }


            }
        }



        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            Manage_LogInOut_Buttons();
            manage_admin_button();
            this.LoadAllSearchTexts();
            if (e.SourcePageType == typeof(HomeMainPage))
            {
                MainMenu.SelectedItem = HomeMenuItem;
            }
            else if (e.SourcePageType == typeof(NoteMainPage))
            {
                MainMenu.SelectedItem = NoteMenuItem;
            }

            else if (e.SourcePageType == typeof(NoteForm))
            {
                MainMenu.SelectedItem = NewNoteMenuItem;
            }
            else if (e.SourcePageType == typeof(SketchMainPage))
            {
                MainMenu.SelectedItem = SketchMenuItem;
            }
            else if (e.SourcePageType == typeof(NotebookMainPage))
            {
                MainMenu.SelectedItem = NotebookMenuItem;
            }
            else if (e.SourcePageType == typeof(TagMainPage))
            {
                MainMenu.SelectedItem = TagMenuItem;
            }

            else if (e.SourcePageType == typeof(FolderMainPage))
            {
                MainMenu.SelectedItem = FolderMenuItem;
            }

            else if (e.SourcePageType == typeof(ProfileMainPage))
            {
                MainMenu.SelectedItem = ProfileMenuItem;
            }
            else if (e.SourcePageType == typeof(AdminMainPage))
            {
                MainMenu.SelectedItem = AdminMenuItem;
            }

            else if (e.SourcePageType == typeof(LoginPage))
            {
                MainMenu.SelectedItem = login;
            }
            else if (e.SourcePageType == typeof(SettingsPage))
            {
                MainMenu.SelectedItem = MainMenu.SettingsItem;
            }


        }



        private void MainMenu_BackRequested(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs args)
        {
            manage_admin_button();
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }

        private void login_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(LoginPage));
        }

        private void logout_Tapped(object sender, TappedRoutedEventArgs e)
        {
            App.userViewModel.LogOut();

            ContentFrame.Navigate(typeof(HomeMainPage));
        }


        private List<string> searchTexts = new List<string>();
        private List<string> suitableItems = new List<string>();
        private List<int> searchIndexes = new List<int>();
        private List<int> suitableIndexes = new List<int>();

        private async void LoadAllSearchTexts()
        {
            searchTexts.Clear();
            searchIndexes.Clear();
            int userid = App.userViewModel.GetCurrentUserID();
            NotebookViewModel nvm = new NotebookViewModel();
            SketchViewModel svm = new SketchViewModel();
            NoteViewModel notevm = new NoteViewModel();
            await nvm.LoadAllSharedAndOwnedNotebooksAsync(userid);
            await svm.LoadAllofUserAsync(userid);
            await notevm.LoadAllofUser(userid);
            foreach (Notebook n in nvm.MyNotebooks)
            {
                searchTexts.Add(n.Title + " - Notebook");
                searchIndexes.Add(n.Id);
            }
            foreach (Notebook n in nvm.SharedNotebooks)
            {
                searchTexts.Add(n.Title + " - Notebook");
                searchIndexes.Add(n.Id);
            }
            foreach (Sketch s in svm.Sketches)
            {
                searchTexts.Add(s.Title + " - Sketch");
                searchIndexes.Add(s.Id);
            }
            foreach (Note n in notevm.Notes)
            {
                searchTexts.Add(n.Title + " - Note");
                searchIndexes.Add(n.Id);
            }

        }
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            //var suitableItems = new List<string>();
            //var suitableIndexes = new List<int>();
            suitableItems = new List<string>();
            suitableIndexes.Clear();
            var splitText = sender.Text.ToLower().Split(" ");
            foreach (var text in searchTexts)
            {
                var found = splitText.All((key) =>
                {
                    return text.ToLower().Contains(key);
                });


                if (found)
                {
                    suitableItems.Add(text);
                    suitableIndexes.Add(searchIndexes[searchTexts.IndexOf(text)]);
                }
            }
            if (suitableItems.Count == 0)
            {
                suitableItems.Add("No results found");
            }
            sender.ItemsSource = suitableItems;
        }

        // Handle user selecting an item, in our case just output the selected item.
        private async void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            string output = args.SelectedItem.ToString();
            if (output.Contains(" - Notebook"))
            {
                NotebookViewModel nvm = new NotebookViewModel();
                int index = suitableItems.IndexOf(output);
                int id = (suitableIndexes[index]);
                //await nvm.FindbyIDAsync(id);
                ContentFrame.Navigate(typeof(NotebookViewPage), await nvm.FindbyIDAsync(id));
            }
            else if (output.Contains(" - Sketch"))
            {
                SketchViewModel svm = new SketchViewModel();
                int index = suitableItems.IndexOf(output);
                int id = (suitableIndexes[index]);
                Sketch sketch = await svm.FindbyIDAsync(id);

                svm.Sketch = sketch;
                await App.UnitOfWork.SketchRepository.UpdateAsync(sketch);
                ContentFrame.Navigate(typeof(SketchDrawPage), svm);

            }
            else if (output.Contains(" - Note"))
            {
                NoteViewModel nvm = new NoteViewModel();
                int index = suitableItems.IndexOf(output);
                int id = (suitableIndexes[index]);

                nvm.Note = await App.UnitOfWork.NoteRepository.FindByIdAsync(id);

                ContentFrame.Navigate(typeof(NoteMainPage), nvm);
            }



        }
    }
}
