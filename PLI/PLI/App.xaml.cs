using Com.OneSignal;
using Com.OneSignal.Abstractions;
using PLI.Dabase;
using PLI.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PLI
{
    public partial class App : Application
    {
        public static Services.ServiceManager oServiceManager { get; private set; }
        public static dbLogic database;

        public App()
        {
            InitializeComponent();

            OneSignal.Current.StartInit("5c914c1a-cb2e-4d7b-abcf-753f334a0725")
                .HandleNotificationReceived(HandleNotificationReceived)
                .EndInit();

            oServiceManager = new Services.ServiceManager(new Services.RestService());

            if (Preferences.Get("login", false))
            {
                oServiceManager.refreshToken();
                Current.MainPage = new MasterDetailPage()
                {
                    Master = new Views.MasterPage() { Title = "Menú" },
                    Detail = new NavigationPage(new Views.HomePage())
                };
            }
            else {
                MainPage = new Views.LoginPage();
            }
        }

        public static dbLogic DataBase
        {
            get
            {
                if (database == null)
                {
                    try
                    {
                        database = new dbLogic(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PLIDataBase.db3"));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(@"ERROR {0}", ex.Message);
                    }
                }
                return database;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static bool CurrentConetion()
        {
            bool success = true;

            try
            {
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.None || current == NetworkAccess.Unknown)
                {
                    success = false;
                    Current.MainPage.DisplayAlert("Notificación", "No se cuenta con conexion a internet.", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }

            return success;
        }

        private static void HandleNotificationReceived(OSNotification notification)
        {
            OSNotificationPayload payload = notification.payload;
            string message = payload.body;

            var mdp = (Application.Current.MainPage as MasterDetailPage);
            var navPage = mdp.Detail as NavigationPage;

            if (navPage.CurrentPage.GetType() == typeof(Views.HomePage))
            {
                if (message.Equals("Embarque recibido"))
                {
                    var page = navPage.CurrentPage as Views.HomePage;
                    IErrorHandler errorHandler = null;
                    page.viewModel.CommandGetStatusUser.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
                }
            }
        }
    }
}
