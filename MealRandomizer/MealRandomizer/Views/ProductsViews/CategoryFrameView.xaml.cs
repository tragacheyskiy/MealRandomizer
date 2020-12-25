using MealRandomizer.Models;
using System.Diagnostics;
using System.Windows.Input;
using MealRandomizer.ViewModels.ProductsViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace MealRandomizer.Views.ProductsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryFrameView : ContentView
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            "Text",
            typeof(string),
            typeof(CategoryFrameView),
            string.Empty,
            propertyChanged: UpdateCategoryIntelligence);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            "Command",
            typeof(Command),
            typeof(CategoryFrameView));
        public static readonly BindableProperty CategoryProperty = BindableProperty.Create(
            "Category",
            typeof(object),
            typeof(CategoryFrameView),
            propertyChanged: UpdateCategoryIntelligence);
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
            "ImageSource",
            typeof(ImageSource),
            typeof(CategoryFrameView),
            propertyChanged: UpdateCategoryIntelligence);
        public static readonly BindableProperty CategoryIntelligenceProperty = BindableProperty.Create(
            "CategoryIntelligence",
            typeof(object),
            typeof(CategoryFrameView));

        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
        public Command Command { get => (Command)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }
        public object Category { get => GetValue(CategoryProperty); set => SetValue(CategoryProperty, value); }
        public ImageSource ImageSource { get => (ImageSource)GetValue(ImageSourceProperty); set => SetValue(ImageSourceProperty, value); }
        public object CategoryIntelligence { get => GetValue(CategoryIntelligenceProperty); set => SetValue(CategoryIntelligenceProperty, value); }

        public CategoryFrameView()
        {
            InitializeComponent();
        }

        private static void UpdateCategoryIntelligence(BindableObject bindableObject, object oldValue, object newValue)
        {
            var view = (CategoryFrameView)bindableObject;

            view.CategoryIntelligence = new CategoriesViewModel.CategoryIntelligence()
            {
                CategoryName = view.Text,
                ProductCategory = view.Category,
                ImageSource = view.ImageSource
            };
        }
    }
}