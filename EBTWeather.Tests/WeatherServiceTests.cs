using EBTWeather.WebService;

namespace EBTWeather.Tests;

public class WeatherServiceTests
{
    private OpenMeteo _weatherService;
    
    private readonly LocationData _locationData = new(new GeoLocation(37.3489, -108.5859, 1972.0), "America/Denver");
    
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _weatherService = new OpenMeteo();
    }

    [Test]
    public async Task GetCurrentWeather()
    {
        var result = await _weatherService.GetCurrentWeather(_locationData);

        Assert.That(result.Temperature, Is.Not.NaN);
        Assert.That(result.RelativeHumidity, Is.Not.NaN);
        Assert.That(result.AirPressure, Is.Not.NaN);
        Assert.That(result.WindSpeed, Is.Not.NaN);
        Assert.That(result.WindDirection, Is.Not.NaN);
    }

    [Test]
    public async Task GetHistoricalWeather()
    {
        var startDate = new DateOnly(2026, 02, 09);
        var endDate = new DateOnly(2026, 02, 18);
        
        var result = await _weatherService.GetHistoricalWeather(_locationData, startDate, endDate);
    }
}
