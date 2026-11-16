using System;
using System.Collections.Generic;
using System.Text;

namespace RecipePlatform.Domain.Interfaces;

public interface IUnitOfWork  
{
    int SaveChanges();
}
