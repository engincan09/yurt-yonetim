﻿using YurtYonetim.Entity.Models.Users;
using YurtYonetim.Entity.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YurtYonetim.Dal.EfCore.Seed.Systems
{
    public static class UserCreator
    {
        public static readonly User SystemUser = new User
        {
            Id = 1,
            Name = "Yönetici",
            Surname = "Admin",
            FullName = "Yönetici Admin",
            Email = "admin@mail.com",
            Password = "9K7Cwg3Qw/8FR/S9VvrNdgl8znxhPagMZ4QrajV/3AQ=", // admin
            CreatedAt = new DateTime(2020, 03, 09),
            DataStatus = DataStatus.Activated
        };

        public static void Create(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User[] { SystemUser });
        }
    }
}
