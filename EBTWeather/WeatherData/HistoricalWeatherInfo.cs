using System.Collections.Generic;

namespace EBTWeather.WeatherData;

public record HistoricalWeatherInfo(List<DailyWeatherInfo> DailyWeatherInfo);
