using LeoJHarris.FormsPlugin.Abstractions;
using PLI.Utilities;
using System;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace PLI.Views.Modal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        private ViewModels.Modal.ChangePasswordPageViewModels viewModel;
        public event EventHandler<object> CallbackEvent;


        public ChangePasswordPage()
        {
            InitializeComponent();

            CloseWhenBackgroundIsClicked = false;

            BindingContext = viewModel = new ViewModels.Modal.ChangePasswordPageViewModels();
            viewModel.Navigation = this.Navigation;

            txtPassword.NextEntry = txtPasswordConfirm;
            txtPassword.ReturnKeyType = ReturnKeyTypes.Go;
            txtPasswordConfirm.ReturnKeyType = ReturnKeyTypes.Done;
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            IErrorHandler errorHandler = null;
            viewModel.CommandChangePassword.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CallbackEvent?.Invoke(this, true);
        }
    }

}