using PokemonCardCollection.Models;

namespace PokemonCardCollection.Data;

/// <summary>
/// In-memory repository that supplies hard-coded sample Pokémon cards.
/// Will be replaced by a real data source in Phase 2.
/// </summary>
public class PokemonCardRepository
{
    private readonly List<PokemonCard> _cards;
    private int _nextId;

    public PokemonCardRepository()
    {
        _cards = new List<PokemonCard>
        {
            new PokemonCard
            {
                Id = 1,
                Name = "Charizard",
                Category = "Fire",
                Rarity = "Rare Holo",
                Condition = "Near Mint",
                EstimatedValue = 350.00m,
                ImageUri = "charizard.png",
                IsFavorite = true,
                Description = "A powerful Fire/Flying Pokémon. Its flame burns hotter when it has experienced harsh battles."
            },
            new PokemonCard
            {
                Id = 2,
                Name = "Pikachu",
                Category = "Electric",
                Rarity = "Common",
                Condition = "Mint",
                EstimatedValue = 15.00m,
                ImageUri = "pikachu.png",
                IsFavorite = true,
                Description = "The iconic Electric-type mascot. It stores electricity in its cheek pouches."
            },
            new PokemonCard
            {
                Id = 3,
                Name = "Blastoise",
                Category = "Water",
                Rarity = "Rare Holo",
                Condition = "Played",
                EstimatedValue = 120.00m,
                ImageUri = "blastoise.png",
                IsFavorite = false,
                Description = "A Water-type Pokémon with powerful hydro cannons on its shell."
            },
            new PokemonCard
            {
                Id = 4,
                Name = "Venusaur",
                Category = "Grass",
                Rarity = "Rare Holo",
                Condition = "Near Mint",
                EstimatedValue = 95.00m,
                ImageUri = "venusaur.png",
                IsFavorite = false,
                Description = "A Grass/Poison-type Pokémon. The flower on its back blooms when absorbing sunlight."
            },
            new PokemonCard
            {
                Id = 5,
                Name = "Mewtwo",
                Category = "Psychic",
                Rarity = "Ultra Rare",
                Condition = "Mint",
                EstimatedValue = 500.00m,
                ImageUri = "mewtwo.png",
                IsFavorite = true,
                Description = "A genetically engineered Psychic-type Pokémon created from Mew's DNA."
            },
            new PokemonCard
            {
                Id = 6,
                Name = "Gengar",
                Category = "Ghost",
                Rarity = "Rare",
                Condition = "Near Mint",
                EstimatedValue = 45.00m,
                ImageUri = "gengar.png",
                IsFavorite = false,
                Description = "A Ghost/Poison-type Pokémon that hides in shadows and drops the room temperature."
            },
            new PokemonCard
            {
                Id = 7,
                Name = "Dragonite",
                Category = "Dragon",
                Rarity = "Rare Holo",
                Condition = "Mint",
                EstimatedValue = 200.00m,
                ImageUri = "dragonite.png",
                IsFavorite = false,
                Description = "A Dragon/Flying-type Pokémon. It can circle the globe in about 16 hours."
            },
            new PokemonCard
            {
                Id = 8,
                Name = "Eevee",
                Category = "Normal",
                Rarity = "Common",
                Condition = "Near Mint",
                EstimatedValue = 10.00m,
                ImageUri = "eevee.png",
                IsFavorite = true,
                Description = "A Normal-type Pokémon with an unstable genetic code that allows it to evolve into many forms."
            }
        };
        _nextId = _cards.Max(c => c.Id) + 1;
    }

    /// <summary>Returns all cards in the collection.</summary>
    public List<PokemonCard> GetAll() => new(_cards);

    /// <summary>Returns a single card by its Id, or null.</summary>
    public PokemonCard? GetById(int id) => _cards.FirstOrDefault(c => c.Id == id);

    /// <summary>Returns only the cards marked as favorite.</summary>
    public List<PokemonCard> GetFavorites() => _cards.Where(c => c.IsFavorite).ToList();

    /// <summary>Adds a new card and assigns it a unique Id.</summary>
    public void Add(PokemonCard card)
    {
        card.Id = _nextId++;
        _cards.Add(card);
    }

    /// <summary>Updates an existing card's data.</summary>
    public void Update(PokemonCard card)
    {
        var index = _cards.FindIndex(c => c.Id == card.Id);
        if (index >= 0)
            _cards[index] = card;
    }

    /// <summary>Removes a card by Id.</summary>
    public void Delete(int id)
    {
        var card = _cards.FirstOrDefault(c => c.Id == id);
        if (card is not null)
            _cards.Remove(card);
    }

    /// <summary>Toggles the IsFavorite flag on a card.</summary>
    public void ToggleFavorite(int id)
    {
        var card = _cards.FirstOrDefault(c => c.Id == id);
        if (card is not null)
            card.IsFavorite = !card.IsFavorite;
    }
}
