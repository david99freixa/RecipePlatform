using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipePlatform.Domain.Entities;

namespace RecipePlatform.Data.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        // Relação entre Comment e User (Autor do comentário)
        builder.HasOne(comment => comment.User)
            .WithMany(user => user.Comments)
            .HasForeignKey(comment => comment.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // Relação entre Comment e Recipe

        builder.HasOne(comment => comment.Recipe)
            .WithMany(recipe => recipe.Comments)
            .HasForeignKey(comment => comment.RecipeId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
