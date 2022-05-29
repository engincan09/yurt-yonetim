using YurtYonetim.Entity.Models.Systems;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YurtYonetim.Dal.EfCore.Seed.Systems
{
    public static class LookupTypeCreator
    {
        public static void Create(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LookupType>().HasData(new LookupType[] {
                new LookupType
                {
                    Id = LookupTypes.Gender,
                    Name = "Cinsiyet"
                },
                 new LookupType
                {
                    Id = LookupTypes.Country,
                    Name = "Ülke"
                },
                 new LookupType
                {
                    Id = LookupTypes.Province,
                    Name = "İl"
                },
                  new LookupType
                {
                    Id = LookupTypes.County,
                    Name = "İlçe"
                },
                  new LookupType
                {
                    Id = LookupTypes.Currency,
                    Name = "Döviz"
                }
            });
        }
    }
}
