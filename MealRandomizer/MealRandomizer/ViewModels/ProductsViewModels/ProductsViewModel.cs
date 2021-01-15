using MealRandomizer.Data;
using MealRandomizer.Models;
using MealRandomizer.Views.ProductsViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MealRandomizer.ViewModels.ProductsViewModels
{
    internal sealed class ProductsViewModel : BaseViewModel
    {
        private const int ProductsPerLoad = 25;

        private readonly object locker = new object();
        private readonly LocalizedCategory localizedCategory;

        private Product selectedProduct;
        private bool isSearchEnabled;
        private bool isInitializing = true;
        private bool isLoading;
        private ProductsRepository productsRepository;
        private List<Product> productsSource;
        private IEnumerator<Product> currentProductsEnumerator;

        public string CategoryName { get; }
        public ImageSource CategoryImage { get; }
        public bool IsSearchEnabled { get => isSearchEnabled; set => SetProperty(ref isSearchEnabled, value); }
        public bool IsInitializing { get => isInitializing; set => SetProperty(ref isInitializing, value); }
        public Product SelectedProduct { get => selectedProduct; set => SetProperty(ref selectedProduct, value); }
        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();

        public Command AddCommand { get; }
        public Command BackCommand { get; }
        public Command SearchCommand { get; }
        public Command SelectProductCommand { get; }
        public Command LoadMoreProductsCommand { get; }

        public ProductsViewModel(LocalizedCategory localizedCategory = null, ImageSource categoryImage = null)
        {
            this.localizedCategory = localizedCategory;

            CategoryName = GetCategoryName();
            CategoryImage = categoryImage;

            InitializeProductsSource();

            AddCommand = new Command(async () =>
            {
                if (!MainPage.IsBusy)
                {
                    await PushPageAsync(new NewProductPage() { BindingContext = GetNewProductViewModel() });
                }
            });

            BackCommand = new Command(async () =>
            {
                if (!MainPage.IsBusy)
                {
                    await PopPageAsync();
                }
            });

            SearchCommand = new Command<string>(soughtProductName =>
            {
                if (isSearchEnabled)
                {
                    Search(soughtProductName);
                }
            });

            SelectProductCommand = new Command(async () =>
            {
                if (!MainPage.IsBusy && SelectedProduct != null)
                {
                    var productViewModel = new ProductViewModel(product: SelectedProduct);

                    await PushPageAsync(new ProductDetaiPage() { BindingContext = new ProductDetailViewModel(productViewModel, RefreshproductsSourceAfterChanging) });

                    SelectedProduct = null;
                }
            });

            LoadMoreProductsCommand = new Command(() =>
            {
                if (!isLoading)
                {
                    Task.Run(LoadMoreProducts);
                }
            });
        }

        private void RefreshproductsSourceAfterChanging()
        {
            Task.Run(SetProductsSourceAndRefresh);
        }

        private string GetCategoryName()
        {
            return localizedCategory == null ? Resources.Categories.None : localizedCategory.CategoryName;
        }

        private NewProductViewModel GetNewProductViewModel()
        {
            if (localizedCategory == null)
            {
                return new NewProductViewModel(new ProductViewModel(), RefreshproductsSourceAfterChanging);
            }
            else
            {
                return new NewProductViewModel(new ProductViewModel(localizedCategory.Category), RefreshproductsSourceAfterChanging);
            }
        }

        private List<Product> GetProductsSource()
        {
            var allProducts = productsRepository.GetAllAsync().Result;

            if (localizedCategory == null)
            {
                return new List<Product>(allProducts);
            }
            else
            {
                return new List<Product>(allProducts.Where(product => product.Category == localizedCategory.Category));
            }
        }

        private void SetProductsSourceAndRefresh()
        {
            productsSource = GetProductsSource();

            RefreshProducts(productsSource);
        }

        private void InitializeProductsSource()
        {
            Task.Run(() =>
            {
                productsRepository = ProductsRepository.Instance;

                SetProductsSourceAndRefresh();

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
                    var soughtProducts = productsSource.Where(product => product.Name.Contains(soughtProductName, StringComparison.OrdinalIgnoreCase));

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
