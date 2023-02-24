// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Runtime.CompilerServices;
using WeatherClientTool;
using WeatherClientTool.APIHelper;

var builder = new HostBuilder()
.ConfigureServices((hostContext, services) =>
{
    services.AddHttpClient();
    services.AddTransient<CityDataHelper>();
    
}).UseConsoleLifetime();
var serviceCollection = new ServiceCollection();
serviceCollection.AddLogging();
var serviceProvider = serviceCollection.BuildServiceProvider();
var _logger = serviceProvider.GetService<ILogger<CityDataHelper>>();


builder.Build();

ICityDataHelper cityDataHelper = new CityDataHelper(_logger);

List<CityData> cities=new List<CityData>();
cities = cityDataHelper.GetCityData().Result;
string cityName = string.Empty;
while(true)
{
    Console.WriteLine("\nPlease Ente City Name : ");
    cityName = Console.ReadLine();
    var cityinfo = cities.Where(p => p.city.ToLower() == cityName.ToLower()).ToList();
    if (cityName == string.Empty)
    {
        Console.WriteLine("Please provide City Name.");
        continue;
    }
    else if (cityName == "X" || cityName.ToLower()=="exit")
    {
        Environment.Exit(0);
    }
    else if(cityinfo.Count() == 0)
    {
        Console.WriteLine("Invalid City Name.");
        continue;
    }
    else
    {
        decimal lng= 0;
            Decimal.TryParse(cityinfo[0].lng,out lng);
        decimal lat = 0;
             Decimal.TryParse(cityinfo[0].lat, out lat); 
        if(lng > 0 && lat > 0)
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude=latParam&longitude=lngParam&current_weather=true";
            url=url.Replace("latParam", lat.ToString()).Replace("lngParam", lng.ToString());
            string weatherInfo = await cityDataHelper.HttpCall(url);
            Console.WriteLine("Weather Info : "+weatherInfo.ToString());
        }

    }
}

