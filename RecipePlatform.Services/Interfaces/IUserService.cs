using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Services.Interfaces;

public interface IUserService
{
    bool CanSubmitRecipe(int userId);
}
