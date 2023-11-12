using Microsoft.AspNetCore.Identity;

namespace Obsługa_baz_danych.Models
{
    public class Session
    {
        public int Id { get; set; }

        public DateTime Start {  get; set; }
        public DateTime End { get; set; }

        public string? UserId { get; set; }
        public virtual IdentityUser? User { get; set; }
    }
}
