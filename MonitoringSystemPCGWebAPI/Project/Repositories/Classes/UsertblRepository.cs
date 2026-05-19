
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using System.Linq.Expressions;

namespace Repositories.Classes
{
    public class UsertblRepository : GenericRepository<Usertbl>, IUsertblRepository
    {
        public Usertbl? GetUsernameOrEmail(string usernameOrEmail)
        {
            return _context.Usertbl
                .Include(c => c.Personnel)
                    .ThenInclude(p => p.Rank)
                .Include(c => c.Role)
                    .ThenInclude(r => r.SidebarRoleMappings!)
                           .ThenInclude(m => m.Sidebar)
                    .ToList().Find(u => u.UserName == usernameOrEmail || u.Email == usernameOrEmail);
        }
        public async Task<IEnumerable<Usertbl>> GetAllAsync(Usertbl? filter)
        {
            return await _context.Usertbl
                 .Include(c => c.Personnel)
                     .ThenInclude(p => p.Rank)
                 .Include(c => c.Role)
                 .ToListAsync();
        }

        public Usertbl? GetByChangePasswordToken(string changePasswordToken)
        {
            return _context.Usertbl.ToList().Find(u => u.ChangePasswordToken == changePasswordToken && u.IsDefaultPassword == true);

        }
        public new async Task<Usertbl?> GetByIdAsync(int id)
        {
            return await _context.Usertbl
                .Include(u => u.Personnel)
                    .ThenInclude(p => p.Rank)
                .Include(u => u.Role)
                    .ThenInclude(r => r.SidebarRoleMappings!)
                        .ThenInclude(m => m.Sidebar)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }
    }
}
