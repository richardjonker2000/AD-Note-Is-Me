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

namespace NoteIsMe.UWP.Views.AdminViews.NotebookManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotebookDataGrid : Page
    {
        public NotebookViewModel ViewModel { get; set; } = new NotebookViewModel();


        public NotebookDataGrid()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadAllAsync();

            base.OnNavigatedTo(e);
        }

        private async void notebookDataGrid_RowEditEnded(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridRowEditEndedEventArgs e)
        {
            try
            {
                ViewModel.Notebook.Owner = null;
                await App.UnitOfWork.NotebookRepository.UpsertAsync(ViewModel.Notebook);
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
                this.Frame.Navigate(typeof(NotebookDataGrid));
            }

        }

        private void addNotebook_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddNotebookDG));
        }

        private async void deleteNotebook_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Notebook != null)
            {
                await ViewModel.DeleteAsync(ViewModel.Notebook);

                this.Frame.Navigate(typeof(NotebookDataGrid));
            }
            else
            {
                ContentDialog cd = new ContentDialog();
                cd.Title = "Select a Notebook column";
                cd.Content = "Please select a column to delete the Notebook.";
                cd.PrimaryButtonText = "Okay";

                await cd.ShowAsync();
            }
        }
    }
}
