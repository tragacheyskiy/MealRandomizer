using MealRandomizer.Data;
using MealRandomizer.Resources;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MealRandomizer.ViewModels.ProductsViewModels
{
    internal sealed class ProductDetailViewModel : BaseViewModel
    {
        private readonly Action productChangedAction;

        private ProductsRepository productsRepository;

        public string ProductName { get; }
        public ProductViewModel ProductViewModel { get; }

        public Command DeleteCommand { get; }
        public Command UpdateCommand { get; }

        public ProductDetailViewModel(ProductViewModel productViewModel, Action productChangedAction = null)
        {
            ProductViewModel = productViewModel;

            ProductName = productViewModel.Name.Value;

            productsRepository = ProductsRepository.Instance;

            this.productChangedAction = productChangedAction;

            DeleteCommand = new Command(async () =>
            {
                if (MainPage.IsBusy)
                {
                    return;
                }

                bool isDelete = await MainPage.DisplayAlert("", $"{Confirmations.ProductDeletion} \"{ProductName}\"?", $"{Confirmations.Yes}", $"{Confirmations.Cancel}");

                if (isDelete)
                {
                    await DeleteProductAsync();
                    await PopPageAsync();
                }
            });

            UpdateCommand = new Command(async () =>
            {
                if (!MainPage.IsBusy && ProductViewModel.IsInputCorrect)
                {
                    await UpdateProductAsync();
                    await PopPageAsync();
                }
            });
        }

        private async Task DeleteProductAsync()
        {
            MainPage.IsBusy = true;

            await productsRepository.DeleteAsync(ProductViewModel.CurrentProduct);
            productChangedAction?.Invoke();

            MainPage.IsBusy = false;
        }

        private async Task UpdateProductAsync()
        {
            MainPage.IsBusy = true;

            await productsRepository.UpdateAsync(ProductViewModel.GetNewProduct());
            productChangedAction?.Invoke();

            MainPage.IsBusy = false;
        }
    }
}
