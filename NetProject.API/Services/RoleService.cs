using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using NetProject.DataModels;
using NetProject.ViewModels;

namespace NetProject.API.Services
{
    public class RoleService
    {
        private readonly NetProjectContext db;

        public RoleService(NetProjectContext db) { this.db = db; }

        public async Task<List<ViewRole>> GetAllRoles()
        {
            List<ViewRole> data = await (from role in db.Roles
                                         where role.IsDelete == false
                                         select new ViewRole
                                         {
                                            Id = role.Id,
                                            Name = role.Name,
                                         })
                                         .ToListAsync();
            return data;
        }

        public async Task<EntityPagination<ViewRole>> GetAllRoles(SearchQuery queries)
        {
            // Ensure pageNumber and showData are valid
            var pageNumber = queries.pageNumber ?? 1;
            var showData = queries.showData ?? 10;

            var query = db.Roles.Where(role => role.IsDelete == false)
                                .Select(role => new ViewRole
                                {
                                    Id = role.Id,
                                    Name = role.Name,
                                }).AsQueryable();

            // Apply search filter if searchTerm is provided
            if (!string.IsNullOrEmpty(queries.searchTerm))
            {
                query = query.Where(e => EF.Functions.Like(e.Name, $"%{queries.searchTerm}%"));
            }

            // Apply sorting
            switch (queries.sortOrder)
            {
                case "name_desc":
                    query = query.OrderByDescending(a => a.Name);
                    break;
                default:
                    query = query.OrderBy(a => a.Name);
                    break;
            }

            // Get the total count and apply pagination
            var totalData = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * showData)
                                   .Take(showData)
                                   .ToListAsync();

            return new EntityPagination<ViewRole>(items, totalData, pageNumber, showData);
        }

        public async Task<ViewRole?> GetRoleById(long id)
        {
            var role = await db.Roles
                       .Where(role => role.Id == id
                              && role.IsDelete == false) 
                       .Select(role => new ViewRole
                       {
                           Id = role.Id,
                           Name = role.Name,
                       })
                       .FirstOrDefaultAsync();

            return role;
        }

        public async Task<ViewRole> AddRole(ViewRole input)
        {
            Role newEntry = new Role();

            newEntry.Name = input.Name;
            newEntry.CreatedBy = input.CreateBy.Value;
            newEntry.CreatedOn = DateTime.Now;

            await db.AddAsync(newEntry);
            await db.SaveChangesAsync();

            // Map back to ViewRole (with generated Id)
            input.Id = newEntry.Id;

            return input;
        }

        public async Task<ViewRole> EditRole(ViewRole input)
        {
            Role? entity = await db.Roles.FirstOrDefaultAsync(role =>  role.Id == input.Id && role.IsDelete == false);
           
            entity.Name = input.Name;
            entity.ModifiedBy = input.ModifyBy;
            entity.ModifiedOn = DateTime.Now;

            db.Update(entity);
            await db.SaveChangesAsync();

            return input;
        }

        public async Task SoftDeleteRole(long id, long userId)
        {
            Role? entity = await db.Roles.FirstOrDefaultAsync(role => role.Id == id);
            if (entity == null)
            {
                throw new ArgumentException("Role not found");
            }

            entity.DeletedBy = userId;
            entity.IsDelete = true;

            db.Update(entity);
            await db.SaveChangesAsync();
        }
    }
}
