﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RedeSocial.Infrastructure.Data;

#nullable disable

namespace RedeSocial.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230605184650_InitialProject")]
    partial class InitialProject
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("RedeSocial.Domain.Models.Base", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Bases");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Base");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("RedeSocial.Domain.Models.Amizade", b =>
                {
                    b.HasBaseType("RedeSocial.Domain.Models.Base");

                    b.Property<int>("AmigoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StatusAmizade")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UsuarioId1")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UsuarioId2")
                        .HasColumnType("INTEGER");

                    b.HasIndex("AmigoId");

                    b.HasIndex("UsuarioId");

                    b.HasIndex("UsuarioId1");

                    b.HasIndex("UsuarioId2");

                    b.HasDiscriminator().HasValue("Amizade");
                });

            modelBuilder.Entity("RedeSocial.Domain.Models.Comentario", b =>
                {
                    b.HasBaseType("RedeSocial.Domain.Models.Base");

                    b.Property<string>("Autor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("AutorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Conteudo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataPublicacao")
                        .HasColumnType("TEXT");

                    b.Property<int>("PostId")
                        .HasColumnType("INTEGER");

                    b.HasIndex("PostId");

                    b.HasDiscriminator().HasValue("Comentario");
                });

            modelBuilder.Entity("RedeSocial.Domain.Models.Curtida", b =>
                {
                    b.HasBaseType("RedeSocial.Domain.Models.Base");

                    b.Property<int>("PostId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("INTEGER");

                    b.HasIndex("PostId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Bases", t =>
                        {
                            t.Property("PostId")
                                .HasColumnName("Curtida_PostId");

                            t.Property("UsuarioId")
                                .HasColumnName("Curtida_UsuarioId");
                        });

                    b.HasDiscriminator().HasValue("Curtida");
                });

            modelBuilder.Entity("RedeSocial.Domain.Models.Mensagem", b =>
                {
                    b.HasBaseType("RedeSocial.Domain.Models.Base");

                    b.Property<int>("AmigoId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Conteudo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataEnvio")
                        .HasColumnType("TEXT");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("INTEGER");

                    b.HasIndex("AmigoId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Bases", t =>
                        {
                            t.Property("AmigoId")
                                .HasColumnName("Mensagem_AmigoId");

                            t.Property("Conteudo")
                                .HasColumnName("Mensagem_Conteudo");

                            t.Property("UsuarioId")
                                .HasColumnName("Mensagem_UsuarioId");
                        });

                    b.HasDiscriminator().HasValue("Mensagem");
                });

            modelBuilder.Entity("RedeSocial.Domain.Models.Post", b =>
                {
                    b.HasBaseType("RedeSocial.Domain.Models.Base");

                    b.Property<int>("AutorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Conteudo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Criacao")
                        .HasColumnType("TEXT");

                    b.Property<int>("Curtidas")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PermissaoVisualizar")
                        .HasColumnType("INTEGER");

                    b.HasIndex("AutorId");

                    b.ToTable("Bases", t =>
                        {
                            t.Property("AutorId")
                                .HasColumnName("Post_AutorId");

                            t.Property("Conteudo")
                                .HasColumnName("Post_Conteudo");
                        });

                    b.HasDiscriminator().HasValue("Post");
                });

            modelBuilder.Entity("RedeSocial.Domain.Models.Usuario", b =>
                {
                    b.HasBaseType("RedeSocial.Domain.Models.Base");

                    b.Property<string>("Apelido")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Cep")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("DataNascimento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ImagemPerfil")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SenhaHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("Usuario");
                });

            modelBuilder.Entity("RedeSocial.Domain.Models.Amizade", b =>
                {
                    b.HasOne("RedeSocial.Domain.Models.Usuario", "Amigo")
                        .WithMany()
                        .HasForeignKey("AmigoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RedeSocial.Domain.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RedeSocial.Domain.Models.Usuario", null)
                        .WithMany("Amigos")
                        .HasForeignKey("UsuarioId1");

                    b.HasOne("RedeSocial.Domain.Models.Usuario", null)
                        .WithMany("Convites")
                        .HasForeignKey("UsuarioId2");

                    b.Navigation("Amigo");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("RedeSocial.Domain.Models.Comentario", b =>
                {
                    b.HasOne("RedeSocial.Domain.Models.Post", "Post")
                        .WithMany("Comentarios")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("RedeSocial.Domain.Models.Curtida", b =>
                {
                    b.HasOne("RedeSocial.Domain.Models.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RedeSocial.Domain.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("RedeSocial.Domain.Models.Mensagem", b =>
                {
                    b.HasOne("RedeSocial.Domain.Models.Usuario", "Amigo")
                        .WithMany()
                        .HasForeignKey("AmigoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RedeSocial.Domain.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Amigo");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("RedeSocial.Domain.Models.Post", b =>
                {
                    b.HasOne("RedeSocial.Domain.Models.Usuario", "Autor")
                        .WithMany("Posts")
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Autor");
                });

            modelBuilder.Entity("RedeSocial.Domain.Models.Post", b =>
                {
                    b.Navigation("Comentarios");
                });

            modelBuilder.Entity("RedeSocial.Domain.Models.Usuario", b =>
                {
                    b.Navigation("Amigos");

                    b.Navigation("Convites");

                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}