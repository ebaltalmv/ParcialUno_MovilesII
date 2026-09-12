using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokemonCardCollection.Data;
using PokemonCardCollection.Data.DTOs;
using PokemonCardCollection.Models;

namespace PokemonCardCollection.ViewModels;

/// <summary>
/// ViewModel for the Form page, reusable for both adding and editing cards.
/// </summary>
[QueryProperty(nameof(CardId), "cardId")]
public partial class FormViewModel : ObservableObject
{
    private readonly PokemonCardRepository _repository;
    private bool _isEditing;

    [ObservableProperty]
    private string _cardId = string.Empty;

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

    public ObservableCollection<string> Conditions { get; } = new()
    {
        "Mint", "Near Mint", "Excellent", "Good", "Light Played", "Played", "Poor"
    };

    [ObservableProperty]
    private string _pageTitle = "Add Card";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSearchVisible))]
    private bool _isCardSelected;

    public bool IsSearchVisible => !IsCardSelected;

    public ObservableCollection<string> AvailableTypes { get; } = new()
    {
        "All", "Colorless", "Darkness", "Dragon", "Fairy", "Fighting", "Fire", "Grass", "Lightning", "Metal", "Psychic", "Water"
    };

    public ObservableCollection<string> AvailableRarities { get; } = new()
    {
        "All", "Common", "Uncommon", "Rare", "Rare Holo", "Ultra Rare", "Secret Rare", "Double rare", "Illustration rare", "Special illustration rare", "Promo"
    };

    [ObservableProperty]
    private string _selectedType = "Todos";

    [ObservableProperty]
    private string _selectedRarity = "Todas";

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    private ObservableCollection<TcgCardDto> _searchResults = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowInitialState))]
    [NotifyPropertyChangedFor(nameof(ShowNoResults))]
    private bool _isSearching;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowInitialState))]
    [NotifyPropertyChangedFor(nameof(ShowNoResults))]
    private bool _isSearchEmpty = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowInitialState))]
    [NotifyPropertyChangedFor(nameof(ShowNoResults))]
    private bool _hasSearched;

    public bool ShowInitialState => !HasSearched && IsSearchEmpty && !IsSearching;
    public bool ShowNoResults => HasSearched && IsSearchEmpty && !IsSearching;

    public FormViewModel(PokemonCardRepository repository)
    {
        _repository = repository;
    }

    partial void OnCardIdChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
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

                IsCardSelected = true;
            }
        }
        else
        {
            _isEditing = false;
            PageTitle = "Add Card";
            IsCardSelected = false;
            SearchResults.Clear();
            SearchQuery = string.Empty;
            SelectedType = "Todos";
            SelectedRarity = "Todas";
            HasSearched = false;
            IsSearchEmpty = true;
        }
    }

    [RelayCommand]
    private async Task SearchApi()
    {
        bool hasQuery = !string.IsNullOrWhiteSpace(SearchQuery);
        bool hasType = !string.IsNullOrWhiteSpace(SelectedType) && SelectedType != "Todos" && SelectedType != "All";
        bool hasRarity = !string.IsNullOrWhiteSpace(SelectedRarity) && SelectedRarity != "Todas" && SelectedRarity != "All";

        if (!hasQuery && !hasType && !hasRarity)
        {
            await Shell.Current.DisplayAlertAsync("Filtros requeridos", "Ingresa un nombre o selecciona al menos un filtro (tipo o rareza) para buscar.", "OK");
            return;
        }

        IsSearching = true;
        HasSearched = true;
        IsSearchEmpty = false;
        SearchResults.Clear();
        
        var results = await _repository.SearchCardsAsync(SearchQuery, SelectedType, SelectedRarity);
        foreach (var r in results)
        {
            SearchResults.Add(r);
        }
        
        IsSearching = false;
        IsSearchEmpty = SearchResults.Count == 0;
    }

    [RelayCommand]
    private void ResetFilters()
    {
        SearchQuery = string.Empty;
        SelectedType = "Todos";
        SelectedRarity = "Todas";
        SearchResults.Clear();
        HasSearched = false;
        IsSearchEmpty = true;
    }

    [RelayCommand]
    private async Task SelectApiCard(TcgCardDto selectedDto)
    {
        if (selectedDto == null || string.IsNullOrEmpty(selectedDto.Id) || string.IsNullOrEmpty(selectedDto.Image)) return;

        IsSearching = true;
        
        var fullCard = await _repository.GetFullCardAsync(selectedDto.Id, selectedDto.Image);
        if (fullCard != null)
        {
            Name = fullCard.Name;
            Category = fullCard.Category;
            Rarity = fullCard.Rarity;
            EstimatedValue = fullCard.EstimatedValue;
            ImageUri = fullCard.ImageUri;
            Description = fullCard.Description;
            
            Condition = "Mint"; 
            IsFavorite = false;
            
            IsCardSelected = true;
        }
        else
        {
            await Shell.Current.DisplayAlertAsync("Error", "No se pudo cargar el detalle de la carta.", "OK");
        }
        
        IsSearching = false;
    }

    [RelayCommand]
    private async Task GuardarArticulo()
    {
        var card = new PokemonCard
        {
            Id = _isEditing ? CardId : string.Empty,
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

    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
