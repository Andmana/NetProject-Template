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
            UserAccount data = await authService.MatchUser(request);
            
            if (data == null)
            {
                response.Success = false;
                response.Message = "Email atau Password tidak terdaftar!";
                return response;
            }

            ViewUserAccount dataDetail = await authService.GetUserDetail(data);

            response.Message = "Login Berhasil";
            response.Entity = dataDetail;
            return response;
        }

    }
}
