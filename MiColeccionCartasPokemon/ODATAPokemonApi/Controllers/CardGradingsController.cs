using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ODATAPokemonApi.Data;
using ODATAPokemonApi.Models;

namespace ODATAPokemonApi.Controllers;

/// <summary>
/// Controlador OData para la gestión y consulta de certificaciones/graduaciones (CardGrading).
/// </summary>
public class CardGradingsController(PokemonDbContext dbContext) : ODataController
{
    /// <summary>
    /// Consulta de calificaciones con soporte OData ($filter, $orderby, etc.).
    /// Ejemplos:
    /// - GET /odata/CardGradings?$filter=Score ge 9.5
    /// - GET /odata/CardGradings?$filter=GraderCompany eq 'PSA'
    /// </summary>
    [EnableQuery]
    public ActionResult<IQueryable<CardGrading>> Get()
    {
        return Ok(dbContext.CardGradings);
    }

    /// <summary>
    /// Consulta una calificación específica por ID.
    /// </summary>
    [EnableQuery]
    public SingleResult<CardGrading> Get([FromRoute] int key)
    {
        var grading = dbContext.CardGradings.Where(g => g.Id == key);
        return SingleResult.Create(grading);
    }

    /// <summary>
    /// Crea una nueva calificación para una carta Pokémon.
    /// </summary>
    public ActionResult Post([FromBody] CardGrading grading)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Validar que la carta asociada exista
        var cardExists = dbContext.PokemonCards.Any(c => c.Id == grading.PokemonCardId);
        if (!cardExists)
        {
            return BadRequest($"No existe ninguna carta Pokémon con el Id {grading.PokemonCardId}.");
        }

        dbContext.CardGradings.Add(grading);
        dbContext.SaveChanges();

        return Created(grading);
    }

    /// <summary>
    /// Actualiza completamente una calificación existente (PUT).
    /// </summary>
    public ActionResult Put([FromRoute] int key, [FromBody] CardGrading updatedGrading)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var grading = dbContext.CardGradings.SingleOrDefault(g => g.Id == key);
        if (grading == null)
        {
            return NotFound();
        }

        grading.PokemonCardId = updatedGrading.PokemonCardId;
        grading.GraderCompany = updatedGrading.GraderCompany;
        grading.Score = updatedGrading.Score;
        grading.CertifiedDate = updatedGrading.CertifiedDate;
        grading.CertificateCode = updatedGrading.CertificateCode;

        dbContext.SaveChanges();

        return Updated(grading);
    }

    /// <summary>
    /// Actualiza parcialmente una calificación existente (PATCH).
    /// </summary>
    public ActionResult Patch([FromRoute] int key, [FromBody] Delta<CardGrading> delta)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var grading = dbContext.CardGradings.SingleOrDefault(g => g.Id == key);
        if (grading == null)
        {
            return NotFound();
        }

        delta.Patch(grading);
        dbContext.SaveChanges();

        return Updated(grading);
    }

    /// <summary>
    /// Elimina una calificación por su ID.
    /// </summary>
    public ActionResult Delete([FromRoute] int key)
    {
        var grading = dbContext.CardGradings.SingleOrDefault(g => g.Id == key);
        if (grading != null)
        {
            dbContext.CardGradings.Remove(grading);
            dbContext.SaveChanges();
        }

        return NoContent();
    }
}
