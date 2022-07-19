using System;
using System.Collections.Generic;
using System.Linq;
using YurtYonetim.Core.Utilities.Results.Abstract;
using YurtYonetim.Dal.EfCore.Abstract;
using YurtYonetim.Dto.Shared;
using YurtYonetim.Dto.Systems;
using YurtYonetim.Entity.Models.Users;

namespace YurtYonetim.Bll.EntityCore.Abstract.Users
{
    public interface IUserRepository : IEntityBaseRepository<Entity.Models.Users.User>
    {
        string BuildToken(Token userToken);

        string IndefiniteBuildToken(Token userToken);

        string GenerateSystemUserToken();

        string PasswordHash(string password);

        string GetHostName(string IpAdress);

        IDataResult<IQueryable<User>> GetAllUser();

        IDataResult<User> GetById(int id);

        IDataResult<LoginUser> LoginUser();

        IResult AddUser(User user);

        IResult DeleteUser(int id);

        IDataResult<ResponseLogin> Authenticate(Login login);

    }
}