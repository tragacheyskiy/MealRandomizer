using MealRandomizer.Models;
using MealRandomizer.Views.ProductsViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace MealRandomizer.ViewModels.ProductsViewModels
{
    internal sealed class CategoriesViewModel : BaseViewModel
    {
        public class LocalizedCategoryWithImage
        {
            public LocalizedCategory LocalizedCategory { get; }
            public Color ImageBackgroundColor { get; }
            public Color TextBackgroundColor { get; }
            public ImageSource CategoryImage { get; }
            public string CategoryName => LocalizedCategory.CategoryName;

            public LocalizedCategoryWithImage(LocalizedCategory localizedCategory, ImageSource categroyImage, Color imageBackgroundColor, Color textBackgroundColor)
            {
                LocalizedCategory = localizedCategory;
                ImageBackgroundColor = imageBackgroundColor;
                TextBackgroundColor = textBackgroundColor;
                CategoryImage = categroyImage;
            }
        }

        private const string CategoriesPicsDirectory = "MealRandomizer.ProductCategoriesPics";

        public static IList<LocalizedCategory> LocalizedCategories { get; } = GetLocalizedCategories();

        private LocalizedCategoryWithImage selectedCategory;

        public IEnumerable<LocalizedCategoryWithImage> Categories { get; }
        public LocalizedCategoryWithImage SelectedCategory { get => selectedCategory; set => SetProperty(ref selectedCategory, value); }

        public Command AddButtonCommand { get; }
        public Command SelectAllCategoriesCommand { get; }
        public Command SelectCategoryCommand { get; }

        public CategoriesViewModel()
        {
            Categories = GetCategories();

            AddButtonCommand = new Command(() =>
            {
                if (!MainPage.IsBusy)
                {
                    var viewModel = new NewProductViewModel(new ProductViewModel());

                    PushPage(new NewProductPage() { BindingContext = viewModel });
                }
            });

            SelectCategoryCommand = new Command(() =>
            {
                if (MainPage.IsBusy && SelectedCategory == null)
                {
                    return;
                }

                var viewModel = new ProductsViewModel(SelectedCategory.LocalizedCategory, SelectedCategory.CategoryImage);

                if (SelectedCategory.LocalizedCategory.Category == ProductCategory.None)
                {
                    PushPage(new AllProductsPage() { BindingContext = viewModel });
                }
                else
                {
                    PushPage(new ProductsPage() { BindingContext = viewModel });
                }

                SelectedCategory = null;
            });
        }

        private List<LocalizedCategoryWithImage> GetCategories()
        {
            var categories = new List<LocalizedCategoryWithImage>
            {
                new LocalizedCategoryWithImage(new LocalizedCategory(ProductCategory.None), null, (Color)Application.Current.Resources["DiamondGreen"], Color.Transparent)
            };

            Color halfOpacityWhite = Color.FromHex("#50000000");

            foreach (LocalizedCategory localizedCategory in GetLocalizedCategories())
            {
                var imageSource = ImageSource.FromResource($"{CategoriesPicsDirectory}.{localizedCategory.EnumName.ToLowerInvariant()}.jpg");
                categories.Add(new LocalizedCategoryWithImage(localizedCategory, imageSource, Color.Transparent, halfOpacityWhite));
            }

            return categories;
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
