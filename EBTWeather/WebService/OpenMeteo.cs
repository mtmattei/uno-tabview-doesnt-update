using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EBTWeather.Models;
using EBTWeather.open_meteo;
using EBTWeather.WeatherData;

namespace EBTWeather.WebService;

public class OpenMeteo
{
    private HttpClient _httpClientForecast, _httpClientHistorical;

    private Dictionary<int, string> _decodeWeatherCode = new Dictionary<int, string>();

    public OpenMeteo()
    {
        _httpClientForecast = new HttpClient()
        {
            BaseAddress = new Uri("https://api.open-meteo.com")
        };

        _httpClientHistorical = new HttpClient()
        {
            BaseAddress = new Uri("https://historical-forecast-api.open-meteo.com")
        };

        #region weather codes
        _decodeWeatherCode[0] = "Clear";
        _decodeWeatherCode[1] = "Mostly Clear";
        _decodeWeatherCode[2] = "Partly Cloudy";
        _decodeWeatherCode[3] = "Cloudy";
        _decodeWeatherCode[45] = "Fog";
        _decodeWeatherCode[48] = "Freezing Fog";
        _decodeWeatherCode[51] = "Light Drizzle";
        _decodeWeatherCode[53] = "Drizzle";
        _decodeWeatherCode[55] = "Heavy Drizzle";
        _decodeWeatherCode[56] = "Light Freezing Drizzle";
        _decodeWeatherCode[57] = "Freezing Drizzle";
        _decodeWeatherCode[61] = "Light Rain";
        _decodeWeatherCode[63] = "Rain";
        _decodeWeatherCode[65] = "Heavy Rain";
        _decodeWeatherCode[66] = "Light Freezing Rain";
        _decodeWeatherCode[67] = "Freezing Rain";
        _decodeWeatherCode[71] = "Light Snow";
        _decodeWeatherCode[73] = "Snow";
        _decodeWeatherCode[75] = "Heavy Snow";
        _decodeWeatherCode[77] = "Snow Grains";
        _decodeWeatherCode[80] = "Light Rain Shower";
        _decodeWeatherCode[81] = "Rain Shower";
        _decodeWeatherCode[82] = "Heavy Rain Shower";
        _decodeWeatherCode[85] = "Snow Shower";
        _decodeWeatherCode[86] = "Heavy Snow Shower";
        _decodeWeatherCode[95] = "Thunderstorm";
        _decodeWeatherCode[96] = "Hailstorm";
        _decodeWeatherCode[99] = "Heavy Hailstorm";
        #endregion
    }

    public async Task<WeatherInfo> GetCurrentWeather(LocationData locationData)
    {
        Console.WriteLine($"GetCurrentWeather called at {DateTime.Now}");
        
        var requestUri = $"/v1/forecast?latitude={locationData.GeoLocation.Latitude}&longitude={locationData.GeoLocation.Longitude}&elevation={locationData.GeoLocation.Elevation}&timezone={locationData.TimeZone}&forecast_days=10&daily=temperature_2m_max,temperature_2m_min,apparent_temperature_max,apparent_temperature_min,sunrise,sunset,daylight_duration,sunshine_duration,uv_index_max,rain_sum,showers_sum,snowfall_sum,precipitation_sum,precipitation_hours,precipitation_probability_max,wind_gusts_10m_max,wind_speed_10m_max,wind_direction_10m_dominant,weather_code&hourly=precipitation_probability,apparent_temperature,temperature_2m,dewpoint_2m,wind_speed_10m,wind_direction_10m,wind_gusts_10m,relative_humidity_2m,pressure_msl,snowfall,rain,showers,weather_code,visibility&current=apparent_temperature,weather_code,temperature_2m,wind_speed_10m,relative_humidity_2m,wind_speed_10m,pressure_msl,wind_direction_10m&temperature_unit=celsius&wind_speed_unit=kmh&pressure_msl_unit=hPa";
        Console.WriteLine(requestUri);
        
        using var response = await _httpClientForecast.GetAsync(requestUri);
        
        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{jsonResponse}");

        var responseObject = JsonSerializer.Deserialize<Response>(jsonResponse);
        
        var location = new GeoLocation(responseObject.latitude, responseObject.longitude, responseObject.elevation);

        var hourlyWeatherInfo = responseObject.hourly.time.Select((dateTime, i) => 
            new HourlyWeatherInfo(
                dateTime, 
                responseObject.hourly.temperature_2m[i], 
                responseObject.hourly.apparent_temperature[i], 
                responseObject.hourly.dewpoint_2m[i], 
                responseObject.hourly.wind_speed_10m[i], 
                responseObject.hourly.wind_direction_10m[i], 
                responseObject.hourly.wind_gusts_10m[i], 
                responseObject.hourly.relative_humidity_2m[i], 
                responseObject.hourly.pressure_msl[i], 
                responseObject.hourly.snowfall[i], 
                responseObject.hourly.rain[i], 
                responseObject.hourly.showers[i], 
                responseObject.hourly.precipitation_probability[i], 
                responseObject.hourly.weather_code[i], 
                responseObject.hourly.visibility[i])).ToList();
        
        hourlyWeatherInfo.Sort();

        var dailyWeatherInfo = responseObject.daily.time.Select((date, i) => new DailyWeatherInfo(
            date, 
            responseObject.daily.temperature_2m_min[i], 
            responseObject.daily.temperature_2m_max[i], 
            responseObject.daily.sunrise[i], 
            responseObject.daily.sunset[i], 
            responseObject.daily.daylight_duration[i], 
            responseObject.daily.sunshine_duration[i], 
            responseObject.daily.uv_index_max[i], 
            responseObject.daily.rain_sum[i], 
            responseObject.daily.showers_sum[i], 
            responseObject.daily.snowfall_sum[i], 
            responseObject.daily.precipitation_sum[i], 
            responseObject.daily.precipitation_hours[i], 
            responseObject.daily.precipitation_probability_max[i], 
            responseObject.daily.wind_gusts_10m_max[i], 
            responseObject.daily.wind_speed_10m_max[i], 
            responseObject.daily.wind_direction_10m_dominant[i], 
            _decodeWeatherCode[responseObject.daily.weather_code[i]],
            new List<HourlyWeatherInfo>())).ToList();

        foreach (var dwi in dailyWeatherInfo)
        {
            var hourlyListForDay = hourlyWeatherInfo.Where(hd => DateOnly.FromDateTime(hd.DateTime).CompareTo(dwi.Date) == 0);
            dwi.HourlyWeatherInfo.AddRange(hourlyListForDay);
        }

        dailyWeatherInfo.Sort();
        
        return new WeatherInfo(
            location, 
            responseObject.current.time, 
            responseObject.current.temperature_2m, 
            responseObject.current.apparent_temperature,
            dailyWeatherInfo[0].TemperatureMin, 
            dailyWeatherInfo[0].TemperatureMax,
            dailyWeatherInfo[0].UVIndexMax,
            dailyWeatherInfo[0].Sunrise,
            dailyWeatherInfo[0].Sunset,
            responseObject.current.relative_humidity_2m, 
            responseObject.current.pressure_msl, 
            responseObject.current.wind_speed_10m, 
            responseObject.current.wind_direction_10m, 
            dailyWeatherInfo[0].PrecipitationProbabilityMax,
            _decodeWeatherCode[responseObject.current.weather_code],
            dailyWeatherInfo,
            hourlyWeatherInfo);
    }

