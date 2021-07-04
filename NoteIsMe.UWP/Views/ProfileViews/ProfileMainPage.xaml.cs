using NoteIsMe.UWP.ViewModels;
using NoteIsMe.UWP.Views.HomeViews;
using NoteIsMe.UWP.Views.LogInOutViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.ProfileViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfileMainPage : Page
    {
        public UserViewModel userViewModel { get; set; }
        public ProfileMainPage()
        {
            this.InitializeComponent();
            userViewModel = new UserViewModel();


            string imageHash = App.userViewModel.HashEmailForGravatar(App.userViewModel.CurrentUser.Email);

            string ImagePath = $"http://www.gravatar.com/avatar/{imageHash}";


            Uri resourceUri = new Uri(@ImagePath, UriKind.RelativeOrAbsolute);
            profilePic.Source = new BitmapImage(resourceUri);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int userid = App.userViewModel.CurrentUser.Id;
            userViewModel.CurrentUser = App.userViewModel.CurrentUser;

            base.OnNavigatedTo(e);

        }
        

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            userViewModel.CurrentUser.Name = userName.Text;
            await App.UnitOfWork.UserRepository.UpdateAsync(userViewModel.CurrentUser);

            this.Frame.Navigate(typeof(ProfileMainPage));
        }

        private async void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            resetPasswordDialog changePasswd = new resetPasswordDialog();

            await changePasswd.ShowAsync();
            if (App.userViewModel.IsAuthenticated())
            {
                this.Frame.Navigate(typeof(ProfileMainPage));
            }
            else
            {
                this.Frame.Navigate(typeof(LoginPage));
            }


        }
    }
}
