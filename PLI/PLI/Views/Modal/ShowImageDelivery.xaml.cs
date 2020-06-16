using PLI.Utilities;
using System;
using Xamarin.Forms.Xaml;

namespace PLI.Views.Modal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowImageDelivery : Rg.Plugins.Popup.Pages.PopupPage
    {
        private ViewModels.Modal.ShowImageDeliveryViewModels viewModel;
        public event EventHandler<object> CallbackEvent;
        public ShowImageDelivery(ViewModels.DeliveryPhotosViewModels.ItemsSourceCollectionView itemSelected)
        {
            InitializeComponent();

            BindingContext = viewModel = new ViewModels.Modal.ShowImageDeliveryViewModels(itemSelected);
            viewModel.Navigation = this.Navigation;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            IErrorHandler errorHandler = null;
            viewModel.CommandDeletePhoto.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
            CallbackEvent?.Invoke(this, true);
        }
    }
}