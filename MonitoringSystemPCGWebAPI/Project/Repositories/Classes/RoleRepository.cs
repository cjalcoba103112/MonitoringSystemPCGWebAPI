
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;

namespace Repositories.Classes
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public new async Task<IEnumerable<Role>> GetAllAsync(Role? filter)
        {
            IQueryable<Role> query = _context.Role;

            // 1. Join with the Mapping table
            // 2. Join from the Mapping table to the Sidebar table
            query = query
                .Include(r => r.SidebarRoleMappings)
                    .ThenInclude(srm => srm.Sidebar);

            // Apply filters if any
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.RoleName))
                {
                    query = query.Where(r => r.RoleName.Contains(filter.RoleName));
                }
                // Add other filters as needed
            }

            return await query.ToListAsync();
        }
    }
}
