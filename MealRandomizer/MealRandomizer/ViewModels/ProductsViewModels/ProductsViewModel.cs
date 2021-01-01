using MealRandomizer.Data;
using MealRandomizer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MealRandomizer.ViewModels.ProductsViewModels
{
    internal class ProductsViewModel : BaseViewModel
    {
        private const int ProductsPerLoad = 25;

        private readonly object locker = new object();

        private bool isSearchEnabled;
        private bool isInitializing = true;
        private bool isLoading;
        private ProductsRepository productsRepository;
        private List<Product> productsSource;
        private IEnumerator<Product> currentProductsEnumerator;

        public string Title { get; }
        public ImageSource ImageSource { get; }
        public bool IsSearchEnabled { get => isSearchEnabled; set => SetProperty(ref isSearchEnabled, value); }
        public bool IsInitializing { get => isInitializing; set => SetProperty(ref isInitializing, value); }
        public Product SelectedProduct { get; set; }
        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();

        public Command SearchCommand { get; }
        public Command BackButtonCommand { get; }
        public Command SelectProductCommand { get; }
        public Command LoadMoreProductsCommand { get; }

        public ProductsViewModel(CategoriesViewModel.CategoryIntelligence categoryIntelligence)
        {
            Title = categoryIntelligence.CategoryName;
            ImageSource = categoryIntelligence.ImageSource;

            InitializeProductsSource((ProductCategory)categoryIntelligence.ProductCategory);

            SearchCommand = new Command<string>(soughtProductName => Search(soughtProductName), soughtProductName => IsSearchEnabled);

            BackButtonCommand = new Command(async () => await PopPageAsync(), () => !MainPage.IsBusy);

            LoadMoreProductsCommand = new Command(() => Task.Run(LoadMoreProducts), () => !isLoading);
        }

        private void InitializeProductsSource(ProductCategory productCategory)
        {
            Task.Run(() =>
            {
                productsRepository = ProductsRepository.Instance;

                var allProducts = productsRepository.GetAllAsync().Result;

                if (productCategory != ProductCategory.None)
                {
                    productsSource = new List<Product>(allProducts.Where(product => product.Category == productCategory));
                }
                else
                {
                    productsSource = new List<Product>(allProducts);
                }

                RefreshProducts(productsSource);

                IsSearchEnabled = true;
                IsInitializing = false;
            });
        }

        private void Search(string soughtProductName)
        {
            if (soughtProductName == null)
            {
                return;
            }

            FindAndRefresh(soughtProductName);
        }

        private void FindAndRefresh(string soughtProductName)
        {
            if (soughtProductName == string.Empty)
            {
                Task.Run(() => RefreshProducts(productsSource));
            }
            else
            {
                Task.Run(() =>
                {
                    var soughtProducts = productsSource.Where(product => product.Name.Contains(soughtProductName));

                    RefreshProducts(soughtProducts);
                });
            }
        }

        private void RefreshProducts(IEnumerable<Product> source)
        {
            lock (locker)
            {
                Products.Clear();
                currentProductsEnumerator = source.GetEnumerator();
            }

            LoadMoreProducts();
        }

        private void LoadMoreProducts()
        {
            lock (locker)
            {
                isLoading = true;

                int counter = 0;

                while (currentProductsEnumerator.MoveNext())
                {
                    if (counter == ProductsPerLoad)
                    {
                        break;
                    }

                    Products.Add(currentProductsEnumerator.Current);
                    counter++;
                }

                isLoading = false;
            }
        }
    }
}
