using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GroupSpace23.Data;
using Microsoft.AspNetCore.Identity;
using GroupSpace23.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using GroupSpace2022.Services;
using NETCore.MailKit.Infrastructure.Internal;

namespace GroupSpace23
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("GroupSpace23Context");

            builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(connectionString ?? throw new InvalidOperationException("Connection string 'GroupSpace23Context' not found.")));

            builder.Services.AddDefaultIdentity<GroupSpace23User>((IdentityOptions options) => options.SignIn.RequireConfirmedAccount = true)
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<MyDbContext>();

            builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();
            builder.Services.Configure<MailKitOptions>(options =>
            {
                options.Server = builder.Configuration["ExternalProviders:MailKit:SMTP:Address"];
                options.Port = Convert.ToInt32(builder.Configuration["ExternalProviders:MailKit:SMTP:Port"]);
                options.Account = builder.Configuration["ExternalProviders:MailKit:SMTP:Account"];
                options.Password = builder.Configuration["ExternalProviders:MailKit:SMTP:Password"];
                options.SenderEmail = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderEmail"];
                options.SenderName = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderName"];

                // Set it to TRUE to enable ssl or tls, FALSE otherwise
                options.Security = true;  // true zet ssl or tls aan
            });


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

     //       builder.Services.AddMvc();

            var app = builder.Build();
            Globals.App = app;          // Zorg ervoor dat we altijd een instantie van de huidige app bijhouden

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                MyDbContext context = new MyDbContext(services.GetRequiredService<DbContextOptions<MyDbContext>>());
                var userManager = services.GetRequiredService<UserManager<GroupSpace23User>>();
                await MyDbContext.DataInitializer(context, userManager);
            }


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}