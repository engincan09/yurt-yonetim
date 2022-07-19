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
        public IActionResult GetAllUser()
        {
            var result = _service.GetAllUser();
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Tekil bilgisine göre user döndürür
        /// </summary>
        [HttpGet, Route("GetById/{key:int}")]
        [Authorize]
        [Produces("application/json")]
        public IActionResult GetById([FromRoute] int key)
        {
            var result = _service.GetById(key);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Giriş yapan kullanıcı bilgilerini döndürür.
        /// </summary>
        [HttpGet, Route("LoginUser")]
        [Authorize]
        [Produces("application/json")]
        public IActionResult LoginUser()
        {
            var result = _service.LoginUser();
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Yeni kullanıcı kaydı
        /// </summary>
        [HttpPost, Route("PostUser")]
        [Authorize]
        [Produces("application/json")]
        public IActionResult PostUser([FromBody] User val)
        {
            var result = _service.AddUser(val);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet, Route("DeleteUser/{key:int}")]
        [Authorize]
        [Produces("application/json")]
        public IActionResult DeleteUser([FromRoute] int key)
        {
            var result = _service.DeleteUser(key);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Yeni giriş kaydı
        /// </summary>
        [HttpPost, Route("Authenticate")]
        [Produces("application/json")]
        public IActionResult Authenticate([FromBody] Login login)
        {
            var result = _service.Authenticate(login);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }



    }
}
