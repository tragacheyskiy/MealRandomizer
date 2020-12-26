using MealRandomizer.Data;
using MealRandomizer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealRandomizer.ViewModels.ProductsViewModels
{
    internal class ProductsViewModel : BaseViewModel
    {
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        private ProductsRepository productsRepository;
        private List<Product> productsSource;
        private bool isInitialized;

        public string Title { get; }
        public ImageSource ImageSource { get; }
        public Product SelectedProduct { get; set; }
        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();

        public Command BackButtonCommand { get; }
        public Command SelectProductCommand { get; }

        public ProductsViewModel(CategoriesViewModel.CategoryIntelligence categoryIntelligence)
        {
            Title = categoryIntelligence.CategoryName;
            ImageSource = categoryIntelligence.ImageSource;

            InitializeProductsSource((ProductCategory)categoryIntelligence.ProductCategory);

            BackButtonCommand = new Command(async () => await PopPageAsync(), () => !MainPage.IsBusy);
        }

        private void InitializeProductsSource(ProductCategory productCategory)
        {
            Task.Run(() =>
            {
                productsRepository = ProductsRepository.Instance;

                var allProducts = productsRepository.GetAllAsync().Result;

                if (productCategory == ProductCategory.None)
                {
                    productsSource = new List<Product>(allProducts);
                }
                else
                {
                    productsSource = new List<Product>(allProducts.Where(product => product.Category == productCategory));
                }

                RefreshProducts();

                isInitialized = true;
            });
        }

        private void RefreshProducts()
        {
            Products.Clear();

            foreach (Product product in productsSource)
            {
                Products.Add(product);
            }
        }
    }
}
