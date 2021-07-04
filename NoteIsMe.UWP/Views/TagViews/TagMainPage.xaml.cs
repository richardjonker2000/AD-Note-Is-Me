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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.TagViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>  
    public sealed partial class TagMainPage : Page
    {
        public TagViewModel tagViewModel { get; set; }
        public NoteTagsViewModel noteTagViewModel { get; set; }
        public SketchTagsViewModel sketchTagViewModel { get; set; }
        public NoteViewModel noteViewModel { get; set; }

        
        public TagMainPage()
        {
            this.InitializeComponent();
            tagViewModel = new TagViewModel();
            noteTagViewModel = new NoteTagsViewModel();
            sketchTagViewModel = new SketchTagsViewModel();

            tagViewModel.Tag.Color = "#FFFFFF";
            tagViewModel.Tag.Name = null;


        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                tagViewModel = e.Parameter as TagViewModel;

                NoteTagGrid.Visibility = 0;
                SketchTagGrid.Visibility = 0;

            }
            await tagViewModel.LoadAllMineAsync();
            base.OnNavigatedTo(e);
        }

        private void editTagDetailButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement b && b.DataContext is Tag tag)
            {
                tagViewModel.Tag = tag;
                this.Frame.Navigate(typeof(AddTagPage), tagViewModel);
            }
        }

        private async void deleteTagButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog cd = new ContentDialog
            {
                Title = "Delete Dialog?",
                Content = "Are you sure you want to remove this Tag?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await cd.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (sender is FrameworkElement b && b.DataContext is Tag tag)
                {
                    tagViewModel.Tag = tag;
                    await tagViewModel.DeleteAsync(tag);
                    this.Frame.Navigate(typeof(TagMainPage));
                }
            }
        }

        private void addNewTagButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddTagPage));
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Tag tag)
            {
                tagViewModel.Tag = tag;
                this.Frame.Navigate(typeof(TagMainPage), tagViewModel);
            }
        }

        private async void deleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog cd = new ContentDialog
            {
                Title = "Delete Dialog?",
                Content = "Are you sure you want to this Note from the Tag?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await cd.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (sender is FrameworkElement b && b.DataContext is NoteTag noteTag)
                {
                    noteTagViewModel.NoteTag = noteTag;
                    await noteTagViewModel.DeleteAsync();
                    this.Frame.Navigate(typeof(TagMainPage));
                }
            }
        }

        private async void addNewNoteButton_Click(object sender, RoutedEventArgs e)
        {
            NoteViewModel n = new NoteViewModel();
            NoteTagsViewModel nt = new NoteTagsViewModel();
            int userid = App.userViewModel.CurrentUser.Id;
            await n.LoadAllofUser(userid);
            await nt.LoadAllTagAsync(tagViewModel.Tag.Id);
            AddNoteDialog addNoteDialog = new AddNoteDialog(nt, n, tagViewModel.Tag.Id);
            await addNoteDialog.ShowAsync();

            this.Frame.Navigate(typeof(TagMainPage));
        }

        private async void deleteSketchButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog cd = new ContentDialog
            {
                Title = "Delete Dialog?",
                Content = "Are you sure you want to this Sketch from the Tag?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };


            ContentDialogResult result = await cd.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (sender is FrameworkElement b && b.DataContext is SketchTag sketchTag)
                {
                    sketchTagViewModel.SketchTag = sketchTag;
                    await sketchTagViewModel.DeleteAsync();

                    this.Frame.Navigate(typeof(TagMainPage));
                }
            }
        }

        private async void addSketchTagButton_Click(object sender, RoutedEventArgs e)
        {
            SketchViewModel s = new SketchViewModel();
            SketchTagsViewModel st = new SketchTagsViewModel();
            int userid = App.userViewModel.CurrentUser.Id;
            await s.LoadAllofUserAsync(userid);
            await st.LoadAllTagAsync(tagViewModel.Tag.Id);
            AddSketchDialog addSketchDialog = new AddSketchDialog(st, s, tagViewModel.Tag.Id);
            await addSketchDialog.ShowAsync();

            this.Frame.Navigate(typeof(TagMainPage));
        }

        private void NoteTagGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is NoteTag nt)
            {
                noteViewModel = new NoteViewModel();

                noteViewModel.Note = nt.Note;
                this.Frame.Navigate(typeof(NoteMainPage), noteViewModel);
            }

        }

        private async void SketchTagGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is SketchTag st)
            {
                Sketch sketch = st.Sketch;


                    SketchViewModel sketchViewModel = new SketchViewModel();
                    sketchViewModel.Sketch = sketch;
                    await App.UnitOfWork.SketchRepository.UpdateAsync(sketch);
                    this.Frame.Navigate(typeof(SketchDrawPage), sketchViewModel);


            }

        }
    }
}
