using System.Collections.Generic;
using System.Threading.Tasks;

namespace MealRandomizer.Data
{
    internal interface IRepository<TKey, TValue>
    {
        Task<TValue> AddAsync(TValue item);
        Task<TValue> UpdateAsync(TValue item);
        Task<TValue> GetAsync(TKey key);
        Task<IEnumerable<TValue>> GetAllAsync();
        Task<bool> DeleteAsync(TValue item);
    }
}
