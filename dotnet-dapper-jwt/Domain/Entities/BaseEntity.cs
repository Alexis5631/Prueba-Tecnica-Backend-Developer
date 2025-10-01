using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class BaseEntity
    { 
      public DateOnly CreatedAt { get; set; } 
      public DateOnly UpdatedAt { get; set; } 
    } 
}