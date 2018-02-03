using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using MQTTDatabaseApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: OwinStartup(typeof(MQTTDatabaseApp.App_Start.Startup))]

namespace MQTTDatabaseApp.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
           
            app.
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = "Server = mosquittodatabase.cffo0eijbrmb.us - east - 1.rds.amazonaws.com,1433; Database = MQTT; Integrated Security = False; User ID = admin; Password = mosquitto; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False; ";
            services.AddDbContext<MQTTContext>(opt => opt.UseSqlServer(connection));
        }

    }
}

