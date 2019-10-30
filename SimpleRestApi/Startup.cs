using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleRestApi.Repositories;
using Swashbuckle.AspNetCore.Swagger;

namespace SimpleRestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SimpleRestApiDbContext>(options =>
                   options
                       .UseLazyLoadingProxies()
                       .UseMySql(this.Configuration.GetConnectionString("Database"), dbOptions =>
                       {
                           dbOptions.CharSetBehavior(Pomelo.EntityFrameworkCore.MySql.Infrastructure.CharSetBehavior.AppendToUnicodeNonIndexAndKeyColumns);
                           dbOptions.UnicodeCharSet(Pomelo.EntityFrameworkCore.MySql.Infrastructure.CharSet.Utf8mb4);
                       })
               );
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "FirefliesApi", Version = "v1" }); });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SimpleRestApiDbContext context)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fireflies Api v1"); });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();

            context.Database.Migrate();
        }
    }
}
