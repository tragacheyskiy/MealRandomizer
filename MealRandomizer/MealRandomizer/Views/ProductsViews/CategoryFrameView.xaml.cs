using MealRandomizer.ViewModels.ProductsViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealRandomizer.Views.ProductsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryFrameView : ContentView
    {
        public static readonly BindableProperty TextProperty = 
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CategoryFrameView), string.Empty, propertyChanged: UpdateCategoryIntelligence);

        public static readonly BindableProperty CommandProperty = 
            BindableProperty.Create(nameof(Command), typeof(Command), typeof(CategoryFrameView));

        public static readonly BindableProperty CategoryProperty = 
            BindableProperty.Create(nameof(Category), typeof(object), typeof(CategoryFrameView), propertyChanged: UpdateCategoryIntelligence);

        public static readonly BindableProperty ImageSourceProperty = 
            BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(CategoryFrameView), propertyChanged: UpdateCategoryIntelligence);

        public static readonly BindableProperty CategoryIntelligenceProperty = 
            BindableProperty.Create(nameof(CategoryIntelligence), typeof(object), typeof(CategoryFrameView));

        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
        public Command Command { get => (Command)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }
        public object Category { get => GetValue(CategoryProperty); set => SetValue(CategoryProperty, value); }
        public ImageSource ImageSource { get => (ImageSource)GetValue(ImageSourceProperty); set => SetValue(ImageSourceProperty, value); }
        public object CategoryIntelligence { get => GetValue(CategoryIntelligenceProperty); set => SetValue(CategoryIntelligenceProperty, value); }

        public CategoryFrameView()
        {
            InitializeComponent();
            CategoryIntelligence = new CategoriesViewModel.CategoryIntelligence();
        }

        private static void UpdateCategoryIntelligence(BindableObject bindableObject, object oldValue, object newValue)
        {
            var view = (CategoryFrameView)bindableObject;

            if (view.CategoryIntelligence is CategoriesViewModel.CategoryIntelligence categoryIntelligence)
            {
                categoryIntelligence.CategoryName = view.Text;
                categoryIntelligence.ProductCategory = view.Category;
                categoryIntelligence.ImageSource = view.ImageSource;
            }
        }
    }
}