using NoteIsMe.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
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
    /// 
    public sealed partial class SketchDrawPage : Page
    {

        public SketchViewModel sketchViewModel { get; set; }

        private InkPresenter _inkPresenter;
        public SketchDrawPage()
        {
            this.InitializeComponent();
            sketchViewModel = new SketchViewModel();

            _inkPresenter = inkCanvas.InkPresenter;
            _inkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen | CoreInputDeviceTypes.Touch;

            UpdatePen();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                sketchViewModel = e.Parameter as SketchViewModel;
            }
            LoadSketch();
          base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SaveSketch();

            base.OnNavigatedFrom(e);
        }

        public void UpdatePen()
        {
            if (_inkPresenter != null)
            {
                var defaultAttributes = _inkPresenter.CopyDefaultDrawingAttributes();

                // If we are using a pencil, changing pentip is not allowed!
                if (defaultAttributes.Kind == InkDrawingAttributesKind.Default)
                {
                    //defaultAttributes.PenTip = (bool)penTipShape.IsChecked ? PenTipShape.Circle : PenTipShape.Rectangle;
                    defaultAttributes.PenTip =  PenTipShape.Circle;
                    _inkPresenter.UpdateDefaultDrawingAttributes(defaultAttributes);
                }
            }
        }

        


        public async void SaveSketch()
        {
            InMemoryRandomAccessStream testStream = new InMemoryRandomAccessStream();
            using (IOutputStream outputStream = testStream.GetOutputStreamAt(0))
            {
                //save inkstrokes to the stream 
                await inkCanvas.InkPresenter.StrokeContainer.SaveAsync(outputStream);
                await outputStream.FlushAsync();
            }
            //use datareader to read the stream
            var dr = new DataReader(testStream.GetInputStreamAt(0));
            //create byte array
            var bytes = new byte[testStream.Size];
            //load stream
            await dr.LoadAsync((uint)testStream.Size);
            //save to byte array
            dr.ReadBytes(bytes);
            sketchViewModel.Sketch.Content = new byte[testStream.Size];
            sketchViewModel.Sketch.Content = bytes;

            sketchViewModel.Sketch.LastModifierUserId = App.userViewModel.CurrentUser.Id;

            sketchViewModel.Sketch.DateModified = DateTime.Now;
            await App.UnitOfWork.SketchRepository.UpdateAsync(sketchViewModel.Sketch);
        }
        public async void LoadSketch()
        {
            InMemoryRandomAccessStream testStream = new InMemoryRandomAccessStream();

            using (IOutputStream outputStream = testStream.GetOutputStreamAt(0))
            {
                byte[] sketch = sketchViewModel.Sketch.Content;
                if (sketch != null) // new sketc
                {
                    await outputStream.WriteAsync(sketch.AsBuffer());


                    await inkCanvas.InkPresenter.StrokeContainer.LoadAsync(testStream.GetInputStreamAt(0));
                    await outputStream.FlushAsync();
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveSketch();

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Get all strokes on the InkCanvas.
            IReadOnlyList<InkStroke> currentStrokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();

            // Strokes present on ink canvas.
            if (currentStrokes.Count > 0)
            {
                // Let users choose their ink file using a file picker.
                // Initialize the picker.
                Windows.Storage.Pickers.FileSavePicker savePicker =
                    new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation =
                    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add(
                    "GIF with embedded ISF",
                    new List<string>() { ".gif" });
                savePicker.DefaultFileExtension = ".gif";
                savePicker.SuggestedFileName = "InkSample";

                // Show the file picker.
                Windows.Storage.StorageFile file =
                    await savePicker.PickSaveFileAsync();
                // When chosen, picker returns a reference to the selected file.
                if (file != null)
                {
                    // Prevent updates to the file until updates are 
                    // finalized with call to CompleteUpdatesAsync.
                    Windows.Storage.CachedFileManager.DeferUpdates(file);
                    // Open a file stream for writing.
                    IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                    // Write the ink strokes to the output stream.
                    using (IOutputStream outputStream = stream.GetOutputStreamAt(0))
                    {
                        await inkCanvas.InkPresenter.StrokeContainer.SaveAsync(outputStream);
                        await outputStream.FlushAsync();
                    }
                    stream.Dispose();

                    // Finalize write so other apps can update file.
                    Windows.Storage.Provider.FileUpdateStatus status =
                        await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);

                    if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                    {
                        // File saved.
                    }
                    else
                    {
                        // File couldn't be saved.
                    }
                }
                // User selects Cancel and picker returns null.
                else
                {
                    // Operation cancelled.
                }
            }
        }
    }
}
