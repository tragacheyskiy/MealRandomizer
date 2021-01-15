using MealRandomizer.Data;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MealRandomizer.ViewModels.ProductsViewModels
{
    internal sealed class NewProductViewModel : BaseViewModel
    {
        private readonly Action productAddedAction;

        private ProductsRepository productsRepository;

        public ProductViewModel ProductViewModel { get; }

        public Command AddCommand { get; }

        public NewProductViewModel(ProductViewModel productViewModel, Action productAddedAction = null)
        {
            ProductViewModel = productViewModel;

            Task.Run(() =>
            {
                productsRepository = ProductsRepository.Instance;
            });

            AddCommand = new Command(async () =>
            {
                if (!MainPage.IsBusy && ProductViewModel.IsInputCorrect)
                {
                    MainPage.IsBusy = true;
                    await AddProductAsync();
                    MainPage.IsBusy = false;
                    await PopPageAsync();
                }
            });

            this.productAddedAction = productAddedAction;
        }

        private async Task AddProductAsync()
        {
            await productsRepository.AddAsync(ProductViewModel.GetNewProduct());

            productAddedAction?.Invoke();
        }
    }
}
