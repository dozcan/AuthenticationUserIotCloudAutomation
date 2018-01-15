using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Models
{
   public class User
   {
      public string name{get;set;}
      public string email{get;set;}
      public string password{get;set;}

   }
}