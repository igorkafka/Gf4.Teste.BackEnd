

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PedidoStore.Infrastructure.Extensions;
using System.Linq.Expressions;
using PedidoStore.Core.SharedKernel;

namespace PedidoStore.Infrastructure.Data
{
    public abstract class BaseDbContext<TContext>(DbContextOptions<TContext> dbOptions) : DbContext(dbOptions)
    where TContext : DbContext
    {
        private const string Collation = "Latin1_General_CI_AI";

        public override ChangeTracker ChangeTracker
        {
            get
            {
                base.ChangeTracker.LazyLoadingEnabled = false;
                base.ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
                base.ChangeTracker.DeleteOrphansTiming = CascadeTiming.OnSaveChanges;
                return base.ChangeTracker;
            }
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<string>()
                .AreUnicode(false)
                .HaveMaxLength(255);

            base.ConfigureConventions(configurationBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation(Collation)
                .RemoveCascadeDeleteConvention();

            base.OnModelCreating(modelBuilder);
        }
        private static LambdaExpression GetIsDeletedFilter(Type type)
        {
            var parameter = Expression.Parameter(type, "e");
            var property = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
            var constant = Expression.Constant(false);
            var equals = Expression.Equal(property, constant);
            return Expression.Lambda(equals, parameter);
        }
    }
}
