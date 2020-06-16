using System.Threading.Tasks;

namespace PLI.Services
{
    public interface IRestService
    {
        #region

        void refreshToken();

        Task<Models.Response.Login> LoginAsync(Models.Post.Login login);
        Task<Models.Response.Login> ChangePassword(Models.Post.ChangePassword change);
        Task<Models.Response.ChangeStatus> ChangeStatusShipment(Models.Post.ChangeStatusUser changeStatusUser);
        Task<Models.Response.Shipments> GetShipments();
        Task<Models.Response.Default> SetLocation(Models.Post.Location location);
        Task<Models.Response.UserStatus> GetUserStatus();
        Task<Models.Response.Default> SetUserTender();
        Task<Models.Response.Default> SetUserReturn();
        Task<Models.Response.Default> SetUserExit();
        Task<Models.Response.Default> SetUserBreak(Models.Post.Break bk);
        Task<Models.Response.Default> SetUserBreakEnd(Models.Post.Break bk);
        Task<Models.Response.Default> SetPhoto(Models.Post.DeliveryPhoto deliveryPhoto);
        Task<Models.Response.Default> SetSignature(Models.Post.DeliverySignature deliverySignature);
        Task<Models.Response.Default> SetIncidence(Models.Post.Incidence incidence);
        Task<Models.Response.Incidences> GetCatIncidences();
        #endregion

    }
}
