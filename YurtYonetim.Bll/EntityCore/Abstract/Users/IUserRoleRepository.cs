using YurtYonetim.Dal.EfCore.Abstract;
using YurtYonetim.Entity.Models.Users;

namespace YurtYonetim.Bll.EntityCore.Abstract.Users
{
    public interface IUserRoleRepository : IEntityBaseRepository<UserRole>
    {
    }
}