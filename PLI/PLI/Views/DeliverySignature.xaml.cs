using PLI.Utilities;
using PLI.ViewModels;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static PLI.ViewModels.DeliveryPhotosViewModels;

namespace PLI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeliverySignature : ContentPage
    {
        public DeliverySignatureViewModels viewModel;

        public DeliverySignature(Models.Shipment currentShipment, ObservableCollection<ItemsSourceCollectionView> delivertyPhotosSource)
        {
            InitializeComponent();

            BindingContext = viewModel = new DeliverySignatureViewModels(currentShipment, delivertyPhotosSource);
            viewModel.Navigation = this.Navigation;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "AllowLandscape");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "PreventLandscape"); //during page close setting back to portrait 
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            viewModel.signatureView = signatureView;
            IErrorHandler errorHandler = null;
            viewModel.CommandDelivery.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }
    }
}