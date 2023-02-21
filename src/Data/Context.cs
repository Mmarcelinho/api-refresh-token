using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AspNetCore.WebApi.Models;
using AspNetCore.WebApi.Services;

namespace AspNetCore.WebApi.Data
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context> options): base(options)
        {

        }
        public DbSet <User> User { get; set;}

        
    }
}