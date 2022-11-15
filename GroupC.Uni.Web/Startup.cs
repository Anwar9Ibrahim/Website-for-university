using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GroupC.Uni.Infrastructure;
using GroupC.Uni.Infrastructure.Data;
using GroupC.Uni.Core;
using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using GroupC.Uni.Core.Services;
using GroupC.Uni.Infrastructure.Logging;
using AppDbContext = GroupC.Uni.Infrastructure.AppDbContext;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace GroupC.Uni.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.LoginPath = "/Account/Login";
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDefaultIdentity<ApplicationUser>()
            //    .AddDefaultUI(UIFramework.Bootstrap4)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            //    services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
            //.AddDefaultUI()
            //.AddDefaultTokenProviders();
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();


            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
               .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
               .AddDataAnnotationsLocalization()
               .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMvc(
                options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser().Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                })
               .AddXmlSerializerFormatters()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
               .AddDataAnnotationsLocalization();

            services.AddHttpContextAccessor();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<CookieTempDataProviderOptions>(options => {
                options.Cookie.IsEssential = true;
            });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en"),
                        new CultureInfo("ar")
                    };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            

            //Added 6/12/2019
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            //Services
            services.AddScoped(typeof(IService<>), typeof(Service<>));
            services.AddScoped(typeof(IQuestionService), typeof(QuestionService));
            services.AddScoped(typeof(ICourseService), typeof(CourseService));
            services.AddScoped(typeof(ITopicService), typeof(TopicService));
            services.AddScoped(typeof(IChoiceService), typeof(ChoiceService));
            services.AddScoped(typeof(IStudentService), typeof(StudentService));
            services.AddScoped(typeof(ITestCenterService), typeof(TestCenterService));
            services.AddScoped(typeof(IGenerateExamService), typeof(GenerateExamService));
            services.AddScoped(typeof(IExamService), typeof(ExamService));
            services.AddScoped(typeof(IAdminService), typeof(AdminService));

            //Repositories
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(ICourseRepository), typeof(CourseRepository));
            services.AddScoped(typeof(ITopicRepository), typeof(TopicRepository));
            services.AddScoped(typeof(IQuestionRepository), typeof(QuestionRepository));
            services.AddScoped(typeof(IChoiceRepository), typeof(ChoiceRepository));
            services.AddScoped(typeof(IStudentRepository), typeof(StudentRepository));
            services.AddScoped(typeof(ITestCenterRepository), typeof(TestCenterRepository));
            services.AddScoped(typeof(IExamRepository), typeof(ExamRepository));
            services.AddScoped(typeof(IAdminRepository), typeof(AdminRepository));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Error");
                //app.UseStatusCodePagesWithReExecute("/Error/{0}");
                // app.UseStatusCodePagesWithRedirects("/Error/{0}");
                app.UseDatabaseErrorPage();
            }
            else
            {
                //  app.UseExceptionHandler("/Home/Error");
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            var supportedCultures = new[]
{
                new CultureInfo("en"),
                new CultureInfo("ar"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=index}/{id?}");

            });
            app.UseCookiePolicy();
        }
    }
}