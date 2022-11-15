using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using GroupC.Uni.Core.Services;
using GroupC.Uni.Infrastructure;
using GroupC.Uni.Infrastructure.Data;
using GroupC.Uni.Infrastructure.Logging;
using GroupC.Uni.Student.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AppDbContext = GroupC.Uni.Infrastructure.AppDbContext;
namespace GroupC.Uni.Student
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().Build());
            });

            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            //services.AddControllers();

            var jwtSection = Configuration.GetSection("JWTSettings");
            services.Configure<JWTSettings>(jwtSection);
            
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            //what to validate
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            //setup validate data
            //            ValidIssuer = Configuration["Jwt:Issuer"],
            //            ValidAudience = Configuration["Jwt:Issuer"],
            //            IssuerSigningKey = symmetricSecurityKey
            //        };

            //    });

            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            //Services
            services.AddScoped(typeof(IService<>), typeof(Service<>));
            services.AddScoped(typeof(IQuestionService), typeof(QuestionService));
            services.AddScoped(typeof(ICourseService), typeof(CourseService));
            services.AddScoped(typeof(ITopicService), typeof(TopicService));
            services.AddScoped(typeof(IChoiceService), typeof(ChoiceService));
            services.AddScoped(typeof(IStudentService), typeof(StudentService));
            services.AddScoped(typeof(IExamService), typeof(ExamService));
            services.AddScoped(typeof(ITestCenterService), typeof(TestCenterService));
            services.AddScoped(typeof(IGenerateExamService), typeof(GenerateExamService));

            //Repositories
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(ICourseRepository), typeof(CourseRepository));
            services.AddScoped(typeof(ITopicRepository), typeof(TopicRepository));
            services.AddScoped(typeof(IQuestionRepository), typeof(QuestionRepository));
            services.AddScoped(typeof(IChoiceRepository), typeof(ChoiceRepository));
            services.AddScoped(typeof(IStudentRepository), typeof(StudentRepository));
            services.AddScoped(typeof(IExamRepository), typeof(ExamRepository)); 
            services.AddScoped(typeof(ITestCenterRepository), typeof(TestCenterRepository));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IService<>), typeof(Service<>));
            services.AddScoped(typeof(ISubmissionRepository), typeof(SubmissionRepository));
            services.AddScoped(typeof(ISubmissionService), typeof(SubmissionService));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //remove this//
                app.UseDatabaseErrorPage();
            }
           
            app.UseAuthentication();
            app.UseMvc();



        }
    }
}
