using Microsoft.EntityFrameworkCore;
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
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseCors("_myAllowSpecificOrigins");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
