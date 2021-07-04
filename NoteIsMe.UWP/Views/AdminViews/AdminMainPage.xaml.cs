using NoteIsMe.UWP.Views.AdminViews.FolderManagement;
using NoteIsMe.UWP.Views.AdminViews.GroupManagement;
using NoteIsMe.UWP.Views.AdminViews.NotebookManagement;
using NoteIsMe.UWP.Views.AdminViews.NoteManagement;
using NoteIsMe.UWP.Views.AdminViews.SketchManagement;
using NoteIsMe.UWP.Views.AdminViews.TagManagement;
using NoteIsMe.UWP.Views.AdminViews.UserManagement;
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

namespace NoteIsMe.UWP.Views.AdminViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminMainPage : Page
    {
        public int totalNotes{ get; set; }
        public int totalSketches { get; set; }
        public int totalNotebooks { get; set; }
        public int totalFolders { get; set; }
        public int totalGroups { get; set; }
        public int totalUsers { get; set; }


        public class Data
        {
            public string Type { get; set; }

            public double Count { get; set; }
        }

        public List<Data> CreateDataAsync()
        {
            List<Data> barItemList = new List<Data>();
            barItemList.Add(new Data { Type = "Notes", Count = totalNotes});
            barItemList.Add(new Data { Type = "Sketches", Count = totalSketches });
            barItemList.Add(new Data { Type = "Notebooks", Count = totalNotebooks });
            barItemList.Add(new Data { Type = "Folders", Count = totalFolders });
            barItemList.Add(new Data { Type = "Groups", Count = totalGroups });
            barItemList.Add(new Data { Type = "Users", Count = totalUsers });
            return barItemList;
        }

        public AdminMainPage()
        {
            this.InitializeComponent();
            

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            totalNotes = await App.UnitOfWork.NoteRepository.FindTotalCountAsync();
            totalSketches = await App.UnitOfWork.SketchRepository.FindTotalCountAsync();
            totalNotebooks = await App.UnitOfWork.NotebookRepository.FindTotalCountAsync();
            totalFolders = await App.UnitOfWork.FolderRepository.FindTotalCountAsync();
            totalGroups = await App.UnitOfWork.GroupRepository.FindTotalCountAsync();
            totalUsers = await App.UnitOfWork.UserRepository.FindTotalCountAsync();


            this.adminCharts.Series[0].ItemsSource = CreateDataAsync();
            base.OnNavigatedTo(e);
        }

        private void userDataGrid_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UserDataGrid));
        }

        private void folderDataGrid_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FolderDataGrid));
        }

        private void notebookDataGrid_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NotebookDataGrid));
        }

        private void tagDataGrid_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TagDataGrid));
        }

        private void groupDataGrid_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GroupDataGrid));
        }

        private void noteDataGrid_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NoteDataGrid));
        }

        private void sketchDataGrid_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SketchDataGrid));
        }
    }
}
