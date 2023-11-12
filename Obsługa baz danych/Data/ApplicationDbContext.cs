using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Obsługa_baz_danych.Models;

namespace Obsługa_baz_danych.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Obsługa_baz_danych.Models.ExerciseType>? ExerciseType { get; set; }
        public DbSet<Obsługa_baz_danych.Models.Exercise>? Exercise { get; set; }
        public DbSet<Obsługa_baz_danych.Models.Session>? Session { get; set; }
    }
}