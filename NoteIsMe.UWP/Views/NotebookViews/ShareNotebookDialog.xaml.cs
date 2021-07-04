using NoteIsMe.Domain.Models;
using NoteIsMe.UWP.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.NotebookViews
{
    public sealed partial class ShareNotebookDialog : ContentDialog
    {
        GroupViewModel groupViewModel { get; set; }
        Notebook notebook { get; set; }

        public ShareNotebookDialog(Notebook nb)
        {
            notebook = nb;
            groupViewModel = new GroupViewModel();
            this.InitializeComponent();
        }



        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            User user = await App.UnitOfWork.UserRepository.FindByEmailAsync(emailToShareNotebook.Text);
            string title = "Shared Successfully";
            string message = "Notebook is shared successfully.";

            if (emailToShareNotebook.Text == "")
            {
                title = "Email cannot be empty";
                message = "You cannot leave email field empty, please enter an email.";
            }
            else if (user == null)
            {
                title = "Invalid Email";
                message = "The email you provided doesn't have a Note Is Me account, please check email and try again.";
            }
            else if (user.Id == notebook.OwnerId)
            {
                title = "Cannot Share";
                message = "Cannot share the notebook to its Owner itself.";
            }

            else if (user.Id == App.userViewModel.CurrentUser.Id)
            {
                title = "Cannot Share";
                message = "Cannot share the notebook yourself.";
            }


            else
            {


                groupViewModel.Group.EditPermission = editSwitch.IsOn;
                groupViewModel.Group.ViewPermission = viewSwitch.IsOn;
                groupViewModel.Group.SharePermission = shareSwitch.IsOn;
                groupViewModel.Group.UserId = user.Id;
                groupViewModel.Group.NotebookId = notebook.Id;

                groupViewModel.Group = await App.UnitOfWork.GroupRepository.UpsertAsync(groupViewModel.Group);

                App.userViewModel.sendEmail(notebook.Id, user.Email, user.Name, "share");

                title = "Shared Successfully";
                message = "The notebook is shared successfully to the specified user.";


            }


            var dialog = new ContentDialog();
            dialog.Title = title;
            dialog.Content = message;
            dialog.PrimaryButtonText = "okay";
            dialog.IsPrimaryButtonEnabled = true;

            await dialog.ShowAsync();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
