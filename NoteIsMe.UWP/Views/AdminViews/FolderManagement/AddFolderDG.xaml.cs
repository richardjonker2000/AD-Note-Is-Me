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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.AdminViews.FolderManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddFolderDG : Page
    {
        public FolderViewModel ViewModel { get; set; } = new FolderViewModel();
        public UserViewModel UserViewModel { get; set; } = new UserViewModel();

        public ObservableCollection<string> FolderCategoryList { get; set; }

        public AddFolderDG()
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
            await UserViewModel.LoadAllAsync();
            base.OnNavigatedTo(e);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(folderName.Text) && ownerID.SelectedValue != null && iconURL.SelectedItem != null)
            {
                ViewModel.Folder.Name = folderName.Text;
                ViewModel.Folder.OwnerId = Convert.ToInt32(ownerID.SelectedValue);
                ViewModel.Folder.IconURL = iconURL.SelectedItem as string;

                await App.UnitOfWork.FolderRepository.CreateAsync(ViewModel.Folder);

                this.Frame.Navigate(typeof(FolderDataGrid));
            }
            else
            {
                ContentDialog cd = new ContentDialog();
                cd.Title = "Please fill all fields";
                cd.Content = "Please fill all fields";
                cd.PrimaryButtonText = "Okay";

                await cd.ShowAsync();
            }
                
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack){
                Frame.GoBack();
            }
        }
    }
}
