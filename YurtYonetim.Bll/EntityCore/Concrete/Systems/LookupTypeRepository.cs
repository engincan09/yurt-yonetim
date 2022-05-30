using System;
using System.Collections.Generic;
using System.Text;
using YurtYonetim.Bll.EntityCore.Abstract.Systems;
using YurtYonetim.Dal.EfCore;
using YurtYonetim.Dal.EfCore.Concrete;
using YurtYonetim.Entity.Models.Systems;

namespace YurtYonetim.Bll.EntityCore.Concrete.Systems
{
    public class LookupTypeRepository : EntityBaseRepository<LookupType>, ILookupTypeRepository
    {
        public LookupTypeRepository(YurtYonetimContext context) : base(context)
        {
        }
    }
}
