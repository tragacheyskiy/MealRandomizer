using Android.Content;
using MealRandomizer.Controls;
using MealRandomizer.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LinelessPicker), typeof(LinelessPickerRenderer))]
namespace MealRandomizer.Droid.Renderers
{
    internal class LinelessPickerRenderer : Xamarin.Forms.Platform.Android.AppCompat.PickerRenderer
    {
        public LinelessPickerRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = null;
            }
        }
    }
}