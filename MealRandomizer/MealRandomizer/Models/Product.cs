using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MealRandomizer.Models
{
    internal class Product : IComparable<Product>
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        public ProductCategory Category { get; set; }
        public float Proteins { get; set; }
        public float Fats { get; set; }
        public float Carbohydrates { get; set; }
        public float Calories { get; set; }

        public ICollection<Recipe> Recipes { get; set; } = new HashSet<Recipe>();

        public int CompareTo(Product other) => Name.CompareTo(other.Name);

        public override string ToString() => $"{Id}-{Name}";
    }
}
