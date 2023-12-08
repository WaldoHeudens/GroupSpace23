using GroupSpace23.Areas.Identity.Data;
using GroupSpace23.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GroupSpace23.Data;

public class MyDbContext : IdentityDbContext<GroupSpace23User>
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }


    public static async Task DataInitializer(MyDbContext context, UserManager<GroupSpace23User> userManager)
    {

        AddParameters();

        if (!context.Users.Any())
        {
            GroupSpace23User dummyuser = new GroupSpace23User
            {
                Id = "Dummy",
                Email = "dummy@dummy.xx",
                UserName = "Dummy",
                FirstName = "Dummy",
                LastName = "Dummy",
                PasswordHash = "Dummy",
                LockoutEnabled = true,
                LockoutEnd = DateTime.MaxValue
            };
            context.Users.Add(dummyuser);
            context.SaveChanges();


            GroupSpace23User adminUser = new GroupSpace23User
            {
                Id = "Admin",
                Email = "admin@dummy.xx",
                UserName = "Admin",
                FirstName = "Administrator",
                LastName = "GroupSpace"
            };

            var result = await userManager.CreateAsync(adminUser, "Abc!12345");

        }

        GroupSpace23User dummy = context.Users.First(u => u.UserName == "Dummy");
        GroupSpace23User admin = context.Users.First(u => u.UserName == "Admin");

        Globals.DummyUser = dummy;  // Make sure the dummy user is always available

        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new IdentityRole { Name = "SystemAdministrator", Id = "SystemAdministrator", NormalizedName = "SYSTEMADMINISTRATOR" },
                new IdentityRole { Name = "User", Id = "User", NormalizedName = "USER" }
            );

            context.UserRoles.Add(new IdentityUserRole<string> { RoleId = "SystemAdministrator", UserId = admin.Id });

            context.SaveChanges();
        }

        if (!context.Groups.Any())
        {
            context.Groups.Add(new Group { Description = "Dummy", Name = "Dummy", Ended = DateTime.Now });
            context.SaveChanges();
        }
        Group dummyGroup = context.Groups.FirstOrDefault(g => g.Name == "Dummy");

        // Was nodig om bij de migratie een foreign-key constraint probleem te hebben
        //List <Group> groups = context.Groups.ToList();
        //foreach (Group g in groups)
        //{ 
        //    g.StartedById = dummy.Id;
        //    context.Update(g);
        //}
        //context.SaveChanges();

        if (!context.Messages.Any())
        {
            context.Messages.Add(new Message { Title = "Dummy", Body = "", Sent = DateTime.Now, Deleted = DateTime.Now, Recipient = dummyGroup });
            context.SaveChanges();
        }
        context.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    static void AddParameters()
    {

    }

    public DbSet<GroupSpace23.Models.Group> Groups { get; set; } = default!;

    public DbSet<GroupSpace23.Models.Message> Messages { get; set; } = default!;

    public DbSet<GroupSpace23.Models.Parameter> Parameters { get; set; } = default!;

    public DbSet<GroupSpace23.Models.GroupMember> GroupMembers { get; set; } = default!;

}
