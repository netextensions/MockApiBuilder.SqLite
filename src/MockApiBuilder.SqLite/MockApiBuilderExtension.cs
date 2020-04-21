using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NetExtensions
{
    public static class MockApiBuilderExtension
    {
        public static IServiceCollection AddMockApi<TContext, TAutoMapper, TMediatR>(this IServiceCollection services, string connectionString,
             string swaggerTitle = null,
            string swaggerDescription = null,
            string swaggerVersion = null)
            where TContext : DbContext 
        {
            services.AddSwashbuckle(swaggerTitle, swaggerDescription, swaggerVersion);
            services.AddControllers().AddNewtonsoftJson();
            services.AddSqliteDb<TContext>(connectionString);
            services.AddAutoMapper(typeof(TAutoMapper).Assembly);
            services.AddMediatR(typeof(TMediatR).Assembly);
            return services;
        }
        public static IServiceCollection AddMockApi<TContext, TAutoMapperMediatR>(this IServiceCollection services, string connectionString,
            string swaggerTitle = null,
            string swaggerDescription = null,
            string swaggerVersion = null)
            where TContext : DbContext
        {
            return services.AddMockApi<TContext, TAutoMapperMediatR, TAutoMapperMediatR>(connectionString, swaggerTitle, swaggerDescription, swaggerVersion);
        }

        public static IApplicationBuilder UseMockApi(this IApplicationBuilder app, IWebHostEnvironment env, string swaggerName = null,
            string swaggerEndpoint = null, bool useSerilogMiddleware = true)
        {
            app.AddSwashbuckle(swaggerName, swaggerEndpoint);
            app.AddSerilogRequestLogging(useSerilogMiddleware);
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            return app;
        }
    }
}
