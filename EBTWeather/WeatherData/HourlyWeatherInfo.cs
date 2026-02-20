using System;

namespace EBTWeather.WeatherData;

public record HourlyWeatherInfo(
    DateTime DateTime,
    double Temperature,
    double ApparentTemperature,
    double DewPoint,
    double WindSpeed,
    int WindDirection,
    double WindGusts,
    double RelativeHumidity,
    double AirPressure,
    double SnowFall,
    double Rain,
    double Showers,
    double PrecipitationProbability,
    int WeatherCode,
    double Visibility
    ) : IComparable<HourlyWeatherInfo>
{
    public int CompareTo(HourlyWeatherInfo? other)
    {
        return DateTime.CompareTo(other!.DateTime);
    }
}
