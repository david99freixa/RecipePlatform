using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipePlatform.Domain.Entities;

namespace RecipePlatform.Data.Configurations;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        // Relação entre Favorite e User

        builder.HasOne(favorite => favorite.User)
            .WithMany(user => user.Favorites)
            .HasForeignKey(favorite => favorite.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relação entre Favorite e Recipe

        builder.HasOne(favorite => favorite.Recipe)
            .WithMany(recipe => recipe.Favorites)
            .HasForeignKey(favorite => favorite.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Impedir favoritos duplicados

        builder.HasIndex(favorite => new { favorite.UserId, favorite.RecipeId })
            .IsUnique();
    }
}
