using CleanEjdg.Core.Application.Common;
using CleanEjdg.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Infrastructure.Persistance
{
    public class OrderDbContext : DbContext, IOrderDbContext
    { 
        public OrderDbContext(DbContextOptions<OrderDbContext> opts) : base(opts)
        {

        }
        static OrderDbContext()
        {

        }
        public DbSet<Order> Orders => Set<Order>();

        void IOrderDbContext.SaveChanges()
        {
            base.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // This is necessary because ShippingAddress nad PruchasedLine has no Id
            builder.Entity<Order>().OwnsOne(o => o.ShippingAddress);
            builder.Entity<Order>().OwnsOne(o => o.PurchasedLines);
        }

        EntityEntry IOrderDbContext.Entry<TEntity>(TEntity entity)
        {
            return base.Entry(entity);
        }
    }
}