using PLI.Utilities;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PLI.ViewModels.Modal
{
    public class IncidencePageViewModels : ViewModelBase
    {
        #region "Properties"
        public INavigation Navigation { get; internal set; }
        public Models.Shipment currentShipment { get; private set; }
        public AsyncCommand CommandInit { get; set; }
        public AsyncCommand CommandIncident { get; set; }

        private ObservableCollection<Models.Incidence> _SourceIncidences;
        public ObservableCollection<Models.Incidence> SourceIncidences
        {
            get { return _SourceIncidences; }
            set
            {
                SetProperty(ref _SourceIncidences, value);
            }
        }

        private Models.Incidence _SelectedIncidence;
        public Models.Incidence SelectedIncidence
        {
            get { return _SelectedIncidence; }
            set
            {
                SetProperty(ref _SelectedIncidence, value);
            }
        }

        private bool _bIncidenceError;
        public bool bIncidenceError
        {
            get { return _bIncidenceError; }
            set
            {
                SetProperty(ref _bIncidenceError, value);
            }
        }

        private string _Comments;
        public string Comments
        {
            get { return _Comments; }
            set
            {
                SetProperty(ref _Comments, value);
            }
        }

        #endregion

        public IncidencePageViewModels(Models.Shipment currentShipment)
        {
            this.currentShipment = currentShipment;

            CommandInit = new AsyncCommand(InitAsync, CanExecuteSubmit);
            CommandIncident = new AsyncCommand(IncidenceAsync, CanExecuteSubmit);
        }

        private async Task IncidenceAsync()
        {
            try
            {
                if (SelectedIncidence != null)
                {
                    IsBusy = true;

                    var request = new GeolocationRequest(GeolocationAccuracy.Best);
                    var location = await Geolocation.GetLocationAsync(request);

                    if (location != null)
                    {
                        Models.Post.Incidence incidence = new Models.Post.Incidence();
                        incidence.incidences_id = SelectedIncidence.id;
                        incidence.shipment_id = currentShipment.id;
                        incidence.comments = Comments;
                        incidence.longitude = location.Longitude;
                        incidence.latitude = location.Latitude;

                        var data = await App.oServiceManager.SetIncidence(incidence);

                        if (data.success)
                        {
                            await PopupNavigation.PopAllAsync();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Error", data.message, "Aceptar");
                        }
                    }
                    else {
                        await App.Current.MainPage.DisplayAlert("Error", "No se puede obtener su ubicación", "Aceptar");
                    }

                } else
                {
                    bIncidenceError = true;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task InitAsync()
        {
            try
            {
                IsBusy = true;
                SourceIncidences = new ObservableCollection<Models.Incidence>();
                var data = await App.oServiceManager.GetCatIncidences();

                foreach (var item in data.incidences)
                {
                    SourceIncidences.Add(item);
                }
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
