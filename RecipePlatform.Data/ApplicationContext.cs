using Microsoft.EntityFrameworkCore;
using RecipePlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Data;

public class ApplicationContext : DbContext 
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; } = null!;    

    public DbSet<Comment> Comments { get; set; } = null!;

    public DbSet<Favorite> Favorites { get; set; } = null!;

    public DbSet<Ingredient> Ingredients { get; set; } = null!;

    public DbSet<Rating> Ratings { get; set; } = null!;

    public DbSet<Recipe> Recipes { get; set; } = null!;

    public DbSet<RecipeIngredient> RecipeIngredients { get; set; } = null!;

    public DbSet<RecipeRevision> RecipeRevisions { get; set; } = null!;

    public DbSet<RecipeRevisionCategory> RecipeRevisionCategories { get; set; } = null!;

    public DbSet<RecipeRevisionIngredient> RecipeRevisionIngredients { get; set; } = null!;

    public DbSet<RecipeRevisionStep> RecipeRevisionSteps { get; set; } = null!;

    public DbSet<Step> Steps { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }

}
