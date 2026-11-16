using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Domain.Entities;

public class Step
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public Recipe Recipe { get; set; } = null!;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Order { get; set; }
}
