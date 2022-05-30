using YurtYonetim.Bll.EntityCore.Abstract.Users;
using YurtYonetim.Dal.EfCore;
using YurtYonetim.Dal.EfCore.Concrete;
using YurtYonetim.Entity.Models.Users;

namespace YurtYonetim.Bll.EntityCore.Concrete.Users
{
    public class RoleRepository : EntityBaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(YurtYonetimContext context) : base(context)
        {
        }
    }
}