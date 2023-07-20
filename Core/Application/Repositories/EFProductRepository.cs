using CleanEjdg.Core.Application.Common;
using CleanEjdg.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CleanEjdg.Core.Application.Repositories
{
    public class EFProductRepository : IRepositoryBase<Product>
    {
        IProductDbContext DbContext;
        public EFProductRepository(IProductDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task Create(Product entity)
        {
            // Save new Cat in db
            await DbContext.Products.AddAsync(entity);
            await DbContext.SaveChangesAsync();
        }

        // Returns false if an the entity didnt exist
        public async Task Delete(int id)
        {
            DbContext.Products.Remove(new Product { Id = id });
            await DbContext.SaveChangesAsync();
        }

        public async Task<Product> Get(int id)
        {
            return await DbContext.Products.Include(p => p.Photos).FirstOrDefaultAsync(p => p.Id == id) ?? new Product();
        }

        public IQueryable<Product> GetAll()
        {
            return DbContext.Products.Include(p => p.Photos);
        }

        public async Task Update(Product entity)
        {
            // entity as it currently exists in the db
            var product = DbContext.Products.Include(p => p.Photos)
                .FirstOrDefault(p => p.Id == entity.Id);
            // update properties on the parent
            DbContext.Entry(product).CurrentValues.SetValues(entity);
            // remove or update child collection items
            var productPhotos = product.Photos;
            foreach (var productPhoto in productPhotos)
            {
                var photo = entity.Photos.SingleOrDefault(p => p.Id == productPhoto.Id);
                if (photo != null)
                {
                    //no need to change the photo since photos never change, htey are only added or removed
                    //DbContext.Entry(catPhotos).CurrentValues.SetValues(photo);
                }
                else
                    DbContext.Remove(productPhoto);
            }
            // add the new items
            foreach (var photo in entity.Photos)
            {
                if (productPhotos.All(i => i.Id != photo.Id))
                {
                    product.Photos.Add(photo);
                }
            }
            await DbContext.SaveChangesAsync();
        }
    }
}
