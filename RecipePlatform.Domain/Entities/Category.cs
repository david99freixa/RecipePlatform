using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Domain.Entities;

public class Category
{

    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public List<Recipe> Recipes { get; set; } = new();

}
