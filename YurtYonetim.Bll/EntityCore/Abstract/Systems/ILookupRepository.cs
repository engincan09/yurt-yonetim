using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YurtYonetim.Dal.EfCore.Abstract;
using YurtYonetim.Entity.Models.Systems;

namespace YurtYonetim.Bll.EntityCore.Abstract.Systems
{
    public interface ILookupRepository : IEntityBaseRepository<Lookup>
    {
    }
}
