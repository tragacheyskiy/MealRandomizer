using MealRandomizer.ViewModels.ProductsViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealRandomizer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public sealed partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            productsTab.BindingContext = new CategoriesViewModel();
        }
    }
}