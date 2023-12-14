using GroupSpace23.Areas.Identity.Data;
using GroupSpace23.Data;
using GroupSpace23.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.ComponentModel.DataAnnotations;

namespace GroupSpace23.Controllers
{
    [Authorize (Roles = "UserAdministrator")]

    public class UsersController : Controller
    {

        MyDbContext _context;

        public UsersController(MyDbContext context)
        {
           _context = context;
        }


        public IActionResult Index()
        {
            List<UserIndexViewModel> vmUsers = new List<UserIndexViewModel>();
            foreach (GroupSpace23User user in _context.Users)
            {
                vmUsers.Add(new UserIndexViewModel
                {
                    Blocked = user.LockoutEnabled,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Roles = (from userRole in _context.UserRoles
                             where userRole.UserId == user.Id
                             orderby userRole.RoleId
                             select userRole.RoleId).ToList()
                });
            }
            return View(vmUsers);
        }
    }
}
