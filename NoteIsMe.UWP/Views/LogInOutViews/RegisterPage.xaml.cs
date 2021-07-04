using NoteIsMe.Domain.Models;
using NoteIsMe.UWP.ViewModels;
using NoteIsMe.UWP.Views.HomeViews;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
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
    public sealed partial class RegisterPage : Page
    {

        public UserViewModel userViewModel{ get; set; }
        
        public RegisterPage()
        {
            this.InitializeComponent();
            userViewModel = new UserViewModel();
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            string nameEntered = RegisterNameBox.Text;
            string emailEntered = RegisterEmailBox.Text;
            string passwordEntered = RegisterPasswordBox.Password;
            string passwordConfirmationEntered = RegisterConfirmationPasswordBox.Password;
            bool valid = true;

            NameErrorMessage.Text = "";
            EmailErrorMessage.Text = "";
            PasswordErrorMessage.Text = "";
            PasswordConfirmationErrorMessage.Text = "";

            
            if (nameEntered == "")
            {
                NameErrorMessage.Text = "Please Enter your name";
                valid = false;
            }
            if (emailEntered == "")
            {
                EmailErrorMessage.Text = "Please Enter an e-mail address";
                valid = false;
            }
            if (passwordEntered == "")
            {
                PasswordErrorMessage.Text = "Please Enter your password";
                valid = false;
            }
            if (passwordConfirmationEntered == "")
            {
                PasswordConfirmationErrorMessage.Text = "Please confirm your password";
                valid = false;
            }

            if (valid)
            {

                try
                {
                    var addr = new System.Net.Mail.MailAddress(emailEntered);
                    if (addr.Address == emailEntered)
                    {
                        Task<User> usrtask = App.UnitOfWork.UserRepository.FindByEmailAsync(emailEntered);
                        User usr = await usrtask;


                        if (usr != null)
                        {
                            EmailErrorMessage.Text = "E-mail address is already registered";
                            valid = false;
                        }
                    }
                }
                catch
                {
                    EmailErrorMessage.Text = "Please Enter a valid e-mail address";
                    valid = false;
                }              
                
                

                if(passwordEntered.Length < 8)
                {
                    PasswordErrorMessage.Text = "Password must be at least 8 caracters long";
                    valid = false;
                }
                else if (passwordEntered != passwordConfirmationEntered)
                {
                    PasswordConfirmationErrorMessage.Text = "Passwords do not match";
                    valid = false;
                }

            }


            if (valid)
            {

                Random rnd = new Random();
                int registerCode = rnd.Next(100000, 999999);


                //save the code sent as reset code
                App._VerifyCode = registerCode;
                App.userViewModel.sendEmail(registerCode, emailEntered, nameEntered, "register");

                var registerVerifyDialog = new verifyCodeDialog();

                var registerVerifyDialogResult = await registerVerifyDialog.ShowAsync();

                if (registerVerifyDialogResult == ContentDialogResult.Primary)
                {
                    int enteredCode = registerVerifyDialog.GetValue;

                    if (enteredCode == App._VerifyCode)
                    {



                        userViewModel.user.Name = nameEntered;
                        userViewModel.user.Email = emailEntered;

                        string salt = App.userViewModel.CreateSalt();

                        userViewModel.user.Salt = salt;
                        userViewModel.user.Password = App.userViewModel.GenerateHash(passwordEntered, salt);

                        userViewModel.user.Role = 1; // 1 => User , 2 => Manager, 3=> Admin




                        User usr = await App.UnitOfWork.UserRepository.CreateAsync(userViewModel.user);

                        await App.userViewModel.LogIn(usr.Id);

                        ContentDialog cd = new ContentDialog
                        {
                            Title = "Registration Complete!",
                            Content = "Thank you for Registering your account.",
                            PrimaryButtonText = "Okay!",

                        };

                        ContentDialogResult result = await cd.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                        {
                            this.Frame.Navigate(typeof(HomeMainPage));
                        }


                    }
                    else
                    {
                        string title = "Code didn't match";
                        string message = "The code you entered doesn't match with the verification code sent to your email.";

                        var dialog = new ContentDialog();
                        dialog.Title = title;
                        dialog.Content = message;
                        dialog.PrimaryButtonText = "okay";
                        dialog.IsPrimaryButtonEnabled = true;
                        await dialog.ShowAsync();
                    }

                }



                

            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }
    }
}
