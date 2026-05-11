using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(jwtOptions =>
{
    jwtOptions.Authority = "https://login.microsoftonline.com/349bca1a-7c38-47e2-94a1-ba4d64ac0e00";
    jwtOptions.Audience = "api://4bf6ce1d-cdca-4341-bb1c-d58eb0828616/.default";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("FrontendOnly", policy =>
        policy.RequireRole("App.Frontend"));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("App.Admin"));
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
