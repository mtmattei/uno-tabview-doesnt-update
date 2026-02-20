using System;
using System.Collections.Generic;
using EBTWeather.Models;
using Microsoft.Kiota.Abstractions;

namespace EBTWeather.WeatherData;

public record WeatherInfo(
    GeoLocation Location, 
    string Time,
    double Temperature,
    double ApparentTemperature,
    double TemperatureMin,
    double TemperatureMax,
    double UVIndexMax,
    string SunRise,
    string SunSet,
    double RelativeHumidity, 
    double AirPressure, 
    double WindSpeed, 
    int WindDirection,
    double PrecipitationProbability,
    string WeatherCode,
    List<DailyWeatherInfo> DailyWeatherInfo,
    List<HourlyWeatherInfo> HourlyWeatherInfo);
