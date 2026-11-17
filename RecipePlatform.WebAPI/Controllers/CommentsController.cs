using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipePlatform.Services.DTOs;
using RecipePlatform.Services.Interfaces;
using System.Security.Claims;

namespace RecipePlatform.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/recipes/{recipeId:int}/comments")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    // Obtém o ID do utilizador autenticado através do JWT
    private int? GetUserId()
    {
        var value = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        return int.TryParse(value, out int id) ? id : null;
    }

    // POST: api/recipes/1/comments
    [HttpPost]
    public IActionResult Create(
        int recipeId, [FromBody] CommentInputDto dto)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        try
        {
            int id = _commentService.Create(
                recipeId, userId.Value, dto.Text);

            return StatusCode(201, new
            {
                Id = id,
                Message = "Comentário criado com sucesso."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    // PUT: api/recipes/1/comments/1
    [HttpPut("{commentId:int}")]
    public IActionResult Update(
        int recipeId,
        int commentId,
        [FromBody] CommentInputDto dto)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        try
        {
            bool updated = _commentService.Update(
                recipeId, commentId, userId.Value, dto.Text);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    // DELETE: api/recipes/1/comments/1
    [HttpDelete("{commentId:int}")]
    public IActionResult Delete(int recipeId, int commentId)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        try
        {
            bool deleted = _commentService.Delete(
                recipeId, commentId, userId.Value);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
