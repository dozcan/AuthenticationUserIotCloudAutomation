using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;

namespace Auth
{
    public class Startup
    {
      public String _connectionString;
      public String[] _host  = new String[2];


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            
            var envs = Environment.GetEnvironmentVariables();
            var DBUSER = envs["DBUSER"];
            var host = envs["DBHOST"];
            var port = envs["DBPORT"];
            var password = envs["DBPASSWORD"];
            var db = envs["DATABASE"];
            var DefaultConnection = "server={host};port={port};database={db};user={DBUSER};password={password};";
            

            String config = DefaultConnection;//Configuration["ConnectionStrings:DefaultConnection".ToString()];
            _host[0] = "172.31.30.175";
            _host[1] = "25";
            _connectionString = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.Add(new ServiceDescriptor(typeof(Auth.Models.TestContext),new Auth.Models.TestContext(_connectionString,_host)));
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
