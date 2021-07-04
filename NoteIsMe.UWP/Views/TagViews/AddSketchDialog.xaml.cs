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
    public sealed partial class AddSketchDialog : ContentDialog
    {

        public SketchViewModel sketchViewModel { get; set; }
        public SketchTagsViewModel sketchTagsViewModel { get; set; }
        public ObservableCollection<Sketch> sketches;
        public int tagId = 1;


        public AddSketchDialog(SketchTagsViewModel st, SketchViewModel s, int tid)
        {
            tagId = tid;
            sketchTagsViewModel = st;
            //await SketchTagsViewModel.LoadAllTagAsync(tagid); // finsish this sstff > need to add load fromtag, put tagid amd should work
            //SketchViewModel = new SketchbookViewModel();
            sketchViewModel = s;


            sketches = new ObservableCollection<Sketch>();
            //to remove items already in the folder-
            foreach (Sketch sketch in sketchViewModel.Sketches)
            {
                sketches.Add(sketch);
                foreach (SketchTag curr in sketchTagsViewModel.SketchTags)
                {

                    if (curr.SketchId == sketch.Id)
                    {
                        sketches.Remove(sketch);
                    }
                }
            }

            this.InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sketchTagsViewModel.SketchTag.TagId = tagId;
            

            if (SketchID.SelectedItem != null)
            {
                int sketchid = ((Sketch)SketchID.SelectedItem).Id;
                sketchTagsViewModel.SketchTag.SketchId = sketchid;
                await sketchTagsViewModel.InsertAsync();
            }


        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        
    }
}
