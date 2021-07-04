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

namespace NoteIsMe.UWP.Views.FolderViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddFolderPage : Page
    {
        public FolderViewModel folderViewModel { get; set; }

        public Dictionary<string, string> FolderCategoryList { get; } = new Dictionary<string, string>()
            {
            {"Home", "/Assets/folderIcons/home.png" },
            {"Work", "/Assets/folderIcons/work.png" },
            {"School", "/Assets/folderIcons/school.png" },
            {"Misc", "/Assets/folderIcons/misc.png" }
            };

        

        public AddFolderPage()
        {
            this.InitializeComponent();
            folderViewModel = new FolderViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                folderViewModel = e.Parameter as FolderViewModel;

                switch (folderViewModel.Folder.IconURL)
                {
                    case "/Assets/folderIcons/home.png":
                        folderCategory.SelectedIndex = 0;
                        break;
                    case "/Assets/folderIcons/work.png":
                        folderCategory.SelectedIndex = 1;
                        break;
                    case "/Assets/folderIcons/school.png":
                        folderCategory.SelectedIndex = 2;
                        break;
                    case "/Assets/folderIcons/misc.png":
                        folderCategory.SelectedIndex = 3;
                        break;
                    default:
                        folderCategory.SelectedIndex = -1;
                        break;

                }
                                }
            base.OnNavigatedTo(e);
        }

        private void CancelFolderCreation_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
                this.Frame.GoBack();
        }

        private async void SaveFolder_Click(object sender, RoutedEventArgs e)
        {
            titleError.Text = "";
            if (folderTitle.Text != "")
            {
                if (folderCategory.SelectedItem != null)
                {
                    folderViewModel.Folder.OwnerId = App.userViewModel.GetCurrentUserID();
                    folderViewModel.Folder.Name = folderTitle.Text;
                    folderViewModel.Folder.IconURL = folderCategory.SelectedValue.ToString();
                    await folderViewModel.UpsertAsync();


                    this.Frame.GoBack();
                }
                else
                {
                    categorErrorMessage.Text = "Please selecta a category for this folder";
                }

            }
            else
            {
                titleError.Text = "Please Enter a name for your folder";
            }
        }
    }
}
