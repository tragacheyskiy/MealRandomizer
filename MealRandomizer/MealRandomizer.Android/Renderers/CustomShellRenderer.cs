﻿using Android.Content;
using Android.Support.Design.Widget;
using MealRandomizer;
using MealRandomizer.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AppShell), typeof(CustomShellRenderer))]
namespace MealRandomizer.Droid.Renderers
{
    public class CustomShellRenderer : ShellRenderer
    {
        public CustomShellRenderer(Context context) : base(context) { }

        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            return new CustomBottomNavView(this, shellItem);
        }
    }

    public class CustomBottomNavView : ShellBottomNavViewAppearanceTracker
    {
        public CustomBottomNavView(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem)
        {
        }

        public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            base.SetAppearance(bottomView, appearance);
            bottomView.SetBackgroundColor(Android.Graphics.Color.Black);
        }
    }
}