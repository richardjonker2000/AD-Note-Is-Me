using NoteIsMe.Domain.Models;
using NoteIsMe.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.NoteViews
{
    public sealed partial class ChangeNotebookDialog : ContentDialog
    {
        NotebookViewModel notebookViewModel { get; set; }
        NoteViewModel noteViewModel { get; set; }

        public ChangeNotebookDialog(NotebookViewModel nvm, NoteViewModel n)
        {
            notebookViewModel = nvm;
            noteViewModel = n;
            this.InitializeComponent();
        }


        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (noteNotebook.SelectedValue as string != "Id")
            {
                noteViewModel.Note.NotebookId = ((Notebook)noteNotebook.SelectedValue).Id;
                noteViewModel.Note.Notebook = await notebookViewModel.FindbyIDAsync(noteViewModel.Note.NotebookId);

                await noteViewModel.UpsertAsync(noteViewModel.Note);

                ContentDialog cd = new ContentDialog
                {
                    Title = "Successful !",
                    Content = "Note has been moved to new Notebook successfully.",
                    PrimaryButtonText = "Okay!",

                };

                await cd.ShowAsync();
            }
            else
            {
                MessageDialog cd2 = new MessageDialog("Please select a Notebook.", "Failed !"); //Message dialogs are not good but 2 content dialogs can't be shown at same time.

                await cd2.ShowAsync();
            }
            

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
