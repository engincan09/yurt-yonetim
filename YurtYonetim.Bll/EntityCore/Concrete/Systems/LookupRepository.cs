using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YurtYonetim.Bll.EntityCore.Abstract.Systems;
using YurtYonetim.Dal.EfCore;
using YurtYonetim.Dal.EfCore.Concrete;
using YurtYonetim.Entity.Models.Systems;

namespace YurtYonetim.Bll.EntityCore.Concrete.Systems
{
    public class LookupRepository : EntityBaseRepository<Lookup>, ILookupRepository
    {
        public LookupRepository(YurtYonetimContext context) : base(context)
        {
        }
    }
}
