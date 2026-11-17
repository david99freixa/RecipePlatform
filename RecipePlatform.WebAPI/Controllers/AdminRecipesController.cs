using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipePlatform.Services.DTOs;
using RecipePlatform.Services.Interfaces;

namespace RecipePlatform.WebAPI.Controllers;

[ApiController]
[Route("api/admin/recipes")]
[Authorize(Roles = "Admin")]
public class AdminRecipesController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public AdminRecipesController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    // GET: api/admin/recipes/pending
    // Lista as receitas pendentes com os seus detalhes
    [HttpGet("pending")]
    public IActionResult GetPending()
    {
        var recipes = _recipeService.GetPending();

        var result = recipes.Select(recipe => new RecipeDetailsDto
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
                }).ToList()
        }).ToList();

        return Ok(result);
    }

    // POST: api/admin/recipes/1/approve
    // Aprova uma receita pendente
    [HttpPost("{id:int}/approve")]
    public IActionResult Approve(int id)
    {
        var approved = _recipeService.ApproveRecipe(id);

        if (!approved)
        {
            return NotFound(new
            {
                Message = "Receita pendente não encontrada."
            });
        }

        return Ok(new
        {
            Message = "Receita aprovada com sucesso."
        });
    }

    // POST: api/admin/recipes/1/reject
    // Rejeita uma receita e guarda o motivo
    [HttpPost("{id:int}/reject")]
    public IActionResult Reject(
        int id,
        [FromBody] RejectRecipeDto dto)
    {
        var rejected = _recipeService.RejectRecipe(
            id, dto.Reason);

        if (!rejected)
        {
            return NotFound(new
            {
                Message = "Receita pendente não encontrada."
            });
        }

        return Ok(new
        {
            Message = "Receita rejeitada com sucesso."
        });
    }
}
