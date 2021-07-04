using NoteIsMe.Domain.Models;
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

namespace NoteIsMe.UWP.Views.NotebookViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotebookForm : Page
    {
        public NotebookViewModel notebookViewModel{ get; set; }
        public GroupViewModel groupViewModel { get; set; }

        public NotebookForm()
        {
            this.InitializeComponent();
            notebookViewModel = new NotebookViewModel();
            groupViewModel = new GroupViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                notebookViewModel = e.Parameter as NotebookViewModel;
                Add.Content = "Update";
                create_update_text.Text = "Edit Notebook";
            }
            base.OnNavigatedTo(e);
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if(TitleBox.Text != "")
            {
                int userid = App.userViewModel.GetCurrentUserID();
                notebookViewModel.Notebook.OwnerId = userid;
                notebookViewModel.Notebook.Title = TitleBox.Text;

                await notebookViewModel.UpsertAsync();

                ContentDialog cd = new ContentDialog
                {
                    Title = "Successful !",
                    Content = "Noteook has been updated successfully.",
                    PrimaryButtonText = "Okay!",

                };

                ContentDialogResult result = await cd.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    this.Frame.Navigate(typeof(NotebookMainPage));
                }
            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
