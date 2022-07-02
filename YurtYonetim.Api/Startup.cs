using YurtYonetim.Dal.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Reflection;
using System.Globalization;
using YurtYonetim.Bll.Helpers.Handlers;
using System;
using Swashbuckle.AspNetCore.SwaggerUI;
using Coravel;
using YurtYonetim.Dal.Middleware;
using YurtYonetim.Bll.EntityCore.Abstract.Systems;
using YurtYonetim.Bll.EntityCore.Concrete.Systems;
using YurtYonetim.Bll.EntityCore.Abstract.Users;
using YurtYonetim.Bll.EntityCore.Concrete.Users;
using Microsoft.OpenApi.Models;

namespace YurtYonetim.Api
{
    public class Startup
    {
        /// <summary>
        /// Confirations
        /// </summary>
        public IConfiguration _configuration { get; }

        /// <summary>
        /// Starter
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration) => _configuration = configuration;


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<YurtYonetimContext>(options =>
             options.UseSqlServer(_configuration.GetConnectionString("Default")));

            services.AddHttpContextAccessor();
            services.AddResponseCompression();
            _addServices(ref services);

            services.AddHttpClient();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
              options.RequireHttpsMetadata = false; // https var ise true ya çek
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = false, //  server bu tokenı yaratmışmı yani geçerli mi diye kontrol eder
                  ValidateAudience = false, // tokenın audience dan alınıp alınmayacağını kontrol eder. bir nevi bu domain yetkilimi der.
                  ValidateLifetime = true, // token ölmüş mü ölmemiş mi ve bu token geçerli bir token mı diye kontrol eder
                  ValidateIssuerSigningKey = true, // gelen token güvenilir anahtarlar barındırıyormu diye kontrol eder
                  ValidIssuer = _configuration["Jwt:Issuer"],
                  ValidAudience = _configuration["Jwt:Issuer"],
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
              };

              options.Events = new JwtBearerEvents
              {
                  OnMessageReceived = context =>
                  {
                      var accessToken = context.Request.Query["access_token"];

                      // If the request is for our hub...
                      var path = context.HttpContext.Request.Path;
                      if (!string.IsNullOrEmpty(accessToken)
                          && (path.StartsWithSegments("/hub")))
                      {
                          // Read the token out of the query string
                          context.Token = accessToken;
                      }
                      return Task.CompletedTask;
                  }
              };
          });

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            })
           .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddMemoryCache();
            services.AddResponseCaching();

            services.AddCors(options =>
            {
                options.AddPolicy("ApiCorsPolicy",
                    p => p.WithOrigins(_configuration.GetSection("Host:AllowedOrigins").Get<string[]>()).
                    AllowAnyMethod().
                    AllowAnyHeader().
                    AllowCredentials());
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();
                    options.CustomSchemaIds(type => type.ToString() + type.GetHashCode());
                    // integrate xml comments
                    options.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml"));
                });



            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });


           

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Server configure
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            env.CreateContentFolders();

            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseAuthentication();
            app.UseCors("ApiCorsPolicy");

            // FOR LINUX PROXYING
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseResponseCaching();
            app.UseMiddleware<ErrorHandler>();
            app.Use(next => context => { context.Request.EnableRewind(); return next(context); });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                        name: "default",
                        template: "{controller}/{action=Index}/{id?}");
            });

            if (!env.IsDevelopment())
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                });

            app.UseSwagger();
            //app.UseSwaggerUI(
            //    options =>
            //    {
            //        // Sayfadaki öğelerin expand özelliğini kapatır. Sayfa boyutunu düşürmek ve donmaları önlemek için.
            //        options.DocExpansion(DocExpansion.List);
            //        // build a swagger endpoint for each discovered API version
            //        foreach (var description in provider.ApiVersionDescriptions)
            //        {
            //            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            //        }
            //    });


            #region Tüm Scheduler İşlemleri

     
            #endregion Tüm Scheduler İşlemleri
        }

        private static void _addServices(ref IServiceCollection services)
        {
            #region Systems
            services.AddScoped<ICustomHttpContextAccessor, CustomHttpContextAccessor>();
            services.AddScoped<ILookupRepository, LookupRepository>();
            services.AddScoped<ILookupTypeRepository, LookupTypeRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IPagePermissionRepository, PagePermissionRepository>();
            #endregion

            #region User
            services.AddScoped<IRoleRepository,RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            #endregion
        }
    }
}
