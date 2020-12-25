using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MealRandomizer.Models
{
    internal class Recipe
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
