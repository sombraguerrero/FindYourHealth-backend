using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

string tenantId = @"349bca1a-7c38-47e2-94a1-ba4d64ac0e00";
string appId = @"39bd4854-b3a7-42ef-9cf5-09067e72935f";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";
        options.Audience = appId;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://login.microsoftonline.com/{tenantId}/v2.0",

            ValidateAudience = true,
            ValidAudience = appId,

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            RoleClaimType = "roles"
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine("JWT ERROR: " + ctx.Exception.ToString());
                return Task.CompletedTask;
            },
            OnTokenValidated = ctx =>
            {
                Console.WriteLine("JWT VALIDATED");
                return Task.CompletedTask;
            },
            OnChallenge = ctx =>
            {
                Console.WriteLine("JWT CHALLENGE: " + ctx.ErrorDescription);
                return Task.CompletedTask;
            }
        };
    });

app.UseAuthorization();

app.MapControllers();

app.Run();
