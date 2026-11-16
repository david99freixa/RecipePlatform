using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipePlatform.Domain.Entities;

namespace RecipePlatform.Data.Configurations;

public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
{
    public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
    {
        // Relação RecipeIngredient com Recipe

        builder.HasOne(recipeIngredient => recipeIngredient.Recipe)
            .WithMany(recipe => recipe.RecipeIngredients)
            .HasForeignKey(recipeIngredient => recipeIngredient.RecipeId)
            .OnDelete(DeleteBehavior.NoAction);

        // Relação RecipeIngredient com Ingredient

        builder.HasOne(recipeIngredient => recipeIngredient.Ingredient)
            .WithMany(ingredient => ingredient.RecipeIngredients)
            .HasForeignKey(recipeIngredient => recipeIngredient.IngredientId)
            .OnDelete(DeleteBehavior.NoAction);

        // Impedir duplicatas de RecipeIngredient

        builder.HasIndex(recipeIngredient => new { recipeIngredient.RecipeId, recipeIngredient.IngredientId })
            .IsUnique();
    }
}
