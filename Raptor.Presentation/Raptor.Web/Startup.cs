﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Raptor.Data;
using Raptor.Data.Core;
using Raptor.Data.Models.Blog;
using Raptor.Data.Models.Content;
using Raptor.Data.Models.Logging;
using Raptor.Data.Models.Security;
using Raptor.Data.Models.Users;
using Raptor.Services.Blog;
using Raptor.Services.Configuration;
using Raptor.Services.Content;
using Raptor.Services.Logging;
using Raptor.Services.Security;
using Raptor.Services.Users;

namespace Raptor.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env) {
            // Build the basic configuration for our web app
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("appsettings.dev.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services) {
            // Register our Database context
            services.AddDbContext<RaptorDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Enable lowercases routes for urls
            services.AddRouting(options => options.LowercaseUrls = true);

            // Add MVC to our web app
            services.AddMvc();

            // Register the repositories
            services.AddTransient<IRepository<BlogComment>, Repository<BlogComment>>();
            services.AddTransient<IRepository<BlogPost>, Repository<BlogPost>>();
            services.AddTransient<IRepository<BlogPostCategory>, Repository<BlogPostCategory>>();
            services.AddTransient<IRepository<Taxonomy>, Repository<Taxonomy>>();
            services.AddTransient<IRepository<Term>, Repository<Term>>();
            services.AddTransient<IRepository<TermRelationship>, Repository<TermRelationship>>();
            services.AddTransient<IRepository<ActivityLog>, Repository<ActivityLog>>();
            services.AddTransient<IRepository<Log>, Repository<Log>>();
            services.AddTransient<IRepository<PermissionRecord>, Repository<PermissionRecord>>();
            services.AddTransient<IRepository<RolePermission>, Repository<RolePermission>>();
            services.AddTransient<IRepository<Address>, Repository<Address>>();
            services.AddTransient<IRepository<BusinessEntity>, Repository<BusinessEntity>>();
            services.AddTransient<IRepository<BusinessEntityAddress>, Repository<BusinessEntityAddress>>();
            services.AddTransient<IRepository<Password>, Repository<Password>>();
            services.AddTransient<IRepository<Person>, Repository<Person>>();
            services.AddTransient<IRepository<PersonRole>, Repository<PersonRole>>();
            services.AddTransient<IRepository<PhoneNumber>, Repository<PhoneNumber>>();
            services.AddTransient<IRepository<Role>, Repository<Role>>();

            // Register our services
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<ISettingService, SettingService>();
            services.AddTransient<IContentService, ContentService>();
            services.AddTransient<ICustomerActivityService, CustomerActivityService>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<IUserRegisterationService, UserRegistrationService>();
            services.AddTransient<IUserService, UserService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole();

            // Register the routes for areas
            app.UseMvc(routes => {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable exception pages
            app.UseDeveloperExceptionPage();

            // Allow to serve static files
            app.UseStaticFiles();

            // Allow to use status code pages
            app.UseStatusCodePages();
        }
    }
}
