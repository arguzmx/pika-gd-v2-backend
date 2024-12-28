﻿using apigenerica.model.modelos;
using Microsoft.EntityFrameworkCore;
using pika.modelo.contenido;
using pika.servicios.contenido.dbcontext.configuraciones;

namespace pika.servicios.contenido.dbcontext;

public class DbContextContenido : DbContext
{
    public const string TablaCarpeta = "cont$carpeta";
    public const string TablaContenido = "cont$contenido";
    public const string TablaRepositorio = "cont$repositorio";
    public const string TablaVolumen = "cont$volumen";
    public DbContextContenido(DbContextOptions<DbContextContenido> options) : base(options)
    {
    }
    public DbSet<Carpeta> Carpetas { get; set; }
    public DbSet<Contenido> Contenidos { get; set; }
    public DbSet<Repositorio> Repositorios { get; set; }
    public DbSet<Volumen> Volumenes { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ConfiguracionCarpeta());
        modelBuilder.ApplyConfiguration(new ConfiguracionContenido());
        modelBuilder.ApplyConfiguration(new ConfiguracionRepositorio());
        modelBuilder.ApplyConfiguration(new ConfiguracionVolumen());
        base.OnModelCreating(modelBuilder);
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
