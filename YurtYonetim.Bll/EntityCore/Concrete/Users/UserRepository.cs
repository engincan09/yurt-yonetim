﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using YurtYonetim.Bll.EntityCore.Abstract.Users;
using YurtYonetim.Core.Utilities.Results.Abstract;
using YurtYonetim.Core.Utilities.Results.Concrete;
using YurtYonetim.Dal.EfCore;
using YurtYonetim.Dal.EfCore.Concrete;
using YurtYonetim.Dal.EfCore.Seed.Systems;
using YurtYonetim.Dal.Middleware;
using YurtYonetim.Dto.Shared;
using YurtYonetim.Dto.Systems;
using YurtYonetim.Entity.Models.Users;
using YurtYonetim.Entity.Shared;

namespace YurtYonetim.Bll.EntityCore.Concrete.Users
{
    public class UserRepository : EntityBaseRepository<Entity.Models.Users.User>, IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ICustomHttpContextAccessor _customHttpContextAccessor;

        /// <summary>
        /// Yapıcı metot
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        /// <param name="userRoleRepository"></param>
        /// <param name="customHttpContextAccessor"></param>
        /// <param name="organizasyonRepository"></param>
        /// <param name="sicilOzlukCalismaRepository"></param>
        public UserRepository(YurtYonetimContext context,
            IConfiguration configuration,
            IUserRoleRepository userRoleRepository,
            ICustomHttpContextAccessor customHttpContextAccessor) : base(context)
        {
            _configuration = configuration;
            _userRoleRepository = userRoleRepository;
            _customHttpContextAccessor = customHttpContextAccessor;
        }

