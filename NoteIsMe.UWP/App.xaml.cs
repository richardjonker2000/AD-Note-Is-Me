﻿using Microsoft.EntityFrameworkCore;
using NoteIsMe.Domain;
using NoteIsMe.Domain.Models;
using NoteIsMe.Infrastructure;
using NoteIsMe.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace NoteIsMe.UWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        static string SqlConnectionString = "Server=localhost; Initial Catalog=note-is-me; Integrated Security = True; Connect Timeout = 30;";

        public static IUnitOfWork UnitOfWork { get; set; }

        public static UserViewModel userViewModel { get; set; }

        public static int _VerifyCode;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {

            userViewModel = new UserViewModel();
            _VerifyCode = 0;

            this.InitializeComponent();
            this.Suspending += OnSuspending;




            // Get theme choice from LocalSettings.
            object value = ApplicationData.Current.LocalSettings.Values["themeSetting"];

            if (value != null)
            {
                // Apply theme choice.
                App.Current.RequestedTheme = (ApplicationTheme)(int)value;
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            DbContextOptionsBuilder<NoteismeDbContext> dbcob = new DbContextOptionsBuilder<NoteismeDbContext>();
            dbcob.UseSqlServer(SqlConnectionString);
            UnitOfWork = new UnitOfWork(dbcob.Options);

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            // Save application state and stop any background activity
            deferral.Complete();
        }


        public static Visibility bool2visibility(bool b)
        {
            if (b)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }


        public static Color getNegativeColor(Color c)
        {
                Color ColorToConvert = c;
                Color invertedColor = Color.FromArgb((byte)~ColorToConvert.A, (byte)~ColorToConvert.R, (byte)~ColorToConvert.G, (byte)~ColorToConvert.B);
                return invertedColor;
        }





    }
}
