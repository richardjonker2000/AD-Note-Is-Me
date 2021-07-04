using NoteIsMe.Domain.Models;
using NoteIsMe.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class FolderMainPage : Page
    {
        
        public FolderViewModel FolderViewModel { get; set; }

        public FolderMainPage()
        {
            this.InitializeComponent();
            FolderViewModel = new FolderViewModel();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await FolderViewModel.LoadAllMineAsync();
            base.OnNavigatedTo(e);
        }

        private void addNewFolderButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddFolderPage));
        }

        private void editFolderDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if(sender is FrameworkElement b && b.DataContext is Folder folder)
            {
                FolderViewModel.Folder = folder;
                this.Frame.Navigate(typeof(AddFolderPage), FolderViewModel);
            }
        }


        private async void deleteFolderButton_Click(object sender, RoutedEventArgs e)
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
                if (sender is FrameworkElement b && b.DataContext is Folder folder)
                {
                    FolderViewModel.Folder = folder;
                    await FolderViewModel.DeleteAsync(folder);
                    this.Frame.Navigate(typeof(FolderMainPage));
                }
            }

        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Folder folder)
            {
                
                    FolderViewModel.Folder= folder;
                    this.Frame.Navigate(typeof(FolderViewPage), FolderViewModel);
                
                
            }
        }
    }
}
