﻿// <auto-generated />
using System;
using CleanEjdg.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.PgsqlDb
{
    [DbContext(typeof(PgsqlDbContext))]
    [Migration("20230630090048_Added Images to Cat with image entity2")]
    partial class AddedImagestoCatwithimageentity2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CleanEjdg.Core.Domain.Entities.Cat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("HasChip")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSterilized")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsVaccinated")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Cats");
                });

            modelBuilder.Entity("CleanEjdg.Core.Domain.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CatId")
                        .HasColumnType("integer");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CatId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("CleanEjdg.Core.Domain.Entities.Image", b =>
                {
                    b.HasOne("CleanEjdg.Core.Domain.Entities.Cat", null)
                        .WithMany("Images")
                        .HasForeignKey("CatId");
                });

            modelBuilder.Entity("CleanEjdg.Core.Domain.Entities.Cat", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
