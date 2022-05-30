using System;
using YurtYonetim.Dal.EfCore.Abstract;
using YurtYonetim.Dto.Shared;

namespace YurtYonetim.Bll.EntityCore.Abstract.Users
{
    public interface IUserRepository : IEntityBaseRepository<Entity.Models.Users.User>
    {
        string BuildToken(Token userToken);

        string IndefiniteBuildToken(Token userToken);

        string GenerateSystemUserToken();

        string PasswordHash(string password);

        string GetHostName(string IpAdress);
    }
}