using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ScreechrDemo.Api.Middleware;
using ScreechrDemo.Api.Validators;
using ScreechrDemo.Business.Core.Interface;
using ScreechrDemo.Business.Core.Services;
using ScreechrDemo.Contracts.Model;
using ScreechrDemo.Databases.Data.Interface;
using ScreechrDemo.Databases.Data.Repositories;
using ScreechrDemo.Security.Auth;
using ScreechrDemo.Security.@interface;

namespace ScreechrDemo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #region Private Property

        private readonly string SwaggerApiTitle = "SCREECH DEMO API";
        #endregion
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Inject Validation Rules dependency injection
            services.AddMvc().AddFluentValidation();

            RegisterAuthenication(services);
            RegisterDbContext(services);

            GenerateSwagger(services);


            //Singleton: Singleton objects are the same for every object and every request (regardless of whether an instance is provided in ConfigureServices)
            RegisterSingleTone(services);

            //Scoped: Scoped objects are the same within a request, but different across different requests
            RegisterScoped(services);

            //Transient: a new instance is provided to every controller and every service
            RegisterTransient(services);
        }



        #region authenication register

        private void RegisterAuthenication(IServiceCollection services)
        {

            services.AddAuthentication(options => options.DefaultScheme = "BasicAuth")
                .AddScheme<BasicAuthSchemeOptions, BasicAuthenticationHandler>("BasicAuth", options =>
                {

                });
        }

        #endregion


        #region LifeTime Scope register
        private void RegisterTransient(IServiceCollection services)
        {
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddTransient<IScreechService, ScreechService>();
        }
        private void RegisterScoped(IServiceCollection services)
        {
            services.AddScoped<IScreechRepositoryContext, ScreechRepositoryContext>();
            services.AddScoped<IUserProfileRepositoryContext, UserProfileRepositoryContext>();
            services.AddScoped<IScreechUserAuthenticationManager, ScreechUserAuthenticationManager>();
        }
        private void RegisterSingleTone(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            services.AddSingleton<IValidator<PageModel>, PageModelValidation>();
            services.AddSingleton<IValidator<UserModel>, UserModelValidation>();
            services.AddSingleton<IValidator<UpdateProfileModel>, UpdateProfileModelValidation>();
            services.AddSingleton<IValidator<ScreechModel>, ScreechModelValidation>();
            services.AddSingleton<IValidator<UpdateScreechModel>, UpdateScreechModelValidation>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }
        #endregion

        #region Invoked dbcontext
        private void RegisterDbContext(IServiceCollection services)
        {
            var connString = Configuration.GetConnectionString("ScreechDB");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<UserProfileRepositoryContext>(options =>
            {
                
                options.UseSqlServer(connString, sql=>sql.MigrationsAssembly(migrationsAssembly));
            });
            services.AddDbContext<ScreechRepositoryContext>(options =>
            {
                options.UseSqlServer(connString, sql => sql.MigrationsAssembly(migrationsAssembly));
            });
        }
  
        #endregion

        #region Swagger generate

        private void GenerateSwagger(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                var version = Assembly.GetEntryAssembly().GetName().Version.ToString();
                c.ResolveConflictingActions(x => x.First());
                var versionName = $"v{version}";
                c.SwaggerDoc(versionName, new OpenApiInfo()
                {
                    Title = SwaggerApiTitle,
                    Version = version,

                });
            });
        }

        #endregion

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, UserProfileRepositoryContext uDb, ScreechRepositoryContext sDb)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // initialize db
            InitializeDatabase(app);


            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                var vs = Assembly.GetEntryAssembly().GetName().Version.ToString();
                var vName = $"/swagger/v{vs}/swagger.json";
                s.SwaggerEndpoint(vName, SwaggerApiTitle);
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMiddleware<ExceptionMiddleware>();

           // uDb.Database.EnsureCreated();
           // sDb.Database.EnsureCreated();

           // CreateAdminRole(serviceProvider);
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //    //endpoints.MapGet("/", async context =>
            //    //{
            //    //    await context.Response.WriteAsync("Hello World!");
            //    //});
            //});
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            var userDbContext = serviceProvider.GetRequiredService<UserProfileRepositoryContext>();
            userDbContext.Database.Migrate();

            var screechDbContext = serviceProvider.GetRequiredService<ScreechRepositoryContext>();
            screechDbContext.Database.Migrate();

        }
    }
}
