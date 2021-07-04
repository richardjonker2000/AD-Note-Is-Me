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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.FolderViews
{
    public sealed partial class AddNotebookDialog : ContentDialog
    {
        public NotebookViewModel notebookViewModel { get; set; }
        public FolderNotebookViewModel folderNotebookViewModel { get; set; }
        public ObservableCollection<Notebook> notebooks;
        public int folderid;

        public AddNotebookDialog(NotebookViewModel n, int fid)
        {
            folderNotebookViewModel = new FolderNotebookViewModel();
            folderid = fid;
            notebookViewModel = n;
            notebooks = new ObservableCollection<Notebook>();
            //to remove items already in the folder
            foreach (Notebook nb in notebookViewModel.MyNotebooks)
            {
                notebooks.Add(nb);
                foreach (Notebook curr in notebookViewModel.Notebooks)
                {
                   
                    if (curr.Id==nb.Id)
                    {
                        notebooks.Remove(nb);
                    }
                }
            }
            foreach (Notebook nb in notebookViewModel.SharedNotebooks)
            {
                notebooks.Add(nb);

                foreach (Notebook curr in notebookViewModel.Notebooks)
                {
                    if (curr.Id == nb.Id )
                    {
                        notebooks.Remove(nb);

                    }
                }
            }

            this.InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            folderNotebookViewModel.folderNotebook.FolderId = folderid;


            if (NotebookID.SelectedItem != null)
            {
                int notebookid = ((Notebook)NotebookID.SelectedItem).Id;
                folderNotebookViewModel.folderNotebook.NoteBookId = notebookid;
                await folderNotebookViewModel.InsertAsync();
            }
            ;




        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
