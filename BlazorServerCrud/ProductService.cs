using BlazorServerCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerCrud
{
    public class ProductService
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

        public ProductService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Products.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product?> UpdateProductAsync(Product updatedProduct)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == updatedProduct.Id);

            if (existingProduct != null)
            {
                existingProduct.Name = updatedProduct.Name;
                existingProduct.Price = updatedProduct.Price;

                await context.SaveChangesAsync();
                return existingProduct;
            }

            return null;
        }

        public async Task DeleteProductAsync(int id)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var existingProduct = await context.Products.FirstOrDefaultAsync(p =>p.Id == id);

            if (existingProduct is not null)
            {
                context.Products.Remove(existingProduct);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return product;
        }
    }
}
