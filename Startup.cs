using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

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

          /*  
            var envs = Environment.GetEnvironmentVariables();
            var DBUSER = envs["DBUSER"];
            var host = envs["DBHOST"];
            var port = envs["DBPORT"];
            var password = envs["DBPASSWORD"];
            var db = envs["DATABASE"];
            var DefaultConnection = "server={host};port={port};database={db};user={DBUSER};password={password};";
            */
            //_connectionString = $@"Server={_config["MYSQL_SERVER_NAME"]}; Database={_config["MYSQL_DATABASE"]}; Uid={_config["MYSQL_USER"]}; Pwd={_config["MYSQL_PASSWORD"]}";
      
            _connectionString = "Host= 172.17.0.2;Database=iot;Port=3306;User=root;Password=105481Do";
            
      
            String config = _connectionString;//Configuration["ConnectionStrings:DefaultConnection".ToString()];
            _host[0] = "172.31.30.175";
            _host[1] = "25";
            _connectionString = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            WaitForDBInit(_connectionString);
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

        private static void WaitForDBInit(string connectionString)
        {         
            var connection = new MySqlConnection(connectionString);
            int retries = 1;
            while (retries < 7)
            {
                try
                {
                    Console.WriteLine("Connecting to db. Trial: {0}", retries);
                    connection.Open();
                    connection.Close();
                    break;
                }
                catch (Exception ex)
                {  
                   Console.WriteLine("doga");
                    Console.WriteLine(ex);
                    Thread.Sleep((int) Math.Pow(2, retries) * 1000);
                    retries++;
                }
            }
    }
}
}