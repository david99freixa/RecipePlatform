using RecipePlatform.Domain.Entities;
using RecipePlatform.Services.DTOs;

namespace RecipePlatform.Services.Interfaces;

public interface IRecipeService
{
    List<Recipe> GetApproved();

    Recipe? GetApprovedByIdWithDetails(int id);

    int SubmitRecipe(SubmitRecipeDto dto, int authorId);
}
