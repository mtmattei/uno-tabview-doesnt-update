namespace EBTWeather.Models;

public class GeoLocation(double latitude, double longitude, double elevation)
{
    public double Latitude { get; } = latitude;
    public double Longitude { get; } = longitude;

    public double Elevation { get; } = elevation;
    
    public override string ToString()
    {
        return $"Latitude: {Latitude} Longitude: {Longitude} Elevation: {Elevation}";
    }
}
