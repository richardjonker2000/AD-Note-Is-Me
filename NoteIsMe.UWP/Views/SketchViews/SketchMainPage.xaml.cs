using NoteIsMe.Domain.Models;
using NoteIsMe.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.SketchViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SketchMainPage : Page
    {
        public SketchViewModel sketchViewModel { get; set; }
        public NotebookViewModel notebookViewModel { get; set; }



        public SketchMainPage()
        {
            this.InitializeComponent();
            sketchViewModel = new SketchViewModel();
            notebookViewModel = new NotebookViewModel();
            
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            int userid = App.userViewModel.CurrentUser.Id;
            await sketchViewModel.LoadAllofUserAsync(userid);

            await notebookViewModel.LoadAllSharedAndOwnedNotebooksAsync(userid);



            base.OnNavigatedTo(e);
        }

        public static ImageSource LoadedImageAsync(byte[] content)
        {
            using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
            {
                // Writes the image byte array in an InMemoryRandomAccessStream
                // that is needed to set the source of BitmapImage.
                using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(content);

                    // The GetResults here forces to wait until the operation completes
                    // (i.e., it is executed synchronously), so this call can block the UI.
                    writer.StoreAsync().GetResults();
                }

                var image = new BitmapImage();
                image.SetSource(ms);
                ms.Dispose();
                return image;
            }

           
            
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SketchForm));
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog cd = new ContentDialog
            {
                Title = "Delete Dialog?",
                Content = "Are you sure you want to delete the category?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await cd.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (sender is FrameworkElement b && b.DataContext is Sketch sketch)
                {
                    sketchViewModel.DeleteAsync(sketch);
                }
            }
            
        }
        public static Visibility getUserPermission(int sketchId)
        {
            Sketch curr = App.UnitOfWork.SketchRepository.FindById(sketchId);
            return App.bool2visibility(App.userViewModel.GetCurrentUserID() == curr.OwnerId);

        }
        public static string ProcessDetails(DateTime DateModified, DateTime DateCreated, int LastModifierUserId, int OwnerId)
        {
            string owner = UserViewModel.getUserName(OwnerId);
            string lastMod = UserViewModel.getUserName(LastModifierUserId);

            return "Created by "+ owner +" on "+ DateCreated.ToString("dd/MM/yyyy") + "\nModified "+RelativeTime(DateModified)+" by "+owner;
        }

        public static string RelativeTime(DateTime old)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - old.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }


        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

            if (sender is FrameworkElement b && b.DataContext is Sketch sketch)
            {
                sketchViewModel.Sketch = sketch;
                this.Frame.Navigate(typeof(SketchForm), sketchViewModel);

            }
            
        }
        private void View_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SketchDrawPage));
        }

        private async void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Sketch sketch)
            {
               
                if (! await sketchViewModel.isEditPermitted(App.userViewModel.CurrentUser,sketch))
                {
                    ContentDialog cd = new ContentDialog
                    {
                        Title = "Cannot edit!",
                        Content = "You do not have access to edit this sketch, please contact the owner.",
                        
                        CloseButtonText = "Cancel"
                    };

                    await cd.ShowAsync();
                }

                else
                {
                    
                    sketchViewModel.Sketch = sketch;
                    await App.UnitOfWork.SketchRepository.UpdateAsync(sketch);
                    this.Frame.Navigate(typeof(SketchDrawPage), sketchViewModel);
                }
               

            }

            
        }
    }
}
