using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PokemonCardCollection.Data;
using PokemonCardCollection.Models;
using PokemonCardCollection.Services;

namespace PokemonCardCollection.ViewModels;

/// <summary>
/// ViewModel for the List page — displays all cards in the collection.
/// </summary>
public partial class ListViewModel : ObservableObject
{
    private const int PageSize = 30;
    private readonly PokemonCardRepository _repository;
    private readonly ITcgdexService _api;
    private bool _filterOptionsLoaded;

    [ObservableProperty]
    private ObservableCollection<PokemonCard> _cards = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _hasError;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private int _currentPage = 1;

    [ObservableProperty]
    private string _searchName = string.Empty;

    [ObservableProperty]
    private string _selectedType = "Todos";

    [ObservableProperty]
    private string _selectedRarity = "Todos";

    [ObservableProperty]
    private bool _hasNextPage;

    [ObservableProperty]
    private bool _canGoPrevious;

    public ObservableCollection<string> AvailableTypes { get; } = new() { "Todos" };

    public ObservableCollection<string> AvailableRarities { get; } = new() { "Todos" };

    public ListViewModel(PokemonCardRepository repository, ITcgdexService api)
    {
        _repository = repository;
        _api = api;
    }

    /// <summary>Loads cards from the repository (triggers API fetch on first call).</summary>
    [RelayCommand]
    private async Task LoadCards()
    {
        if (!_filterOptionsLoaded)
            await LoadFilterOptionsAsync();

        await LoadCurrentPageAsync();
    }

    [RelayCommand]
    private async Task Search()
    {
        CurrentPage = 1;
        await LoadCurrentPageAsync();
    }

    [RelayCommand]
    private async Task NextPage()
    {
        if (IsLoading || !HasNextPage) return;

        CurrentPage++;
        await LoadCurrentPageAsync();
    }

    [RelayCommand]
    private async Task PreviousPage()
    {
        if (IsLoading || CurrentPage <= 1) return;

        CurrentPage--;
        await LoadCurrentPageAsync();
    }

    private async Task LoadCurrentPageAsync()
    {
        if (IsLoading) return;

        IsLoading = true;
        HasError = false;
        ErrorMessage = string.Empty;

        var count = await _repository.LoadCardsFromApiAsync(
            CurrentPage,
            EmptyAsNull(SearchName),
            EmptyAsNull(SelectedType, "Todos"),
            EmptyAsNull(SelectedRarity, "Todos"));

        IsLoading = _repository.IsLoading;
        HasError = _repository.HasError;
        ErrorMessage = _repository.ErrorMessage;

        Cards = new ObservableCollection<PokemonCard>(_repository.GetAll());
        HasNextPage = !HasError && count == PageSize;
        CanGoPrevious = CurrentPage > 1;
    }

    private async Task LoadFilterOptionsAsync()
    {
        try
        {
            var typesTask = _api.GetTypesAsync();
            var raritiesTask = _api.GetRaritiesAsync();
            await Task.WhenAll(typesTask, raritiesTask);

            foreach (var type in typesTask.Result.Where(value => !string.IsNullOrWhiteSpace(value)).Distinct())
                AvailableTypes.Add(type);

            foreach (var rarity in raritiesTask.Result.Where(value => !string.IsNullOrWhiteSpace(value)).Distinct())
                AvailableRarities.Add(rarity);

            _filterOptionsLoaded = true;
        }
        catch (HttpRequestException)
        {
            // The card list can still be used with name search and pagination.
        }
        catch (TaskCanceledException)
        {
            // The card list can still be used with name search and pagination.
        }
        catch (JsonException)
        {
            // The card list can still be used with name search and pagination.
        }
    }

    private static string? EmptyAsNull(string value, string? defaultValue = null)
    {
        return string.IsNullOrWhiteSpace(value) || value == defaultValue ? null : value;
    }

    /// <summary>Navigates to the Detail page for the selected card.</summary>
    [RelayCommand]
    private async Task GoToDetail(PokemonCard card)
    {
        if (card is null) return;

        await Shell.Current.GoToAsync($"DetailPage?cardId={card.Id}");
    }

    /// <summary>Navigates to the Form page to add a new card.</summary>
    [RelayCommand]
    private async Task GoToAddCard()
    {
        await Shell.Current.GoToAsync("FormPage");
    }

    /// <summary>Deletes a card after user confirmation.</summary>
    [RelayCommand]
    private async Task DeleteCard(PokemonCard card)
    {
        if (card is null) return;

        bool confirm = await Shell.Current.DisplayAlertAsync(
            "Eliminar carta",
            $"¿Seguro que deseas eliminar {card.Name}?",
            "Eliminar",
            "Cancelar");

        if (!confirm) return;

        _repository.Delete(card.Id);
        Cards = new ObservableCollection<PokemonCard>(_repository.GetAll());
    }
}
