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
    public sealed partial class NoteDataGrid : Page
    {


        public NoteViewModel ViewModel { get; set; } = new NoteViewModel();

        public NoteDataGrid()
        {
            this.InitializeComponent();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadAllAsync();

            base.OnNavigatedTo(e);
        }

        private async void noteDataGrid_RowEditEnded(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridRowEditEndedEventArgs e)
        {
            try
            {
                ViewModel.Note.Notebook = null;
                ViewModel.Note.Owner = null;

                ViewModel.Note.LastModifierUserId = App.userViewModel.GetCurrentUserID();
                ViewModel.Note.DateModified = DateTime.Now;

                await App.UnitOfWork.NoteRepository.UpsertAsync(ViewModel.Note);
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
                this.Frame.Navigate(typeof(NoteDataGrid));
            }
        }

        private void addNote_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddNoteDG));
        }

        private async void deleteNote_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Note != null)
            {
                await ViewModel.DeleteAsync(ViewModel.Note);

                this.Frame.Navigate(typeof(NoteDataGrid));
            }
            else
            {
                ContentDialog cd = new ContentDialog();
                cd.Title = "Select a Note column";
                cd.Content = "Please select a column to delete the Note.";
                cd.PrimaryButtonText = "Okay";

                await cd.ShowAsync();
            }
        }
    }
}
