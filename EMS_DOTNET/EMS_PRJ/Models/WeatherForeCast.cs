using System;
using System.Collections.Generic;

namespace EmployeeManagementSystem.Models
{
    public class WeatherForeCast
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double GenerationTimeMilliseconds { get; set; }
        public int UtcOffsetSeconds { get; set; }
        public string Timezone { get; set; }
        public string TimezoneAbbreviation { get; set; }
        public double Elevation { get; set; }
        public HourlyUnits Hourly_Units { get; set; }
        public Hourly Hourly { get; set; }
    }

    public class HourlyUnits
    {
        public string Time { get; set; }
        public string Temperature_2m { get; set; }
        public string RelativeHumidity_2m { get; set; }
    }

    public class Hourly
    {
        public List<DateTime> Time { get; set; }
        public List<double> Temperature_2m { get; set; }
        public List<int> Relative_Humidity_2m { get; set; }
    }
}
