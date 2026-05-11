using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var tenantId = builder.Configuration["AzureAd:TenantId"];
var audience = builder.Configuration["AzureAd:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine("JWT ERROR: " + ctx.Exception.ToString());
                return Task.CompletedTask;
            },
            OnTokenValidated = ctx =>
            {
                Console.WriteLine("JWT VALIDATED: " + ctx.Principal.Identity?.Name);
                return Task.CompletedTask;
            },
            OnChallenge = ctx =>
            {
                Console.WriteLine("JWT CHALLENGE: " + ctx.ErrorDescription);
                return Task.CompletedTask;
            }
        };

        options.Authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";
        options.Audience = audience;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://login.microsoftonline.com/{tenantId}/v2.0",

            ValidateAudience = true,
            ValidAudience = audience,

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            // Map "roles" claim
            RoleClaimType = "roles"
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("FrontendOnly", policy =>
        policy.RequireRole("App.Frontend"));
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
