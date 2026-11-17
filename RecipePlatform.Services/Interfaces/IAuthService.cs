using RecipePlatform.Domain.Entities;
using RecipePlatform.Services.DTOs;

namespace RecipePlatform.Services.Interfaces;

public interface IAuthService
{
    User Register(RegisterDto dto);

    User? Login(LoginDto dto);
}
