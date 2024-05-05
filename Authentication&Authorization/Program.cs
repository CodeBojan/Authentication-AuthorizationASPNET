var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

        /*o.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents()
        {
            OnCreatingTicket = e =>
            {

            };
        };*/


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


var app = builder.Build();

// Configure the HTTP request pipeline.
var dev = app.Environment.IsDevelopment();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
