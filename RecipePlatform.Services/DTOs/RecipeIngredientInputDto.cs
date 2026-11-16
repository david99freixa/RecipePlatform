using RecipePlatform.Domain.Enums;

namespace RecipePlatform.Services.DTOs;

public class RecipeIngredientInputDto
{
    public int IngredientId { get; set; }
    public double Quantity { get; set; }
    public IngredientUnit Unit { get; set; }
}
