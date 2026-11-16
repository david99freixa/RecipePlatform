using RecipePlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    User? GetUserByEmail(string email);
}
