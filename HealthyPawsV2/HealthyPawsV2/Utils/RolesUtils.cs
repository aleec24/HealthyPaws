using HealthyPawsV2.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace HealthyPawsV2.Utils
{
    public static class RolesUtils
    {
        //Get users according to their role
        public static async Task<List<ApplicationUser>> GetUsersPerRole(
            RoleManager<IdentityRole> _roleManager,
            UserManager<ApplicationUser> _userManager,
            string roleName
            )
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return new List<ApplicationUser>();
            }

            var userIdsInRole = await _userManager.GetUsersInRoleAsync(roleName);
            var usersInRole = _userManager.Users.Where(u => userIdsInRole.Contains(u)).ToListAsync();

            return usersInRole.Result;
        }

        //Get logged user
        public static async Task<ApplicationUser> GetLoggedUser(UserManager<ApplicationUser> _userManager, ClaimsPrincipal userIdentity)
        {
            string userLoggedId = userIdentity.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;

            var loggedUser = await _userManager.FindByIdAsync(userLoggedId);
            return loggedUser;
        }

        //Confirm if logged user has a specific role
        public static Boolean LoggedUserRole(ClaimsPrincipal userIdentity, String role)
        {
            var roles = userIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            return roles.Select(r => r.Value).Contains(role);
        }

		// Get all roles in the database
		public static async Task<List<string>> GetAllRoles(RoleManager<IdentityRole> _roleManager)
		{
			return await _roleManager.Roles.Select(r => r.Name).ToListAsync();
		}
	}
}
