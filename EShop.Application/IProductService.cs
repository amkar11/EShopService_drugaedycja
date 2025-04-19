using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain;

namespace EShop.Application
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> ShowAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        void Add(Product product);
    }
}

