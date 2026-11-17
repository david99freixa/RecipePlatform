using Microsoft.AspNetCore.Mvc;
using RecipePlatform.Services.DTOs;
using RecipePlatform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RecipePlatform.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase // Serve para gerir as receitas na API
{
    private readonly IRecipeService _recipeService;

    public RecipesController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [HttpGet]
    public ActionResult<List<RecipeSummaryDto>> GetApprovedRecipes() // Serve para obter a lista de receitas aprovadas
    {
        var recipes = _recipeService.GetApproved();

        var result = recipes.Select(r => new RecipeSummaryDto
        {
            Id = r.Id,
            Title = r.Title,
            AuthorName = r.Author.Name,
            DurationInMinutes = r.DurationInMinutes,
            Difficulty = r.Difficulty.ToString()
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<RecipeDetailsDto> GetById(int id)
    {
        var recipe = _recipeService.GetApprovedByIdWithDetails(id);

        if (recipe == null)
        {
            return NotFound();
        }

        var result = new RecipeDetailsDto
        {
            Id = recipe.Id,
            Title = recipe.Title,
            Description = recipe.Description,
            AuthorName = recipe.Author.Name,
            DurationInMinutes = recipe.DurationInMinutes,
            Difficulty = recipe.Difficulty.ToString(),

            Categories = recipe.Categories
               .Select(category => category.Title)
               .ToList(),

            Ingredients = recipe.RecipeIngredients
               .Select(item => new RecipeIngredientResponseDto
               {
                   Name = item.Ingredient.Title,
                   Quantity = item.Quantity,
                   Unit = item.Unit.ToString()
               }).ToList(),

            Steps = recipe.Steps
               .OrderBy(step => step.Order)
               .Select(step => new RecipeStepResponseDto
               {
                   Order = step.Order,
                   Title = step.Title,
                   Description = step.Description
               }).ToList(),

            Comments = recipe.Comments
               .Select(comment => new RecipeCommentResponseDto
               {
                   AuthorName = comment.User.Name,
                   Text = comment.Text,
                   CreatedAt = comment.CreatedAt
               }).ToList(),

            RatingCount = recipe.Ratings.Count,

            AverageRating = recipe.Ratings.Count == 0
               ? null
               : Math.Round(recipe.Ratings.Average(r => r.Value), 1)
        };

        return Ok(result);
    }


    // POST: api/recipes
    // Permite a um utilizador autenticado submeter uma receita
    [Authorize]
    [HttpPost]
    public IActionResult SubmitRecipe(
        [FromBody] SubmitRecipeDto dto)
    {
        // Obtém o ID do utilizador através do token JWT
        var userId = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        // Confirma que o ID do utilizador é válido
        if (!int.TryParse(userId, out int authorId))
        {
            return Unauthorized();
        }

        try
        {
            // Valida e guarda a receita com estado Pending
            int recipeId = _recipeService.SubmitRecipe(
                dto, authorId);

            // Devolve o ID e confirma a criação da receita
            return StatusCode(201, new
            {
                Id = recipeId,
                Status = "Pending",
                Message = "Receita submetida para aprovação."
            });
        }
        catch (ArgumentException ex)
        {
            // Devolve 400 se os dados da receita forem inválidos
            return BadRequest(new { Message = ex.Message });
        }
        catch (InvalidOperationException)
        {
            // Impede a submissão por utilizadores bloqueados
            return Forbid();
        }
    }

}
