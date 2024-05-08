using Authentication_Authorization.Data;
using Authentication_Authorization.IdentityPolicies;
using Authentication_Authorization.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Authorization.Extensions
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            //ASP.NET Identity Configuration

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:DefaultConnection").Get<string>()));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 7;
                options.Password.RequireDigit = true;

                options.User.RequireUniqueEmail = true;
            });

            builder.Services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordPolicy>(); //added custom password validator -> it combines with the default one and the overriden options from identity options
            builder.Services.AddTransient<IUserValidator<ApplicationUser>, CustomUserPolicy>();
            builder.Services.AddScoped<UserManager<ApplicationUser>>();

            builder.Services.AddRazorPages();

            builder.Services.AddAuthentication("cookie")
                .AddCookie("cookie", o =>
                {
                    o.Cookie.Name = "demo";
                    o.ExpireTimeSpan = TimeSpan.FromHours(8);
                    o.SlidingExpiration = true; //TODO: what is this?
                    o.LoginPath = "/account/login";
                    o.AccessDeniedPath = "/account/accessdenied";
                })
                .AddCookie("temp")
                .AddGoogle("Google", o =>
                {
                    o.ClientId = "952910843833-8qgmhvoru29ar8mr9ge3b5juc0orjive.apps.googleusercontent.com";
                    o.ClientSecret = "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";

                    o.CallbackPath = "/signin-google";
                    o.SignInScheme = "temp";
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ManageCustomers", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("department", "sales");
                    policy.RequireClaim("status", "senior");
                });
            });

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();//.RequireAuthorization();

            return app;
        }
    }
}
