using System;
using System.Collections.Generic;
using System.Text;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ApplicationDbContext : DbContext    
    {

        public DbSet<Produto> Produtos {get;set;}
    public ApplicationDbContext (DbContextOptions<ApplicationDbContext>options): base(options)  
    {

    
    }


    }

}