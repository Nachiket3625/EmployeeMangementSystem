using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using EmployeeManagementSystem.Data;


namespace EmployeeManagementSystem.Controllers
{
    public class WeatherInfoController : Controller
    {
        private readonly WeatherInfo _weatherService;

        public WeatherInfoController(WeatherInfo weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<IActionResult> Index(string city)
        {
            if (string.IsNullOrEmpty(city))
                city = "Mumbai"; 

            try
            {
                var weatherForecast = await _weatherService.GetWeatherForecastAsync(city);
                return View(weatherForecast);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving weather forecast: {ex.Message}";
                return View();
            }
        }
    }
}
