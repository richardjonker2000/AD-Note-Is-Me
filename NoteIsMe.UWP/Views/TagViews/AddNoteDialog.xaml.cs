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

namespace NoteIsMe.UWP.Views.TagViews
{
    public sealed partial class AddNoteDialog : ContentDialog
    {
        public NoteViewModel noteViewModel { get; set; }
        public NoteTagsViewModel noteTagsViewModel { get; set; }
        public ObservableCollection<Note> notes;
        public int tagId = 1;


        public AddNoteDialog(NoteTagsViewModel nt, NoteViewModel n, int tid)
        {
            tagId = tid;
            noteTagsViewModel = nt;
            //await noteTagsViewModel.LoadAllTagAsync(tagid); // finsish this sstff > need to add load fromtag, put tagid amd should work
            //noteViewModel = new NotebookViewModel();
            noteViewModel = n;
          
            
            notes = new ObservableCollection<Note>();
            //to remove items already in the folder-
            foreach (Note note in noteViewModel.Notes)
            {
                notes.Add(note);
                foreach (NoteTag curr in noteTagsViewModel.NoteTags)
                {

                    if (curr.NoteId == note.Id)
                    {
                        notes.Remove(note);
                    }
                }
            }
            

            this.InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            noteTagsViewModel.NoteTag.TagId= tagId;


            if (NoteID.SelectedItem != null)
            {
                int noteid = ((Note)NoteID.SelectedItem).Id;
                noteTagsViewModel.NoteTag.NoteId = noteid;
                await noteTagsViewModel.InsertAsync();
            }         




        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
