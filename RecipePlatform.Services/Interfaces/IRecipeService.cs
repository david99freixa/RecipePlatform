using RecipePlatform.Domain.Entities;
using RecipePlatform.Services.DTOs;

namespace RecipePlatform.Services.Interfaces;

public interface IRecipeService
{
    List<Recipe> GetApproved();

    Recipe? GetApprovedByIdWithDetails(int id);

    int SubmitRecipe(SubmitRecipeDto dto, int authorId);

    // Devolve as receitas que aguardam aprovação
    List<Recipe> GetPending();

    // Aprova uma receita pendente
    bool ApproveRecipe(int id);

    // Rejeita uma receita pendente
    bool RejectRecipe(int id, string? reason);

}


