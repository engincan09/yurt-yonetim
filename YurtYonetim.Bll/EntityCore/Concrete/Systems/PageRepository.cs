using System;
using System.Collections.Generic;
using System.Text;
using YurtYonetim.Bll.EntityCore.Abstract.Systems;
using YurtYonetim.Dal.EfCore;
using YurtYonetim.Dal.EfCore.Abstract;
using YurtYonetim.Dal.EfCore.Concrete;
using YurtYonetim.Entity.Models.Systems;

namespace YurtYonetim.Bll.EntityCore.Concrete.Systems
{
    public class PageRepository : EntityBaseRepository<Page>, IPageRepository
    {
        public PageRepository(YurtYonetimContext context) : base(context)
        {
        }
    }
}
