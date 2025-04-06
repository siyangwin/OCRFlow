using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using Kogel.Dapper.Extension;
using Project.Core;
using Project.Model.EnumModel;
using Project.MvcCore.Extension;
using Project.MvcCore.Extension.Auth;
using Project.MvcCore.Extension.AutoFac;
using Project.MvcCore.Extension.Swagger;
using Project.Service;
using Project.ViewModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Project.AppApi;

namespace Project.AppApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        private IServiceCollection Services;

        /// <summary>
        /// 
        /// </summary>
        private const string ApiName = "Project.AppApi";

        /// <summary>
        /// 
        /// </summary>
        private ResourceManage ResourceManage;

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            this.Services = services;
            //設置Cors共享不攔截
            services.AddCors(options =>
            {
                options.AddPolicy("cors", builders =>
                {
                    builders
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        ;
                });
            });
            //注入語言資源管理
            ResourceManage = new ResourceManage(new ResourceManager("Project.AppApi.Config.LanguageResource", typeof(Program).Assembly));
            services.AddSingleton(x => ResourceManage);
            //身份認證
            services.AddAuthentications(Configuration.GetSection("ConfigSettings:Domain").Value, "/api/appuser/denied");
            //获取域名地址
            GlobalConfig.ResourcesUrl = Configuration.GetSection("ConfigSettings:ResourcesUrl").Value;
            //获取连接字符串
            GlobalConfig.ConnectionString = Configuration.GetConnectionString("SqlServer");
            //第三方系统获取连接字符串
            GlobalConfig.OtherSystemConnectionString = Configuration.GetConnectionString("CloudniferServer");

            //注入配置讀取器
            services.AddScoped(x => Configuration);
            //注入log
            var log = services.AddLogs("Config/log4net.xml");
            //注入httphelper
            services.AddTransient(typeof(HttpHelper));

            //Swagger UI Service
            if (Convert.ToBoolean(Configuration.GetSection("ConfigSettings:SwaggerEnable").Value?.ToString() ?? "false"))
                services.AddSwaggerGens(ApiName, new string[] { "Project.ViewModel.xml" });
            //add mvc
            services.AddMvc(options =>
            {
                //過濾器
                //options.Filters.Add<ModelStateFilter>();
                //注入全局異常捕獲
            }).AddJsonOptions(options =>
            {
                //配置序列化时时间格式为yyyy/MM/dd HH:mm:ss
                options.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<ApiBehaviorOptions>(options =>
            {
                //禁用默認驗證行為
                options.SuppressModelStateInvalidFilter = true;
            });
            //注入配置日志
            GlobalConfig.SystemLogService = new SystemLogServic(log);
            GlobalConfig.AuthorizationTokenCoreService = new AuthorizationTokenCoreService();

            //将需要使用的文件夹，在此处创建。判断文件夹是否存在,不存在则创建
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"Files");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //AutoFac
            return services.AddAutoFacs(new string[] { "Project.Service.dll" });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("cors");
            //Swagger
            if (Convert.ToBoolean(Configuration.GetSection("ConfigSettings:SwaggerEnable").Value?.ToString() ?? "false"))
                app.UseSwaggers(ApiName);
            //身份認證
            app.AddUseAuthentications("/api/appuser/denied");

            //開啟靜態目錄瀏覽
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"Files")),
                RequestPath = new PathString("/Files"),
                //所有
                ServeUnknownFileTypes = true,
                //下面设置是指定类型的可下载
                //ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
                //{
                //     { ".apk","application/vnd.android.package-archive"}
                //})
            });
            //设置部分参数规范在头部
            app.Use(async (context, next) =>
            {
                context.Request.Headers.Add("ClientType", "APP");

                //注入Guid每次请求唯一编码
                context.Request.Headers.Add("GuidPwd", Guid.NewGuid().ToString("N"));
                //语言
                string language = context.QueryOrHeaders("language");
                if (string.IsNullOrEmpty(language))
                    language = ((int)LanguageEnum.CN).ToString();
                context.SetHeaders("Language", language);
                //token
                context.SetHeaders("Token", context.QueryOrHeaders("Token"));
                //街市id
                string marketId = context.QueryOrHeaders("marketId");
                if (string.IsNullOrEmpty(marketId))
                    marketId = Configuration.GetSection("Cloudnifier:MarketId").Value;
                context.SetHeaders("MarketId", marketId);

                await next.Invoke();
            });
            //mvc
            app.UseHttpsRedirection();
            app.UseMvc();
        }

    }
}
