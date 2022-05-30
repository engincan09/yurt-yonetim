using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YurtYonetim.Bll.EntityCore.Abstract.Systems;
using YurtYonetim.Bll.EntityCore.Abstract.Users;
using YurtYonetim.Dal.Middleware;
using YurtYonetim.Dto.Shared;
using YurtYonetim.Dto.Systems;
using YurtYonetim.Entity.Models.Users;
using YurtYonetim.Entity.Shared;

namespace YurtYonetim.Api.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _service;
        private readonly IPagePermissionRepository _pagePermissionRepository;
        private readonly ICustomHttpContextAccessor _customHttpContextAccessor;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ILookupRepository _lookupRepository;
        private readonly IRoleRepository _roleRepository;

        /// <summary>
        /// Yapıcı method.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="pagePermissionRepository"></param>
        /// <param name="userSessionRepository"></param>
        /// <param name="userRoleRepository"></param>
        /// <param name="sicilRepository"></param>
        /// <param name="lookupRepository"></param>
        /// <param name="organizasyonRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="mailService"></param>
        /// <param name="customHttpContextAccessor"></param>
        public UserController(IUserRepository service,
            IPagePermissionRepository pagePermissionRepository,
            IUserRoleRepository userRoleRepository,
            ILookupRepository lookupRepository,
            IRoleRepository roleRepository,
            ICustomHttpContextAccessor customHttpContextAccessor)
        {
            _service = service;
            _pagePermissionRepository = pagePermissionRepository;
            _customHttpContextAccessor = customHttpContextAccessor;
            _userRoleRepository = userRoleRepository;
            _lookupRepository = lookupRepository;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Tüm User verilerini getirir.
        /// </summary>
        [HttpGet, Route("GetAllUser")]
        [Authorize]
        [Produces("application/json")]
        public IActionResult GetAllPagePermissions()
        {
            var result = _service.FindBy(m => m.DataStatus == Entity.Shared.DataStatus.Activated);
            return Ok(result);
        }

        /// <summary>
        /// Tekil bilgisine göre user döndürür
        /// </summary>
        [HttpGet, Route("GetById/{key:int}")]
        [Authorize]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] int key)
        {
            var result = _service.FindBy(a => a.Id == key).FirstOrDefault();
            if (result != null)
                return Ok(result);
            else
                return BadRequest("İstenilen kullanıcı bulunamadı!");
        }

        /// <summary>
        /// Giriş yapan kullanıcı bilgilerini döndürür.
        /// </summary>
        [HttpGet, Route("LoginUser")]
        [Authorize]
        [Produces("application/json")]
        public IActionResult LoginUser()
        {
            int userId = _customHttpContextAccessor.GetUserId().Value;
            var user = _service.FindBy(a => a.Id == userId && a.DataStatus == DataStatus.Activated)
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

                return Ok(new LoginUser
                {
                    Id = user.Id,
                    Name = user.Name,
                    Image = user.Photo,
                    Surname = user.Surname,
                    IpAddress = ipAdress,
                    HostName = _service.GetHostName(ipAdress),
                    //FirstFireLink = _pagePermissionRepository.GetFisrtFireLink(userRoleId),
                    FirstFireLink = "/yonetim",
                });
            }

            return null;
        }

        /// <summary>
        /// Yeni kullanıcı kaydı
        /// </summary>
        [HttpPost, Route("PostUser")]
        [Authorize]
        [Produces("application/json")]
        public IActionResult PostUser([FromBody] User val)
        {
            if (!string.IsNullOrWhiteSpace(val.Username) && _service.FindBy(x => x.DataStatus == DataStatus.Activated && x.Username == val.Username).Any())
            {
                return BadRequest(new Response(false, "Bu kullanıcı adı daha önce alınmış. Yeni bir kullanıcı adı giriniz", 5000));
            }

            val.Password = _service.PasswordHash(val.Password);
            val.FullName = val.Name + " " + val.Surname;

            _service.Add(val);
            _service.Commit();

            return Ok(val);
        }

        /// <summary>
        /// Yeni giriş kaydı
        /// </summary>
        [HttpPost, Route("Authenticate")]
        [Produces("application/json")]
        public IActionResult Authenticate([FromBody] Login login)
        {
            var pasifUser = _service.FindBy(a =>
                                              (a.Email == login.Email || a.Username == login.Email)
                                           && a.Password == _service.PasswordHash(login.Password)
                                           && a.DataStatus == DataStatus.DeActivated)
                                      .FirstOrDefault();
            if (pasifUser != null)
                return BadRequest(new Response(false, "Kullanıcınız pasif durumdadır. Sistem yöneticiniz ile irtibata geçiniz"));

            var user = _service.FindBy(a =>
                                              (a.Email == login.Email || a.Username == login.Email)
                                           && a.Password == _service.PasswordHash(login.Password)
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
                return BadRequest(new Response(false, "Mevcut parolanız ile girdiğiniz parolanız eşleşmedi."));

            var userRoleId = _userRoleRepository.FindBy(a => a.UserId == user.Id && a.DataStatus == DataStatus.Activated)
                    .Select(a => a.RoleId).Distinct().ToArray();
            var ipAdress = HttpContext.Connection.RemoteIpAddress.ToString();
            var responseLogin = new ResponseLogin
            {
                Token = _service.BuildToken(new Token { UserId = user.Id, UserRoleId = userRoleId, FullName = user.Name + " " + user.Surname }),
                LoginUser = new LoginUser
                {
                    Id = user.Id,
                    Name = user.Name,
                    Image = user.Photo,
                    Surname = user.Surname,
                    IpAddress = ipAdress,
                    HostName = _service.GetHostName(ipAdress),
                    //FirstFireLink = _pagePermissionRepository.GetFisrtFireLink(userRoleId)
                    FirstFireLink = "/yonetim"
                }
            };
            return Ok(responseLogin);
        }



    }
}
