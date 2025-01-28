using Azure;
using Microsoft.AspNetCore.Mvc;
using NetProject.Services;
using NetProject.ViewModels;

namespace NetProject.Controllers
{
    public class RoleController : Controller
    {
        private RoleService roleService;
        private ApiResponse response = new ApiResponse();
        public RoleController(RoleService roleService) 
        {
            this.roleService = roleService;
        }

        public async Task<IActionResult> Index(SearchQuery queries)
        {
            queries.showData = queries.showData ?? 5;
            queries.pageNumber = queries.pageNumber ?? 1;

            // Set for next queries (placed on view)
            ViewBag.nameSort = string.IsNullOrEmpty(queries.sortOrder) ? "name_desc" : "";

            // Set on view for display
            ViewBag.currentSort = queries.sortOrder;
            ViewBag.currentShowData = queries.showData;
            ViewBag.currentSearchTerm = queries.searchTerm;

            EntityPagination<ViewRole> data = await roleService.GetAll(queries);

            return View(data);
            //return View(EntityPagination<ViewMedicine>.(data, query.pageNumber ?? 1, query.showData ?? 5));
        }

        public IActionResult Add()
        {
            ViewRole role = new ViewRole();
            return PartialView(role);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ViewRole dataForm)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 1;
            dataForm.CreateBy = userId;

            ApiResponse response = await roleService.Add(dataForm);

            return Json(new { dataResponse = response });
        }

        public async Task<IActionResult> Edit(long id)
        {
            ViewRole entity = await roleService.GetById(id);
            return  PartialView(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ViewRole dataForm)
        {
            long userId = HttpContext.Session.GetInt32("UserId") ?? 1;
            dataForm.ModifyBy = userId;

            response = await roleService.Edit(dataForm);

            return Json(new { dataResponse = response });
        }

        public async Task<IActionResult> Delete([FromQuery] long id)
        {
            ViewRole entity = await roleService.GetById(id);
            return PartialView(entity);
        }

        [HttpPost]
        public async Task<IActionResult> SureDelete(long id)
        {
            long userId = HttpContext.Session.GetInt32("UserId") ?? 1;
            response = await roleService.Delete(id, userId);

            return Json(new { dataResponse = response });
    
        }
    }
}
