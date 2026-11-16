using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipePlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Data.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.HasOne(recipe => recipe.Author)
            .WithMany(user => user.Recipes)
            .HasForeignKey(recipe => recipe.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
