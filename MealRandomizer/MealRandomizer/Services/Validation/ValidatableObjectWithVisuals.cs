using System.ComponentModel;
using Xamarin.Forms;

namespace MealRandomizer.Services.Validation
{
    internal class ValidatableObjectWithVisuals<T> : ValidatableObject<T>, INotifyPropertyChanged
    {
        private static readonly Color colorDefault = (Color)Application.Current.Resources["Primary"];
        private static readonly Color colorAccent = (Color)Application.Current.Resources["DiamondGreen"];
        private static readonly Color colorError = (Color)Application.Current.Resources["LightRed"];

        private bool isFocused;

        public Color BorderColor { get; set; }
        public bool IsFocused { get => isFocused; set => OnFocuseChanged(value); }

        public void OnError()
        {
            BorderColor = colorError;
            OnPropertyChanged(nameof(ValidationMessage));
            OnPropertyChanged(nameof(BorderColor));
        }

        private void OnFocuseChanged(bool value)
        {
            isFocused = value;

            if (value)
            {
                ValidationMessage = string.Empty;
                BorderColor = colorAccent;
                OnPropertyChanged(nameof(ValidationMessage));
                OnPropertyChanged(nameof(BorderColor));
            }
            else
            {
                BorderColor = colorDefault;
                OnPropertyChanged(nameof(BorderColor));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
