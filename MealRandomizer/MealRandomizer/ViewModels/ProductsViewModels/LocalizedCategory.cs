using MealRandomizer.Models;
using MealRandomizer.Resources;
using System;
using System.Reflection;

namespace MealRandomizer.ViewModels.ProductsViewModels
{
    internal class LocalizedCategory : IEquatable<LocalizedCategory>, IComparable<LocalizedCategory>
    {
        public string CategoryName { get; }
        public string EnumName { get; }
        public ProductCategory Category { get; }

        public LocalizedCategory(ProductCategory category)
        {
            Category = category;

            EnumName = Enum.GetName(typeof(ProductCategory), category);

            CategoryName = (string)typeof(Categories).GetProperty(EnumName, BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
        }

        public int CompareTo(LocalizedCategory other) => CategoryName.CompareTo(other.CategoryName);

        public bool Equals(LocalizedCategory other)
        {
            return other != null
                && Category == other.Category;
        }

        public override string ToString() => CategoryName;

        public override bool Equals(object obj)
        {
            return obj is LocalizedCategory category
                && Equals(category);
        }

        public override int GetHashCode() => CategoryName.GetHashCode();
    }
}
