using NoteIsMe.Domain.Models;
using NoteIsMe.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.NotebookViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotebookMainPage : Page
    {

        public NotebookViewModel NotebookViewModel { get; set; }


        public NotebookMainPage()
        {
            this.InitializeComponent();
            NotebookViewModel = new NotebookViewModel();
        }

        public static BitmapImage GetOwnerGravatarURI(string email)
        {
            return new BitmapImage(App.userViewModel.getUserGravatarURI(email));
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            int userid = App.userViewModel.CurrentUser.Id;

            await NotebookViewModel.LoadAllSharedAndOwnedNotebooksAsync(userid);
            base.OnNavigatedTo(e);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NotebookForm));
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog cd = new ContentDialog
            {
                Title = "Delete Dialog?",
                Content = "Are you sure you want to delete the category?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await cd.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (sender is FrameworkElement b && b.DataContext is Notebook notebook)
                {
                    await NotebookViewModel.DeleteAsync(notebook);
                }
            }

        }

        private void NotebookListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Notebook notebook)
            {
                              
              
             this.Frame.Navigate(typeof(NotebookViewPage), notebook);
               
            }
        }

        public static ImageSource LoadedImageAsync(byte[] url)
        {
            using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
            {
                // Writes the image byte array in an InMemoryRandomAccessStream
                // that is needed to set the source of BitmapImage.
                using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(url);

                    // The GetResults here forces to wait until the operation completes
                    // (i.e., it is executed synchronously), so this call can block the UI.
                    writer.StoreAsync().GetResults();
                }

                var image = new BitmapImage();
                image.SetSource(ms);
                ms.Dispose();
                return image;
            }



        }

        public static string actionsAllowed(int notebookID, int ownerID)
        {
            string text = "";
            Group group = App.UnitOfWork.GroupRepository.getUserNotebookGroup(notebookID, App.userViewModel.GetCurrentUserID());

            if (ownerID == App.userViewModel.GetCurrentUserID())
            {
                text = "\uE890      \uE70F      \uE74D      \uE8EB";
            }
            else
            {
                if (group.ViewPermission)
                {
                    text += "\uE890";
                    text += "      ";
                }
                if (group.EditPermission)
                {
                    text += "\uE70F";
                    text += "      ";
                }
                if (group.SharePermission)
                {
                    text += "\uE8EB";
                    text += "      ";
                }
            }

            return text;
        }


    }
        
}
