using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Services.DTOs;

public class RecipeStepInputDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
}
