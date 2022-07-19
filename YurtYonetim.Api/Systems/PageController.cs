using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YurtYonetim.Bll.EntityCore.Abstract.Systems;
using YurtYonetim.Dal.Middleware;
using YurtYonetim.Entity.Models.Systems;

namespace YurtYonetim.Api.Systems
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private readonly IPageRepository _service;
        private readonly IPagePermissionRepository _pagePermissionRepository;
        private readonly ICustomHttpContextAccessor _customHttpContextAccessor;

        /// <summary>
        /// Yapıcı method.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="pagePermissionRepository"></param>
        /// <param name="customHttpContextAccessor"></param>
        public PageController(IPageRepository service,
                              IPagePermissionRepository pagePermissionRepository,
                              ICustomHttpContextAccessor customHttpContextAccessor)
        {
            _service = service;
            _pagePermissionRepository = pagePermissionRepository;
            _customHttpContextAccessor = customHttpContextAccessor;
        }

        /// <summary>
        /// Tüm Page verilerini getirir.
        /// </summary>
        /// <param name="sicilId">Bordro tekil bilgisidir.</param>
        /// <returns>Istenen bordro detay bilgisini döndürür.</returns>
        [HttpGet, Route("GetAllPagePermissions")]
        [Authorize]
        [Produces("application/json")]
        public IActionResult GetAllPagePermissions()
        {
            var result = _service.FindBy(x => x.MenuShow == true)
                                 .OrderBy(a => a.ParentId)
                                 .ThenBy(a => a.Order)
                                 .Distinct()
                                 .AsQueryable();
            return Ok(result);
        }

        /// <summary>
        /// Tekil bilgisine göre page döndürür
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
                return BadRequest("İstenilen sayfa bulunamadı!");
        }

        /// <summary>
        /// Yeni page oluşturur
        /// </summary>
        [HttpPost, Route("PostPage")]
        [Authorize]
        [Produces("application/json")]
        public IActionResult PostPage([FromBody] Page page)
        {
            try
            {
                _service.Add(page);
                _service.Commit();
                return Ok("İşlem Başarılı!");
            }
            catch (Exception e)
            {
                return BadRequest("Sayfa eklenirken bir hata oluştu!");
            }
        }

        /// <summary>
        /// Page Günceller
        /// </summary>
        [HttpPost, Route("UpdatePage")]
        [Authorize]
        [Produces("application/json")]
        public IActionResult Update([FromBody] Page page)
        {
            try
            {
                var hasData = _service.FindBy(m => m.Id == page.Id).FirstOrDefault();
                if (hasData == null)
                    return BadRequest("Güncellenmek istenilen sayfa bulunamadı!");
                else
                {
                    _service.Update(page);
                    _service.Commit();
                    return Ok("İşlem Başarılı!");
                }
            }
            catch (Exception e)
            {
                return BadRequest("Sayfa eklenirken bir hata oluştu!");
            }
        }

        /// <summary>
        /// Page Siler
        /// </summary>
        [HttpGet, Route("DeletePage/{key:int}")]
        [Authorize]
        [Produces("application/json")]
        public IActionResult DeletePage([FromRoute] int id)
        {
            try
            {
                var hasData = _service.FindBy(m => m.Id == id).FirstOrDefault();
                if (hasData == null)
                    return BadRequest("Silinmek istenilen sayfa bulunamadı!");
                else
                {
                    _service.Delete(hasData);
                    _service.Commit();
                    return Ok("İşlem Başarılı!");
                }
            }
            catch (Exception e)
            {
                return BadRequest("Sayfa eklenirken bir hata oluştu!");
            }
        }

        /// <summary>
        /// Tüm Page verilerini getirir.
        /// </summary>
        [HttpGet, Route("GetPermissionPage")]
 
        [Produces("application/json")]
        public IActionResult GetPermissionPage()
        {
            if (_customHttpContextAccessor.GetUserRoleId().Contains(1))
            {
                List<Page> page = _service.FindBy(x => x.MenuShow)
                                        .OrderBy(a => a.ParentId)
                                        .ThenBy(a => a.Order)
                                        .Distinct()
                                        .ToList();

                return Ok(page.AsQueryable());
            }
            else
            {
                List<Page> pages = _pagePermissionRepository.GetPagePermissionListCache(false).Where(w =>
                                                                       (w.UserId == _customHttpContextAccessor.GetUserId() || (w.RoleId.HasValue && _customHttpContextAccessor.GetUserRoleId().Contains(w.RoleId.Value))) &&
                                                                       !w.Forbidden &&
                                                                       w.DataStatus == Entity.Shared.DataStatus.Activated &&
                                                                       w.Page.MenuShow &&
                                                                       !_pagePermissionRepository
                                                                            .GetPagePermissionListCache(false).Where(w2 => (
                                                                            w2.UserId == _customHttpContextAccessor.GetUserId() ||
                                                                            (w2.RoleId.HasValue && _customHttpContextAccessor.GetUserRoleId().Contains(w2.RoleId.Value))) &&
                                                                            w2.Forbidden && w2.DataStatus == Entity.Shared.DataStatus.Activated)
                                                                .Select(a => a.PageId).Contains(w.PageId))
                                                                .Select(a => a.Page)
                                                                .OrderBy(a => a.ParentId)
                                                                .ThenBy(a => a.Order)
                                                                .Distinct()
                                                                .ToList();

                return Ok(pages.AsQueryable());
            }
        }




    }
}
