using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using ToDoer.Api.Data;
using ToDoer.Api.Repository;
using Microsoft.OpenApi.Models;
using System.IO;
using ToDoer.Api.Filters;

namespace ToDoer.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnv;
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _hostingEnv = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ToDoerDataContext>(opt => opt.UseInMemoryDatabase(databaseName: "Test"));  
            services.AddTransient<ToDoerDataContext>();
            services.AddTransient<ITaskListRepository, TaskListRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "ToDo API",
                        Description = "ToDo API (ASP.NET Core 3.1)",
                        Contact = new OpenApiContact()
                        {
                           Name = "Sukant Agarwal",
                           Email = "sukant.agarwal@gmail.com"
                        }
                    });
                    c.CustomSchemaIds(type => type.FullName);

                    // Include DataAnnotation attributes on Controller Action parameters as Swagger validation rules (e.g required, pattern, ..)
                    // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                    c.OperationFilter<GeneratePathParamsValidationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API");

            });

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
