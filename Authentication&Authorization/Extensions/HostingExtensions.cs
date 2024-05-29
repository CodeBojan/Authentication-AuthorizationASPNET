using Authentication_Authorization.Data;
using Authentication_Authorization.IdentityPolicies;
using Authentication_Authorization.Models;
using Authentication_Authorization.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Authorization.Extensions
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<IdentityServerSettings>(builder.Configuration.GetSection("IdentityServerSettings"));
            builder.Services.AddSingleton<ITokenService, TokenService>();

            //ASP.NET Identity Configuration

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:DefaultConnection").Get<string>()));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddSignInManager()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddRoleManager<RoleManager<IdentityRole>>()
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
            builder.Services.AddTransient<IAuthorizationHandler, AllowUsersHandler>();

            builder.Services.AddRazorPages();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {
                    o.Cookie.Name = "demo";
                    o.ExpireTimeSpan = TimeSpan.FromHours(8);
                    o.SlidingExpiration = true; //TODO: what is this?
                    o.LoginPath = "/Account/Login";
                    o.AccessDeniedPath = "/Account/AccessDenied";
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    
                    options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
                    options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
                    options.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];
                    options.Scope.Add(builder.Configuration["InteractiveServiceSettings:Scopes:0"]);
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.ResponseType = "code";
                    options.ResponseMode = "query";
                    options.SaveTokens = true;
                    options.UsePkce = true;
                    options.CallbackPath = "/signin-oidc";
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
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin");
                    policy.RequireUserName("bojan");
                });
                options.AddPolicy("AllowBojan", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireUserName("bojan");
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

            app.MapRazorPages().RequireAuthorization();
            /*app.Use(async (context, next) =>
            {
                var csp = "default-src 'self'; connect-src 'self' https://localhost:5003/";
                context.Response.Headers.Add("Content-Security-Policy", csp);

                await next();
            });*/
            return app;
        }
    }
}
