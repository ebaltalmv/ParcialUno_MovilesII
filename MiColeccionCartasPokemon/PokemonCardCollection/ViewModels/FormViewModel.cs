using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokemonCardCollection.Data;
using PokemonCardCollection.Models;

namespace PokemonCardCollection.ViewModels;

/// <summary>
/// ViewModel for the Form page — reusable for both Add and Edit operations.
/// When cardId == 0, a new card is created; otherwise the existing card is edited.
/// </summary>
[QueryProperty(nameof(CardId), "cardId")]
public partial class FormViewModel : ObservableObject
{
    private readonly PokemonCardRepository _repository;
    private bool _isEditing;

    [ObservableProperty]
    private int _cardId;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _category = string.Empty;

    [ObservableProperty]
    private string _rarity = string.Empty;

    [ObservableProperty]
    private string _condition = string.Empty;

    [ObservableProperty]
    private decimal _estimatedValue;

    [ObservableProperty]
    private string _imageUri = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private bool _isFavorite;

    [ObservableProperty]
    private string _pageTitle = "Add Card";

    public FormViewModel(PokemonCardRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Called automatically when CardId changes via query parameter.</summary>
    partial void OnCardIdChanged(int value)
    {
        if (value > 0)
        {
            _isEditing = true;
            PageTitle = "Edit Card";

            var card = _repository.GetById(value);
            if (card is not null)
            {
                Name = card.Name;
                Category = card.Category;
                Rarity = card.Rarity;
                Condition = card.Condition;
                EstimatedValue = card.EstimatedValue;
                ImageUri = card.ImageUri;
                Description = card.Description;
                IsFavorite = card.IsFavorite;
            }
        }
        else
        {
            _isEditing = false;
            PageTitle = "Add Card";
        }
    }

    /// <summary>Saves the card (creates or updates) and navigates back.</summary>
    [RelayCommand]
    private async Task SaveCard()
    {
        var card = new PokemonCard
        {
            Id = _isEditing ? CardId : 0,
            Name = Name,
            Category = Category,
            Rarity = Rarity,
            Condition = Condition,
            EstimatedValue = EstimatedValue,
            ImageUri = ImageUri,
            Description = Description,
            IsFavorite = IsFavorite
        };

        if (_isEditing)
            _repository.Update(card);
        else
            _repository.Add(card);

        await Shell.Current.GoToAsync("..");
    }

    /// <summary>Cancels the operation and navigates back.</summary>
    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
