using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Domain.Interfaces;

public interface IRepository<T> where T : class
{ 
    List<T> GetAll();

    T? GetById(int id);

    void Create(T entity);

    T Update(T entity);

    T? Delete(int id);
}
