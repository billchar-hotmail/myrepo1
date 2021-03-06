using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureNotesWebClient.Services;
using Microsoft.Extensions.Logging;
using AspNetCore.Identity.Dapper;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SecureNotesWebClient
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            // removing this original code
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<ApplicationUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddDapperStores(connectionString)
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            //services.AddTransient<ISmsSender, AuthMessageSender>();


            // Add support for non-distributed memory cache in the application.
            services.AddMemoryCache();

            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false; // true
                options.Password.RequiredLength = 4; // 6;
                options.Password.RequireNonAlphanumeric = false; // true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false; // true;
                options.Password.RequiredUniqueChars = 1;
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 6;
                options.Lockout.AllowedForNewUsers = true;
                // User settings.
                options.User.RequireUniqueEmail = true;
            });

            // Configure cookie settings.
            services.ConfigureApplicationCookie(options => {
                options.Cookie.HttpOnly = false;
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.LoginPath = "/identity/account/login";
                options.LogoutPath = "/identity/account/log-off";
                options.AccessDeniedPath = "/identity/account/login";
                options.SlidingExpiration = true;
            });

            // Map appsettings.json file elements to a strongly typed class.
            //services.Configure<AppSettings>(Configuration);
            // Add services required for using options.
            //services.AddOptions();

            // Configure custom services to be used by the framework.
            //services.AddTransient<IDatabaseConnectionFactory>(e => new SqlConnectionFactory(connectionString));
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory>();
            //services.AddSingleton<ICacheManagerService, CacheManagerService>();
            //services.AddTransient<IUserRepository, UserRepository>();

            //services.AddTransient<IEmailService>(e => new EmailService(new SmtpSettings
            //{
            //    From = Configuration["SmtpSettings:From"],
            //    Host = Configuration["SmtpSettings:Host"],
            //    Port = int.Parse(Configuration["SmtpSettings:Port"]),
            //    SenderName = Configuration["SmtpSettings:SenderName"],
            //    LocalDomain = Configuration["SmtpSettings:LocalDomain"],
            //    Password = Configuration["SmtpSettings:Password"],
            //    UserName = Configuration["SmtpSettings:UserName"]
            //}));

            services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = "google.external"; //"idsrv.external";// IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = "448832310229-cd23f2bh45eijne91b1vivt983l9org6.apps.googleusercontent.com";
                    options.ClientSecret = "dttRxVHjtmy0d0UIDG2-ZQIG";
                });


            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(setupAction => {
                    // Configure the contract resolver that is used when serializing .NET objects to JSON and vice versa.
                    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
       }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory log)
        {
            log.AddConsole(Configuration.GetSection("Logging"));
            log.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
