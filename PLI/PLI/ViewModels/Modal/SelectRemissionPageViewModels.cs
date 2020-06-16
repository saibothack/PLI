using PLI.Utilities;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PLI.ViewModels.Modal
{
    public class SelectRemissionPageViewModels : ViewModelBase
    {
        #region "Properties"
        public INavigation Navigation { get; internal set; }
        public IAsyncCommand CommandSelectedRemissions { get; set; }

        private ObservableCollection<Models.Remission> _SourceRemission;
        public ObservableCollection<Models.Remission> SourceRemission
        {
            get { return _SourceRemission; }
            set
            {
                SetProperty(ref _SourceRemission, value);
            }
        }

        private Models.Remission _SelectedRemission;
        public Models.Remission SelectedRemission
        {
            get { return _SelectedRemission; }
            set
            {
                SetProperty(ref _SelectedRemission, value);
            }
        }

        #endregion
        public SelectRemissionPageViewModels(List<Models.Remission> remissions) {
            CommandSelectedRemissions = new AsyncCommand(SelectedRemissionsAsync, CanExecuteSubmit);

            SourceRemission = new ObservableCollection<Models.Remission>();

            foreach(var remission in remissions)
            {
                remission.IsEnabled = true;
                SourceRemission.Add(remission);
            }

        }

        private async Task SelectedRemissionsAsync()
        {
            try
            {
                if (validate())
                {
                    IsBusy = true;
                    await PopupNavigation.PopAllAsync();
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

        private bool validate()
        {
            bool success = true;

            if ((from x in SourceRemission
                           where (x.IsEnabled == true && x.IsChecked == true)
                           select x).Count() < 1)
                success = false;

            return success;
        }
    }
}
