using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using AtHomeProject.Data;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Validators;
using AtHomeProject.Web.Auth;
using AtHomeProject.Web.Extensions;
using Autofac;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Serilog.ILogger;

namespace AtHomeProject.Web
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.Console(new RenderedCompactJsonFormatter())
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        public ILogger Logger { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(o =>
                o.UseInMemoryDatabase("AtHomeDb")
            );

            services.AddProblemDetails(options =>
            {
                // Control when an exception is included
                options.IncludeExceptionDetails = (ctx, ex) => true;

                options.Map<Exception>(exception =>
                    {
                        var realError = exception;

                        while (realError.InnerException != null)
                            realError = realError.InnerException;

                        return new ProblemDetails
                        {
                            Status = (int)HttpStatusCode.BadRequest,
                            Detail = realError.Message
                        };
                    }
                );
            });

            services.AddControllers()
                .AddNewtonsoftJson()
                .AddXmlSerializerFormatters()
                .AddXmlDataContractSerializerFormatters()
                .AddFluentValidation(s =>
                    {
                        s.RegisterValidatorsFromAssemblyContaining<UserModelValidator>();
                        s.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    }
                );

            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
            services.AddSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog(Logger);

            app.UseSwagger();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseProblemDetails();
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}"
                );
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Add any Autofac modules or registrations.
            // register here OVERRIDE things registered in ConfigureServices.
            // This is called AFTER ConfigureServices so things you
            //
            // You must have the call to `UseServiceProviderFactory(new AutofacServiceProviderFactory())`
            // when building the host or this won't be called.
            builder.RegisterModule(new AutofacModule());
        }
    }
}
