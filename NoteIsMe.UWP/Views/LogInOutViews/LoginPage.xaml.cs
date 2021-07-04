using NoteIsMe.Domain.Models;
using NoteIsMe.Infrastructure;
using NoteIsMe.UWP.ViewModels;
using NoteIsMe.UWP.Views.HomeViews;
using NoteIsMe.UWP.Views.NoteViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.LogInOutViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegisterPage));
        }

        private async void LogIn_Click(object sender, RoutedEventArgs e)
        {
            string message = "Something went wrong.";
            string title = "ERROR";
            bool showDiag = true;


            string emailEntered = LoginEmailBox.Text;
            string PasswordEntered = LoginPasswordBox.Password;

            Task<User> usrtask = App.UnitOfWork.UserRepository.FindByEmailAsync(emailEntered);
            User usr = await usrtask;

            if (emailEntered.Length == 0)
            {
                title = "Please enter your email";
                message = "Please enter your email, email field cannot be blank.";
            }

            else if (PasswordEntered.Length == 0)
            {
                title = "Please enter your password";
                message = "Please enter your password, password field cannot be blank.";
            }

            else if ((usr != null) && (usr.Email == emailEntered) && (App.userViewModel.VerifyPassword(PasswordEntered, usr.Password, usr.Salt)))
            {

                await App.userViewModel.LogIn(usr.Id);
                showDiag = false;

                this.Frame.Navigate(typeof(HomeMainPage));
                
            }
            else
            {
                title = "Email and password do not match !";
                message = "Please check and enter your email and password again.";
            }
            
           

            if (showDiag)
            {
                // Show dialog and save result
                var dialog = new ContentDialog();
                dialog.Title = title;
                dialog.Content = message;
                dialog.PrimaryButtonText = "okay";
                dialog.IsPrimaryButtonEnabled = true;
                var result = await dialog.ShowAsync();
            }         

            LoginEmailBox.Text = "";
            LoginPasswordBox.Password = "";

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private async void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            string title = "";
            string message = "";


            Task<User> usrtask = App.UnitOfWork.UserRepository.FindByEmailAsync(LoginEmailBox.Text);
            User usr = await usrtask;
            App.userViewModel.CurrentUser = usr;

            if (LoginEmailBox.Text.Length == 0)
            {
                title = "Please provide email";
                message = "Please enter your email associated with your account to reset password.";
                LoginEmailBox.Text = "";

                var dialog = new ContentDialog();
                dialog.Title = title;
                dialog.Content = message;
                dialog.PrimaryButtonText = "okay";
                dialog.IsPrimaryButtonEnabled = true;

                var result = await dialog.ShowAsync();
            }
            else if (usr == null)
            {
                title = "Email doesn't exist";
                message = "We cannot find an account associated with entered email.";
                LoginEmailBox.Text = "";

                var dialog = new ContentDialog();
                dialog.Title = title;
                dialog.Content = message;
                dialog.PrimaryButtonText = "okay";
                dialog.IsPrimaryButtonEnabled = true;

                var result = await dialog.ShowAsync();
            }
            else
            {


                Random rnd = new Random();
                int resetVerifyCode = rnd.Next(100000, 999999);


                //save the code sent as reset code
                App._VerifyCode = resetVerifyCode;

                App.userViewModel.sendEmail(resetVerifyCode, usr.Email, usr.Name, "reset");


                var resetVerifyDialog = new verifyCodeDialog();
                
                var resetVerifyDialogResult = await resetVerifyDialog.ShowAsync();


                if (resetVerifyDialogResult == ContentDialogResult.Primary)
                {
                    int enteredCode = resetVerifyDialog.GetValue;

                    if (enteredCode == App._VerifyCode)
                    {
                        var passwordChangeDialog = new resetPasswordDialog();
                        var passwordChangeResult = await passwordChangeDialog.ShowAsync();




                        this.Frame.Navigate(typeof(LoginPage));


                    }
                    else
                    {
                        title = "Code didn't match";
                        message = "The code you entered doesn't match with the verification code sent to your email.";

                        var dialog = new ContentDialog();
                        dialog.Title = title;
                        dialog.Content = message;
                        dialog.PrimaryButtonText = "okay";
                        dialog.IsPrimaryButtonEnabled = true;

                        var result = await dialog.ShowAsync();
                    }

                }


            }

        }
    }
}
