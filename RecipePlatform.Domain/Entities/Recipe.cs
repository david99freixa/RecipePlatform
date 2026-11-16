using RecipePlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Domain.Entities;

public class Recipe
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int DurationInMinutes { get; set; }

    public Difficulty Difficulty { get; set; }

    public int AuthorId { get; set; }

    public User Author { get; set; } = null!;

    public Status Status { get; set; } = Status.Draft;

    public List<Category> Categories { get; set; } = new();

    public List<RecipeIngredient> RecipeIngredients { get; set; } = new();

    public List<Step> Steps { get; set; } = new();

    public List<Comment> Comments { get; set; } = new();

    public List<Rating> Ratings { get; set; } = new();

    public List<Favorite> Favorites { get; set; } = new();

    public List<RecipeRevision> Revisions { get; set; } = new();

    public DateTime? ReviewesAt { get; set; }

    public string? RejectionReason { get; set; }
}
