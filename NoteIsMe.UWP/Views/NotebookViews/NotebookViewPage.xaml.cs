using NoteIsMe.Domain.Models;
using NoteIsMe.UWP.ViewModels;
using NoteIsMe.UWP.Views.NoteViews;
using NoteIsMe.UWP.Views.SketchViews;
using System;
using System.Collections.Generic;
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
    /// 

    public sealed partial class NotebookViewPage : Page
    {

        public NotebookViewModel NotebookViewModel { get; set; }
        public SketchViewModel SketchViewModel { get; set; }
        public NoteViewModel NoteViewModel { get; set; }

        public NotebookViewPage()
        {
            this.InitializeComponent();
            NotebookViewModel = new NotebookViewModel();
            SketchViewModel = new SketchViewModel();
            NoteViewModel = new NoteViewModel();

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                NotebookViewModel.Notebook = e.Parameter as Notebook;
            }


            if (NotebookViewModel.Notebook.OwnerId == App.userViewModel.GetCurrentUserID())
            {
                showNotebookSharedButton.Visibility = Visibility.Visible;
                ShareNotebookButton.Visibility = Visibility.Visible;
                editNotebookButton.Visibility = Visibility.Visible;
                deleteNotebookButton.Visibility = Visibility.Visible;
            }
            else
            {
                showNotebookSharedButton.Visibility = App.bool2visibility(await NotebookViewModel.isSharePermitted(App.userViewModel.CurrentUser, NotebookViewModel.Notebook));
                ShareNotebookButton.Visibility = App.bool2visibility(await NotebookViewModel.isSharePermitted(App.userViewModel.CurrentUser, NotebookViewModel.Notebook));
                editNotebookButton.Visibility = Visibility.Collapsed;
                deleteNotebookButton.Visibility = Visibility.Collapsed;
            }

            await SketchViewModel.LoadNotebookAsync(NotebookViewModel.Notebook.Id);
            await NoteViewModel.LoadNotebookAsync(NotebookViewModel.Notebook.Id);
            base.OnNavigatedTo(e);
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

        private async void ShareNotebookButton_Click(object sender, RoutedEventArgs e)
        {
            ShareNotebookDialog shareNotebookDialog = new ShareNotebookDialog(NotebookViewModel.Notebook);
            await shareNotebookDialog.ShowAsync();

            this.Frame.Navigate(typeof(NotebookMainPage));
        }

        private void editNotebookButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NotebookForm), NotebookViewModel);
        }

        private async void deleteNotebookButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog cd = new ContentDialog
            {
                Title = "Delete Notebook?",
                Content = "Are you sure you want to delete the notebook?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await cd.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await NotebookViewModel.DeleteAsync(NotebookViewModel.Notebook);
                this.Frame.Navigate(typeof(NotebookMainPage));
            }
        }

        private void NotesGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Note note)
            {

                NoteViewModel.Note = note;
                this.Frame.Navigate(typeof(NoteMainPage), NoteViewModel);
            }
        }

        private async void SketchesGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Sketch sketch)
            {

                SketchViewModel.Sketch = sketch;
                await App.UnitOfWork.SketchRepository.UpdateAsync(sketch);
                this.Frame.Navigate(typeof(SketchDrawPage), SketchViewModel);

            }
        }

        private async void showNotebookSharedButton_Click(object sender, RoutedEventArgs e)
        {
            ShowNotebookGroupsDialog showNotebookGroupsDialog = new ShowNotebookGroupsDialog(NotebookViewModel.Notebook);
            await showNotebookGroupsDialog.ShowAsync();

            this.Frame.Navigate(typeof(NotebookMainPage));

        }
    }
}
