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
    internal class ProductsViewModel
    {
        private readonly ProductsRepository productsRepository = ProductsRepository.Instance;
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        private Task categoryChangingTask;

        public string Title { get; }
        public ImageSource ImageSource { get; }
        public ICommand AddNewProductCommand { get; }
        public ICommand SelectCategoryCommand { get; }
        public ICommand SelectProductCommand { get; }
        public Product SelectedProduct { get; set; }
        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();

        public ProductsViewModel(CategoriesViewModel.CategoryIntelligence categoryIntelligence)
        {
            Title = categoryIntelligence.CategoryName;
            ImageSource = categoryIntelligence.ImageSource;

            SelectCategoryCommand = new Command<ProductCategory>(async category =>
            {
                if (categoryChangingTask != null)
                {
                    tokenSource.Cancel();
                    categoryChangingTask.Wait();
                }

                await Task.Factory.StartNew(cancellationToken =>
                {
                    var token = (CancellationToken)cancellationToken;

                    if (token.IsCancellationRequested)
                    {
                        return;
                    }

                    IEnumerable<Product> products = productsRepository.GetAllAsync().Result;

                    if (category != ProductCategory.None)
                    {
                        products = products.Where(product => product.Category == category);
                    }

                    RefreshProducts(products, token);
                }, tokenSource.Token, tokenSource.Token);

                categoryChangingTask = null;
            });
        }

        private void RefreshProducts(IEnumerable<Product> productsSource, CancellationToken token)
        {
            Products.Clear();

            foreach (Product product in productsSource)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                Products.Add(product);
            }
        }
    }
}
