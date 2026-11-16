using RecipePlatform.Domain.Interfaces;
using RecipePlatform.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public bool CanSubmitRecipe(int userId)
    {
        if (userId <= 0)
        {
            return false;
        }
        var user = _userRepository.GetById(userId);

        if (user == null)
        {
            return false;
        }
        return !user.IsBlocked;
    }
}
