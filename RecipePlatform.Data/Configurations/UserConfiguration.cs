using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipePlatform.Domain.Entities;

namespace RecipePlatform.Data.Configurations;


public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Nome obrigatório
        builder.Property(user => user.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Email obrigatório
        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(256);

        //Impedir que o email seja duplicado
        builder.HasIndex(user => user.Email)
            .IsUnique();

        // Senha obrigatória
        builder.Property(user => user.PasswordHash)
            .IsRequired();
    }
}
