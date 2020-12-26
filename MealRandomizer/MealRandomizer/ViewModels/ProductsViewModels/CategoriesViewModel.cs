using MealRandomizer.Models;
using MealRandomizer.Views.ProductsViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealRandomizer.ViewModels.ProductsViewModels
{
    internal class CategoriesViewModel : BaseViewModel
    {
        public class CategoryIntelligence
        {
            public string CategoryName { get; set; }
            public object ProductCategory { get; set; }
            public ImageSource ImageSource { get; set; }
        }

        public Command SelectAllCategoriesCommand { get; }
        public Command SelectedCategoryCommand { get; }

        public CategoriesViewModel()
        {
            SelectAllCategoriesCommand = new Command<string>(async (categoryName) =>
            {
                var categoryIntelligence = new CategoryIntelligence()
                {
                    CategoryName = categoryName,
                    ProductCategory = ProductCategory.None
                };

                await PushPageAsync(new AllProductsPage() { BindingContext = new ProductsViewModel(categoryIntelligence) });
            }, categoryName => !MainPage.IsBusy);

            SelectedCategoryCommand = new Command<CategoryIntelligence>(async categoryIntelligence =>
            {
                await PushPageAsync(new ProductsPage() { BindingContext = new ProductsViewModel(categoryIntelligence) });
            }, categoryIntelligence => !MainPage.IsBusy);
        }
    }
}
