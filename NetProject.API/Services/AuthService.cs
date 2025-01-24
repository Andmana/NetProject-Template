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

        public async Task<UserAccount> MatchUser(ViewUserAccount request)
        {
            UserAccount data = db.UserAccounts.Where(u => u.Email == request.Email && u.Password == u.Password).FirstOrDefault();
            return data;
        }

        public async Task<ViewUserAccount> GetUserDetail(UserAccount request)
        {
            ViewUserAccount data = (from user in db.UserAccounts
                                    join role in db.Roles
                                    on user.RoleId equals role.Id
                                    where request.Id == user.Id
                                    select new ViewUserAccount
                                    {
                                        Id = user.Id,
                                        RoleId = role.Id,
                                        RoleName = role.Name,
                                        Name = user.Name,
                                        Email = user.Email,

                                    }).FirstOrDefault()!;
            return data;
        }
    }
}
