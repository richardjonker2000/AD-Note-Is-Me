using NoteIsMe.Domain.Models;
using NoteIsMe.UWP.ViewModels;
using System;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.NoteViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NoteMainPage : Page
    {
        public NoteViewModel NoteViewModel { get; set; }
        public NotebookViewModel NotebookViewModel { get; set; }

        public NoteMainPage()
        {
            this.InitializeComponent();
            NoteViewModel = new NoteViewModel();
            NotebookViewModel = new NotebookViewModel();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            int userid = App.userViewModel.GetCurrentUserID();

            if (e.Parameter != null)
            {
                NoteViewModel = e.Parameter as NoteViewModel;

                NotesList_ItemClick_Simulated(this, NoteViewModel.Note);
            }

            await NoteViewModel.LoadAllofUser(userid);
            await NotebookViewModel.LoadAllSharedAndOwnedNotebooksAsync(userid);

            base.OnNavigatedTo(e);
        }


        private void showSplitPane_Click(object sender, RoutedEventArgs e)
        {
            if (noteSplitView.IsPaneOpen == true)
            {
                noteSplitView.IsPaneOpen = false;
                showSplitPaneButtonText.Text = "Show Pane";
            }
            else
            {
                noteSplitView.IsPaneOpen = true;
                showSplitPaneButtonText.Text = "Hide Pane";
            }
        }


        private void addNewNoteButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NoteForm));
        }


        private async void ownNoteDelete_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ContentDialog cd = new ContentDialog
            {
                Title = "Delete Note?",
                Content = "Are you sure you want to delete the note?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await cd.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await NoteViewModel.DeleteAsync(NoteViewModel.Note);
                this.Frame.Navigate(typeof(NoteMainPage));
            }
        }


        private async void ownNoteEdit_Tapped(object sender, TappedRoutedEventArgs e)
        {

            await App.UnitOfWork.NoteRepository.UpdateAsync(NoteViewModel.Note);

            this.Frame.Navigate(typeof(NoteForm), NoteViewModel.Note);


        }


        private async void NotesList_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (e.ClickedItem is Note note)
            {
                NoteViewModel.Note = note;
                NoteViewModel.Note.Notebook = await NotebookViewModel.FindbyIDAsync(NoteViewModel.Note.NotebookId);
                noneSelectedText.Visibility = Visibility.Collapsed;
                notebookTextBlock.Text = NoteViewModel.Note.Notebook.Title;

                titleTextBlock.Text = note.Title;


                using (var memory = new InMemoryRandomAccessStream())
                {
                    //load the content
                    var dataWriter = new DataWriter(memory);

                    dataWriter.WriteBytes(note.Content);

                    await dataWriter.StoreAsync();

                    bodyTextBlock.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, memory);
                }

                bodyTextBlock.IsReadOnly = true;

                titleTextBlock.Visibility = Visibility.Visible;
                contentTitleTextBlock.Visibility = Visibility.Visible;
                bodyTextBlock.Visibility = Visibility.Visible;

                notebookTitleTextBlock.Visibility = Visibility.Visible;
                notebookTextBlock.Visibility = Visibility.Visible;

                ownNoteDelete.Visibility = App.bool2visibility(App.userViewModel.GetCurrentUserID() == note.OwnerId);
                ownNotebookChange.Visibility = App.bool2visibility(App.userViewModel.GetCurrentUserID() == note.OwnerId);

                ownNoteEdit.Visibility = App.bool2visibility(await NoteViewModel.isEditPermitted(App.userViewModel.CurrentUser, NoteViewModel.Note));

            }
        }



        private async void NotesList_ItemClick_Simulated(object sender, Note note)
        {
            NotesList.SelectedItem = note;

            NoteViewModel.Note = note;
            NoteViewModel.Note.Notebook = await NotebookViewModel.FindbyIDAsync(NoteViewModel.Note.NotebookId);
            noneSelectedText.Visibility = Visibility.Collapsed;
            notebookTextBlock.Text = NoteViewModel.Note.Notebook.Title;

            titleTextBlock.Text = note.Title;


            using (var memory = new InMemoryRandomAccessStream())
            {
                //load the content
                var dataWriter = new DataWriter(memory);

                dataWriter.WriteBytes(note.Content);

                await dataWriter.StoreAsync();

                bodyTextBlock.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, memory);
            }

            bodyTextBlock.IsReadOnly = true;

            titleTextBlock.Visibility = Visibility.Visible;
            contentTitleTextBlock.Visibility = Visibility.Visible;
            bodyTextBlock.Visibility = Visibility.Visible;

            notebookTitleTextBlock.Visibility = Visibility.Visible;
            notebookTextBlock.Visibility = Visibility.Visible;

            ownNoteDelete.Visibility = App.bool2visibility(App.userViewModel.GetCurrentUserID() == note.OwnerId);
            ownNotebookChange.Visibility = App.bool2visibility(App.userViewModel.GetCurrentUserID() == note.OwnerId);

            ownNoteEdit.Visibility = App.bool2visibility(await NoteViewModel.isEditPermitted(App.userViewModel.CurrentUser, NoteViewModel.Note));

        }

        private async void ownNotebookChange_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ChangeNotebookDialog changeNotebookDialog = new ChangeNotebookDialog(NotebookViewModel, NoteViewModel);
            await changeNotebookDialog.ShowAsync();

            this.Frame.Navigate(typeof(NoteMainPage));
        }
    }
}
