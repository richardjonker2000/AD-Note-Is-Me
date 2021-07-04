using NoteIsMe.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.TagViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddTagPage : Page
    {

        private Color currentColor = Colors.Crimson;
        public TagViewModel tagViewModel { get; set; }
        public AddTagPage()
        {

            this.InitializeComponent();
            tagViewModel = new TagViewModel();
            tagViewModel.Tag.Color = "Crimson";
           
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                tagViewModel = e.Parameter as TagViewModel;
                title.Text = "Edit Tag";
            }
            base.OnNavigatedTo(e);
        }

        private async void SaveTag_Click(object sender, RoutedEventArgs e)
        {
            nameError.Text = "";
            if (tagName.Text != "")
            {
                tagViewModel.Tag.UserId = App.userViewModel.CurrentUser.Id;

                tagViewModel.Tag.Color = currentColor.ToString();
                tagViewModel.Tag.Name = tagName.Text;
                await tagViewModel.UpsertAsync();

                this.Frame.GoBack();
            }
            else
            {
                nameError.Text = "Please enter a name for yor tag";
            }
        }

        private void CancelTagCreation_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        

       

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            // Extract the color of the button that was clicked.
            Button clickedColor = (Button)sender;
            var rectangle = (Windows.UI.Xaml.Shapes.Rectangle)clickedColor.Content;
            var color = ((Windows.UI.Xaml.Media.SolidColorBrush)rectangle.Fill).Color;

            
            CurrentColor.Fill = new SolidColorBrush(color);
            
            myColorButton.Flyout.Hide();
            currentColor = color;

            ColorText.Background = rectangle.Fill;
        }
    }
}
