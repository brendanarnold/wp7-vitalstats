using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Pocketailor.View;
using Pocketailor.ViewModel;
using Pocketailor.Model;
using System.IO.IsolatedStorage;
using Pocketailor.Model.Adjustments;

namespace Pocketailor
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        // Allow access to a single view model instance throughout the app
        public static AppViewModel VM;
        public static SettingsHelpers Settings;
        public static FeedbackAgent FeedbackAgent;
        public static Cache Cache;

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Initialise the ViewModel
            VM = new AppViewModel();

            // Setup settings, since are used to resolve some XAML loaded
            Settings = new SettingsHelpers();

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Set theme dictionaries before  loading components
            // Load appropriate resource dictionary
            if (VM.ApplicationTheme == ApplicationTheme.Light)
                ThemeManager.ToLightTheme();
            else
                ThemeManager.ToDarkTheme();
            ThemeHelpers.LoadThemeDictionary(VM.ApplicationTheme);
            

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                //Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            
            
            // Create DB if not there already
            using (AppDataContext db = new AppDataContext(AppConstants.APP_DB_CONNECTION_STRING))
            {
                if (!db.DatabaseExists())
                    SetupDatabase.InitialiseDB(db);
            }
            // Load cache
            Cache = new Cache();
            // Connect to databases
            VM.ConnectToAppDB(AppConstants.APP_DB_CONNECTION_STRING);
            if (VersionHelpers.IsWP8)
            {
                VM.ConnectToConversionsDB(AppConstants.CONVERSIONS_DB_WP8_CONNECTION_STRING);
            }
            else
            {
                VM.ConnectToConversionsDB(AppConstants.CONVERSIONS_DB_WP7_CONNECTION_STRING);
            }

            // Load and send feedback
            FeedbackAgent = new FeedbackAgent();
            FeedbackAgent.DeliverAdjustmentsTaskAsync();

            //  Check if ready to rate
            VM.AddALaunch();

            // Update to cool new tiles if v.7.8+
            if (VersionHelpers.IsWideTileCapable)
            {
                TileHelpers.UpdateFlipTile("Pocketailor", "", "", "", 0, new Uri("/", UriKind.Relative),
                    new Uri("/Images/Tiles/SmallBackgroundImage.png", UriKind.Relative),
                    new Uri("/Images/Tiles/BackgroundImage.png", UriKind.Relative),
                    new Uri("/Images/Tiles/BackBackgroundImage.png", UriKind.Relative),
                    new Uri("/Images/Tiles/WideBackgroundImage.png", UriKind.Relative),
                    new Uri("/Images/Tiles/WideBackBackgroundImage.png", UriKind.Relative));
            }



        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            App.VM.UpdateLicenseInfo();
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            App.VM.UpdateLicenseInfo();
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // I'm not logging the Nokia Ad Exchange's shitty exceptions
            if (e.ExceptionObject.StackTrace.Contains("Inneractive.Ad.InneractiveAdControl"))
            {
                e.Handled = true;
                return;

            }


            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}