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
    public static class UserRoleCreator
    {
        public static void Create(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasData(new UserRole[] {
                    new UserRole { Id = 1,
                        RoleId = 1,
                        UserId = 1,
                        CreatedAt = new DateTime(2020, 03, 09),
                        DataStatus = DataStatus.Activated
                    }
            });
        }
    }
}
