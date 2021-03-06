﻿using System;
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
using DotNetEnv;

namespace Auth
{
    public class Startup
    {
      public String _connectionString;
      public String[] _host  = new String[2];
        public Startup(IConfiguration configuration)
        {  
           _connectionString = $@"host=mysqlDb;Database=iot; Uid=root; Pwd=105481Do";   
            String config = _connectionString;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //WaitForDBInit(_connectionString);
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
            int count2;
            //string cmdStr = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'iot' AND table_name = 'user'";
            string cmdStr = "create table user(id int(10) auto_increment primary key,name varchar(50),password varchar(50),email varchar(50),flag varchar(2))";
    
            while (retries < 7)
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(cmdStr, connection);
                    int rows = cmd.ExecuteNonQuery();
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