using PLI.Utilities;
using PLI.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PLI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePageViewModels viewModel;
        public HomePage()
        {
            InitializeComponent();

            BindingContext = viewModel = new HomePageViewModels();
            viewModel.Navigation = this.Navigation;

            IErrorHandler errorHandler = null;
            viewModel.CommandInit.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }

        private void SwipeItem_Invoked(object sender, EventArgs e)
        {
            IErrorHandler errorHandler = null;
            viewModel.CommandNext.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }

        private void SwipeItemView_Invoked(object sender, EventArgs e)
        {
            IErrorHandler errorHandler = null;
            viewModel.CommandBreak.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }

        private void SwipeItemView_Invoked_1(object sender, EventArgs e)
        {
            IErrorHandler errorHandler = null;
            viewModel.CommandIncident.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            IErrorHandler errorHandler = null;
            viewModel.CommandSetTenderUser.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Preferences.Get("endShipments", false))
            {
                Preferences.Set("endShipments", false);
                IErrorHandler errorHandler = null;
                viewModel.CommandDelivery.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            IErrorHandler errorHandler = null;
            viewModel.CommandEnd.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            IErrorHandler errorHandler = null;
            viewModel.CommandBreakEnd.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {

        }

        void ToolbarItem_Clicked_1(System.Object sender, System.EventArgs e)
        {

        }
    }
}