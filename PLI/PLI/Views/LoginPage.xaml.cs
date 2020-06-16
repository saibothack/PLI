using LeoJHarris.FormsPlugin.Abstractions;
using PLI.Utilities;
using PLI.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PLI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private LoginPageViewModels viewModel;

        public LoginPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new LoginPageViewModels();
            viewModel.Navigation = this.Navigation;

            txtUser.NextEntry = txtPassword;
            txtUser.ReturnKeyType = ReturnKeyTypes.Go;
            txtPassword.ReturnKeyType = ReturnKeyTypes.Done;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            IErrorHandler errorHandler = null;
            viewModel.CommandLogin.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }
    }
}