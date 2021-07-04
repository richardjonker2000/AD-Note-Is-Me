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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.AdminViews.GroupManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddGroupDG : Page
    {
        public AddGroupDG()
        {
            this.InitializeComponent();
        }

        public GroupViewModel ViewModel { get; set; } = new GroupViewModel();
        public UserViewModel UserViewModel { get; set; } = new UserViewModel();
        public NotebookViewModel NotebookViewModel { get; set; } = new NotebookViewModel();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await UserViewModel.LoadAllAsync();
            await NotebookViewModel.LoadAllAsync();
            base.OnNavigatedTo(e);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (notebookID.SelectedValue != null && userID.SelectedValue != null)
            {
                ViewModel.Group.UserId = Convert.ToInt32(userID.SelectedValue);
                ViewModel.Group.NotebookId = Convert.ToInt32(notebookID.SelectedValue);
                int notebookOwnerId = (await App.UnitOfWork.NotebookRepository.FindByIdAsync(ViewModel.Group.NotebookId)).OwnerId;
                ViewModel.Group.ViewPermission = viewPerm.IsOn;
                ViewModel.Group.EditPermission = editPerm.IsOn;
                ViewModel.Group.SharePermission = sharePerm.IsOn;

                if (ViewModel.Group.UserId != notebookOwnerId)
                {
                    await App.UnitOfWork.GroupRepository.UpsertAsync(ViewModel.Group);
                }
                else
                {
                    ContentDialog cd = new ContentDialog();
                    cd.PrimaryButtonText = "Okay";
                    cd.Title = "Cannot share to owner itself.";
                    cd.Content = "Cannot share the notebook to the owner itself.";
                    await cd.ShowAsync();
                }


                this.Frame.Navigate(typeof(GroupDataGrid));
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
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
