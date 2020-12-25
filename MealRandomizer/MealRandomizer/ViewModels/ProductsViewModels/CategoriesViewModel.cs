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
        public Command<CategoryIntelligence> SelectedCategoryCommand { get; }

        public CategoriesViewModel()
        {

            SelectedCategoryCommand = new Command<CategoryIntelligence>(async categoryIntelligence =>
            {
                Debug.WriteLine($"Category: {categoryIntelligence}");
                Debug.WriteLine($"Category name: {categoryIntelligence?.CategoryName}");
                Debug.WriteLine($"Category: {categoryIntelligence?.ProductCategory}");
                Debug.WriteLine($"Image source: {categoryIntelligence?.ImageSource}");

                MainPage.IsBusy = true;
                await MainPage.Navigation.PushAsync(new ProductsPage() { BindingContext = new ProductsViewModel(categoryIntelligence) });
                MainPage.IsBusy = false;

            }, categoryIntelligence => !MainPage.IsBusy);
        }
    }
}
