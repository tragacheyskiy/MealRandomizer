using MealRandomizer.Models;
using MealRandomizer.Services.Validation;
using System.Collections.Generic;
using System.Linq;

namespace MealRandomizer.ViewModels.ProductsViewModels
{
    internal class ProductViewModel
    {
        public IList<LocalizedCategory> LocalizedCategories { get; } = CategoriesViewModel.LocalizedCategories;
        public ValidatableLocalizedCategoryWithVisuals LocalizedCategory { get; } = new ValidatableLocalizedCategoryWithVisuals();
        public ValidatableStringWithVisuals Name { get; } = new ValidatableStringWithVisuals();
        public ValidatableStringWithVisuals Proteins { get; } = new ValidatableStringWithVisuals();
        public ValidatableStringWithVisuals Fats { get; } = new ValidatableStringWithVisuals();
        public ValidatableStringWithVisuals Carbohydrates { get; } = new ValidatableStringWithVisuals();
        public ValidatableStringWithVisuals Calories { get; } = new ValidatableStringWithVisuals();

        public bool IsInputCorrect => ValidateObject(LocalizedCategory)
                                    & ValidateObject(Name)
                                    & ValidateObject(Proteins)
                                    & ValidateObject(Fats)
                                    & ValidateObject(Carbohydrates)
                                    & ValidateObject(Calories);

        public ProductViewModel(ProductCategory currentCategory = ProductCategory.None, Product product = null)
        {
            SetCurrentCategory(currentCategory);

            SetValues(product);

            AddValidationRules();
        }

        public Product GetNewProduct()
        {
            return new Product()
            {
                Category = LocalizedCategory.Value.Category,
                Name = Name.Value.Trim(),
                Proteins = ParseValue(Proteins.Value),
                Fats = ParseValue(Fats.Value),
                Carbohydrates = ParseValue(Carbohydrates.Value),
                Calories = ParseValue(Calories.Value)
            };
        }

        private void SetValues(Product product)
        {
            if (product != null)
            {
                Name.Value = product.Name;
                Proteins.Value = $"{product.Proteins}";
                Fats.Value = $"{product.Fats}";
                Carbohydrates.Value = $"{product.Carbohydrates}";
                Calories.Value = $"{product.Calories}";
            }
        }

        private void AddValidationRules()
        {
            var isNotNullOrWhiteSpaceRule = new IsNotNullOrWhiteSpaceRule("Поле обязательно для заполнения");
            var isFloatNumberRule = new IsFloatNumberRule("Некорректное значение");

            LocalizedCategory.AddValidationRule(new IsNotNullRule<LocalizedCategory>("Выберите категорию"));

            Name.AddValidationRule(isNotNullOrWhiteSpaceRule);

            Proteins.AddValidationRule(isNotNullOrWhiteSpaceRule);
            Proteins.AddValidationRule(isFloatNumberRule);

            Fats.AddValidationRule(isNotNullOrWhiteSpaceRule);
            Fats.AddValidationRule(isFloatNumberRule);

            Carbohydrates.AddValidationRule(isNotNullOrWhiteSpaceRule);
            Carbohydrates.AddValidationRule(isFloatNumberRule);

            Calories.AddValidationRule(isNotNullOrWhiteSpaceRule);
            Calories.AddValidationRule(isFloatNumberRule);
        }

        private void SetCurrentCategory(ProductCategory currentCategory)
        {
            if (currentCategory != ProductCategory.None)
            {
                LocalizedCategory.Value = LocalizedCategories.FirstOrDefault(category => category.Category == currentCategory);
            }
        }

        private float ParseValue(string value)
        {
            return float.Parse(value);
        }

        private bool ValidateObject<T>(ValidatableObjectWithVisuals<T> validatableObject)
        {
            if (!validatableObject.Validate())
            {
                validatableObject.OnError();
                return false;
            }

            return true;
        }
    }
}
