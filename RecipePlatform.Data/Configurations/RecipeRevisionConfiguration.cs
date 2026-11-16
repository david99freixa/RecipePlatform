using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipePlatform.Domain.Entities;

namespace RecipePlatform.Data.Configurations;

public class RecipeRevisionConfiguration : IEntityTypeConfiguration<RecipeRevision>
{
    public void Configure(EntityTypeBuilder<RecipeRevision> builder)
    {
        // Relação entre RecipeRevision e Recipe

        builder.HasOne(revision => revision.Recipe)
            .WithMany(recipe => recipe.Revisions)
            .HasForeignKey(revision => revision.RecipeId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
