
using SignalRMongoChat.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SignalRMongoChat.Service;
using SignalRCallingSolution.Service;

namespace SignalRCallingSolution
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            corsUrls = Configuration.GetSection("CorsSettings:AllowUrls").Value.Split(';');

        }
        string[] corsUrls = null;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(MyAllowSpecificOrigins,
            //        builder => builder.WithOrigins(corsUrls)
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials()
            //       );
            //});
            //services.AddCors(o =>
            //     o.AddPolicy("AllowOrigins", builder =>
            //        {
            //    builder.AllowAnyMethod()
            //           .AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials();
            //})
            //);

            services.AddCors(options =>
            {
                //options.AddPolicy("AllowSpecific",
                //    builder => builder.WithOrigins(Configuration.GetSection("CorsSettings:AllowUrls").Value.Split(';'))
                //    .AllowAnyHeader().AllowAnyMethod());

                options.AddPolicy("AllowOrigins",
                    builder => builder.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials());
            });
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET 5 Web API",
                    Description = "Authentication and Authorization in ASP.NET 5 with JWT and Swagger"
                });


            });
            // requires using Microsoft.Extensions.Options
            services.Configure<MongoDatabaseSettings>(
                Configuration.GetSection(nameof(MongoDatabaseSettings)));

            services.AddSingleton<IMongoDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
            services.AddSingleton<MongoService>();

            services.AddScoped<IWaitingRoomService, WaitingRoomService>();


        }
        readonly string MyAllowSpecificOrigins = "AllowOrigins";

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("v1/swagger.json", "API v1"); 

                });
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<Chat>("/chatHub");
                //endpoints.MapHub<NotificationHub>("/notificationHub");
            });
        }
    }
}
