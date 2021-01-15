using MealRandomizer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealRandomizer.Data
{
    internal sealed class ProductsRepository : IRepository<int, Product>
    {
        private static readonly Lazy<ProductsRepository> instance = new Lazy<ProductsRepository>(() => new ProductsRepository(), true);

        public static ProductsRepository Instance => instance.Value;

        private readonly object locker = new object();
        private readonly AppStorage appStorage;
        private readonly Dictionary<int, Product> localProducts;

        private ProductsRepository()
        {
            appStorage = new AppStorage();

            localProducts = new Dictionary<int, Product>(
                appStorage.Products.Include(product => product.Recipes).ToDictionary(product => product.Id));

            //localProducts = Services.Test.GetRandomProducts(5000).ToDictionary(product => product.Id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    if (localProducts.ContainsKey(product.Id))
                    {
                        return null;
                    }

                    appStorage.Products.Add(product);
                    int affected = appStorage.SaveChanges();

                    if (affected == 1)
                    {
                        localProducts.Add(product.Id, product);
                        return product;
                    }

                    return null;
                }
            });
        }

        public async Task<bool> DeleteAsync(Product product)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    if (!localProducts.ContainsKey(product.Id))
                    {
                        return false;
                    }

                    appStorage.Entry(product).State = EntityState.Deleted;
                    int affected = appStorage.SaveChanges();

                    if (affected == 1)
                    {
                        return localProducts.Remove(product.Id);
                    }

                    return false;
                }
            });
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await Task.Run<IEnumerable<Product>>(() => localProducts.Values);
        }

        public async Task<Product> GetAsync(int key)
        {
            return await appStorage.Products.FindAsync(key);
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    appStorage.Entry(product).State = EntityState.Modified;
                    int affected = appStorage.SaveChanges();

                    if (affected == 1)
                    {
                        localProducts.Remove(product.Id);
                        localProducts.Add(product.Id, product);
                        return product;
                    }

                    return null;
                }
            });
        }
    }
}
