using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using prueba.Models;

namespace prueba.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<prueba.Models.Cooperativa> Cooperativa { get; set; }
        public DbSet<prueba.Models.Areatrabajo> Areatrabajo { get; set; }
        public DbSet<prueba.Models.Datopersona> Datopersona { get; set; }
        public DbSet<prueba.Models.Mutual> Mutual { get; set; }
    }
}
