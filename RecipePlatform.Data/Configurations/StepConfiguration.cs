using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipePlatform.Domain.Entities;

namespace RecipePlatform.Data.Configurations;

public class StepConfiguration : IEntityTypeConfiguration<Step>
{
    public void Configure(EntityTypeBuilder<Step> builder)
    {
        // Relação entre Step e Recipe

        builder.HasOne(step => step.Recipe)
            .WithMany(recipe => recipe.Steps)
            .HasForeignKey(step => step.RecipeId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
