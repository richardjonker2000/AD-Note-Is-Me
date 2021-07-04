using NoteIsMe.Domain.Models;
using NoteIsMe.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace NoteIsMe.UWP.Views.SketchViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SketchForm : Page
    {
        public SketchViewModel SketchViewModel { get; set; }
        public NotebookViewModel NotebookViewModel { get; set; }

       

        public int selected = -1;

        public SketchForm()
        {
            this.InitializeComponent();
            SketchViewModel = new SketchViewModel();
            NotebookViewModel = new NotebookViewModel();   
           
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            
            int userid = App.userViewModel.CurrentUser.Id;
            
            await NotebookViewModel.LoadAllSharedAndOwnedNotebooksAsync(userid);
           

            selected = -1;

            if (e.Parameter != null)
            {
                SketchViewModel = e.Parameter as SketchViewModel;
                
               
                int i = 0;

                if (SketchViewModel.Sketch.OwnerId == App.userViewModel.GetCurrentUserID())
                {
                    NotebookID.IsEnabled = true;
                }
                else
                {
                    NotebookID.IsEnabled = false;
                }

                

                foreach (Notebook n in NotebookViewModel.MyNotebooks)
                {
                    if (n.Id == SketchViewModel.Sketch.NotebookId)
                    {
                        selected = i;
                       
                        Title.Text = SketchViewModel.Sketch.Title;
                    }
                    i++;
                }
                foreach (Notebook n in NotebookViewModel.SharedNotebooks)
                {
                    if (n.Id == SketchViewModel.Sketch.NotebookId)
                    {
                        selected= i;
                        Title.Text = SketchViewModel.Sketch.Title;
                    }
                    i++;
                }

                NotebookID.SelectedIndex = selected;

            }
            base.OnNavigatedTo(e);
           
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            TitleErrorMessage.Text = "";
            NotebookErrorMessage.Text = "";
            if (NotebookID.SelectedItem!= null)
            {
                
                int notebookID = ((Notebook)NotebookID.SelectedItem).Id;
                int userid = App.userViewModel.CurrentUser.Id;
                Sketch test = SketchViewModel.Sketch;
                if (Title.Text != "")
                {
                   SketchViewModel.Sketch.Title = Title.Text;
                   SketchViewModel.Sketch.NotebookId = notebookID;
                    SketchViewModel.Sketch.Notebook = await NotebookViewModel.FindbyIDAsync(notebookID);
                   SketchViewModel.Sketch.LastModifierUserId = userid;
                   SketchViewModel.Sketch.DateModified = DateTime.Now;
                    if (SketchViewModel.Sketch.DateCreated == DateTime.MinValue)
                    {

                        SketchViewModel.Sketch.Content = null; // to change                        
                        SketchViewModel.Sketch.DateCreated = DateTime.Now;                                              
                        SketchViewModel.Sketch.OwnerId = userid;
                    }

                    await App.UnitOfWork.SketchRepository.UpsertAsync(SketchViewModel.Sketch);
                    this.Frame.GoBack();
                }
                else
                {
                    TitleErrorMessage.Text = "Please enter a name.";
                }
            }
            else
            {
                NotebookErrorMessage.Text = "Please select a Notebook.";
            }


        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
           
            this.Frame.GoBack();

        }

      
    }
}
