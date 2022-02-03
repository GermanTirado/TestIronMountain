using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestIronMountain.Models;

    public class NewDbContext : DbContext
    {
        public NewDbContext (DbContextOptions<NewDbContext> options)
            : base(options)
        {
        }

        public DbSet<TestIronMountain.Models.Contact> Contacts { get; set; }
    }
