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

namespace NoteIsMe.UWP.Views.AdminViews.UserManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserDataGrid : Page
    {
        public UserViewModel ViewModel { get; set; } = new UserViewModel();

        public ObservableCollection<int> userRoleList { get; set; }

        public UserDataGrid()
        {
            this.InitializeComponent();

            userRoleList = new ObservableCollection<int>();
            userRoleList.Add(1);
            userRoleList.Add(2);
            userRoleList.Add(3);
        }

        public static string role2string()
        {
            return "YEAH";
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadAllAsync();

            base.OnNavigatedTo(e);
        }

        private async void userDataGrid_RowEditEnded(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridRowEditEndedEventArgs e)
        {
            try
            {

                await App.UnitOfWork.UserRepository.UpsertAsync(ViewModel.user);
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
                this.Frame.Navigate(typeof(UserDataGrid));
            }

        }
        private void addUser_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddUserDG));
        }

        private async void deleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.user != null)
            {
                if (ViewModel.user.Id != App.userViewModel.GetCurrentUserID())
                {
                    await ViewModel.DeleteAsync(ViewModel.user);
                }
                else
                {
                    ContentDialog cd = new ContentDialog();
                    cd.Title = "Cannot delete current user";
                    cd.Content = "Deleteing currently logged in user is not possible.";
                    cd.PrimaryButtonText = "Okay";

                    await cd.ShowAsync();
                }
                

                this.Frame.Navigate(typeof(UserDataGrid));
            }
            else
            {
                ContentDialog cd = new ContentDialog();
                cd.Title = "Select a User column";
                cd.Content = "Please select a column to delete the User.";
                cd.PrimaryButtonText = "Okay";

                await cd.ShowAsync();
            }
        }
    }
}
