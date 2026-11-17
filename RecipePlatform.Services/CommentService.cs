using RecipePlatform.Domain.Entities;
using RecipePlatform.Domain.Enums;
using RecipePlatform.Domain.Interfaces;
using RecipePlatform.Services.Interfaces;

namespace RecipePlatform.Services;

public class CommentService : ICommentService
{
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(
        IRepository<Comment> commentRepository,
        IRecipeRepository recipeRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _recipeRepository = recipeRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public int Create(int recipeId, int userId, string text)
    {
        // Confirma que o utilizador existe e não está bloqueado
        ValidateUser(userId);

        // Só permite comentar receitas aprovadas
        var recipe = _recipeRepository.GetById(recipeId);

        if (recipe == null || recipe.Status != Status.Approved)
        {
            throw new KeyNotFoundException(
                "Receita aprovada não encontrada.");
        }

        // Cria o comentário associado ao autor e à receita
        var comment = new Comment
        {
            Text = ValidateText(text),
            RecipeId = recipeId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _commentRepository.Create(comment);
        _unitOfWork.SaveChanges();

        return comment.Id;
    }

    public bool Update(
        int recipeId, int commentId, int userId, string text)
    {
        ValidateUser(userId);

        // Procura o comentário na receita indicada
        var comment = _commentRepository.GetById(commentId);

        if (comment == null || comment.RecipeId != recipeId)
            return false;

        // Só o autor pode editar o próprio comentário
        if (comment.UserId != userId)
            throw new UnauthorizedAccessException();

        comment.Text = ValidateText(text);

        // O EF Core acompanha a alteração ao comentário
        _unitOfWork.SaveChanges();

        return true;
    }

    public bool Delete(int recipeId, int commentId, int userId)
    {
        ValidateUser(userId);

        var comment = _commentRepository.GetById(commentId);

        if (comment == null || comment.RecipeId != recipeId)
            return false;

        // Impede eliminar comentários de outros utilizadores
        if (comment.UserId != userId)
            throw new UnauthorizedAccessException();

        _commentRepository.Delete(commentId);
        _unitOfWork.SaveChanges();

        return true;
    }

    private void ValidateUser(int userId)
    {
        // Bloqueia operações de utilizadores inexistentes ou bloqueados
        var user = _userRepository.GetById(userId);

        if (user == null || user.IsBlocked)
            throw new UnauthorizedAccessException();
    }

    private static string ValidateText(string? text)
    {
        // Valida o conteúdo antes de guardar
        if (string.IsNullOrWhiteSpace(text) ||
            text.Trim().Length > 1000)
        {
            throw new ArgumentException(
                "O comentário deve ter entre 1 e 1000 caracteres.");
        }

        return text.Trim();
    }
}
