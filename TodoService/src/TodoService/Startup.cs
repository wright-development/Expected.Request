using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TodoService
{

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
 
    public RequestResponseLoggingMiddleware(RequestDelegate next,
                                            ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory
                  .CreateLogger<RequestResponseLoggingMiddleware>();
    }


private async Task<string> FormatRequest(HttpRequest request)
{
    var body = request.Body;
    request.EnableRewind();
 
    var buffer = new byte[Convert.ToInt32(request.ContentLength)];
    await request.Body.ReadAsync(buffer, 0, buffer.Length);
    var bodyAsText = Encoding.UTF8.GetString(buffer);
    request.Body = body;
 
    return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
}

private async Task<string> FormatResponse(HttpResponse response)
{
    response.Body.Seek(0, SeekOrigin.Begin);
    var text = await new StreamReader(response.Body).ReadToEndAsync(); 
    response.Body.Seek(0, SeekOrigin.Begin);
 
    return $"Response {text}";
}

    public async Task Invoke(HttpContext context)
{
    _logger.LogInformation(await FormatRequest(context.Request));
 
    var originalBodyStream = context.Response.Body;
 
    using (var responseBody = new MemoryStream())
    {
        context.Response.Body = responseBody;
 
        await _next(context);
 
        _logger.LogInformation(await FormatResponse(context.Response));
        await responseBody.CopyToAsync(originalBodyStream);
    }
}
}

public static class RequestResponseLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
    }
}
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
            .AddDataProtection()
            .SetDefaultKeyLifetime(TimeSpan.FromDays(14));
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            // loggerFactory.AddDebug();
            //app.UseRequestResponseLogging();
            app.UseMvc();

        }
    }
}
