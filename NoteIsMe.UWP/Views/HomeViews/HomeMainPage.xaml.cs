using NoteIsMe.UWP.ViewModels;
using NoteIsMe.UWP.Views.NotebookViews;
using NoteIsMe.UWP.Views.NoteViews;
using NoteIsMe.UWP.Views.SketchViews;
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

namespace NoteIsMe.UWP.Views.HomeViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeMainPage : Page
    {



        public HomeMainPage()
        {
            
            this.InitializeComponent();


            try
            {
                if (App.userViewModel.IsAuthenticated() && (App.UnitOfWork.UserRepository.FindById(App.userViewModel.GetCurrentUserID())).Id == App.userViewModel.CurrentUser.Id)
                {
                    // this will throw error on load also you need to put the currentuser .
                    //homeWelcomeMessage.Text = App.userViewModel.CurrentUser.Name;


                    //TODO: check if the db has user thingys and if user table is empty logout here
                    homeWelcomeMessage.Text += App.userViewModel.CurrentUser.Name;
                }
                else
                {

                    homeWelcomeMessage.Text += " - ";
                    App.userViewModel.LogOut();
                }
            }
            catch
            {
                homeWelcomeMessage.Text += " - ";
            }
           
            
        }

        private void Notebooks_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NotebookMainPage));
        }

        private void Notes_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NoteMainPage));

        }

        private void Sketches_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SketchMainPage));
        }
    }
}
