using PLI.Utilities;
using PLI.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PLI.ViewModels
{
    public class MasterViewModel : IAsyncInitialization
    {
        public Task Initialization { get; private set; }
        public string nameCurrentUser { get; set; }

        public MasterViewModel()
        {

        }

        public ICommand NavigationCommand
        {
            get
            {
                return new Command((value) =>
                {
                    // COMMENT: This is just quick demo code. Please don't put this in a production app.
                    var mdp = (Application.Current.MainPage as MasterDetailPage);
                    var navPage = mdp.Detail as NavigationPage;

                    // Hide the Master page
                    mdp.IsPresented = false;

                    switch (value)
                    {
                        case "1":
                            navPage.PushAsync(new HomePage());
                            break;
                        case "9":
                            App.Current.MainPage = new NavigationPage(new LoginPage());
                            break;
                    }

                });
            }
        }
    }
}
