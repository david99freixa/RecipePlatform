using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RecipePlatform.Services.DTOs;
using RecipePlatform.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecipePlatform.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;

    // Recebe o serviço de autenticação e as configurações
    public AuthController(
        IAuthService authService,
        IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
    }

    // POST: api/auth/register
    // Regista um novo utilizador
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register(RegisterDto dto)
    {
        try
        {
            var user = _authService.Register(dto);

            return StatusCode(201, new
            {
                user.Id,
                user.Name,
                user.Email
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // POST: api/auth/login
    // Verifica as credenciais e gera um token JWT
    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<AuthResponseDto> Login(LoginDto dto)
    {
        var user = _authService.Login(dto);

        // Rejeita credenciais inválidas
        if (user == null)
        {
            return Unauthorized(new
            {
                message = "Credenciais inválidas."
            });
        }

        // Dados do utilizador incluídos no token
        var claims = new List<Claim>
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(ClaimTypes.Name, user.Name),

            new Claim(ClaimTypes.Email, user.Email),

            new Claim(
                ClaimTypes.Role,
                user.Role.ToString())
        };

        // Obtém a chave utilizada para assinar o token
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        // Cria o token com validade de duas horas
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        // Devolve o token e os dados básicos do utilizador
        return Ok(new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler()
                .WriteToken(token),

            ExpiresAt = token.ValidTo,
            UserId = user.Id,
            Name = user.Name,
            Role = user.Role.ToString()
        });
    }

    // GET: api/auth/me
    // Testa se o token identifica um utilizador autenticado
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            Id = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value,

            Name = User.Identity?.Name,

            Role = User.FindFirst(
                ClaimTypes.Role)?.Value
        });
    }
}
