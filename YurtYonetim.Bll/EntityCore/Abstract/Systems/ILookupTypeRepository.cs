using System;
using System.Collections.Generic;
using System.Text;
using YurtYonetim.Dal.EfCore.Abstract;
using YurtYonetim.Entity.Models.Systems;

namespace YurtYonetim.Bll.EntityCore.Abstract.Systems
{
    public interface ILookupTypeRepository : IEntityBaseRepository<LookupType>
    {
    }
}
