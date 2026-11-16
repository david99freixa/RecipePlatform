using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipePlatform.Domain.Entities;

namespace RecipePlatform.Data.Configurations;

public class RecipeRevisionCategoryConfiguration : IEntityTypeConfiguration<RecipeRevisionCategory> 
{
    public void Configure(EntityTypeBuilder<RecipeRevisionCategory> builder)
    {
        // Relaçao com a recipeRevision

        builder.HasOne(rc => rc.RecipeRevision)
            .WithMany(revision => revision.Categories) 
            .HasForeignKey(rc => rc.RecipeRevisionId)
            .OnDelete(DeleteBehavior.NoAction);

        // Relaçao com a category

        builder.HasOne(rc => rc.Category)
            .WithMany()
            .HasForeignKey(rc => rc.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        // Impedir categorias repetidas para a mesma receita

        builder.HasIndex(rc => new { rc.RecipeRevisionId, rc.CategoryId })
            .IsUnique();
    }

}
