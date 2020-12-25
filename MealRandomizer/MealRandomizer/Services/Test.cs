using MealRandomizer.Models;
using System;
using System.Collections.Generic;

namespace MealRandomizer.Services
{
    internal class Test
    {
        internal static List<Product> GetRandomProducts(int amount)
        {
            List<Product> products = new List<Product>();
            Random random = new Random();
            string name;

            for (int i = 0; i < amount; i++)
            {
                name = "";

                for (int j = 0; j < random.Next(5, 10); j++)
                {
                    name += (char)random.Next('a', 'z' + 1);
                }

                ProductCategory category = (ProductCategory)random.Next(1, Enum.GetNames(typeof(ProductCategory)).Length);

                products.Add(new Product()
                {
                    Id = i + 1,
                    Name = name,
                    Category = category,
                    Proteins = random.Next(10, 501),
                    Fats = random.Next(10, 501),
                    Carbohydrates = random.Next(10, 501),
                    Calories = random.Next(100, 1001)
                });
            }

            products.Sort();
            return products;
        }
    }
}
