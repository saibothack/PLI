using System.Threading.Tasks;

namespace PLI.Services
{
    public class ServiceManager
    {
        IRestService restService;

        public ServiceManager(IRestService service)
        {
            restService = service;
        }

        public void refreshToken()
        {
            restService.refreshToken();
        }

        #region "Auth"

        public Task<Models.Response.Login> LoginAsync(Models.Post.Login login)
        {
            return restService.LoginAsync(login);
        }

        public Task<Models.Response.Login> ChangePassword(Models.Post.ChangePassword change)
        {
            return restService.ChangePassword(change);
        }

        #endregion

        public Task<Models.Response.ChangeStatus> ChangeStatusShipment(Models.Post.ChangeStatusUser changeStatusUser)
        {
            return restService.ChangeStatusShipment(changeStatusUser);
        }

        public Task<Models.Response.Shipments> GetShipments()
        {
            return restService.GetShipments();
        }

        public Task<Models.Response.Default> SetLocation(Models.Post.Location location)
        {
            return restService.SetLocation(location);
        }

        public Task<Models.Response.UserStatus> GetUserStatus()
        {
            return restService.GetUserStatus();
        }

        public Task<Models.Response.Default> SetUserExit()
        {
            return restService.SetUserExit();
        }

        public Task<Models.Response.Default> SetUserBreak(Models.Post.Break bk)
        {
            return restService.SetUserBreak(bk);
        }

        public Task<Models.Response.Default> SetUserBreakEnd(Models.Post.Break bk)
        {
            return restService.SetUserBreakEnd(bk);
        }

        public Task<Models.Response.Default> SetUserTender()
        {
            return restService.SetUserTender();
        }

        public Task<Models.Response.Default> SetUserReturn()
        {
            return restService.SetUserReturn();
        }

        public Task<Models.Response.Default> SetPhoto(Models.Post.DeliveryPhoto deliveryPhoto)
        {
            return restService.SetPhoto(deliveryPhoto);
        }

        public Task<Models.Response.Default> SetSignature(Models.Post.DeliverySignature deliverySignature)
        {
            return restService.SetSignature(deliverySignature);
        }

        public Task<Models.Response.Default> SetIncidence(Models.Post.Incidence incidence)
        {
            return restService.SetIncidence(incidence);
        }

        public Task<Models.Response.Incidences> GetCatIncidences()
        {
            return restService.GetCatIncidences();
        }

    }
}
