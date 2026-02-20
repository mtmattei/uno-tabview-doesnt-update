using System;
using System.Threading;
using System.Threading.Tasks;
using EBTWeather.Models;
using EBTWeather.WeatherData;

namespace EBTWeather.WebService;

public class WeatherService
{
    private OpenMeteo _openMeteo = new();

    private LocationData _locationData = new LocationData(new GeoLocation(37.3489, -108.5859, 1972.0), "America/Denver");
    
    public async Task<WeatherInfo> GetCurrentWeather()
    {
        return await _openMeteo.GetCurrentWeather(_locationData);
    }

    public async Task<HistoricalWeatherInfo> GetHistoricalWeather(DateOnly startDate, DateOnly endDate)
    {
        return await _openMeteo.GetHistoricalWeather(_locationData, startDate, endDate);
    }
}
