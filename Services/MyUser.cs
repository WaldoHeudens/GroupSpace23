using GroupSpace23.Areas.Identity.Data;
using GroupSpace23.Data;
using Microsoft.EntityFrameworkCore;

namespace GroupSpace23.Services
{
    public interface IMyUser
    {
        public GroupSpace23User User();
    }

    public class MyUser : IMyUser
    {
        readonly MyDbContext _context;
        readonly IHttpContextAccessor _httpContext;
        GroupSpace23User user = null;


        public MyUser(MyDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public GroupSpace23User User()
        {

            string name = _httpContext.HttpContext.User.Identity.Name;
            if (user == null || user.UserName != name)
                user = _context.Users.First(u => u.UserName == (string.IsNullOrEmpty(name) ? "Dummy" : name));

            return user;
        }

    }
}
