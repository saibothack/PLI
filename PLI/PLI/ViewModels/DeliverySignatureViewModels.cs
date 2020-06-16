using PLI.Models;
using PLI.Utilities;
using Rg.Plugins.Popup.Services;
using SignaturePad.Forms;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static PLI.ViewModels.DeliveryPhotosViewModels;

namespace PLI.ViewModels
{
    public class DeliverySignatureViewModels : ViewModelBase
    {
        #region properties
        private Shipment currentShipment;
        public INavigation Navigation { get; internal set; }

        public AsyncCommand CommandDelivery { get; set; }
        
        public ObservableCollection<ItemsSourceCollectionView> delivertyPhotosSource { get; set; }
        public SignaturePadView signatureView { get; internal set; }

        #endregion

        public DeliverySignatureViewModels(Shipment currentShipment, ObservableCollection<ItemsSourceCollectionView> delivertyPhotosSource)
        {
            CommandDelivery = new AsyncCommand(DeliveryAsync, CanExecuteSubmit);
            this.delivertyPhotosSource = delivertyPhotosSource;
            this.currentShipment = currentShipment;
        }

        public async Task DeliveryAsync()
        {
            try
            {
                IsBusy = true;

                foreach (var item in delivertyPhotosSource)
                {
                    Models.Post.DeliveryPhoto deliveryPhoto = new Models.Post.DeliveryPhoto
                    {
                        shipment_id = currentShipment.id,
                        remissions_id = (from x in currentShipment.remissions
                                         where x.IsChecked == true && x.IsEnabled == true
                                         select x.id).ToList(),
                        photo = item.imageBase64

                    };

                    var deliveryRequest = await App.oServiceManager.SetPhoto(deliveryPhoto);

                    //if (!deliveryRequest.success)
                    //{
                    //    var deliveryRequestSecond = await App.oServiceManager.SetPhoto(deliveryPhoto);

                    //    if (!deliveryRequestSecond.success)
                    //    {

                    //    } 
                    //}
                }

                var img = await signatureView.GetImageStreamAsync(SignatureImageFormat.Png);
                var signatureMemoryStream = new MemoryStream();
                img.CopyTo(signatureMemoryStream);
                byte[] data = signatureMemoryStream.ToArray();
                var base64Signature = Convert.ToBase64String(data);

                Models.Post.DeliverySignature deliverySignature = new Models.Post.DeliverySignature
                {
                    shipment_id = currentShipment.id,
                    remissions_id = (from x in currentShipment.remissions
                                     where x.IsChecked == true && x.IsEnabled == true
                                     select x.id).ToList(),
                    signature = base64Signature

                };

                var signatureRequest = await App.oServiceManager.SetSignature(deliverySignature);

                if (signatureRequest.success)
                {
                    //var signatureRequestSecond = await App.oServiceManager.SetSignature(deliverySignature);

                    //if (!signatureRequestSecond.success)
                    //{

                    //}

                    Preferences.Set("endShipments", true);
                    await Navigation.PopToRootAsync();
                }

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
    }
}
