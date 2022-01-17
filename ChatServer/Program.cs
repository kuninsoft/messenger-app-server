using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChatServer
{
    public class Program
    {
        private const string ListenedIp = "192.168.3.69";
        private const int ListenedPortHttp = 5000;
        private const int ListenedPortHttps = 5001;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>().UseUrls(
                    string.Format("http://{0}:{1}", ListenedIp, ListenedPortHttp),
                    string.Format("https://{0}:{1}", ListenedIp, ListenedPortHttps)); });
    }
}