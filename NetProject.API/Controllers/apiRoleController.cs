using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using NetProject.API.Services;
using NetProject.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetProject.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class apiRoleController : ControllerBase
    {
        private ApiResponse response = new ApiResponse();
        private RoleService roleService;


        public apiRoleController(RoleService roleService)
        {
            this.roleService = roleService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] SearchQuery queries)
        {
            EntityPagination<ViewRole> data = await roleService.GetAllRoles(queries);
            response.Entity = data;
            return Ok(response);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ApiResponse>> GetById(long id)
        {
            var entity = await roleService.GetRoleById(id);
            response.Entity = entity;
            return Ok(response);
        }

        [HttpPost("Add")]
        public async Task<ActionResult<ApiResponse>> Add(ViewRole request)
        {
            if (request == null)
            {
                response.Success = false;
                response.Message = "Request data cannot be null.";
                return StatusCode(204, response);  // Return 400 BadRequest if the request is null
            }

            try
            {
                request = await roleService.AddRole(request);
                response.Message = "Role berhasil ditambhakan";
                response.Entity = request;
                return Ok(response);

            }catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Gagal Menambah role : " + ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<ApiResponse>> Edit(ViewRole request)
        {
            ViewRole? entity = await roleService.GetRoleById(request.Id);
            if (entity == null)
            {
                response.Success = false;
                response.Message = "Role tidak ditemukan";
                return StatusCode(204, response);
            }

            try
            {
                request = await roleService.EditRole(request);

                response.Message = "Role berhasil diedit";
                response.Entity = request;
                return Ok(response);

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Gagal mengedit role : " + ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpDelete("delete/{id}/{userId}")]
        public async Task<ActionResult<ApiResponse>> Delete(long id, long userId)
        {
            ViewRole? entity = await roleService.GetRoleById(id);
            if (entity == null)
            {
                response.Success = false;
                response.Message = "Data tidak ditemukan";
                return NotFound(response); // 404 Not Found
            }

            try
            {
                await roleService.SoftDeleteRole(id, userId);

                response.Message = "Data berhasil dihapus";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Gagal menghapus data : " + ex. Message;
                return StatusCode(500, response);
            }
        }


    }
}
