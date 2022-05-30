using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YurtYonetim.Dal.EfCore.Abstract;
using YurtYonetim.Dto.Shared;
using YurtYonetim.Entity.Models.Systems;

namespace YurtYonetim.Bll.EntityCore.Abstract.Systems
{
    public interface IPagePermissionRepository : IEntityBaseRepository<PagePermission>
    {
        Task<Response> CustomPost(CustomPostPagePermission val);

        string GetFisrtFireLink(int[] allUserRoles);

        List<PagePermission> GetPagePermissionListCache(bool isRefresh);

        List<MenuPage> GetMenuPageListCache(bool isRefresh);
    }
}
