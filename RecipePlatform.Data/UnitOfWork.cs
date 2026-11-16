using RecipePlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _context; 

    public UnitOfWork(ApplicationContext context) 
    {
        _context = context;
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }
}
