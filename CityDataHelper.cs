using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherClientTool.APIHelper;

namespace WeatherClientTool
{
    internal class CityDataHelper : ICityDataHelper
    {
        private readonly ILogger _logger;
        
        
        public CityDataHelper(ILogger<CityDataHelper> logger)
        {
            _logger = logger;
        }

        
        public async Task<string> HttpCall(string url)
        {
            try
            {
                _logger.LogInformation("Application {applicationEvent} at {dateTime}", "Started", DateTime.UtcNow);
                var httlClient = new HttpClient();
                using var client = new HttpClient();

                var response =  client.GetAsync(url);             


                _logger.LogInformation("HttpCall {applicationEvent} at {dateTime}", "End", DateTime.UtcNow);

                if (response.Result.IsSuccessStatusCode)
                {
                    var result= await response.Result.Content.ReadAsStringAsync();
                    return result.ToString();
                }
                else
                {
                    return $"StatusCode: {response.Result.StatusCode}";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<CityData>> GetCityData()
        {
            try
            {
                string text = File.ReadAllText(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName+@"/data/citydata.json");
                text=text.Replace("ā", "a");
                var cities = JsonSerializer.Deserialize<List<CityData>>(text);
                return cities;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
