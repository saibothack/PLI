using PLI.Models;
using PLI.Utilities;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PLI.ViewModels
{
    public class DeliveryPhotosViewModels : ViewModelBase
    {
        #region properties
        private Shipment currentShipment;
        public INavigation Navigation { get; internal set; }
        
        public AsyncCommand CommandDeliveryTakePhoto { get; set; }
        public AsyncCommand CommandShowPhoto { get; set; }
        public AsyncCommand CommandSignature { get; set; }
        public ObservableCollection<ItemsSourceCollectionView> delivertyPhotosSource { get; set; }

        private ItemsSourceCollectionView _itemSelected;
        public ItemsSourceCollectionView itemSelected
        {
            get { return _itemSelected; }
            set
            {
                SetProperty(ref _itemSelected, value);
            }
        }
        #endregion

        public DeliveryPhotosViewModels(Shipment currentShipment)
        {
            delivertyPhotosSource = new ObservableCollection<ItemsSourceCollectionView>();
            CommandDeliveryTakePhoto = new AsyncCommand(DeliveryTakePhoto, CanExecuteSubmit);
            CommandShowPhoto = new AsyncCommand(ShowPhoto, CanExecuteSubmit);
            CommandSignature = new AsyncCommand(SignatureAsync, CanExecuteSubmit);
            
            this.currentShipment = currentShipment;
        }

        public async Task ShowPhoto()
        {
            try
            {
                IsBusy = true;
                var showImageDelivery = new Views.Modal.ShowImageDelivery(itemSelected);
                showImageDelivery.CallbackEvent += ShowImageDelivery_CallbackEvent;
                await Navigation.PushPopupAsync(showImageDelivery);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ShowImageDelivery_CallbackEvent(object sender, object e)
        {
            try
            {
                IsBusy = true;
                delivertyPhotosSource.Remove(itemSelected);
                itemSelected = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task SignatureAsync()
        {
            try
            {
                await Navigation.PushAsync(new Views.DeliverySignature(currentShipment, delivertyPhotosSource));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
            }
        }

        public async Task DeliveryTakePhoto()
        {
            try
            {
                IsBusy = true;
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Su camara no esta disponible para tomar fotos", "Aceptar");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                });

                if (file == null)
                    return;

                var item = new ItemsSourceCollectionView();

                var stream = file.GetStream();
                var imageByte = Constants.ReadToEnd(stream);
                var base64 = Convert.ToBase64String(imageByte);


                item.imageBase64 = base64;
                item.image = ImageSource.FromStream(() =>
                {
                    return stream;
                });

                delivertyPhotosSource.Add(item);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
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

        public class ItemsSourceCollectionView
        {
            public ImageSource image { get; set; }
            public string imageBase64 { get; set; }
        }
    }
}
