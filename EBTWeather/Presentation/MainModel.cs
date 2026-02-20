using System;
using System.Threading.Tasks;
using EBTWeather.Models;
using EBTWeather.WeatherData;
using EBTWeather.WebService;
using Uno.Extensions.Reactive;

namespace EBTWeather.Presentation;

public partial record MainModel
{
    public MainModel()
    {
        Title = "Main";

        LocationData = new LocationData(new GeoLocation(37.3489, -108.5859, 1972.0), "America/Denver");
    }

    public WeatherService WeatherService = new();

    public IState<WeatherInfo> CurrentAndFutureWeather => State.Async(this, async ct => await WeatherService.GetCurrentWeather());
    
    private DateOnly _startDate = new(2026, 02, 09);
    private DateOnly _endDate = new(2026, 02, 18);
    
    public IState<HistoricalWeatherInfo> HistoricalWeather =>
        State.Async(this, async ct => await WeatherService.GetHistoricalWeather(_startDate, _endDate));
    
    public string? Title { get; }
    
    public LocationData LocationData { get; }

    public WeatherInfo WeatherInfo { get; set; }
}
