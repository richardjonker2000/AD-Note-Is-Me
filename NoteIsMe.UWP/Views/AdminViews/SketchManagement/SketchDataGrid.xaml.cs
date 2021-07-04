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

namespace NoteIsMe.UWP.Views.AdminViews.SketchManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SketchDataGrid : Page
    {

        public SketchViewModel ViewModel { get; set; } = new SketchViewModel();

        public SketchDataGrid()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadAllAsync();

            base.OnNavigatedTo(e);
        }

        private async void sketchDataGrid_RowEditEnded(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridRowEditEndedEventArgs e)
        {
            try
            {
                ViewModel.Sketch.Notebook = null;
                ViewModel.Sketch.Owner = null;

                ViewModel.Sketch.LastModifierUserId = App.userViewModel.GetCurrentUserID();
                ViewModel.Sketch.DateModified = DateTime.Now;

                await App.UnitOfWork.SketchRepository.UpsertAsync(ViewModel.Sketch);
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
                this.Frame.Navigate(typeof(SketchDataGrid));
            }
        }


        private void addSketch_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddSketchDG));
        }

        private async void deleteSketch_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Sketch != null)
            {
                await ViewModel.DeleteAsync(ViewModel.Sketch);

                this.Frame.Navigate(typeof(SketchDataGrid));
            }
            else
            {
                ContentDialog cd = new ContentDialog();
                cd.Title = "Select a Sketch column";
                cd.Content = "Please select a column to delete the Sketch.";
                cd.PrimaryButtonText = "Okay";

                await cd.ShowAsync();
            }
        }
    }
}
