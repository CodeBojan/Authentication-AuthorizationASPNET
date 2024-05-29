using Microsoft.AspNetCore.Authentication.JwtBearer;
using WeatherAPI;
using Authentication_Authorization.Services;
using Authentication_Authorization.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityServer(options =>
{
    options.EmitStaticAudienceClaim = true;
    options.Events.RaiseSuccessEvents = true;
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
})
    .AddTestUsers(Config.Users)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryIdentityResources(Config.IdentityResources);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = "https://localhost:5003";
        options.Audience = "weatherapi";
        options.TokenValidationParameters.ValidTypes = [ "at+jwt" ];
    }); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages().RequireAuthorization();

app.Run();
