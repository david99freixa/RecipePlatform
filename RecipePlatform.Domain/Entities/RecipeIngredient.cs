using RecipePlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Domain.Entities;

public class RecipeIngredient
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public Recipe Recipe { get; set; } = null!;

    public int IngredientId { get; set; }

    public Ingredient Ingredient { get; set; } = null!;

    public double Quantity { get; set; } 

    public IngredientUnit Unit { get; set; }

}
