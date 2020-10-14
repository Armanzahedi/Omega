using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Omega.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Omega.Infrastructure
{
    public class MyDbContext : IdentityDbContext<User>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        //public DbSet<Brand> Brands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
