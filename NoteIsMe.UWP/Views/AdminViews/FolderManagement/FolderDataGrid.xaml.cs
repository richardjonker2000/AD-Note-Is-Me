using NoteIsMe.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace NoteIsMe.UWP.Views.AdminViews.FolderManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FolderDataGrid : Page
    {
        public FolderViewModel ViewModel { get; set; } = new FolderViewModel();
        public ObservableCollection<string> FolderCategoryList { get; set; }

        public FolderDataGrid()
        {
            this.InitializeComponent();
            FolderCategoryList = new ObservableCollection<string>();
            FolderCategoryList.Add("/Assets/folderIcons/home.png");
            FolderCategoryList.Add("/Assets/folderIcons/work.png");
            FolderCategoryList.Add("/Assets/folderIcons/school.png");
            FolderCategoryList.Add("/Assets/folderIcons/misc.png");

        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadAllAsync();

            base.OnNavigatedTo(e);
        }

        private async void folderDataGrid_RowEditEnded(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridRowEditEndedEventArgs e)
        {
            try
            {
                ViewModel.Folder.Owner = null;

                await App.UnitOfWork.FolderRepository.UpsertAsync(ViewModel.Folder);
            }
            catch
            {
                ContentDialog errorDialog = new ContentDialog();
                errorDialog.Title = "Error, Invalid Data";
                errorDialog.Content = "Could't update the entity, please make sure the changed data is correct.";
                errorDialog.PrimaryButtonText = "Okay";

                await errorDialog.ShowAsync();
            }
            finally
            {
                this.Frame.Navigate(typeof(FolderDataGrid));
            }
        }

        private void addFolder_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddFolderDG));
        }

        private async void deleteFolder_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Folder != null)
            {
                await ViewModel.DeleteAsync(ViewModel.Folder);

                this.Frame.Navigate(typeof(FolderDataGrid));
            }
            else
            {
                ContentDialog cd = new ContentDialog();
                cd.Title = "Select a Folder column";
                cd.Content = "Please select a column to delete the folder.";
                cd.PrimaryButtonText = "Okay";

                await cd.ShowAsync();
            }    
        }
    }
}
