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
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NoteIsMe.UWP.Views.NoteViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NoteForm : Page
    {
        NotebookViewModel notebookViewModel { get; set; }
        NoteViewModel noteViewModel { get; set; }

        private Color currentColor = Colors.White;
        //TODO: change


        private string LastFormattedText = "";
        private int LastRawTextLength = 0;

        public NoteForm()
        {
            this.InitializeComponent();
            noteViewModel = new NoteViewModel();
            notebookViewModel = new NotebookViewModel();
        }


        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if(noteTitleText.Text != "" && ( noteNotebook.SelectedItem != null || noteViewModel.Note.NotebookId != 0))
            {
                int userID = App.userViewModel.GetCurrentUserID();

                noteViewModel.Note.LastModifierUserId = userID;
                noteViewModel.Note.DateModified = DateTime.Now;

                if (noteViewModel.Note.DateCreated == DateTime.MinValue)
                {
                    noteViewModel.Note.DateCreated = DateTime.Now;
                    noteViewModel.Note.NotebookId = ((Notebook)noteNotebook.SelectedValue).Id;
                    noteViewModel.Note.OwnerId = (await notebookViewModel.FindbyIDAsync(noteViewModel.Note.NotebookId)).OwnerId;

                }
                

                byte[] bytes;

                using (var memory = new InMemoryRandomAccessStream())
                {

                    note_content.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, memory);

                    var streamToSave = memory.AsStream();

                    var dataReader = new DataReader(streamToSave.AsInputStream());

                    bytes = new byte[streamToSave.Length];

                    await dataReader.LoadAsync((uint)streamToSave.Length);

                    dataReader.ReadBytes(bytes);


                }

                noteViewModel.Note.Content = bytes;


                await noteViewModel.UpsertAsync(noteViewModel.Note);

                if(sender != null)
                {
                    this.Frame.GoBack();
                }
                
            }

            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            currentColor = App.getNegativeColor(((SolidColorBrush)note_content.Background).Color);

            if (e.Parameter != null)
            {
                noteViewModel.Note = e.Parameter as Note;



                using (var memory = new InMemoryRandomAccessStream())
                {
                    var dataWriter = new DataWriter(memory);

                    dataWriter.WriteBytes(noteViewModel.Note.Content);

                    await dataWriter.StoreAsync();

                    note_content.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, memory);
                }
                

                noteNotebook.Visibility = Visibility.Collapsed;
                noteNotebookTitle.Visibility = Visibility.Collapsed;



            }

            int userid = App.userViewModel.CurrentUser.Id;
            await notebookViewModel.LoadMyOwnedNotebooksAsync(userid);
            base.OnNavigatedTo(e);


        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Save_Click(null,null);
            base.OnNavigatedFrom(e);
        }


        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });

            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Document";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until we
                // finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);
                // write to file
                using (Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                {
                    note_content.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, randAccStream);
                }

                // Let Windows know that we're finished changing the file so the
                // other app can update the remote version of the file.
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status != FileUpdateStatus.Complete)
                {
                    Windows.UI.Popups.MessageDialog errorBox =
                        new Windows.UI.Popups.MessageDialog("File " + file.Name + " couldn't be saved.");
                    await errorBox.ShowAsync();
                }
            }
        }


        private void colorPicker_Tapped(object sender, TappedRoutedEventArgs e)
        {
            note_content.Document.Selection.CharacterFormat.ForegroundColor = colorPicker.Color;

            fontColorButton.Flyout.Hide();
            note_content.Focus(Windows.UI.Xaml.FocusState.Keyboard);
            currentColor = colorPicker.Color;
        }



        private void FindBoxHighlightMatches()
        {
            FindBoxRemoveHighlights();

            Color highlightBackgroundColor = (Color)App.Current.Resources["SystemColorHighlightColor"];
            Color highlightForegroundColor = (Color)App.Current.Resources["SystemColorHighlightTextColor"];

            string textToFind = findBox.Text;
            if (textToFind != null)
            {
                ITextRange searchRange = note_content.Document.GetRange(0, 0);
                while (searchRange.FindText(textToFind, TextConstants.MaxUnitCount, FindOptions.None) > 0)
                {
                    searchRange.CharacterFormat.BackgroundColor = highlightBackgroundColor;
                    searchRange.CharacterFormat.ForegroundColor = highlightForegroundColor;
                }
            }
        }

        private void FindBoxRemoveHighlights()
        {
            ITextRange documentRange = note_content.Document.GetRange(0, TextConstants.MaxUnitCount);
            SolidColorBrush defaultBackground = note_content.Background as SolidColorBrush;
            SolidColorBrush defaultForeground = note_content.Foreground as SolidColorBrush;

            documentRange.CharacterFormat.BackgroundColor = defaultBackground.Color;
            documentRange.CharacterFormat.ForegroundColor = defaultForeground.Color;
        }

        private void note_content_GotFocus(object sender, RoutedEventArgs e)
        {
            note_content.Document.GetText(TextGetOptions.UseCrlf, out string currentRawText);
            if (currentRawText.Length != LastRawTextLength)
            {
                // User used cut or paste from action command, skip the event
                return;
            }
            // reset colors to correct defaults for Focused state
            ITextRange documentRange = note_content.Document.GetRange(0, TextConstants.MaxUnitCount);
            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["TextControlBackgroundFocused"];
            SolidColorBrush foreground = (SolidColorBrush)App.Current.Resources["TextControlForegroundFocused"];

            note_content.Document.ApplyDisplayUpdates();

            if (background != null && foreground != null)
            {
                documentRange.CharacterFormat.BackgroundColor = background.Color;
            }
            // saving selection span
            var caretPosition = note_content.Document.Selection.GetIndex(TextRangeUnit.Character) - 1;
            if (caretPosition <= 0)
            {
                caretPosition = 1;
            }
            var selectionLength = note_content.Document.Selection.Length;
            // restoring text styling, unintentionally sets caret position at beginning of text
            //note_content.Document.SetText(TextSetOptions.FormatRtf, LastFormattedText);
            // restoring selection position
            note_content.Document.Selection.SetIndex(TextRangeUnit.Character, caretPosition, false);
            note_content.Document.Selection.SetRange(caretPosition, caretPosition + selectionLength);
            // note_content might have gained focus because user changed color.
            // Change selection color
            // Note that only way to regain with selection containing text is using the change color button
            note_content.Document.Selection.CharacterFormat.ForegroundColor = currentColor;
        }

        private void note_content_LosingFocus(object sender, RoutedEventArgs e)
        {
            // Save text length to determine text length change
            note_content.Document.GetText(TextGetOptions.UseCrlf, out string lastRawText);
            LastRawTextLength = lastRawText.Length;

            // Save formatted to restore formatting upon regaining focus
            note_content.Document.GetText(TextGetOptions.FormatRtf, out LastFormattedText);
        }

        private void note_content_TextChanging(object sender, RichEditBoxTextChangingEventArgs e)
        {
            // Fix bug where selected text would get colored when note_content loses focus
            if (FocusManager.GetFocusedElement() == note_content)
            {
                note_content.Document.Selection.CharacterFormat.ForegroundColor = currentColor;
            }
        }


    }
}
