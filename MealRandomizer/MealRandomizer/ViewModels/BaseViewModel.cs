using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MealRandomizer.ViewModels
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected Page MainPage => Application.Current.MainPage;

        protected void PushPageModal(Page page)
        {
            OccupyPage(() => MainPage.Navigation.PushModalAsync(page));
        }

        protected void PopPageModal()
        {
            OccupyPage(() => MainPage.Navigation.PopModalAsync());
        }

        protected void PushPage(Page page)
        {
            OccupyPage(() => MainPage.Navigation.PushAsync(page));
        }

        protected void PopPage()
        {
            OccupyPage(() => MainPage.Navigation.PopAsync());
        }

        private async void OccupyPage(Func<Task> navigationFunc)
        {
            if (!MainPage.IsBusy)
            {
                MainPage.IsBusy = true;
                await navigationFunc.Invoke();
                MainPage.IsBusy = false;
            }
        }

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T source, T value, [CallerMemberName] string propertyName = "")
        {
            source = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
