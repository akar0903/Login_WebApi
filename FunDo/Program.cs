using FunDoNotes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FunDo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logpath = Path.Combine(Directory.GetCurrentDirectory(), "LogFile");
            NLog.GlobalDiagnosticsContext.Set("myvar", logpath);
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging(config =>
                {
                    config.ClearProviders();
                    config.SetMinimumLevel(LogLevel.Debug);
                }).UseNLog();
    }
}