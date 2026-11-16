using RecipePlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationContext _context;

    public Repository(ApplicationContext context)
    {
        _context = context;
    }

    public List<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public T? GetById(int id)
    {
        if (id <= 0)
        {
            return null;
        }
        return _context.Set<T>().Find(id);
    }

    public void Create(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity;
    }

    public T? Delete(int id)
    {
        var entity = GetById(id);
        if (entity == null)
        {
            return null;
        }
        _context.Set<T>().Remove(entity);
        return entity;
    }
}
