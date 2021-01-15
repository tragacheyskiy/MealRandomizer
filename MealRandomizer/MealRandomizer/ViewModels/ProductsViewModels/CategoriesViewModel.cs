using MealRandomizer.Models;
using MealRandomizer.Views.ProductsViews;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MealRandomizer.ViewModels.ProductsViewModels
{
    internal sealed class CategoriesViewModel : BaseViewModel
    {
        public static IList<LocalizedCategory> LocalizedCategories { get; } = GetLocalizedCategories();

        public class CategoryIntelligence
        {
            public string CategoryName { get; set; }
            public object ProductCategory { get; set; }
            public ImageSource ImageSource { get; set; }
        }

        public Command AddButtonCommand { get; }
        public Command SelectAllCategoriesCommand { get; }
        public Command SelectedCategoryCommand { get; }

        public CategoriesViewModel()
        {
            AddButtonCommand = new Command(async () =>
            {
                if (!MainPage.IsBusy)
                {
                    await PushPageAsync(new NewProductPage() { BindingContext = new NewProductViewModel(new ProductViewModel()) });
                }
            });

            SelectAllCategoriesCommand = new Command<string>(async (categoryName) =>
            {
                if (!MainPage.IsBusy)
                {
                    var categoryIntelligence = new CategoryIntelligence()
                    {
                        CategoryName = categoryName,
                        ProductCategory = ProductCategory.None
                    };

                    await PushPageAsync(new AllProductsPage() { BindingContext = new ProductsViewModel() });
                }
            });

            SelectedCategoryCommand = new Command<CategoryIntelligence>(async categoryIntelligence =>
            {
                if (!MainPage.IsBusy)
                {
                    await PushPageAsync(new ProductsPage()
                    {
                        BindingContext =
                        new ProductsViewModel(LocalizedCategories.FirstOrDefault(localizedCategory => localizedCategory.Category == (ProductCategory)categoryIntelligence.ProductCategory), categoryIntelligence.ImageSource)
                    });
                }
            });
        }

        private static List<LocalizedCategory> GetLocalizedCategories()
        {
            var categories = new List<LocalizedCategory>();

            for (int i = 1; i <= (int)ProductCategory.Vegetable; i++)
            {
                categories.Add(new LocalizedCategory((ProductCategory)i));
            }

            categories.Sort();

            return categories;
        }
    }
}
