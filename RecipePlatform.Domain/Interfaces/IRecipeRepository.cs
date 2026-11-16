using RecipePlatform.Domain.Entities;

namespace RecipePlatform.Domain.Interfaces;

public interface IRecipeRepository : IRepository<Recipe>
{
    List<Recipe> GetApproved();

    List<Recipe> GetByAuthorId(int authorId);

    Recipe? GetApprovedByIdWithDetails(int id);
}
