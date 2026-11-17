using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using RecipePlatform.Domain.Entities;
using RecipePlatform.Domain.Enums;
using RecipePlatform.Domain.Interfaces;
using RecipePlatform.Services.DTOs;
using RecipePlatform.Services.Interfaces;

namespace RecipePlatform.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public User Register(RegisterDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var name = dto.Name?.Trim() ?? "";
        var email = dto.Email?.Trim().ToLowerInvariant() ?? "";

        if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
            throw new ArgumentException("Nome inválido.");

        if (email.Length > 256 ||
            !new EmailAddressAttribute().IsValid(email))
            throw new ArgumentException("Email inválido.");

        if (string.IsNullOrWhiteSpace(dto.Password) ||
            dto.Password.Length < 8 ||
            dto.Password.Length > 128)
            throw new ArgumentException(
                "A password deve ter entre 8 e 128 caracteres.");

        if (_userRepository.GetUserByEmail(email) != null)
            throw new InvalidOperationException(
                "Este email já está registado.");

        var user = new User
        {
            Name = name,
            Email = email,
            Role = Role.User,
            IsBlocked = false
        };

        // Guarda a password de forma protegida, nunca em texto simples
        user.PasswordHash = _passwordHasher.HashPassword(
            user, dto.Password);

        _userRepository.Create(user);
        _unitOfWork.SaveChanges();

        return user;
    }

    public User? Login(LoginDto dto)
    {
        if (dto == null ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrEmpty(dto.Password))
            return null;

        var email = dto.Email.Trim().ToLowerInvariant();

        var user = _userRepository.GetUserByEmail(email);

        if (user == null || user.IsBlocked)
            return null;

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            dto.Password);

        if (result == PasswordVerificationResult.Failed)
            return null;

        return user;
    }
}
