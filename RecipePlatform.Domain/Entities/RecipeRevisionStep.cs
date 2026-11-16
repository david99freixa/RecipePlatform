using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Domain.Entities;

public class RecipeRevisionStep
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public int Order { get; set; }

    public int RecipeRevisionId { get; set; }

    public RecipeRevision RecipeRevision { get; set; } = null!;

}
