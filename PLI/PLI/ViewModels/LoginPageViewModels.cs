using Com.OneSignal;
using PLI.Utilities;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PLI.ViewModels
{
    public class LoginPageViewModels : ViewModelBase
    {
        #region "Properties"
        public INavigation Navigation { get; internal set; }
        public AsyncCommand CommandLogin { get; set; }

            private string _sUser;
            public string sUser
            {
                get { return _sUser; }
                set
                {
                    SetProperty(ref _sUser, value);
                }
            }

        private bool _bUserError;
        public bool bUserError
        {
            get { return _bUserError; }
            set
            {
                SetProperty(ref _bUserError, value);
            }
        }

        private string _sPassword;
        public string sPassword
        {
            get { return _sPassword; }
            set
            {
                SetProperty(ref _sPassword, value);
            }
        }

        private bool _bPasswordError;
        public bool bPasswordError
        {
            get { return _bPasswordError; }
            set
            {
                SetProperty(ref _bPasswordError, value);
            }
        }

        #endregion

        public LoginPageViewModels() {
            CommandLogin = new AsyncCommand(LoginAsync, CanExecuteSubmit);

            bUserError = false;
            bPasswordError = false;
            IsBusy = false;
        }

        private async Task LoginAsync()
        {
                try
                {
                if (validate())
                {
                    IsBusy = true;

                    Models.Post.Login loginPost = new Models.Post.Login
                    {
                        email = sUser,
                        password = sPassword
                    };

                    Models.Response.Login loginResponse = await App.oServiceManager.LoginAsync(loginPost);

                    if (loginResponse.success)
                    {
                        Preferences.Set("login", true);
                        Preferences.Set("token", loginResponse.token);
                        Preferences.Set("changePassword", loginResponse.changePassword);
                        Preferences.Set("id", loginResponse.id);

                        OneSignal.Current.SetExternalUserId(loginResponse.id);

                        App.oServiceManager.refreshToken();

                        try
                        {
                            var master = new Views.MasterPage() { Title = "Menú" };
                            var detail = new NavigationPage(new Views.HomePage());

                            Application.Current.MainPage = new MasterDetailPage()
                            {
                                Master = master,
                                Detail = detail
                            };
                        } catch(Exception ex)
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
                        }
                    }
                    else {
                        await Application.Current.MainPage.DisplayAlert("Error", loginResponse.message, "Aceptar");
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanExecuteSubmit()
        {
            return !IsBusy;
        }

        private bool validate()
        {
            bool success = true;


            if (string.IsNullOrEmpty(sUser))
            {
                success = false;
                bUserError = true;
            }
            else
            {
                bUserError = false;
            }

            if (string.IsNullOrEmpty(sPassword))
            {
                success = false;
                bPasswordError = true;
            }
            else
            {
                bPasswordError = false;
            }

            return success;
        }
    }
}
