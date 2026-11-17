using RecipePlatform.Domain.Entities;
using RecipePlatform.Domain.Enums;
using RecipePlatform.Domain.Interfaces;
using RecipePlatform.Services.DTOs;
using RecipePlatform.Services.Interfaces;

namespace RecipePlatform.Services;

public class RecipeService : IRecipeService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<Ingredient> _ingredientRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public RecipeService(IRecipeRepository recipeRepository, IRepository<Category> categoryRepository, IRepository<Ingredient> ingredientRepository, IUnitOfWork unitOfWork, IUserService userService)
    {
        _recipeRepository = recipeRepository;
        _categoryRepository = categoryRepository;
        _ingredientRepository = ingredientRepository;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public List<Recipe> GetApproved()
    {
        return _recipeRepository.GetApproved();
    }

    public Recipe? GetApprovedByIdWithDetails(int id)
    {
        if (id <= 0)
        {
            return null;
        }

        return _recipeRepository.GetApprovedByIdWithDetails(id);
    }

    public int SubmitRecipe(SubmitRecipeDto dto, int authorId)
    {
        ArgumentNullException.ThrowIfNull(dto);

        // 1.Validar Autor
        if (!_userService.CanSubmitRecipe(authorId))
        {
            throw new InvalidOperationException("O utilizador não pode submeter receitas.");
        }

        //2. Validar os dados principais da receita
        if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Description) || dto.DurationInMinutes <= 0 || !Enum.IsDefined(dto.Difficulty))
        {
            throw new ArgumentException("Os dados principais da receita são inválidos.");
        }

        // 3. Validar as listas obrigatórias
        if (dto.Ingredients == null || dto.Ingredients.Count == 0 || dto.CategoryIds == null || dto.CategoryIds.Count == 0 || dto.Steps == null || dto.Steps.Count == 0)
        {
            throw new ArgumentException("A receita deve ter pelo menos um ingrediente, uma categoria e passos.");
        }

        //4. Impedir categorias e ingredientes duplicados
        if (dto.CategoryIds.Distinct().Count() != dto.CategoryIds.Count || dto.Ingredients.Select(i => i.IngredientId).Distinct().Count() != dto.Ingredients.Count)
        {
            throw new ArgumentException("Existem categorias ou ingredientes repetidos.");
        }

        //5. Validar ingredientes
        if (dto.Ingredients.Any(i => i.IngredientId <= 0 || !double.IsFinite(i.Quantity) || i.Quantity <= 0 || !Enum.IsDefined(i.Unit)))
        {
            throw new ArgumentException("Os ingredientes são inválidos.");
        }

        // 6. Validar passos e numeração
        if (dto.Steps.Any(s =>
            string.IsNullOrWhiteSpace(s.Title) ||
            string.IsNullOrWhiteSpace(s.Description)) ||
            !dto.Steps.Select(s => s.Order)
                .OrderBy(order => order)
                .SequenceEqual(Enumerable.Range(1, dto.Steps.Count)))
        {
            throw new ArgumentException(
                "Os passos devem estar preenchidos e numerados desde 1.");
        }

        // 7. Criar a receita pendente
        var recipe = new Recipe
        {
            Title = dto.Title.Trim(),
            Description = dto.Description.Trim(),
            DurationInMinutes = dto.DurationInMinutes,
            Difficulty = dto.Difficulty,
            AuthorId = authorId,
            Status = Status.Pending
        };

        // 8. Associar categorias existentes
        foreach (var categoryId in dto.CategoryIds)
        {
            var category = _categoryRepository.GetById(categoryId)
                ?? throw new ArgumentException(
                    $"A categoria {categoryId} não existe.");

            recipe.Categories.Add(category);
        }

        // 9. Associar ingredientes existentes
        foreach (var item in dto.Ingredients)
        {
            var ingredient = _ingredientRepository.GetById(item.IngredientId)
                ?? throw new ArgumentException(
                    $"O ingrediente {item.IngredientId} não existe.");

            recipe.RecipeIngredients.Add(new RecipeIngredient
            {
                IngredientId = ingredient.Id,
                Quantity = item.Quantity,
                Unit = item.Unit
            });
        }

        // 10. Criar os passos de preparação
        foreach (var step in dto.Steps)
        {
            recipe.Steps.Add(new Step
            {
                Title = step.Title.Trim(),
                Description = step.Description.Trim(),
                Order = step.Order
            });
        }

        // 11. Preparar e guardar as alterações
        _recipeRepository.Create(recipe);
        _unitOfWork.SaveChanges();

        return recipe.Id;
    }


    public List<Recipe> GetPending()
    {
        // Consulta as receitas que aguardam aprovação
        return _recipeRepository.GetPending();
    }

    public bool ApproveRecipe(int id)
    {
        // Procura a receita pelo ID
        var recipe = _recipeRepository.GetById(id);

        // Apenas permite aprovar receitas pendentes
        if (recipe == null || recipe.Status != Status.Pending)
        {
            return false;
        }

        // Atualiza o estado e a data de aprovação
        recipe.Status = Status.Approved;
        recipe.ReviewedAt = DateTime.UtcNow;
        recipe.RejectionReason = null;

        // Guarda as alterações no SQL Server
        _unitOfWork.SaveChanges();

        return true;
    }

    public bool RejectRecipe(int id, string? reason)
    {
        // Procura a receita pelo ID
        var recipe = _recipeRepository.GetById(id);

        // Apenas permite rejeitar receitas pendentes
        if (recipe == null || recipe.Status != Status.Pending)
        {
            return false;
        }

        // Regista a rejeição e o motivo, caso exista
        recipe.Status = Status.Rejected;
        recipe.ReviewedAt = DateTime.UtcNow;

        recipe.RejectionReason = string.IsNullOrWhiteSpace(reason)
            ? null
            : reason.Trim();

        // Guarda as alterações no SQL Server
        _unitOfWork.SaveChanges();

        return true;
    }

}

