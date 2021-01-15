using Xamarin.Forms;

namespace MealRandomizer.Controls
{
    public sealed class BorderlessEntry : Entry
    {
        public static BindableProperty CornerRadiusProperty = 
            BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(BorderlessEntry), 0);

        public int CornerRadius { get => (int)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }
    }
}
