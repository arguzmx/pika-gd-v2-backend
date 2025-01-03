﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using pika.servicios.contenido.dbcontext;

#nullable disable

namespace pika.servicios.contenido.data.migrations
{
    [DbContext(typeof(DbContextContenido))]
    [Migration("20241226165957_MigracionInicial")]
    partial class MigracionInicial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("pika.modelo.contenido.Carpeta", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("CarpetaPadreId")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("CreadorId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<bool>("EsRaiz")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("PermisoId")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("RepositorioId")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("RepositorioId");

                    b.ToTable("cont$carpeta", (string)null);
                });

            modelBuilder.Entity("pika.modelo.contenido.Contenido", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("CarpetaId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<int>("ConteoAnexos")
                        .HasColumnType("int");

                    b.Property<string>("CreadorId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("IdExterno")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("PermisoId")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("RepositorioId")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<long>("TamanoBytes")
                        .HasColumnType("bigint");

                    b.Property<string>("TipoElemento")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("VolumenId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("CarpetaId");

                    b.HasIndex("RepositorioId");

                    b.HasIndex("VolumenId");

                    b.ToTable("cont$contenido", (string)null);
                });

            modelBuilder.Entity("pika.modelo.contenido.Repositorio", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("DominioId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("UOrgId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("VolumenId")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("VolumenId");

                    b.ToTable("cont$repositorio", (string)null);
                });

            modelBuilder.Entity("pika.modelo.contenido.Volumen", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<bool>("Activo")
                        .HasColumnType("tinyint(1)");

                    b.Property<long>("CanidadElementos")
                        .HasColumnType("bigint");

                    b.Property<long>("CanidadPartes")
                        .HasColumnType("bigint");

                    b.Property<bool>("ConfiguracionValida")
                        .HasColumnType("tinyint(1)");

                    b.Property<long>("ConsecutivoVolumen")
                        .HasColumnType("bigint");

                    b.Property<string>("DominioId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<bool>("EscrituraHabilitada")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<long>("Tamano")
                        .HasColumnType("bigint");

                    b.Property<long>("TamanoMaximo")
                        .HasColumnType("bigint");

                    b.Property<string>("TipoGestorESId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("UOrgId")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.HasKey("Id");

                    b.ToTable("cont$volumen", (string)null);
                });

            modelBuilder.Entity("pika.modelo.contenido.Carpeta", b =>
                {
                    b.HasOne("pika.modelo.contenido.Repositorio", "Repositorio")
                        .WithMany("Carpetas")
                        .HasForeignKey("RepositorioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Repositorio");
                });

            modelBuilder.Entity("pika.modelo.contenido.Contenido", b =>
                {
                    b.HasOne("pika.modelo.contenido.Carpeta", "Carpeta")
                        .WithMany("Contenido")
                        .HasForeignKey("CarpetaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("pika.modelo.contenido.Repositorio", "Repositorio")
                        .WithMany("Contenido")
                        .HasForeignKey("RepositorioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("pika.modelo.contenido.Volumen", "Volumen")
                        .WithMany("Contenido")
                        .HasForeignKey("VolumenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Carpeta");

                    b.Navigation("Repositorio");

                    b.Navigation("Volumen");
                });

            modelBuilder.Entity("pika.modelo.contenido.Repositorio", b =>
                {
                    b.HasOne("pika.modelo.contenido.Volumen", "Volumen")
                        .WithMany("Repositorios")
                        .HasForeignKey("VolumenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Volumen");
                });

            modelBuilder.Entity("pika.modelo.contenido.Carpeta", b =>
                {
                    b.Navigation("Contenido");
                });

            modelBuilder.Entity("pika.modelo.contenido.Repositorio", b =>
                {
                    b.Navigation("Carpetas");

                    b.Navigation("Contenido");
                });

            modelBuilder.Entity("pika.modelo.contenido.Volumen", b =>
                {
                    b.Navigation("Contenido");

                    b.Navigation("Repositorios");
                });
#pragma warning restore 612, 618
        }
    }
}
