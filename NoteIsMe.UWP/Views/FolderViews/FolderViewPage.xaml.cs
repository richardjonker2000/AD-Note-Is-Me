using NoteIsMe.Domain.Models;
using NoteIsMe.UWP.ViewModels;
using NoteIsMe.UWP.Views.NotebookViews;
using NoteIsMe.UWP.Views.SketchViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.FolderViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FolderViewPage : Page
    {

        public FolderNotebookViewModel folderNotebookViewModel { get; set; }
        public FolderViewModel folderViewModel{ get; set; }
        public NotebookViewModel notebookViewModel{ get; set; }
        public FolderViewPage()
        {
            this.InitializeComponent();
            folderNotebookViewModel = new FolderNotebookViewModel();
            folderViewModel = new FolderViewModel();
            notebookViewModel = new NotebookViewModel();

        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                folderViewModel = e.Parameter as FolderViewModel;
            }


            int userid = App.userViewModel.CurrentUser.Id;
            await notebookViewModel.LoadAllFoldersAsync(folderViewModel.Folder.Id);
            base.OnNavigatedTo(e);
        }


        private void CreateSubfolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void AddNotebook_Click(object sender, RoutedEventArgs e)
        {
            int userid = App.userViewModel.CurrentUser.Id;
            NotebookViewModel nb = new NotebookViewModel();
            
            await nb.LoadAllSharedAndOwnedNotebooksAsync(userid);

            nb.Notebooks = notebookViewModel.Notebooks;

            AddNotebookDialog addNotebookDialog = new AddNotebookDialog(nb, folderViewModel.Folder.Id);
            await addNotebookDialog.ShowAsync();
            this.Frame.Navigate(typeof(FolderViewPage), folderViewModel); //reload page
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog cd = new ContentDialog
            {
                Title = "Delete Dialog?",
                Content = "Are you sure you want to remove the notebook from the folder?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await cd.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (sender is FrameworkElement b && b.DataContext is Notebook notebook)
                {
                    folderNotebookViewModel.folderNotebook.FolderId = folderViewModel.Folder.Id;
                    folderNotebookViewModel.folderNotebook.NoteBookId = notebook.Id;
                    await folderNotebookViewModel.DeleteAsync();
                    this.Frame.Navigate(typeof(FolderViewPage), folderViewModel);
                }
            }

        }

        private void Notebooks_ItemClick(object sender, ItemClickEventArgs e)
        {   
            if(e.ClickedItem is Notebook notebook)
            {
                this.Frame.Navigate(typeof(NotebookViewPage), notebook);
            }
        }
    }
}
