using PLI.Utilities;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PLI.ViewModels.Modal
{
    class ChangePasswordPageViewModels : ViewModelBase
    {
        #region "Properties"
        public INavigation Navigation { get; internal set; }
        public IAsyncCommand CommandChangePassword { get; set; }

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

        private string _sPasswordConfirm;
        public string sPasswordConfirm
        {
            get { return _sPasswordConfirm; }
            set
            {
                SetProperty(ref _sPasswordConfirm, value);
            }
        }

        private bool _bPasswordErrorConfirm;
        public bool bPasswordErrorConfirm
        {
            get { return _bPasswordErrorConfirm; }
            set
            {
                SetProperty(ref _bPasswordErrorConfirm, value);
            }
        }

        private string _sPasswordErrorConfirm;
        public string sPasswordErrorConfirm
        {
            get { return _sPasswordErrorConfirm; }
            set
            {
                SetProperty(ref _sPasswordErrorConfirm, value);
            }
        }

        #endregion

        public ChangePasswordPageViewModels()
        {
            CommandChangePassword = new AsyncCommand(ChangePasswordAsync, CanExecuteSubmit);

            bPasswordError = false;
            bPasswordErrorConfirm = false;
            IsBusy = false;
        }

        private async Task ChangePasswordAsync()
        {
            try
            {
                if (validate())
                {
                    IsBusy = true;

                    Models.Post.ChangePassword change = new Models.Post.ChangePassword
                    {
                        password = sPassword
                    };

                    Models.Response.Login loginResponse = await App.oServiceManager.ChangePassword(change);

                    if (loginResponse.success)
                    {
                        Preferences.Set("changePassword", loginResponse.changePassword);
                        Preferences.Set("token", loginResponse.token);
                        App.oServiceManager.refreshToken();

                        await PopupNavigation.PopAllAsync();
                    }
                    else
                    {
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


            if (string.IsNullOrEmpty(sPassword))
            {
                success = false;
                bPasswordError = true;
            }
            else
            {
                bPasswordError = false;
            }

            if (string.IsNullOrEmpty(sPasswordConfirm))
            {
                success = false;
                bPasswordErrorConfirm = true;
                sPasswordErrorConfirm = "Por favor confirme su contraseña";
            }
            else
            {
                if (sPasswordConfirm.Equals(sPassword))
                {
                    bPasswordErrorConfirm = false;
                }   
                else {
                    success = false;
                    bPasswordErrorConfirm = true;
                    sPasswordErrorConfirm = "Sus contraseñas no coincide";
                }
            }

            return success;
        }
    }
}
