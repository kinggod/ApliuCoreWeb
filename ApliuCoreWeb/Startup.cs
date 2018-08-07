﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApliuCoreWeb.Models;
using ApliuCoreWeb.Models.WeChat;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApliuCoreWeb
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //注册HttpContext
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //注册缓存服务
            services.AddMemoryCache();
            services.AddSingleton<MemoryCacheCore>();//自定义缓存

            //注册SignalR
            services.AddSignalR();

            //注册Session
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(300);
                options.Cookie.HttpOnly = true;
            });
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();//关闭Https重定向
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //添加Hub路由
            app.UseSignalR(routes =>
            {
                routes.MapHub<WeChatHub>("/weChatHub");
            });

            //开启Session
            app.UseSession();

            //https://docs.microsoft.com/zh-cn/aspnet/core/signalr/hubcontext?view=aspnetcore-2.1
            //app.Use(next => (context) =>
            //{
            //    var hubContext = (Microsoft.AspNetCore.SignalR.IHubContext<WeChatHub>)context
            //                        .RequestServices
            //                        .GetServices<Microsoft.AspNetCore.SignalR.IHubContext<WeChatHub>>();
            //    return null;
            //});
        }
    }
}
