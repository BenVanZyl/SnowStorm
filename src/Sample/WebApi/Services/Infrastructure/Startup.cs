using SnowStorm;


namespace WebApi.Services.Infrastructure
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddHttpContextAccessor(); //TODO:  move into SnowStorm

            services.AddCors(options =>
            {
                options.AddPolicy("CorsAllowAll",
                builder =>
                {
                    builder.WithOrigins("ApiClientCors").AllowAnyHeader().WithMethods("GET, PATCH, DELETE, PUT, POST, OPTIONS");
                });
            });

            services.AddSnowStorm("WebApi", ConnectionStringData);

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseRouting();

            //app.MapControllers();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }

        private string ConnectionStringData => Configuration.GetConnectionString("Data");
    }
}
