using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Postulate.Models;

namespace Postulate.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    // public DbSet<Actividad> Actividades { get; set; } 
    public DbSet<Localidad> Localidades { get; set; }
    public DbSet<Persona> Personas { get; set; }
    public DbSet<Profesion> Profesiones { get; set; }
    public DbSet<Provincia> Provincias { get; set; }
    
    // public DbSet<Resenia> Resenias { get; set; }
    public DbSet<Servicio> Servicios { get; set; }
    public DbSet<Trabajo> Trabajos { get; set; }
  public DbSet<ContratoRespondido> ContratoRespondidos { get; set; }

  


    
}
