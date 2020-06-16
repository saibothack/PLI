using PLI.Utilities;
using System;
using Xamarin.Forms.Xaml;

namespace PLI.Views.Modal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncidencePage : Rg.Plugins.Popup.Pages.PopupPage
    {
        private ViewModels.Modal.IncidencePageViewModels viewModel;
        public event EventHandler<object> CallbackEvent;
        public IncidencePage(Models.Shipment currentShipment)
        {
            InitializeComponent();

            BindingContext = viewModel = new ViewModels.Modal.IncidencePageViewModels(currentShipment);
            viewModel.Navigation = this.Navigation;

            IErrorHandler errorHandler = null;
            viewModel.CommandInit.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CallbackEvent?.Invoke(this, true);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            IErrorHandler errorHandler = null;
            viewModel.CommandIncident.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }
    }
}