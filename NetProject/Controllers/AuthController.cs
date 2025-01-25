using Azure;
using Microsoft.AspNetCore.Mvc;
using NetProject.Services;
using NetProject.ViewModels;
using Newtonsoft.Json;

namespace NetProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService authService;
        private ApiResponse response = new();

        public AuthController(AuthService _authService)
        {
            authService = _authService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> LoginSubmit(string email, string password)
        {
            response = await authService.CheckLogin(email, password);

            if (response.Entity != null)
            {
                ViewUserAccount? user = JsonConvert.DeserializeObject<ViewUserAccount>(response.Entity.ToString() ?? string.Empty);

                if (user != null)
                {
                    response.Message = $"Hello, {user.Name} Welcome to Med 341";

                    //Store user information in session
                    HttpContext.Session.SetInt32("UserId", (int)user.Id);
                    HttpContext.Session.SetString("Name", user.Name ?? "");
                    HttpContext.Session.SetString("Email", user.Email ?? "");
                    HttpContext.Session.SetString("RoleName", user.RoleName ?? "");
                    HttpContext.Session.SetInt32("RoleId", (int)(user.RoleId ?? 0));
                }
            }

            return Json(new { dataResponse = response });
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
