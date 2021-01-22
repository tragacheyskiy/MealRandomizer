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

            Task.Run(() => productsRepository = ProductsRepository.Instance);

            AddCommand = new Command(async () =>
            {
                if (!MainPage.IsBusy && ProductViewModel.IsInputCorrect)
                {
                    await AddProductAsync();

                    PopPage();
                }
            });

            this.productAddedAction = productAddedAction;
        }

        private async Task AddProductAsync()
        {
            MainPage.IsBusy = true;

            await productsRepository.AddAsync(ProductViewModel.GetNewProduct());
            productAddedAction?.Invoke();

            MainPage.IsBusy = false;
        }
    }
}
