using RecipePlatform.Domain.Enums;

namespace RecipePlatform.Services.DTOs;

public class SubmitRecipeDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int DurationInMinutes { get; set; }

    public Difficulty Difficulty { get; set; }

    public List<int> CategoryIds { get; set; } = new();
    
    public List<RecipeIngredientInputDto> Ingredients { get; set; } = new();

    public List<RecipeStepInputDto> Steps { get; set; } = new();

}
