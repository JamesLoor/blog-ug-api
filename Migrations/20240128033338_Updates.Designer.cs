﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using blog_ug_api.Models;

#nullable disable

namespace blog_ug_api.Migrations
{
    [DbContext(typeof(RailwayContext))]
    [Migration("20240128033338_Updates")]
    partial class Updates
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");

            modelBuilder.Entity("blog_ug_api.Models.Comentario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Contenido")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("blog_ug_api.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Categoria")
                        .HasColumnType("longtext");

                    b.Property<string>("Contenido")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FechaPublicacion")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Imagen")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("blog_ug_api.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Contrasena")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Last_login")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("blog_ug_api.Models.Comentario", b =>
                {
                    b.HasOne("blog_ug_api.Models.Post", "Post")
                        .WithMany("Comentarios")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("blog_ug_api.Models.User", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("blog_ug_api.Models.Post", b =>
                {
                    b.HasOne("blog_ug_api.Models.User", "Usuario")
                        .WithMany("Posts")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("blog_ug_api.Models.Post", b =>
                {
                    b.Navigation("Comentarios");
                });

            modelBuilder.Entity("blog_ug_api.Models.User", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
