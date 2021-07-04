using NoteIsMe.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.LogInOutViews
{
    public sealed partial class resetPasswordDialog : ContentDialog
    {
        public resetPasswordDialog()
        {
            this.InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string errorMessage = "Your password is updated successfully, please login with new password.";
            bool valid = true;
            if (RegisterPasswordBox.Password.Length < 8)
            {
                errorMessage = "Password must be at least 8 caracters long";
                valid = false;
            }
            else if (RegisterPasswordBox.Password != RegisterConfirmationPasswordBox.Password)
            {
                errorMessage = "Passwords do not match";
                valid = false;
            }



            if (valid)
            {
                string salt = App.userViewModel.CreateSalt();

                string hashedPass = App.userViewModel.GenerateHash(RegisterPasswordBox.Password, salt);


                App.userViewModel.CurrentUser.Password = hashedPass;
                App.userViewModel.CurrentUser.Salt = salt;

                await App.UnitOfWork.UserRepository.UpdateAsync(App.userViewModel.CurrentUser);

                if (App.userViewModel.IsAuthenticated())
                {
                    App.userViewModel.LogOut();
                }
                


                App.userViewModel.CurrentUser = null;

                string title = "Password Updated Successfully";
                string message = "Your password is updated successfully, please login with new password.";
                var dialog = new ContentDialog();
                dialog.Title = title;
                dialog.Content = message;
                dialog.PrimaryButtonText = "Okay";
                dialog.IsPrimaryButtonEnabled = true;
                var result = await dialog.ShowAsync();

            }
            else
            {
                var dlg = new MessageDialog(errorMessage);
                await dlg.ShowAsync();

                
            }



            

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }


    }
}
