using PLI.Utilities;
using System;
using System.Collections.Generic;
using Xamarin.Forms.Xaml;

namespace PLI.Views.Modal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectRemissionPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        private ViewModels.Modal.SelectRemissionPageViewModels viewModel;
        public event EventHandler<object> CallbackEvent;

        public SelectRemissionPage(List<Models.Remission> remissions)
        {
            InitializeComponent();

            BindingContext = viewModel = new ViewModels.Modal.SelectRemissionPageViewModels(remissions);
            viewModel.Navigation = this.Navigation;
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            IErrorHandler errorHandler = null;
            viewModel.CommandSelectedRemissions.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CallbackEvent?.Invoke(this, true);
        }
    }
}