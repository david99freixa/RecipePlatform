using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Domain.Entities;

public class RecipeRevisionCategory
{
    public int Id { get; set; }

    public int RecipeRevisionId { get; set; }

    public RecipeRevision RecipeRevision { get; set; } = null!;

    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

}
