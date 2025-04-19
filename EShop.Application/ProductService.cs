using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Repositories;
using EShop.Domain;
using EShop.Domain.ProductProvidersExceptions;
namespace EShop.Application;
public class ProductService : IProductService
{
    public readonly IRepository _repository;
    public ProductService(IRepository repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<Product>> ShowAllProductsAsync()
    {
        return await _repository.GetAllAsync();
    }
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    public async Task AddProductAsync(Product product)
    {
        if (await GetProductByIdAsync(product.Id) != null)
            throw new ProductAllreadyExistsException("Product with this Id allready exists!");
        await _repository.AddAsync(product);
    }
    public async Task UpdateProductAsync(Product product)
    {
        await _repository.UpdateAsync(product);
    }
    public async Task DeleteProductAsync(int id)
    {
        if (await GetProductByIdAsync(id) == null) throw new ProductDoesNotExistException("Product with this Id does not exist!");
        await _repository.DeleteAsync(id);
    }
    public void Add(Product product) {
        _repository.Add(product);
    }
}

