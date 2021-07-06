using Innumerati.Processes.Implementations;
using Innumerati.Processes.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Innumerati
{
    public class Program
    {
        private ILogger<Program> _logger;
        private IRomanNumerals _romanNumerals;

        public Program(ILogger<Program> logger, IRomanNumerals romanNumerals)
        {
            _romanNumerals = romanNumerals;
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogInformation("Test " + _romanNumerals.IntToNumerals(39));
            foreach (var item in _romanNumerals.ListAll())
            {
                try
                {
                    Console.WriteLine(item.Key.ToString() + ": " + item.Value);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureServices(
                services =>
                {
                    services.AddTransient<Program>();
                    services.AddTransient<IRomanNumerals, RomanNumerals>();
                });
        }

        private static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<Program>().Run();
        }
    }
}