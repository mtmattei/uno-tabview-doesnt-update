namespace EBTWeather.Models;

public class LocationData(GeoLocation geoLocation, string timeZone)
{
    public GeoLocation GeoLocation { get; set; } = geoLocation;

    public string TimeZone { get; set; } = timeZone;

    public override string ToString()
    {
        return $"GeoLocation: {GeoLocation}, Timezone: {TimeZone}";
    }
}
