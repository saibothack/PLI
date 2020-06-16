using PLI.Utilities;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PLI.ViewModels.Modal
{
    public class ShowImageDeliveryViewModels : ViewModelBase
    {
        #region "Properties"
        public INavigation Navigation { get; internal set; }
        public IAsyncCommand CommandDeletePhoto { get; set; }

        private DeliveryPhotosViewModels.ItemsSourceCollectionView _itemSelectedPhoto;

        public DeliveryPhotosViewModels.ItemsSourceCollectionView itemSelectedPhoto
        {
            get { return _itemSelectedPhoto; }
            set
            {
                SetProperty(ref _itemSelectedPhoto, value);
            }
        }

        #endregion

        public ShowImageDeliveryViewModels(DeliveryPhotosViewModels.ItemsSourceCollectionView itemSelected)
        {
            CommandDeletePhoto = new AsyncCommand(DeletePhoto, CanExecuteSubmit);
            this.itemSelectedPhoto = itemSelected;
        }

        private async Task DeletePhoto()
        {
            try
            {
                IsBusy = true;
                await PopupNavigation.PopAllAsync();
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
    }
}
