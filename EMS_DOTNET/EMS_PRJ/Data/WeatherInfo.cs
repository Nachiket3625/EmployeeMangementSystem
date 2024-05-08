namespace EmployeeManagementSystem.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Policy;
    using RestSharp;
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using Azure;
    using EmployeeManagementSystem.Models;

    public class WeatherInfo
    {
        private readonly HttpClient _httpClient;
        String queryString;

        public WeatherInfo(HttpClient httpClient)
        {
            string url = "https://api.open-meteo.com/v1/forecast";

            // Create query parameters
            var parameters = new Dictionary<string, string>
        {
            { "latitude", "52.52" },
            { "longitude", "13.41" },
            { "hourly", "temperature_2m,relative_humidity_2m" },
            { "timezone", "GMT" },
            { "forecast_days", "1" }
        };

            // Create query string
            queryString = string.Join("&", parameters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));

            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.71 Safari/537.36");
        }

        public async Task<WeatherForeCast> GetWeatherForecastAsync(string city)
        {
            string url = $"https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&hourly=temperature_2m,relative_humidity_2m&timezone=GMT&forecast_days=1";

            var client = new RestClient(url);

            // Create request
            var request = new RestRequest();

            // Execute request asynchronously
            var response = await client.ExecuteAsync(request);

            // Check response status code
            if (response.IsSuccessful)
            {
                // Deserialize response content
                var content = response.Content;
                var data = JsonConvert.DeserializeObject<WeatherForeCast>(content);

                // Process deserialized data
                Console.WriteLine($"Data: {data}");
                return data;
            }
            else
            {
                throw new Exception($"Failed to retrieve weather forecast. Status code: {response.StatusCode}");
            }
        }
    }

   

}
