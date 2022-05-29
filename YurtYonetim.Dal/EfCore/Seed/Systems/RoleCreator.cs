using YurtYonetim.Entity.Models.Users;
using YurtYonetim.Entity.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YurtYonetim.Dal.EfCore.Seed.Systems
{
    public static class RoleCreator
    {
        public static void Create(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role[] {
                    new Role {
                        Id = 1,
                        Name = "Admin",
                        CreatedAt = new DateTime(2020, 03, 09),
                        DataStatus = DataStatus.Activated
                    }
            });
        }
    }
}
