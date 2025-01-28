using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetProject.API.Services;
using NetProject.DataModels;
using NetProject.ViewModels;

namespace NetProject.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class apiAuthController : ControllerBase
    {
        private ApiResponse response= new();
        private readonly AuthService authService;

        public apiAuthController(AuthService _authService )
        {
            authService = _authService;
        }

        [HttpPost("CheckLogin")]
        public async Task<ApiResponse> CheckLogin(ViewUserAccount request)
        {
            ViewUserAccount? data = await authService.GetUserDetail(request);
            
            if (data == null)
            {
                response.Success = false;
                response.Message = "Email atau Password tidak terdaftar!";
                return response;
            }

            response.Message = "Login Berhasil";
            response.Entity = data;
            return response;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponse>> Login(ViewUserAccount request)
        {
            if (request == null)
            {
                response.Success = false;
                response.Message = "Input tidak boleh kosong";
                return BadRequest(response);  // Return 400 BadRequest if the request is null
            }

            try
            {
                ViewUserAccount? data = await authService.GetUserDetail(request);

                if (data == null)
                {
                    response.Success = false;
                    response.Message = "Email atau Password tidak terdaftar!";
                    return Unauthorized(response);  // Return 401 Unauthorized if no user found
                }

                // If user is found, respond with success message
                response.Success = true;
                response.Message = "Login Berhasil";
                response.Entity = data;
                return Ok(response);  // Return 200 OK with the user data
            }
            catch (Exception ex)
            {
                // Log the exception here
                response.Success = false;
                response.Message = "Error saat login : " + ex.Message;
                return StatusCode(500, response);  // Return 500 Internal Server Error if something goes wrong
            }
        }

        
    }
}
