using RecipePlatform.Domain.Entities;
using RecipePlatform.Domain.Interfaces;

namespace RecipePlatform.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public List<User> GetAll()
    {
        return _context.Users.ToList();
    }

    public User? GetUserByEmail(string email)
    {
        return _context.Users.SingleOrDefault(user => user.Email == email); 
    }

    public User? GetById(int id)
    {
        return _context.Users.Find(id);
    }

    public void Create(User user)
    {
        _context.Users.Add(user);
    }

    public User Update(User user)
    {
        _context.Users.Update(user);
        return user;
    }

    public User? Delete(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return null;
        }
        _context.Users.Remove(user);

        return user;
    }
}
