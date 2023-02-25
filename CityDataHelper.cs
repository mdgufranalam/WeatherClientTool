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
                _logger.LogInformation("HttpCall {applicationEvent} at {dateTime}", "Started", DateTime.UtcNow);
                
                using var client = new HttpClient();

                var response = client.GetAsync(url);             


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

                return ex.Message;
            }

        }

        public async Task<List<CityData>> GetCityData()
        {
            try
            {
                string filePath = $"{Directory.GetCurrentDirectory()}/Data/CityData.json";
                if(File.Exists(filePath))
                {
                    string text = File.ReadAllText(filePath);
                    text = text.Replace("ā", "a");
                    var cities = JsonSerializer.Deserialize<List<CityData>>(text);
                    return cities;
                }
                else
                {
                    throw new Exception("File not found at :"+ filePath);
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
