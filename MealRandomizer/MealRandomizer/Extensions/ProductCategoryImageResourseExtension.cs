using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealRandomizer.Extensions
{
    [ContentProperty(nameof(Source))]
    public class ProductCategoryImageResourseExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            string imagePath = $"MealRandomizer.ProductCategoriesPics.{Source}";
            return ImageSource.FromResource(imagePath, typeof(ProductCategoryImageResourseExtension).GetTypeInfo().Assembly);
        }
    }
}
