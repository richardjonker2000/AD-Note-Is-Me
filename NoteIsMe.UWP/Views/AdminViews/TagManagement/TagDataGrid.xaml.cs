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

namespace NoteIsMe.UWP.Views.AdminViews.TagManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TagDataGrid : Page
    {



        public TagViewModel ViewModel { get; set; } = new TagViewModel();

        public TagDataGrid()
        {
            this.InitializeComponent();
        }



        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadAllAsync();

            base.OnNavigatedTo(e);
        }

        private async void tagDataGrid_RowEditEnded(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridRowEditEndedEventArgs e)
        {
            ViewModel.Tag.User = null;
            try
            {
                await App.UnitOfWork.TagRepository.UpsertAsync(ViewModel.Tag);
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
                this.Frame.Navigate(typeof(TagDataGrid));
            }
        }


        private void addTag_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddTagDG));
        }

        private async void deleteTag_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Tag != null)
            {
                await ViewModel.DeleteAsync(ViewModel.Tag);

                this.Frame.Navigate(typeof(TagDataGrid));
            }
            else
            {
                ContentDialog cd = new ContentDialog();
                cd.Title = "Select a Tag column";
                cd.Content = "Please select a column to delete the Tag.";
                cd.PrimaryButtonText = "Okay";

                await cd.ShowAsync();
            }
        }
    }
}
