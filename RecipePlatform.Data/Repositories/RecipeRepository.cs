using Microsoft.EntityFrameworkCore;
using RecipePlatform.Domain.Entities;
using RecipePlatform.Domain.Enums;
using RecipePlatform.Domain.Interfaces;

namespace RecipePlatform.Data.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly ApplicationContext _context;

    public RecipeRepository(ApplicationContext context)
    {
        _context = context;
    }

    public List<Recipe> GetAll()
    {
        return _context.Recipes.ToList();
    }

    public Recipe? GetById(int id)
    {
        return _context.Recipes.Find(id);
    }

    public List<Recipe> GetApproved()
    {
        return _context.Recipes
            .Where(recipe => recipe.Status == Status.Approved)
            .Include(recipe => recipe.Author)
            .AsNoTracking()
            .OrderBy(recipe => recipe.Title)
            .ToList();
    }

    public List<Recipe> GetByAuthorId(int authorId)
    {
        return _context.Recipes
            .Where(recipe => recipe.AuthorId == authorId)
            .AsNoTracking()
            .ToList();
    }

    public Recipe? GetApprovedByIdWithDetails(int id)
    {
        return _context.Recipes
            .Where(recipe => recipe.Id == id && recipe.Status == Status.Approved)
            .Include(recipe => recipe.Author)
            .Include(recipe => recipe.Categories)
            .Include(recipe => recipe.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .Include(recipe => recipe.Steps)
            .Include(recipe => recipe.Comments)
                .ThenInclude(review => review.User)
            .Include(recipe => recipe.Ratings)
            .AsSplitQuery()   
            .AsNoTracking()
            .SingleOrDefault();
    }   

    public void Create(Recipe recipe)
    {
        _context.Recipes.Add(recipe);
    }

    public Recipe Update(Recipe recipe)
    {
        _context.Recipes.Update(recipe);
        return recipe;
    }

    public Recipe? Delete(int id)
    {
        var recipe = _context.Recipes.Find(id);
        if (recipe == null)
        {
            return null;
        }
        _context.Recipes.Remove(recipe);
        return recipe;
    }
}
