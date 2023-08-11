using CleanEjdg.Core.Application.Common;
using CleanEjdg.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CleanEjdg.Core.Application.Repositories
{
    public class EFOrderRepository : IRepositoryBase<Order>
    {
        IOrderDbContext DbContext;
        public EFOrderRepository(IOrderDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public void Create(Order entity)
        {
            DbContext.Orders.Add(entity);
            DbContext.SaveChanges();
        }

        public async Task Delete(int id)
        {
            DbContext.Orders.Remove(new Order { Id = id });
            await DbContext.SaveChangesAsync();
        }

        public async Task Update(Order entity)
        {
            // entity as it currently exists in the db
            var order = DbContext.Orders.FirstOrDefault(o => o.Id == entity.Id);
            // update properties on the parent
            DbContext.Entry(order).CurrentValues.SetValues(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task<Order> Get(int id)
        {
            return await DbContext.Orders.FindAsync(id) ?? new Order();
        }

        IQueryable<Order> IRepositoryBase<Order>.GetAll()
        {
            return DbContext.Orders.Include(o => o.PurchasedLines).Include(o => o.ShippingAddress);
        }
    }
}
