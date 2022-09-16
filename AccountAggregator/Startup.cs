using AccountAggregator._GlobalHelper.Filter.ExceptionMiddleware;
using AccountAggregator.InterfaceLayer;
using AccountAggregator.ServiceLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;

namespace AccountAggregator
{
    public class Startup
    {
        public static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static string projectPath = appDirectory.Substring(0, appDirectory.IndexOf("\\bin"));
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(projectPath, "/nlog.config"));

            LogManager.Configuration.Variables["mydir"] = string.Concat(projectPath, "/bin/Logger");

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //services.AddMvc(config =>
            //{
            //    config.Filters.Add(typeof(CustomExceptionFilter));
            //});

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("EnableCORS", builder =>
            //     {
            //         builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            //     });
            //});
            services.AddSingleton<IBOLRequestNResponse, ClsBolRequestNResponse>();
            services.AddSingleton<IAuthentication, Authentication>();
            services.AddSingleton<IConsent, ConsentService>();
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

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseMvc();
            //app.UseCors("EnableCORS");



            //app.UseExceptionHandler(appError =>
            //{
            //    appError.Run(async context =>
            //    {
            //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //        context.Response.ContentType = "application/json";
            //        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            //        if (contextFeature != null)
            //        {
            //            await context.Response.WriteAsync(context.Response.StatusCode.ToString());
            //        }
            //    });
            //});
        }
    }
}
