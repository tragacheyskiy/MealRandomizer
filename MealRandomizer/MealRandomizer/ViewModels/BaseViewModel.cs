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

        protected Task PushPageModalAsync(Page page)
        {
            return OccupyPage(() => MainPage.Navigation.PushModalAsync(page));
        }

        protected Task PopPageModalAsync()
        {
            return OccupyPage(() => MainPage.Navigation.PopModalAsync());
        }

        protected Task PushPageAsync(Page page)
        {
            return OccupyPage(() => MainPage.Navigation.PushAsync(page));
        }

        protected Task PopPageAsync()
        {
            return OccupyPage(() => MainPage.Navigation.PopAsync());
        }

        private async Task OccupyPage(Func<Task> navigationFunc)
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