    public async Task<HistoricalWeatherInfo> GetHistoricalWeather(LocationData locationData, DateOnly startDate, 
        DateOnly endDate)
    {
        Console.WriteLine($"GetHistoricalWeather called at {DateTime.Now}");

        var dateFormat = "yyyy-MM-dd";
        
        var startDateString = startDate.ToString(dateFormat, CultureInfo.InvariantCulture);
        var endDateString = endDate.ToString(dateFormat, CultureInfo.InvariantCulture);
        
        var requestUri = $"/v1/forecast?latitude={locationData.GeoLocation.Latitude}&longitude={locationData.GeoLocation.Longitude}&elevation={locationData.GeoLocation.Elevation}&timezone={locationData.TimeZone}&start_date={startDateString}&end_date={endDateString}&daily=weather_code,uv_index_max,temperature_2m_max,temperature_2m_min,apparent_temperature_min,apparent_temperature_max,sunrise,sunset,daylight_duration,sunshine_duration,precipitation_sum,rain_sum,showers_sum,snowfall_sum,precipitation_sum,precipitation_hours,wind_speed_10m_max,wind_gusts_10m_max,wind_direction_10m_dominant";
        Console.WriteLine(requestUri);
        
        using var response = await _httpClientHistorical.GetAsync(requestUri);
        
        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{jsonResponse}");

        var responseObject = JsonSerializer.Deserialize<Response>(jsonResponse);
        
        var dailyWeatherInfo = responseObject.daily.time.Select((date, i) => new DailyWeatherInfo(
            date, 
            responseObject.daily.temperature_2m_min[i], 
            responseObject.daily.temperature_2m_max[i], 
            responseObject.daily.sunrise[i], 
            responseObject.daily.sunset[i], 
            responseObject.daily.daylight_duration[i], 
            responseObject.daily.sunshine_duration[i], 
            responseObject.daily.uv_index_max[i], 
            responseObject.daily.rain_sum[i], 
            responseObject.daily.showers_sum[i], 
            responseObject.daily.snowfall_sum[i], 
            responseObject.daily.precipitation_sum[i], 
            responseObject.daily.precipitation_hours[i],
            0, 
            responseObject.daily.wind_gusts_10m_max[i], 
            responseObject.daily.wind_speed_10m_max[i], 
            responseObject.daily.wind_direction_10m_dominant[i], 
            _decodeWeatherCode[responseObject.daily.weather_code[i]],
            new List<HourlyWeatherInfo>())).ToList();

        dailyWeatherInfo.Sort();
        
        var historicalWeatherInfo = new HistoricalWeatherInfo(dailyWeatherInfo);

        return historicalWeatherInfo;
    }
}
