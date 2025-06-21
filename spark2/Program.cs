using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using spark2.Services;
using spark2.Data;
using spark2.Models.Entities; 
using spark2.Services.Account;
using spark2.Repos;
using spark2.Repos.ResumeRepositary;
 



namespace spark2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");            
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            // options.UseSqlServer(connectionString
            //options.UseSqlServer(connectionString, sqlOptions =>
            // options.UseSqlServer(connectionString));
            //{
            //    sqlOptions.EnableRetryOnFailure(
            //        maxRetryCount: 5,
            //        maxRetryDelay: TimeSpan.FromSeconds(10),
            //        errorNumbersToAdd: null);
            //}

            //));



            var key = builder.Configuration["OpenAI:Key"];
            builder.Services.AddSingleton<Kernel>(sp =>
            {
                var kernelBuilder = Kernel.CreateBuilder();
                kernelBuilder.AddOpenAIChatCompletion("gpt-4", key);
                return kernelBuilder.Build();
            });

           
            
            builder.Services.AddIdentity<Person, IdentityRole>(options =>options.SignIn.RequireConfirmedAccount = false)
              .AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders()
              .AddDefaultUI();

            builder.Services.AddScoped<IPortofolioParserService, PortofolioParserService>();
            builder.Services.AddScoped<IResumeParserService, ResumeParserService>();
            builder.Services.AddScoped<IDashboardService,DashboardService>();
             builder.Services.AddScoped<IPortofolioRepo, PortofolioRepo>();
            builder.Services.AddScoped<IResumeRepo, ResumeRepo>();
                 

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            // Seed roles on startup
            //using (var scope = app.Services.CreateScope())
            //{
            //    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //    string[] roles = new[] { "Admin", "EndUser" };

            //    foreach (var role in roles)
            //    {
            //        if (!await roleManager.RoleExistsAsync(role))
            //        {
            //            await roleManager.CreateAsync(new IdentityRole(role));
            //        }
            //    }
            //}




            app.Run();
        }
    }
}
