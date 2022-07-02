using YurtYonetim.Entity.Models.Systems;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YurtYonetim.Dal.EfCore.Seed.Systems
{
    public static class PageCreator
    {
        public static readonly Page[] AllPages = new Page[] {
                new Page
                {
                    Id = 1,
                    ParentId = null,
                    Order = 0,
                    Name = "Yönetim Paneli",
                    AllName = "Yönetim Paneli",
                    RouterLink = "/yonetim",
                    AllRouterLink = "/yonetim",
                    MenuShow = true
                },
                new Page
                {
                    Id = 2,
                    ParentId = 1,
                    Order = 0,
                    Name = "Ana Sayfa",
                    AllName = "Yönetim Paneli / Ana Sayfa",
                    RouterLink = "/ana-sayfa",
                    AllRouterLink = "/yonetim/ana-sayfa",
                    Icon = "fa fa-home",
                    MenuShow = true
                },
                 new Page
                {
                    Id = 3,
                    ParentId = 1,
                    Order = 1,
                    Name = "İdari İşler",
                    AllName = "Yönetim Paneli / İdari İşler",
                    RouterLink = "/idari-isler",
                    AllRouterLink = "/yonetim/idari-isler",
                    Icon = "fa fa-copy",
                    MenuShow = true
                },
                new Page
                {
                    Id = 4,
                    ParentId =3,
                    Order = 1,
                    Name = "Kullanıcı İşlemleri",
                    AllName = "Yönetim Paneli / İdari İşler/ Kullanıcı İşlemleri",
                    RouterLink = "/kullanici-islemleri",
                    AllRouterLink = "/yonetim/idari-isler/kullanici-islemleri",
                    Icon = "fa fa-user",
                    MenuShow = true
                },
                new Page
                {
                    Id = 5,
                    ParentId = 4,
                    Order = 0,
                    Name = "Tüm Kullanıcılar",
                    AllName = "Yönetim Paneli / İdari İşler/ Kullanıcı İşlemleri / Tüm Kullanıcılar",
                    RouterLink = "/tum-kullanicilar",
                    AllRouterLink = "/yonetim/idari-isler/kullanici-islemleri/tum-kullanicilar",
                    MenuShow = true
                },
                new Page
                {
                    Id = 6,
                    ParentId = 4,
                    Order = 1,
                    Name = "Yeni Kullanıcı",
                    AllName = "Yönetim Paneli / İdari İşler/ Kullanıcı İşlemleri / Yeni Kullanıcı",
                    RouterLink = "/yeni-kullanici",
                    AllRouterLink = "/yonetim/idari-isler/kullanici-islemleri/tum-kullanicilar/yeni-kullanici",
                    MenuShow = true
                },
        };
        public static void Create(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Page>().HasData(AllPages);
        }
    }
}
