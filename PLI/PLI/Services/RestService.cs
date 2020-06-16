using Newtonsoft.Json;
using PLI.Utilities;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PLI.Services
{
    public class RestService : IRestService
    {
        HttpClient client;

        public RestService()
        {
            client = new HttpClient();
        }

        public void refreshToken() {
            client.DefaultRequestHeaders
                    .Authorization = new AuthenticationHeaderValue("Bearer", Preferences.Get("token", string.Empty));
            client.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region "user"

        public async Task<Models.Response.Login> LoginAsync(Models.Post.Login login)
        {
            var uri = new Uri(Constants.urlApi + "auth/login");

            Models.Response.Login loginResponse = new Models.Response.Login();

            if (App.CurrentConetion())
            {
                try
                {
                    var dataPost = JsonConvert.SerializeObject(login);
                    var content = new StringContent(dataPost, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;
                    response = await client.PostAsync(uri, content);

                    var request = await response.Content.ReadAsStringAsync();
                    loginResponse = JsonConvert.DeserializeObject<Models.Response.Login>(request);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                }
            }

            return loginResponse;

        }

        public async Task<Models.Response.Login> ChangePassword(Models.Post.ChangePassword change)
        {
            var uri = new Uri(Constants.urlApi + "auth/password");

            Models.Response.Login loginResponse = new Models.Response.Login();

            if (App.CurrentConetion())
            {
                try
                {
                    var dataPost = JsonConvert.SerializeObject(change);
                    var content = new StringContent(dataPost, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;
                    response = await client.PostAsync(uri, content);

                    var request = await response.Content.ReadAsStringAsync();
                    loginResponse = JsonConvert.DeserializeObject<Models.Response.Login>(request);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                }
            }

            return loginResponse;
        }

        #endregion

        public async Task<Models.Response.ChangeStatus> ChangeStatusShipment(Models.Post.ChangeStatusUser changeStatusUser)
        {
            var uri = new Uri(Constants.urlApi + "shipments/status");

            Models.Response.ChangeStatus responseDefault = new Models.Response.ChangeStatus();

            if (App.CurrentConetion())
            {
                try
                {
                    var dataPost = JsonConvert.SerializeObject(changeStatusUser);
                    var content = new StringContent(dataPost, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;
                    response = await client.PostAsync(uri, content);

                    var request = await response.Content.ReadAsStringAsync();
                    responseDefault = JsonConvert.DeserializeObject<Models.Response.ChangeStatus>(request);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                }
            }

            return responseDefault;
        }

        public async Task<Models.Response.Shipments> GetShipments()
        {
            var uri = new Uri(Constants.urlApi + "shipments");

            Models.Response.Shipments responseShipments = new Models.Response.Shipments();

            if (App.CurrentConetion())
            {
                try
                {
                    HttpResponseMessage response = null;
                    response = await client.GetAsync(uri);

                    var request = await response.Content.ReadAsStringAsync();
                    responseShipments = JsonConvert.DeserializeObject<Models.Response.Shipments>(request);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                }
            }

            return responseShipments;
        }

        public async Task<Models.Response.Default> SetLocation(Models.Post.Location location)
        {
            var uri = new Uri(Constants.urlApi + "user/tracking");

            Models.Response.Default responseDefault = new Models.Response.Default();

            if (App.CurrentConetion())
            {
                try
                {
                    var dataPost = JsonConvert.SerializeObject(location);
                    var content = new StringContent(dataPost, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;
                    response = await client.PostAsync(uri, content);

                    var request = await response.Content.ReadAsStringAsync();
                    responseDefault = JsonConvert.DeserializeObject<Models.Response.Default>(request);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                }
            }

            return responseDefault;
        }

        public async Task<Models.Response.UserStatus> GetUserStatus()
        {
            var uri = new Uri(Constants.urlApi + "user/status");

            Models.Response.UserStatus userStatus = new Models.Response.UserStatus();

            if (App.CurrentConetion())
            {
                try
                {
                    HttpResponseMessage response = null;
                    response = await client.GetAsync(uri);

                    var request = await response.Content.ReadAsStringAsync();
                    userStatus = JsonConvert.DeserializeObject<Models.Response.UserStatus>(request);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                }
            }

            return userStatus;
        }

        public async Task<Models.Response.Default> SetUserTender()
        {
            var uri = new Uri(Constants.urlApi + "user/tender");

            Models.Response.Default responseDefault = new Models.Response.Default();

            if (App.CurrentConetion())
            {
                try
                {
                    HttpResponseMessage response = null;
                    response = await client.PostAsync(uri, null);

                    var request = await response.Content.ReadAsStringAsync();
                    responseDefault = JsonConvert.DeserializeObject<Models.Response.Default>(request);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                }
            }

            return responseDefault;
        }

        public async Task<Models.Response.Default> SetPhoto(Models.Post.DeliveryPhoto deliveryPhoto)
        {
            var uri = new Uri(Constants.urlApi + "shipments/delivery");

            Models.Response.Default responseDefault = new Models.Response.Default();

            if (App.CurrentConetion())
            {
                try
                {
                    var dataPost = JsonConvert.SerializeObject(deliveryPhoto);
                    var content = new StringContent(dataPost, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;
                    response = await client.PostAsync(uri, content);

                    var request = await response.Content.ReadAsStringAsync();
                    responseDefault = JsonConvert.DeserializeObject<Models.Response.Default>(request);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                }
            }

            return responseDefault;
        }

        public async Task<Models.Response.Default> SetSignature(Models.Post.DeliverySignature deliverySignature)
        {
            var uri = new Uri(Constants.urlApi + "shipments/signature");

            Models.Response.Default responseDefault = new Models.Response.Default();

            if (App.CurrentConetion())
            {
                try
                {
                    var dataPost = JsonConvert.SerializeObject(deliverySignature);
                    var content = new StringContent(dataPost, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;
                    response = await client.PostAsync(uri, content);

                    var request = await response.Content.ReadAsStringAsync();
                    responseDefault = JsonConvert.DeserializeObject<Models.Response.Default>(request);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                }
            }

            return responseDefault;
        }

        public async Task<Models.Response.Default> SetUserReturn()
        {
            var uri = new Uri(Constants.urlApi + "user/return");

            Models.Response.Default responseDefault = new Models.Response.Default();

            if (App.CurrentConetion())
            {
                try
                {
                    HttpResponseMessage response = null;
                    response = await client.PostAsync(uri, null);

                    var request = await response.Content.ReadAsStringAsync();
                    responseDefault = JsonConvert.DeserializeObject<Models.Response.Default>(request);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                }
            }

            return responseDefault;
        }

        public async Task<Models.Response.Default> SetUserExit()
        {
            var uri = new Uri(Constants.urlApi + "user/exit");

            Models.Response.Default responseDefault = new Models.Response.Default();

            if (App.CurrentConetion())
            {
                try
                {
                    HttpResponseMessage response = null;
                    response = await client.PostAsync(uri, null);

                    var request = await response.Content.ReadAsStringAsync();
                    responseDefault = JsonConvert.DeserializeObject<Models.Response.Default>(request);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                }
            }

            return responseDefault;
        }

        public async Task<Models.Response.Default> SetUserBreak(Models.Post.Break bk)
        {
            var uri = new Uri(Constants.urlApi + "user/break");

            Models.Response.Default responseDefault = new Models.Response.Default();

            if (App.CurrentConetion())
            {
                try
                {
                    var dataPost = JsonConvert.SerializeObject(bk);
                    var content = new StringContent(dataPost, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;
                    response = await client.PostAsync(uri, content);

                    var request = await response.Content.ReadAsStringAsync();
                    responseDefault = JsonConvert.DeserializeObject<Models.Response.Default>(request);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                }
            }

            return responseDefault;
        }

        public async Task<Models.Response.Default> SetUserBreakEnd(Models.Post.Break bk)
        {
            var uri = new Uri(Constants.urlApi + "user/breakend");

            Models.Response.Default responseDefault = new Models.Response.Default();

            if (App.CurrentConetion())
            {
                try
                {
                    var dataPost = JsonConvert.SerializeObject(bk);
                    var content = new StringContent(dataPost, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;
                    response = await client.PostAsync(uri, content);

                    var request = await response.Content.ReadAsStringAsync();
                    responseDefault = JsonConvert.DeserializeObject<Models.Response.Default>(request);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                }
            }

            return responseDefault;
        }

        public async Task<Models.Response.Default> SetIncidence(Models.Post.Incidence incidence)
        {
            var uri = new Uri(Constants.urlApi + "incidences");

            Models.Response.Default responseDefault = new Models.Response.Default();

            if (App.CurrentConetion())
            {
                try
                {
                    var dataPost = JsonConvert.SerializeObject(incidence);
                    var content = new StringContent(dataPost, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;
                    response = await client.PostAsync(uri, content);

                    var request = await response.Content.ReadAsStringAsync();
                    responseDefault = JsonConvert.DeserializeObject<Models.Response.Default>(request);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                }
            }

            return responseDefault;
        }

        public async Task<Models.Response.Incidences> GetCatIncidences()
        {
            var uri = new Uri(Constants.urlApi + "incidences");

            Models.Response.Incidences incidences = new Models.Response.Incidences();

            if (App.CurrentConetion())
            {
                try
                {
                    HttpResponseMessage response = null;
                    response = await client.GetAsync(uri);

                    var request = await response.Content.ReadAsStringAsync();
                    incidences = JsonConvert.DeserializeObject<Models.Response.Incidences>(request);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                }
            }

            return incidences;
        }
    }
}
