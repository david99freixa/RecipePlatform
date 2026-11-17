namespace RecipePlatform.Services.Interfaces;

public interface ICommentService 
{
    // Criar um comentário numa receita
    int Create(int recipeId, int userId, string text);

    // Editar um comentário do próprio utilizador
    bool Update(int recipeId, int commentId, int userId, string text);

    // Eliminar um comentário do próprio utilizador
    bool Delete(int recipeId, int commentId, int userId);

}
