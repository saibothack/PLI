using PLI.Utilities;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PLI.ViewModels
{
    public class HomePageViewModels : ViewModelBase
    {
        #region properties
        public INavigation Navigation { get; internal set; }
        public AsyncCommand CommandEnd { get; set; }
        public AsyncCommand CommandInit { get; internal set; }
        public AsyncCommand CommandNext { get; set; }
        public AsyncCommand CommandBreak { get; set; }
        public AsyncCommand CommandBreakEnd { get; set; }
        public AsyncCommand CommandIncident { get; set; }
        public AsyncCommand CommandDelivery { get; set; }
        public AsyncCommand CommandSetTenderUser { get; set; }
        public AsyncCommand CommandGetStatusUser { get; set; }
        public AsyncCommand CommandGetCommandUser { get; set; }
        public AsyncCommand CommandDeliveryTakePhoto { get; set; }
        

        public string[] status { get; set; }

        private Color _cNext;
        public Color cNext
        {
            get { return _cNext; }
            set
            {
                SetProperty(ref _cNext, value);
            }
        }

        private Color _cBreak;
        public Color cBreak
        {
            get { return _cBreak; }
            set
            {
                SetProperty(ref _cBreak, value);
            }
        }

        private Color _cIncident;
        public Color cIncident
        {
            get { return _cIncident; }
            set
            {
                SetProperty(ref _cIncident, value);
            }
        }

        private Color _ColorTextRemission;
        public Color ColorTextRemission
        {
            get { return _ColorTextRemission; }
            set
            {
                SetProperty(ref _ColorTextRemission, value);
            }
        }

        private string _lblBntNext;
        public string lblBntNext
        {
            get { return _lblBntNext; }
            set
            {
                SetProperty(ref _lblBntNext, value);
            }
        }

        private Models.Shipment _CurrentShipment;
        public Models.Shipment CurrentShipment
        {
            get { return _CurrentShipment; }
            set
            {
                SetProperty(ref _CurrentShipment, value);
            }
        }

        private string _txtShipment;
        public string txtShipment
        {
            get { return _txtShipment; }
            set
            {
                SetProperty(ref _txtShipment, value);
            }
        }

        private DateTime _txtInitialTime;
        public DateTime txtInitialTime
        {
            get { return _txtInitialTime; }
            set
            {
                SetProperty(ref _txtInitialTime, value);
            }
        }

        private string _txtTimeArrival;
        public string txtTimeArrival
        {
            get { return _txtTimeArrival; }
            set
            {
                SetProperty(ref _txtTimeArrival, value);
            }
        }

        private string _txtTimeElapsed;
        public string txtTimeElapsed
        {
            get { return _txtTimeElapsed; }
            set
            {
                SetProperty(ref _txtTimeElapsed, value);
            }
        }

        private bool _ChangeTender;
        public bool ChangeTender
        {
            get { return _ChangeTender; }
            set
            {
                SetProperty(ref _ChangeTender, value);
            }
        }

        private bool _ActiveTender;
        public bool ActiveTender
        {
            get { return _ActiveTender; }
            set
            {
                SetProperty(ref _ActiveTender, value);
            }
        }

        private bool _ActiveReturn;
        public bool ActiveReturn
        {
            get { return _ActiveReturn; }
            set
            {
                SetProperty(ref _ActiveReturn, value);
            }
        }

        private bool _UserBreak;
        public bool UserBreak
        {
            get { return _UserBreak; }
            set
            {
                SetProperty(ref _UserBreak, value);
            }
        }
        #endregion

        public HomePageViewModels()
        {
            CommandEnd = new AsyncCommand(EndAsync, CanExecuteSubmit);
            CommandInit = new AsyncCommand(InitAsync, CanExecuteSubmit);
            CommandNext = new AsyncCommand(NextAsync, CanExecuteSubmit);
            CommandBreak = new AsyncCommand(BreakAsync, CanExecuteSubmit);
            CommandBreakEnd = new AsyncCommand(BreakEndAsync, CanExecuteSubmit);
            CommandDelivery = new AsyncCommand(DeliveryAsync, CanExecuteSubmit);
            CommandIncident = new AsyncCommand(IncidentAsync, CanExecuteSubmit);
            CommandSetTenderUser = new AsyncCommand(SetTenderAsync, CanExecuteSubmit);
            CommandGetStatusUser = new AsyncCommand(GetUserStatusAsync, CanExecuteSubmit);
            CommandGetCommandUser = new AsyncCommand(GetUserCommandAsync, CanExecuteSubmit);
            CommandDeliveryTakePhoto = new AsyncCommand(DeliveryTakePhoto, CanExecuteSubmit);

            defaultColor();

            status = new string[8];
            status[1] = "Iniciar Carga";
            status[2] = "Finalizar carga";
            status[3] = "Iniciar ruta";
            status[4] = "Llegada con cliente";
            status[5] = "Iniciar Entrega";
            status[6] = "Finalizar Entrega";
            status[7] = "Regresar a planta";

            lblBntNext = status[1];

            txtShipment = "N/A";
            txtTimeArrival = "N/A";
            txtTimeElapsed = "N/A";
        }

        private async Task EndAsync()
        {
            try
            {
                IsBusy = true;
                var endShipment = await App.oServiceManager.SetUserExit();
                
                if (endShipment.success)
                {
                    ChangeTender = true;
                    ActiveTender = false;
                    ActiveReturn = false;
                    UserBreak = false;
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
                if (Preferences.Get("changePassword", false))
                {
                    var changePasswordPage = new Views.Modal.ChangePasswordPage();
                    changePasswordPage.CallbackEvent += ChangePasswordPage_CallbackEvent;
                    await Navigation.PushPopupAsync(changePasswordPage);
                } else
                {
                    await GetUserStatusAsync();
                }

            } finally
            {
                IsBusy = false;
            }
        }

        private async Task DeliveryAsync()
        {
            try
            {
                IsBusy = true;
                await ChangeStatus(6);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ChangePasswordPage_CallbackEvent(object sender, object e)
        {
            IErrorHandler errorHandler = null;
            CommandGetStatusUser.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }

        public async Task GetUserCommandAsync()
        {
            try
            {
                IsBusy = true;

                Models.Response.Shipments responseShipment = await App.oServiceManager.GetShipments();


                if (responseShipment.success)
                {
                    foreach (var shipment in responseShipment.shipments)
                    {
                        CurrentShipment = shipment;
                        txtShipment = CurrentShipment.number;
                        lblBntNext = status[CurrentShipment.status_id];
                        if (CurrentShipment.start_time != null)
                            txtInitialTime = (DateTime)CurrentShipment.start_time;

                        if (CurrentShipment.status_id == 4)
                        {
                            try
                            {
                                var tracking = DependencyService.Get<ITracking>();
                                if (tracking != null)
                                {
                                    tracking.Initial();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error " + ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(responseShipment.message))
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Ocurrio un error por favor notifiquelo al administrador codigo 002UserStatus.", "Aceptar");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", responseShipment.message, "Aceptar");
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SelectRemissionPage_CallbackEvent(object sender, object e)
        {
            IErrorHandler errorHandler = null;
            CommandDeliveryTakePhoto.ExecuteAsync().FireAndForgetSafeAsync(errorHandler);
        }

        public async Task GetUserStatusAsync()
        {
            try
            {
                IsBusy = true;

                Models.Response.UserStatus userStatus = await App.oServiceManager.GetUserStatus();

                if (userStatus.success) {

                    ActiveTender = false;
                    ActiveReturn = false;
                    UserBreak = false;

                    switch (userStatus.status[0].status_user_id)
                    {
                        case 1:

                            bool answer = await Application.Current.MainPage.DisplayAlert("Inactivo",
                                "Su estatus es inactivo, ¿desea cambiar su estatus a ofertar?", "Si", "No");

                            if (answer)
                            {
                                await SetTenderAsync();
                            } else
                            {
                                ChangeTender = true;
                            }

                            break;
                        case 2:
                            ActiveTender = true;
                            break;
                        case 3:
                            await GetUserCommandAsync();
                            ActiveTender = false;
                            break;
                        case 4:
                            ActiveReturn = true;
                            break;
                        case 5:
                            await GetUserCommandAsync();
                            UserBreak = true;
                            break;
                    }
                }
            } finally
            {
                IsBusy = false;
            }
        }

        private async Task SetTenderAsync() {
            try
            {
                IsBusy = true;

                Models.Response.Default responseDefault = await App.oServiceManager.SetUserTender();

                if (!responseDefault.success)
                {
                    if (string.IsNullOrEmpty(responseDefault.message))
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Ocurrio un error por favor notifiquelo al administrador codigo 002UserStatus.", "Aceptar");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", responseDefault.message, "Aceptar");
                    }

                    ChangeTender = true;
                }
                else
                {
                    ActiveReturn = false;
                    ChangeTender = false;
                    ActiveTender = true;
                }
            } finally
            {
                IsBusy = false;
            }
        }

        private async Task NextAsync()
        {
            try
            {
                Models.Response.Default defaultResponse = new Models.Response.Default();
                int idStatus = 0;

                switch (lblBntNext)
                {
                    case "Iniciar Carga":

                        IsBusy = true;
                        idStatus = 2;
                        await ChangeStatus(idStatus);

                        break;
                    case "Finalizar carga":

                        IsBusy = true;
                        idStatus = 3;
                        await ChangeStatus(idStatus);

                        break;
                    case "Iniciar ruta":

                        try
                        {
                            var tracking = DependencyService.Get<ITracking>();
                            if (tracking != null)
                            {
                                tracking.Initial();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error " + ex.Message);
                        }

                        IsBusy = true;
                        idStatus = 4;
                        await ChangeStatus(idStatus);
                        await GetUserStatusAsync();
                        break;
                    case "Llegada con cliente":
                        IsBusy = true;

                        try
                        {
                            var tracking = DependencyService.Get<ITracking>();
                            if (tracking != null)
                            {
                                tracking.Stop();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error " + ex.Message);
                        }

                        try
                        {
                            var request = new GeolocationRequest(GeolocationAccuracy.Best);
                            var location = await Geolocation.GetLocationAsync(request);

                            if (location != null)
                            {
                                Models.Post.Location postLocation = new Models.Post.Location
                                {
                                    latitude = location.Latitude,
                                    longitude = location.Longitude
                                };

                                await App.oServiceManager.SetLocation(postLocation);
                            }
                        }
                        catch (FeatureNotSupportedException fnsEx)
                        {
                            Console.WriteLine("Error " + fnsEx.Message);
                        }
                        catch (FeatureNotEnabledException fneEx)
                        {
                            Console.WriteLine("Error " + fneEx.Message);
                        }
                        catch (PermissionException pEx)
                        {
                            Console.WriteLine("Error " + pEx.Message);
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine("Error " + ex.Message);
                        }

                        idStatus = 5;
                        await ChangeStatus(idStatus);

                        break;
                    case "Iniciar Entrega":
                        if (CurrentShipment.remissions.Count > 1)
                        {
                            var selectRemissionPage = new Views.Modal.SelectRemissionPage(CurrentShipment.remissions);
                            selectRemissionPage.CallbackEvent += SelectRemissionPage_CallbackEvent;
                            await Navigation.PushPopupAsync(selectRemissionPage);
                        } else
                        {
                            CurrentShipment.remissions[0].IsChecked = true;
                            await DeliveryTakePhoto();
                        }

                        IsBusy = true;

                        break;
                    case "Finalizar Entrega":

                        IsBusy = true;
                        idStatus = 7;
                        await ChangeStatus(idStatus);

                        break;
                    case "Regresar a planta":

                        IsBusy = true;
                        idStatus = 8;
                        await ChangeStatus(idStatus);
                        ActiveReturn = true;

                        break;
                }

            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task DeliveryTakePhoto()
        {
            try
            {
                var takePhotos = new Views.DeliveryPhotos(CurrentShipment);
                await Navigation.PushAsync(takePhotos);
            } catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
            }
        }

        public async Task ChangeStatus(int statusUser) 
        {
            if (statusUser == 8)
            {
                await App.oServiceManager.SetUserReturn();
                statusUser = 1;

                txtShipment = "N/A";
                txtTimeArrival = "N/A";
                txtTimeElapsed = "N/A";
                CurrentShipment = new Models.Shipment();
            }
            else
            {


                Models.Post.ChangeStatusUser changeStatusUser = new Models.Post.ChangeStatusUser
                {
                    status = statusUser,
                    shipment_id = CurrentShipment.id
                };

                if (statusUser == 6)
                {
                    var items = from x in CurrentShipment.remissions
                                where x.IsChecked == true && x.IsEnabled == true
                                select x;

                    changeStatusUser.remissions_id = (from x in items
                                                      select x.id).ToList();

                    foreach (var item in items)
                    {
                        item.IsEnabled = false;
                    }
                }

                Models.Response.ChangeStatus defaultResponse = await App.oServiceManager.ChangeStatusShipment(changeStatusUser);

                if (defaultResponse.success)
                {
                    if (!defaultResponse.endShipment)
                    {
                        statusUser--;
                    }

                    cNext = Color.Green;
                    Device.StartTimer(TimeSpan.FromSeconds(5), () =>
                    {
                        cNext = Color.FromHex("#08207B");
                        lblBntNext = status[statusUser];
                        return false;
                    });
                }
                else
                {
                    if (string.IsNullOrEmpty(defaultResponse.message))
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Ocurrio un error por favor notifiquelo al administrador codigo 001UserStatus.", "Aceptar");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", defaultResponse.message, "Aceptar");
                    }
                }
            }
        }

        private async Task BreakAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);
                IsBusy = true;

                if (location != null)
                {
                    Models.Post.Break postLocation = new Models.Post.Break
                    {
                        latitude = location.Latitude,
                        longitude = location.Longitude,
                        shipment_id = CurrentShipment.id
                    };

                    var requestBreack = await App.oServiceManager.SetUserBreak(postLocation);

                    if (requestBreack.success)
                    {
                        cBreak = Color.Orange;
                        UserBreak = true;
                        Device.StartTimer(TimeSpan.FromSeconds(5), () =>
                        {
                            cBreak = Color.FromHex("#08207B");
                            return true;
                        });
                    }
                    else { 
                        
                    }
                }
            }
            finally
            {
                IsBusy = false;

            }
        }

        private async Task BreakEndAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                IsBusy = true;

                if (location != null)
                {
                    Models.Post.Break postLocation = new Models.Post.Break
                    {
                        latitude = location.Latitude,
                        longitude = location.Longitude,
                        shipment_id = CurrentShipment.id
                    };

                    await App.oServiceManager.SetUserBreakEnd(postLocation);
                    UserBreak = false;
                }

                cBreak = Color.Orange;
                Device.StartTimer(TimeSpan.FromSeconds(5), () =>
                {
                    cBreak = Color.FromHex("#08207B");
                    return true;
                });
            }
            catch (Exception ex) {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task IncidentAsync()
        {
            try
            {
                var incidencePage = new Views.Modal.IncidencePage(CurrentShipment);
                incidencePage.CallbackEvent += IncidencePage_CallbackEvent;
                await Navigation.PushPopupAsync(incidencePage);
            }
            finally
            {

            }
        }

        private void IncidencePage_CallbackEvent(object sender, object e)
        {
            cIncident = Color.Red;
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                cIncident = Color.FromHex("#08207B");
                return true;
            });
        }

        private void defaultColor() {
            cNext = Color.FromHex("#08207B");
            cBreak = Color.FromHex("#08207B");
            cIncident = Color.FromHex("#08207B");
        }

        private bool CanExecuteSubmit()
        {
            return !IsBusy;
        }
    }
}
