using Microsoft.EntityFrameworkCore;
using System.Reflection;
using YurtYonetim.Dal.EfCore.Extensions;
using YurtYonetim.Entity.Models.Systems;
using YurtYonetim.Entity.Models.Users;
using YurtYonetim_Entity.Shared;

namespace YurtYonetim.Dal.EfCore
{
    public class YurtYonetimContext : DbContext
    {
        public YurtYonetimContext(DbContextOptions<YurtYonetimContext> options) : base(options)
        {
        }

        #region Users
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        #endregion Users

        #region Systems
        public DbSet<Lookup> Lookup { get; set; }
        public DbSet<LookupType> LookupType { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PagePermission> PagePermissions { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /// <summary>
            /// https://docs.microsoft.com/en-us/ef/core/modeling/indexes#indexes
            /// </summary>

            /// IndexAttribute ile oluşturulan indexleri create eder.
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
                foreach (var prop in entity.GetProperties())
                    try
                    {
                        var attr = prop.PropertyInfo.GetCustomAttribute<IndexAttribute>();
                        if (attr != null)
                        {
                            var index = entity.AddIndex(prop);
                            index.IsUnique = attr.IsUnique;
                        }
                    }
                    catch
                    {
                    }

            // Seed
            modelBuilder.Seed();

            base.OnModelCreating(modelBuilder);
        }
    }
}
