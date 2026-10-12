using ODATAPokemonApi.Models;

namespace ODATAPokemonApi.Data;

public static class Seeder
{
    public static void SeedData(PokemonDbContext context)
    {
        if (!context.PokemonCards.Any())
        {
            var cards = new List<PokemonCard>
            {
                new()
                {
                    Id = 1,
                    Name = "Charizard Base Set Holo",
                    Category = "Fire",
                    Rarity = "Rare Holo",
                    Condition = "Near Mint",
                    EstimatedValue = 420.00m,
                    ImageUri = "https://images.tcgdex.net/en/base/base1/4/low.webp",
                    IsFavorite = true,
                    Description = "El legendario Charizard Holo de la primera edición Base Set (1999). Una de las cartas más icónicas y valiosas del TCG.",
                    Gradings = new List<CardGrading>
                    {
                        new()
                        {
                            Id = 1,
                            PokemonCardId = 1,
                            GraderCompany = "PSA",
                            Score = 9.0m,
                            CertifiedDate = new DateTime(2023, 4, 15),
                            CertificateCode = "PSA-68214501"
                        },
                        new()
                        {
                            Id = 2,
                            PokemonCardId = 1,
                            GraderCompany = "Beckett",
                            Score = 9.5m,
                            CertifiedDate = new DateTime(2023, 11, 20),
                            CertificateCode = "BGS-00148923"
                        }
                    }
                },
                new()
                {
                    Id = 2,
                    Name = "Pikachu VMAX Rainbow",
                    Category = "Electric",
                    Rarity = "Secret Rare",
                    Condition = "Mint",
                    EstimatedValue = 280.00m,
                    ImageUri = "https://images.tcgdex.net/en/swsh/swsh04/188/low.webp",
                    IsFavorite = true,
                    Description = "Pikachu Gigamax conocido cariñosamente como 'Chonkachu' en versión Hiper Rara Arcoíris.",
                    Gradings = new List<CardGrading>
                    {
                        new()
                        {
                            Id = 3,
                            PokemonCardId = 2,
                            GraderCompany = "PSA",
                            Score = 10.0m,
                            CertifiedDate = new DateTime(2024, 1, 10),
                            CertificateCode = "PSA-99120482"
                        },
                        new()
                        {
                            Id = 4,
                            PokemonCardId = 2,
                            GraderCompany = "CGC",
                            Score = 9.5m,
                            CertifiedDate = new DateTime(2024, 5, 18),
                            CertificateCode = "CGC-77401928"
                        }
                    }
                },
                new()
                {
                    Id = 3,
                    Name = "Blastoise Base Set Holo",
                    Category = "Water",
                    Rarity = "Rare Holo",
                    Condition = "Near Mint",
                    EstimatedValue = 190.00m,
                    ImageUri = "https://images.tcgdex.net/en/base/base1/2/low.webp",
                    IsFavorite = false,
                    Description = "Poderoso Blastoise original con su clásico ataque Hidrobomba de 1999.",
                    Gradings = new List<CardGrading>
                    {
                        new()
                        {
                            Id = 5,
                            PokemonCardId = 3,
                            GraderCompany = "Beckett",
                            Score = 8.5m,
                            CertifiedDate = new DateTime(2022, 9, 5),
                            CertificateCode = "BGS-00918234"
                        },
                        new()
                        {
                            Id = 6,
                            PokemonCardId = 3,
                            GraderCompany = "PSA",
                            Score = 9.0m,
                            CertifiedDate = new DateTime(2023, 8, 12),
                            CertificateCode = "PSA-55102938"
                        }
                    }
                },
                new()
                {
                    Id = 4,
                    Name = "Mewtwo GX Tube Alt Art",
                    Category = "Psychic",
                    Rarity = "Ultra Rare",
                    Condition = "Mint",
                    EstimatedValue = 135.00m,
                    ImageUri = "https://images.tcgdex.net/en/sm/sm35/78/low.webp",
                    IsFavorite = true,
                    Description = "Mewtwo en el tubo de laboratorio de Isla Nueva de la expansión Leyendas Luminosas.",
                    Gradings = new List<CardGrading>
                    {
                        new()
                        {
                            Id = 7,
                            PokemonCardId = 4,
                            GraderCompany = "PSA",
                            Score = 10.0m,
                            CertifiedDate = new DateTime(2024, 2, 14),
                            CertificateCode = "PSA-88201944"
                        },
                        new()
                        {
                            Id = 8,
                            PokemonCardId = 4,
                            GraderCompany = "CGC",
                            Score = 9.0m,
                            CertifiedDate = new DateTime(2023, 12, 1),
                            CertificateCode = "CGC-66291044"
                        }
                    }
                },
                new()
                {
                    Id = 5,
                    Name = "Venusaur Base Set Holo",
                    Category = "Grass",
                    Rarity = "Rare Holo",
                    Condition = "Played",
                    EstimatedValue = 85.00m,
                    ImageUri = "https://images.tcgdex.net/en/base/base1/15/low.webp",
                    IsFavorite = false,
                    Description = "El tercer inicial legendario de Kanto en su versión holo original del set base.",
                    Gradings = new List<CardGrading>
                    {
                        new()
                        {
                            Id = 9,
                            PokemonCardId = 5,
                            GraderCompany = "PSA",
                            Score = 7.5m,
                            CertifiedDate = new DateTime(2021, 6, 25),
                            CertificateCode = "PSA-44109823"
                        },
                        new()
                        {
                            Id = 10,
                            PokemonCardId = 5,
                            GraderCompany = "SGC",
                            Score = 8.0m,
                            CertifiedDate = new DateTime(2022, 11, 19),
                            CertificateCode = "SGC-33190284"
                        }
                    }
                },
                new()
                {
                    Id = 6,
                    Name = "Rayquaza VMAX Alt Art",
                    Category = "Dragon",
                    Rarity = "Secret Rare",
                    Condition = "Mint",
                    EstimatedValue = 340.00m,
                    ImageUri = "https://images.tcgdex.net/en/swsh/swsh07/218/low.webp",
                    IsFavorite = false,
                    Description = "Arte alternativo especial de Rayquaza surcando las montañas en Cielos Evolutivos.",
                    Gradings = new List<CardGrading>
                    {
                        new()
                        {
                            Id = 11,
                            PokemonCardId = 6,
                            GraderCompany = "PSA",
                            Score = 9.5m,
                            CertifiedDate = new DateTime(2024, 3, 30),
                            CertificateCode = "PSA-91028374"
                        },
                        new()
                        {
                            Id = 12,
                            PokemonCardId = 6,
                            GraderCompany = "Beckett",
                            Score = 9.5m,
                            CertifiedDate = new DateTime(2024, 4, 12),
                            CertificateCode = "BGS-00339182"
                        }
                    }
                }
            };

            context.PokemonCards.AddRange(cards);
            context.SaveChanges();
        }
    }
}
