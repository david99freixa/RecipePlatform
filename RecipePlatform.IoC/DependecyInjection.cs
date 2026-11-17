using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipePlatform.Data;
using RecipePlatform.Data.Repositories;
using RecipePlatform.Domain.Interfaces;
using RecipePlatform.Services;
using RecipePlatform.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using RecipePlatform.Domain.Entities;

namespace RecipePlatform.IoC;

public static class DependecyInjection // static porque não queremos instanciar essa classe, apenas chamar o método de extensão
{
    public static IServiceCollection AddRecipePlatformServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Connection string
        var connenctionString = configuration.GetConnectionString("RecipePlatform")
            ?? throw new InvalidOperationException("Connection string 'RecipePlatform' not found.");

        // Registar a EF Core DbContext
        services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(connenctionString));

        // Registar repositories, services e a UnitOfWork com o IoC container
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IAuthService, AuthService>(); // Regista o serviço responsável pelo registo e login

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>(); // Permite criar e verificar hashes de passwords

        services.AddScoped<ICommentService, CommentService>(); // Regista as regras de negócio dos comentários


        return services;
    }
}
