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
    public sealed partial class AddTagDG : Page
    {
        public AddTagDG()
        {
            this.InitializeComponent();
        }

        public TagViewModel ViewModel { get; set; } = new TagViewModel();
        public UserViewModel UserViewModel { get; set; } = new UserViewModel();



        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await UserViewModel.LoadAllAsync();
            base.OnNavigatedTo(e);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TagName.Text) && tagColor.Color != null && ownerID.SelectedValue != null)
            {
                ViewModel.Tag.Name = TagName.Text;
                ViewModel.Tag.UserId = Convert.ToInt32(ownerID.SelectedValue);
                ViewModel.Tag.Color = tagColor.Color.ToString();

                await App.UnitOfWork.TagRepository.CreateAsync(ViewModel.Tag);

                this.Frame.Navigate(typeof(TagDataGrid));
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
    }
}
