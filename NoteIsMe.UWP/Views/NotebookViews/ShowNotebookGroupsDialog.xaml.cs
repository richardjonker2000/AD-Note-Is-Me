using NoteIsMe.Domain.Models;
using NoteIsMe.UWP.ViewModels;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.NotebookViews
{
    public sealed partial class ShowNotebookGroupsDialog : ContentDialog
    {
        public Notebook notebook { get; set; }
        public GroupViewModel groupViewModel { get; set; }

        public ShowNotebookGroupsDialog(Notebook nb)
        {
            this.InitializeComponent();
            notebook = nb;
            groupViewModel = new GroupViewModel();
        }



        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void RemoveShare_Click(object sender, RoutedEventArgs e)
        {

            if (sender is FrameworkElement b && b.DataContext is Group group)
            {
                await groupViewModel.DeleteAsync(group);
            }

            this.Hide();

        }

    }
}
