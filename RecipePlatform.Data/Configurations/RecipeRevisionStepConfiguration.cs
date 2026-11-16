using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipePlatform.Domain.Entities;

namespace RecipePlatform.Data.Configurations;

public class RecipeRevisionStepConfiguration : IEntityTypeConfiguration<RecipeRevisionStep>
{
    public void Configure(EntityTypeBuilder<RecipeRevisionStep> builder)
    {
        // Relação com a revisão da receita

        builder.HasOne(step => step.RecipeRevision)
            .WithMany(revision => revision.Steps)
            .HasForeignKey(step => step.RecipeRevisionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
