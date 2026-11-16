using RecipePlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Domain.Entities;

public class RecipeRevision
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public Recipe Recipe { get; set; } = null!;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int DurationInMinutes { get; set; }

    public Difficulty Difficulty { get; set; }

    public Status Status { get; set; } = Status.Pending;

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReviewedAt { get; set; }

    public string? RejectionReason { get; set; }

    public List<RecipeRevisionStep> Steps { get; set; } = new();

    public List<RecipeRevisionIngredient> Ingredients { get; set; } = new(); 

    public List<RecipeRevisionCategory> Categories { get; set; } = new();

}
