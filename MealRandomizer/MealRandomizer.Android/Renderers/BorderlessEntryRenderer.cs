using Android.Content;
using Android.Graphics.Drawables;
using MealRandomizer.Controls;
using MealRandomizer.Droid.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace MealRandomizer.Droid.Renderers
{
    internal class BorderlessEntryRenderer : EntryRenderer
    {
        public BorderlessEntryRenderer(Context context) : base(context) { }

        public BorderlessEntry ElementV2 => Element as BorderlessEntry;

        protected override FormsEditText CreateNativeControl()
        {
            var control = base.CreateNativeControl();
            UpdateBackground(control);
            return control;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == BorderlessEntry.CornerRadiusProperty.PropertyName)
            {
                UpdateBackground();
            }

            base.OnElementPropertyChanged(sender, e);
        }

        protected override void UpdateBackgroundColor()
        {
            UpdateBackground();
        }

        protected void UpdateBackground(FormsEditText control)
        {
            if (control == null)
            {
                return;
            }

            var gd = new GradientDrawable();
            gd.SetColor(Element.BackgroundColor.ToAndroid());
            gd.SetCornerRadius(Context.ToPixels(ElementV2.CornerRadius));
            control.SetBackground(gd);
        }

        protected override void UpdateBackground()
        {
            UpdateBackground(Control);
        }
    }
}