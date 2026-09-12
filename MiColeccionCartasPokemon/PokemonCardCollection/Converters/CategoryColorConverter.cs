using Microsoft.Maui.Graphics;

namespace PokemonCardCollection.Converters;

public class CategoryColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is string category)
        {
            return category.ToLower() switch
            {
                "fire" => Color.FromArgb("#E3350D"), // Primary
                "water" => Color.FromArgb("#3168B1"), // Secondary
                "electric" or "lightning" => Color.FromArgb("#F9A825"), // Darker yellow for white text contrast
                "grass" => Color.FromArgb("#4CAF50"),
                "psychic" => Color.FromArgb("#9C27B0"),
                "ghost" => Color.FromArgb("#673AB7"),
                "dragon" => Color.FromArgb("#3F51B5"),
                "normal" or "colorless" => Color.FromArgb("#9E9E9E"),
                "darkness" or "dark" => Color.FromArgb("#212121"),
                "metal" or "steel" => Color.FromArgb("#607D8B"),
                "fairy" => Color.FromArgb("#E91E63"),
                "fighting" => Color.FromArgb("#D32F2F"),
                _ => Color.FromArgb("#79747E") // Outline
            };
        }
        return Color.FromArgb("#79747E");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        => throw new NotImplementedException();
}
