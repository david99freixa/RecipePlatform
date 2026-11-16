using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipePlatform.Domain.Entities;

namespace RecipePlatform.Data.Configurations;

public class RecipeRevisionIngredientConfiguration : IEntityTypeConfiguration<RecipeRevisionIngredient>
{
    public void Configure(EntityTypeBuilder<RecipeRevisionIngredient> builder)
    {
        // Relaçao com a revisão da receita

        builder.HasOne(ingredient => ingredient.RecipeRevision)
            .WithMany(revision => revision.Ingredients)
            .HasForeignKey(ingredient => ingredient.RecipeRevisionId)
            .OnDelete(DeleteBehavior.NoAction);

        // Relaçao com o ingrediente

        builder.HasOne(ingredient => ingredient.Ingredient)
            .WithMany()
            .HasForeignKey(ingredient => ingredient.IngredientId)
            .OnDelete(DeleteBehavior.NoAction);

        // Impedir duplicação de ingredientes na mesma revisão da receita
        builder.HasIndex(ingredient => new { ingredient.RecipeRevisionId, ingredient.IngredientId })
            .IsUnique();
    }
}
