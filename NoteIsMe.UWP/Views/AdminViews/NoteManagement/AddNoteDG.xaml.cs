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

namespace NoteIsMe.UWP.Views.AdminViews.NoteManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddNoteDG : Page
    {
        public NoteViewModel ViewModel { get; set; } = new NoteViewModel();
        public NotebookViewModel NotebookViewModel { get; set; } = new NotebookViewModel();
        public UserViewModel UserViewModel { get; set; } = new UserViewModel();

        public AddNoteDG()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await UserViewModel.LoadAllAsync();
            base.OnNavigatedTo(e);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NoteName.Text) && notebookID.SelectedValue != null && ownerID.SelectedValue != null)
            {
                ViewModel.Note.Title = NoteName.Text;
                ViewModel.Note.OwnerId = Convert.ToInt32(ownerID.SelectedValue);
                ViewModel.Note.NotebookId = Convert.ToInt32(notebookID.SelectedValue);
                ViewModel.Note.LastModifierUserId = App.userViewModel.GetCurrentUserID();
                ViewModel.Note.DateCreated = DateTime.Now;
                ViewModel.Note.DateModified = DateTime.Now;

                await App.UnitOfWork.NoteRepository.CreateAsync(ViewModel.Note);

                this.Frame.Navigate(typeof(NoteDataGrid));
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

        private async void ownerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await NotebookViewModel.LoadAllSharedAndOwnedNotebooksAsync(Convert.ToInt32(ownerID.SelectedValue));
        }
    }
}
