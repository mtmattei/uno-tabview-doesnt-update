using System;

namespace EBTWeather.open_meteo;

public class Response
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public int utc_offset_seconds { get; set; }
    public string timezone { get; set; }
    public string timezone_abbreviation { get; set; }
    public double elevation { get; set; }
    public Current current { get; set; }
    public Daily daily { get; set; }
    public Hourly hourly { get; set; }
}

public class Current
{
    public string time { get; set; }
    public int interval { get; set; }
    public int weather_code { get; set; }
    public double temperature_2m { get; set; }
    public double apparent_temperature { get; set; }
    public double wind_speed_10m { get; set; }
    public int relative_humidity_2m { get; set; }
    public double pressure_msl { get; set; }
    public int wind_direction_10m { get; set; }
    public double visibility { get; set; }
}

public class Daily
{
    public DateOnly[] time { get; set; }
    public double[] temperature_2m_max  { get; set; }
    public double[] temperature_2m_min { get; set; }
    public double[] apparent_temperature_max { get; set; }
    public double[] apparent_temperature_min { get; set; }
    public string[] sunrise { get; set; }
    public string[] sunset { get; set; }
    public double[] daylight_duration { get; set; }
    public double[] sunshine_duration { get; set; }
    public double[] uv_index_max { get; set; }
    public double[] rain_sum { get; set; }
    public double[] showers_sum { get; set; }
    public double[] snowfall_sum { get; set; }
    public double[] precipitation_sum { get; set; }
    public double[] precipitation_hours { get; set; }
    public double[] precipitation_probability_max { get; set; }
    public double[] wind_gusts_10m_max { get; set; }
    public double[] wind_speed_10m_max { get; set; }
    public int[] wind_direction_10m_dominant { get; set; }
    public int[] weather_code { get; set; }
}

public class Hourly
{
    public DateTime[] time { get; set; }
    public double[] temperature_2m {  get; set; }
    public double[] apparent_temperature { get; set; }
    public double[] dewpoint_2m {  get; set; }
    public double[] wind_speed_10m {  get; set; }
    public int[] wind_direction_10m {  get; set; }
    public double[] wind_gusts_10m {  get; set; }
    public double[] relative_humidity_2m {  get; set; }
    public double[] pressure_msl {  get; set; }
    public double[] snowfall {  get; set; }
    public double[] rain {  get; set; }
    public double[] showers {  get; set; }
    public double[] precipitation_probability { get; set; }
    public int[] weather_code {  get; set; }
    public double[] visibility {  get; set; }
}
