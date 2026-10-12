using System.ComponentModel.DataAnnotations;
using PokemonCardCollection.Data.DTOs;

namespace PokemonCardCollection.Models;

/// <summary>
/// Modelo de datos para el formulario de creación de cartas Pokémon.
/// Implementa validaciones declarativas mediante DataAnnotations requeridas para EditForm.
/// </summary>
public class CreateCardModel
{
    [Required(ErrorMessage = "El nombre de la carta es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La categoría o tipo es obligatoria.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "La categoría debe tener entre 3 y 50 caracteres.")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "La rareza de la carta es obligatoria.")]
    public string Rarity { get; set; } = "Common";

    [Required(ErrorMessage = "La condición física es obligatoria.")]
    public string Condition { get; set; } = "Near Mint";

    [Required(ErrorMessage = "El valor estimado es obligatorio.")]
    [Range(1, 100000, ErrorMessage = "El valor estimado debe estar entre 1 y 100,000 USD.")]
    public decimal EstimatedValue { get; set; } = 10m;

    [Url(ErrorMessage = "La URL de la imagen debe tener un formato web válido (http:// o https://).")]
    public string ImageUri { get; set; } = string.Empty;

    public bool IsFavorite { get; set; } = false;

    [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Convierte el modelo de formulario a un DTO de transferencia compatible con el servicio OData.
    /// </summary>
    public PokemonCardDto ToDto()
    {
        return new PokemonCardDto
        {
            Name = Name?.Trim() ?? string.Empty,
            Category = Category?.Trim() ?? string.Empty,
            Rarity = Rarity?.Trim() ?? "Common",
            Condition = Condition?.Trim() ?? "Near Mint",
            EstimatedValue = EstimatedValue,
            ImageUri = string.IsNullOrWhiteSpace(ImageUri)
                ? "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/items/poke-ball.png"
                : ImageUri.Trim(),
            IsFavorite = IsFavorite,
            Description = Description?.Trim() ?? string.Empty
        };
    }
}
