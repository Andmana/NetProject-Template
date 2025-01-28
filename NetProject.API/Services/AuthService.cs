using Microsoft.EntityFrameworkCore;
using NetProject.DataModels;
using NetProject.ViewModels;

namespace NetProject.API.Services
{
    public class AuthService
    {
        private readonly NetProjectContext db;
        public AuthService(NetProjectContext _db)
        {
            db = _db;
        }

        public async Task<UserAccount?> MatchUser(ViewUserAccount request)
        {
            var data = await db.UserAccounts.Where(u => u.Email == request.Email && u.Password == request.Password).FirstOrDefaultAsync();
            return data;
        }

        public async Task<ViewUserAccount?> GetUserDetail(ViewUserAccount request)
        {
            ViewUserAccount? data = await (from user in db.UserAccounts
                                    join role in db.Roles
                                    on user.RoleId equals role.Id
                                    where request.Email == user.Email
                                    && request.Password == user.Password
                                    select new ViewUserAccount
                                    {
                                        Id = user.Id,
                                        RoleId = role.Id,
                                        RoleName = role.Name,
                                        Name = user.Name,
                                        Email = user.Email,

                                    }).FirstOrDefaultAsync();
            return data;
        }
    }
}
