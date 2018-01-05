using Guard.Controllers.Api;
using Guard.Dal;
using Guard.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Guard
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthenticationOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = AuthenticationOptions.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthenticationOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddMvc();

            services.AddTransient(s => new MongoDbContext());
            services.AddTransient<IMongoDbRepository<User>>(s =>
                new MongoDbRepository<User>(UserController.DbCollectionName, s.GetService<MongoDbContext>()));
            services.AddTransient<IMongoDbRepository<Account>>(s =>
                new MongoDbRepository<Account>(AccountController.DbCollectionName, s.GetService<MongoDbContext>()));
            services.AddTransient<IMongoDbRepository<Post>>(s =>
                new MongoDbRepository<Post>(PostsController.DbCollectionName, s.GetService<MongoDbContext>()));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "angularapp",
                    template: "ng/{*.}",
                    defaults: new {controller = "Home", action = "Index"});

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "api",
                    template: "api/{controller}/{action}/{id?}");
            });
        }
    }
}