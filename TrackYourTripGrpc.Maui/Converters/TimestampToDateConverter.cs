using Google.Protobuf.WellKnownTypes;
using System.Globalization;

namespace TrackYourTripGrpc.Maui.Converters;

public class TimestampToDateConverter : IValueConverter
{
    public object? Convert(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is Timestamp ts)
        {
            return ts.ToDateTime().ToString("dd-MM-yyyy");
        }
        return string.Empty;
    }

    public object? ConvertBack(object? value, System.Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