        /// <summary>
        /// Web kullanıcıları için süreli token oluşturur.
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public string BuildToken(Token userToken)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("userId", userToken.UserId.ToString()),
                new Claim("userRoleId", string.Join(',', userToken.UserRoleId)),
                new Claim("fullName", userToken.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime dt = DateTime.UtcNow;

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: dt.AddMinutes(
                    int.Parse(_configuration[
                        "Jwt:expMinute"])),
                notBefore: dt,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Sistem ve mobil kulllanıcılar için süresiz token oluşturur.
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public string IndefiniteBuildToken(Token userToken)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("userId", userToken.UserId.ToString()),
                new Claim("userRoleId", string.Join(',', userToken.UserRoleId)),
                new Claim("fullName", userToken.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime dt = DateTime.UtcNow;

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: dt.AddYears(15),
                notBefore: dt,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Servislerin birbirleriyle iletişim kurması için system kullanıcısı üzerinden token oluşturulmaktadır
        /// </summary>
        /// <returns></returns>
        public string GenerateSystemUserToken() => IndefiniteBuildToken(new Token
        {
            UserId = UserCreator.SystemUser.Id,
            UserRoleId = _userRoleRepository
                .FindBy(x =>
                    x.DataStatus == Entity.Shared.DataStatus.Activated && x.UserId == UserCreator.SystemUser.Id)
                .Select(s => s.RoleId).ToArray(),
            FullName = UserCreator.SystemUser.FullName
        });

        /// <summary>
        /// Parolanýn kendisini yine kendisiyle 256-bit SHA1 ile hasler ve geriye hasli parolayý döndürür.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string PasswordHash(string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes(password);
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8)
            );
        }

        /// <summary>
        /// Bilgisayar adını döndürür
        /// </summary>
        /// <param name="IpAdress"></param>
        /// <returns></returns>
        public string GetHostName(string IpAdress)
        {
            IPAddress myIP = IPAddress.Parse(IpAdress);
            IPHostEntry GetIPHost;
            try
            {
                GetIPHost = Dns.GetHostEntry(myIP);
            }
            catch (Exception)
            {
                return "";
            }

            List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
            return compName.FirstOrDefault();
        }

        /// <summary>
        /// Aktif olan tüm kullanıcı listesini döner
        /// </summary>
        /// <returns></returns>
        public IDataResult<IQueryable<User>> GetAllUser()
        {
            var result = FindBy(m => m.DataStatus == DataStatus.Activated);
            return new SuccessDataResult<IQueryable<User>>(result);
        }

        /// <summary>
        /// Tekil bilgisine göre kullanıcıyı getirir.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IDataResult<User> GetById(int id)
        {
            var result = FindBy(m => m.DataStatus == DataStatus.Activated &&
                                     m.Id == id)
                         .FirstOrDefault();

            if (result != null)
                return new SuccessDataResult<User>(result);
            else
                return new ErrorDataResult<User>(null, "Kullanıcı bulunamadı!");
        }

        /// <summary>
        /// Giriş yapan kullanıcı bilgilerini döndürür.
        /// </summary>
        public IDataResult<LoginUser> LoginUser()
        {
            int userId = _customHttpContextAccessor.GetUserId().Value;
            var user = FindBy(a => a.Id == userId && a.DataStatus == DataStatus.Activated)
                                        .Select(a => new
                                        {
                                            a.Id,
                                            a.Name,
                                            a.Surname,
                                            a.Photo,
                                        })
                                        .FirstOrDefault();

            var ipAdress = HttpContext.Connection.RemoteIpAddress.ToString();
            if (user != null)
            {
                var userRoleId = _userRoleRepository.FindBy(a => a.UserId == user.Id && a.DataStatus == DataStatus.Activated)
                    .Select(a => a.RoleId).Distinct().ToArray();

                var result = new LoginUser
                {
                    Id = user.Id,
                    Name = user.Name,
                    Image = user.Photo,
                    Surname = user.Surname,
                    IpAddress = ipAdress,
                    HostName = GetHostName(ipAdress),
                    //FirstFireLink = _pagePermissionRepository.GetFisrtFireLink(userRoleId),
                    FirstFireLink = "/yonetim",
                };

                return new SuccessDataResult<LoginUser>(result);
            }
            return new SuccessDataResult<LoginUser>(null,"Giriş yapan kullanıcının bilgileri bulunamadı!");
        }

        /// <summary>
        /// Yeni kullanıcı kaydı
        /// </summary>
        public IResult AddUser(User user)
        {
            if (!string.IsNullOrWhiteSpace(user.Username) && FindBy(x => x.DataStatus == DataStatus.Activated && x.Username == user.Username).Any())
                return new ErrorResult("Bu kullanıcı adı daha önce alınmış. Yeni bir kullanıcı adı giriniz");

            if (!string.IsNullOrWhiteSpace(user.Username) && FindBy(x => x.DataStatus == DataStatus.Activated && x.Email == user.Email).Any())
                return new ErrorResult("Bu mail adresine kayıtlı bir kullanıcı daha önceden oluşturulmuş.");

            if (!string.IsNullOrWhiteSpace(user.Username) && FindBy(x => x.DataStatus == DataStatus.Activated && x.PhoneNumber == user.PhoneNumber).Any())
                return new ErrorResult("Bu telefon numarasına ait kayıtlı bir kullanıcı daha önceden oluşturulmuş");

            user.Password = PasswordHash(user.Password);

            Add(user);
            Commit();

            return new SuccessResult("Kullanıcı kaydı başarılı şekilde oluşturuldu!");
        }

        /// <summary>
        /// Kullanıcı kaydını siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IResult DeleteUser(int id)
        {
            var result = FindBy(a => a.Id == id).FirstOrDefault();
            if (result == null)
                return new ErrorResult("Silinecek kullanıcı bulunamadı!");

            else
            {
                Delete(result);
                Commit();
                return new SuccessResult("Silme işlemi başarılı!");

            }
        }

        /// <summary>
        /// Kullanıcıyı login yapar
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public IDataResult<ResponseLogin> Authenticate(Login login)
        {
            var pasifUser = FindBy(a =>
                                               (a.Email == login.Email || a.Username == login.Email)
                                            && a.Password == PasswordHash(login.Password)
                                            && a.DataStatus == DataStatus.DeActivated)
                                       .FirstOrDefault();
            if (pasifUser != null)
                return new ErrorDataResult<ResponseLogin>(null, "Kullanıcınız pasif durumdadır. Sistem yöneticiniz ile irtibata geçiniz");

            var user = FindBy(a =>
                                              (a.Email == login.Email || a.Username == login.Email)
                                           && a.Password == PasswordHash(login.Password)
                                           && a.DataStatus == DataStatus.Activated)
                                      .Select(a => new
                                      {
                                          a.Id,
                                          a.Name,
                                          a.Surname,
                                          a.Photo,
                                      })
                                      .FirstOrDefault();

            if (user == null)
                return new ErrorDataResult<ResponseLogin>(null, "Mevcut parolanız ile girdiğiniz parolanız eşleşmedi.");

            var userRoleId = _userRoleRepository.FindBy(a => a.UserId == user.Id && a.DataStatus == DataStatus.Activated)
                    .Select(a => a.RoleId).Distinct().ToArray();
            var ipAdress = HttpContext.Connection.RemoteIpAddress.ToString();
            var responseLogin = new ResponseLogin
            {
                Token = BuildToken(new Token { UserId = user.Id, UserRoleId = userRoleId, FullName = user.Name + " " + user.Surname }),
                LoginUser = new LoginUser
                {
                    Id = user.Id,
                    Name = user.Name,
                    Image = user.Photo,
                    Surname = user.Surname,
                    IpAddress = ipAdress,
                    HostName = GetHostName(ipAdress),
                    //FirstFireLink = _pagePermissionRepository.GetFisrtFireLink(userRoleId)
                    FirstFireLink = "/yonetim"
                }
            };
            return new SuccessDataResult<ResponseLogin>(responseLogin);
        }
    }
}