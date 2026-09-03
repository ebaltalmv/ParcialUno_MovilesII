using Microsoft.Maui.Graphics;

namespace PokemonCardCollection.Converters;

public class RarityColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is string rarity)
        {
            return rarity.ToLower() switch
            {
                "common" => Color.FromArgb("#9E9E9E"),
                "uncommon" => Color.FromArgb("#4CAF50"),
                "rare" => Color.FromArgb("#2196F3"),
                "rare holo" => Color.FromArgb("#FF9800"),
                "ultra rare" => Color.FromArgb("#E91E63"),
                _ => Color.FromArgb("#79747E")
            };
        }
        return Color.FromArgb("#79747E");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        => throw new NotImplementedException();
}
