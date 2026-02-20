using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;

namespace EBTWeather.WeatherData;

public record DailyWeatherInfo(
    DateOnly Date,
    double TemperatureMin, 
    double TemperatureMax, 
    string Sunrise, 
    string Sunset, 
    double DaylightDuration, 
    double SunshineDuration, 
    double UVIndexMax, 
    double RainSum, 
    double ShowersSum,
    double SnowFallSum,
    double PrecipitationSum,
    double PrecipitationHours,
    double PrecipitationProbabilityMax,
    double WindGustsMax,
    double WindSpeedMax,
    int WindDirectionDominant,
    string WeatherCode,
    List<HourlyWeatherInfo> HourlyWeatherInfo
    ) : IComparable<DailyWeatherInfo>
{
    public int CompareTo(DailyWeatherInfo? other)
    {
        return Date.CompareTo(other!.Date);
    }
}
