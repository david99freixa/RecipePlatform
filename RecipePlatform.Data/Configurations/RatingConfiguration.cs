using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipePlatform.Domain.Entities;

namespace RecipePlatform.Data.Configurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        // Relação entre Rating e User

        builder.HasOne(rating => rating.User)
            .WithMany(user => user.Ratings)
            .HasForeignKey(rating => rating.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relação entre Rating e Recipe

        builder.HasOne(rating => rating.Recipe)
            .WithMany(recipe => recipe.Ratings)
            .HasForeignKey(rating => rating.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Impedir avaliações duplicadas

        builder.HasIndex(rating => new { rating.UserId, rating.RecipeId })
            .IsUnique();
    }
}
