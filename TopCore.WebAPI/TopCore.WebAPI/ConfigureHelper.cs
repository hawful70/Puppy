﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using TopCore.Framework.DependencyInjection;

namespace TopCore.WebAPI
{
    public static class ConfigureHelper
    {
        public static IConfigurationRoot Configuration;
        public static IHostingEnvironment Environment;

        internal static class DependencyInjection
        {
            public static void Service(IServiceCollection services)
            {
                services
                    .AddDependencyInjectionScanner()
                    .ScanFromAllAssemblies($"{nameof(TopCore)}.{nameof(WebAPI)}.*.dll", Path.GetFullPath(PlatformServices.Default.Application.ApplicationBasePath));

                // Write out all dependency injection services
                services.WriteOut($"{nameof(TopCore)}.{nameof(WebAPI)}");
            }
        }

        internal static class Api
        {
            public static void Service(IServiceCollection services)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy($"{nameof(TopCore)}.{nameof(WebAPI)}",
                        policy =>
                        {
                            policy
                                .WithOrigins()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        });
                });
            }

            public static void Middleware(IApplicationBuilder app)
            {
                app.UseCors($"{nameof(TopCore)}.{nameof(WebAPI)}");
            }
        }

        internal static class Mvc
        {
            public static void Service(IServiceCollection services)
            {
                services.AddMvc()
                    .AddXmlDataContractSerializerFormatters()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                        // Indented for Development only
                        options.SerializerSettings.Formatting = Environment.IsDevelopment()
                            ? Formatting.Indented
                            : Formatting.None;

                        // Serialize Json as Camel case
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    });
            }

            public static void Middleware(IApplicationBuilder app)
            {
                if (Environment.IsDevelopment())
                {
                    app.UseBrowserLink();
                }
                app.UseStaticFiles();

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"assets", "images", "favicons")),
                    RequestPath = new PathString("/favicons")
                });

                app.UseMvcWithDefaultRoute();
            }
        }

        internal static class Log
        {
            public static void Service(IServiceCollection services)
            {
                string logPath = Configuration.GetValue<string>("Developers:LogUrl");
                services.AddElm(options =>
                {
                    options.Path = new PathString(logPath);
                    options.Filter = (name, level) => level >= LogLevel.Error;
                });
            }

            public static void Middleware(IApplicationBuilder app, ILoggerFactory loggerFactory)
            {
                // Write log
                Serilog.Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();
                loggerFactory.AddSerilog();

                // Log page
                app.UseElmPage();
                app.UseElmCapture();
            }
        }

        internal static class Exception
        {
            public static void Middleware(IApplicationBuilder app)
            {
                if (Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                }
            }
        }

        internal static class Swagger
        {
            private static readonly string DocumentApiBaseUrl = Configuration.GetValue<string>("Developers:ApiDocumentUrl");
            private static readonly string DocumentTitle = Configuration.GetValue<string>("Developers:ApiDocumentTitle");
            private static readonly string DocumentName = Configuration.GetValue<string>("Developers:ApiDocumentName");
            private static readonly string documentJsonFile = Configuration.GetValue<string>("Developers:ApiDocumentJsonFile");

            public static void Service(IServiceCollection services)
            {
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(DocumentName, new Info
                    {
                        Title = DocumentTitle,
                        Version = DocumentName,
                        Contact = new Contact
                        {
                            Name = "Top Nguyen",
                            Email = "TopNguyen92@gmail.com",
                            Url = "http://topnguyen.net"
                        }
                    });

                    options.DescribeAllEnumsAsStrings();

                    var apiDocumentFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TopCore.WebAPI.xml");
                    options.IncludeXmlComments(apiDocumentFilePath);
                });
            }

            public static void Middleware(IApplicationBuilder app)
            {
                string documentFileBase = DocumentApiBaseUrl.Replace(DocumentName, string.Empty).TrimEnd('/');
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = documentFileBase.TrimStart('/') + "/{documentName}/" + documentJsonFile;
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Host = httpReq.Host.Value);
                });

                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = DocumentApiBaseUrl.TrimStart('/');
                    c.SwaggerEndpoint($"{documentFileBase}/{DocumentName}/{documentJsonFile}", DocumentTitle);
                });
            }
        }
    }
}