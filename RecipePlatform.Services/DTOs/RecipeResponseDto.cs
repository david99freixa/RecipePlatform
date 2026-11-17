using RecipePlatform.Domain.Enums;

namespace RecipePlatform.Services.DTOs;

public class RecipeSummaryDto // Serve para representar o resumo de uma receita na resposta da API
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public int DurationInMinutes { get; set; }
    public string Difficulty { get; set; } = string.Empty;
}

public class RecipeDetailsDto : RecipeSummaryDto // Serve para representar os detalhes de uma receita na resposta da API
{
    public string Description { get; set; } = string.Empty;
    public List<string> Categories { get; set; } = new();
    public List<RecipeIngredientResponseDto> Ingredients { get; set; } = new();
    public List<RecipeStepResponseDto> Steps { get; set; } = new();
    public List<RecipeCommentResponseDto> Comments { get; set; } = new();
    public double? AverageRating { get; set; }
    public int RatingCount { get; set; }
}

public class RecipeIngredientResponseDto  // Serve para representar os ingredientes de uma receita na resposta da API
{
    public string Name { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
}

public class RecipeStepResponseDto // Serve para representar os passos de uma receita na resposta da API
{
    public int Order { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class RecipeCommentResponseDto // Serve para representar os comentários de uma receita na resposta da API
{
    public string AuthorName { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
