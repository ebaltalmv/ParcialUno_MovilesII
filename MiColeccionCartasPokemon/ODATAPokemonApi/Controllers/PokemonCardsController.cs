using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ODATAPokemonApi.Data;
using ODATAPokemonApi.Models;

namespace ODATAPokemonApi.Controllers;

/// <summary>
/// Controlador OData para la gestión y consulta de cartas Pokémon.
/// Soporta $select, $filter, $orderby, $expand, $count, $top, $skip y operaciones CRUD.
/// </summary>
public class PokemonCardsController(PokemonDbContext dbContext) : ODataController
{
    /// <summary>
    /// Obtiene la lista de cartas Pokémon con soporte completo de OData.
    /// Ejemplos:
    /// - GET /odata/PokemonCards?$expand=Gradings
    /// - GET /odata/PokemonCards?$filter=EstimatedValue gt 100
    /// - GET /odata/PokemonCards?$filter=Category eq 'Fire'
    /// </summary>
    [EnableQuery]
    public ActionResult<IQueryable<PokemonCard>> Get()
    {
        return Ok(dbContext.PokemonCards);
    }

    /// <summary>
    /// Obtiene una carta Pokémon específica por su ID.
    /// Soporta $expand=Gradings para traer sus calificaciones asociadas.
    /// Ejemplo: GET /odata/PokemonCards(1)?$expand=Gradings
    /// </summary>
    [EnableQuery]
    public SingleResult<PokemonCard> Get([FromRoute] int key)
    {
        var card = dbContext.PokemonCards.Where(c => c.Id == key);
        return SingleResult.Create(card);
    }

    /// <summary>
    /// Permite obtener directamente las calificaciones de una carta específica mediante navegación OData.
    /// Ejemplo: GET /odata/PokemonCards(1)/Gradings
    /// </summary>
    [EnableQuery]
    [HttpGet("odata/PokemonCards({key})/Gradings")]
    public ActionResult<IQueryable<CardGrading>> GetGradings([FromRoute] int key)
    {
        var gradings = dbContext.CardGradings.Where(g => g.PokemonCardId == key);
        return Ok(gradings);
    }

    /// <summary>
    /// Inserta una nueva carta Pokémon en la base de datos.
    /// </summary>
    public ActionResult Post([FromBody] PokemonCard card)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        dbContext.PokemonCards.Add(card);
        dbContext.SaveChanges();

        return Created(card);
    }

    /// <summary>
    /// Actualiza completamente una carta Pokémon existente (PUT).
    /// </summary>
    public ActionResult Put([FromRoute] int key, [FromBody] PokemonCard updatedCard)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var card = dbContext.PokemonCards.SingleOrDefault(c => c.Id == key);
        if (card == null)
        {
            return NotFound();
        }

        card.Name = updatedCard.Name;
        card.Category = updatedCard.Category;
        card.Rarity = updatedCard.Rarity;
        card.Condition = updatedCard.Condition;
        card.EstimatedValue = updatedCard.EstimatedValue;
        card.ImageUri = updatedCard.ImageUri;
        card.IsFavorite = updatedCard.IsFavorite;
        card.Description = updatedCard.Description;

        dbContext.SaveChanges();

        return Updated(card);
    }

    /// <summary>
    /// Actualiza parcialmente una carta Pokémon (PATCH).
    /// </summary>
    public ActionResult Patch([FromRoute] int key, [FromBody] Delta<PokemonCard> delta)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var card = dbContext.PokemonCards.SingleOrDefault(c => c.Id == key);
        if (card == null)
        {
            return NotFound();
        }

        delta.Patch(card);
        dbContext.SaveChanges();

        return Updated(card);
    }

    /// <summary>
    /// Elimina una carta Pokémon de la base de datos (con eliminación en cascada de sus gradings).
    /// </summary>
    public ActionResult Delete([FromRoute] int key)
    {
        var card = dbContext.PokemonCards.SingleOrDefault(c => c.Id == key);
        if (card != null)
        {
            dbContext.PokemonCards.Remove(card);
            dbContext.SaveChanges();
        }

        return NoContent();
    }
}
