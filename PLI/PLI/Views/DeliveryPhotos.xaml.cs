using PLI.Utilities;
using PLI.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PLI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeliveryPhotos : ContentPage
    {
        public DeliveryPhotosViewModels viewModel;

        public DeliveryPhotos(Models.Shipment currentShipment)
        {
            InitializeComponent();

            BindingContext = viewModel = new DeliveryPhotosViewModels(currentShipment);
            viewModel.Navigation = this.Navigation;
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            IErrorHandler errorHandler = null;
            viewModel.CommandDeliveryTakePhoto.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }

        private void Button_Clicked_1(object sender, System.EventArgs e)
        {
            IErrorHandler errorHandler = null;
            viewModel.CommandSignature.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IErrorHandler errorHandler = null;
            viewModel.CommandShowPhoto.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }
    }
}