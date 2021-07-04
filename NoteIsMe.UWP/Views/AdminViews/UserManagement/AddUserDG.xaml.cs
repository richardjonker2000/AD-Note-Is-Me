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

namespace NoteIsMe.UWP.Views.AdminViews.UserManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddUserDG : Page
    {
        public UserViewModel ViewModel { get; set; } = new UserViewModel();

        public AddUserDG()
        {
            this.InitializeComponent();
        }

        public Dictionary<int, string> userRoleList { get; } = new Dictionary<int, string>()
            {
            {1, "User" },
            {2, "Manager" },
            {3, "Admin" },
            };

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(UserName.Text) && !string.IsNullOrWhiteSpace(UserEmail.Text) && !string.IsNullOrWhiteSpace(userPassword.Password) && userRole.SelectedValue != null)
            {
                ViewModel.user.Name = UserName.Text;
                try
                {
                    var addr = new System.Net.Mail.MailAddress(UserEmail.Text);
                    if (addr.Address == UserEmail.Text)
                    {
                        ViewModel.user.Email = UserEmail.Text;

                        ViewModel.user.Role = Convert.ToInt32(userRole.SelectedValue);

                        string salt = App.userViewModel.CreateSalt();

                        ViewModel.user.Salt = salt;
                        ViewModel.user.Password = App.userViewModel.GenerateHash(userPassword.Password, salt);
                        ViewModel.user.Password = App.userViewModel.GenerateHash(userPassword.Password, salt);

                        try
                        {
                            await App.UnitOfWork.UserRepository.CreateAsync(ViewModel.user);
                        }
                        catch
                        {
                            ContentDialog cd = new ContentDialog();
                            cd.Title = "Error Duplicate email";
                            cd.Content = "The email provided is already registered, please check the data.";
                            cd.PrimaryButtonText = "Okay";
                            

                            await cd.ShowAsync();
                        }
                        finally
                        {
                            this.Frame.Navigate(typeof(UserDataGrid));
                        }
                    }



                }
                catch
                {
                    emailValidError.Visibility = Visibility.Visible;
                }
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
