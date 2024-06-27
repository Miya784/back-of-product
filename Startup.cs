using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PosgresDb.Data;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy(name: "_myAllowSpecificOrigins",
                              policy =>
                              {
                                  policy.WithOrigins("http://localhost:3000");
                              });
        });

        services.AddEntityFrameworkNpgsql()
            .AddDbContext<AppDbContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("simpleConnection")));

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            c.TagActionsBy(api =>
            {
                if (api.GroupName != null)
                {
                    return new[] { api.GroupName };
                }
                var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                if (controllerActionDescriptor != null)
                {
                    return new[] { controllerActionDescriptor.ControllerName };
                }
                throw new InvalidOperationException("Unable to determine tag for endpoint.");
            });
            c.DocInclusionPredicate((name, api) => true);
        });

        var jwtKey = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);  //"Loaded Jwt:Key: System.Byte[]," suggests that jwtKey is being loaded as a byte array which is the expected behavior when you use Encoding.UTF8.GetBytes to convert a string into bytes.orHowever, when you retrieve the key using builder.Configuration["Jwt:Key"]
        var jwtIssuer = Configuration["Jwt:Issuer"];
        var jwtAudience = Configuration["Jwt:Audience"];

        Console.WriteLine($"Loaded Jwt:Key: {jwtKey}");
        Console.WriteLine($"Loaded Jwt:Issuer: {jwtIssuer}");
        Console.WriteLine($"Loaded Jwt:Audience: {jwtAudience}");
        

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
        {
            var jwtKey = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = Configuration["Jwt:Audience"], // Set the expected audience
                ValidateLifetime = true, // You can set this to true to validate token lifetime
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(jwtKey)
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                    return Task.CompletedTask;
                }
            };
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
                options.AddPolicy("Customer", policy => policy.RequireClaim("Customer"));
            });
    });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseCors("_myAllowSpecificOrigins");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
