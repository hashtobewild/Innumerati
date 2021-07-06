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
            ShowOptions();
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

        private void IntToNumeralConversion()
        {
            Console.WriteLine("Please enter an integer between 1 and 3999 and press enter:");

            var integerString = Console.ReadLine();
            try
            {
                int working;
                if (int.TryParse(integerString, out working))
                {
                    Console.WriteLine("That integer (" + working.ToString() + ") converts to the roman numeral: " + _romanNumerals.IntToNumerals(working));
                }
                else
                {
                    throw new InvalidOperationException("Unable to convert that integer to a numeral");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private void NumeralToIntConversion()
        {
            Console.WriteLine("Please enter a roman numeral between I and MMMCMXCIX and press enter:");
            string input = Console.ReadLine();

            try
            {
                Console.WriteLine("That roman numeral (" + input + ") converts to the integer: " + _romanNumerals.NumeralsToInt(input));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private void ParseOptions(string input)
        {
            Console.Clear();
            var working = input.Trim().ToLowerInvariant();
            switch (working)
            {
                case "1":
                    {
                        ShowList();
                    }
                    break;

                case "2":
                    {
                        IntToNumeralConversion();
                    }
                    break;

                case "3":
                    {
                        NumeralToIntConversion();
                    }
                    break;

                case "q":
                    {
                        Environment.Exit(0);
                    }
                    break;

                default:
                    {
                        _logger.LogError("That does not seem to be a valid option. Please try again.");
                    }
                    break;
            }
            // Restart the app loop
            ShowOptions();
        }

        private void ShowList()
        {
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

        private void ShowOptions()
        {
            Console.WriteLine("");
            Console.WriteLine("Welcome!");
            Console.WriteLine("--------");
            Console.WriteLine("");
            Console.WriteLine("Options:");
            Console.WriteLine("--------");
            Console.WriteLine("1. List All Roman Numerals");
            Console.WriteLine("2. Convert an integer to roman numeral");
            Console.WriteLine("3. Convert a roman numeral to an integer");
            Console.WriteLine("");
            Console.WriteLine("Q. Quit");
            Console.WriteLine("");
            Console.WriteLine("(Enter the number matching your choice and press enter, to continue)");
            Console.WriteLine("");
            ParseOptions(Console.ReadLine());
        }
    }
}