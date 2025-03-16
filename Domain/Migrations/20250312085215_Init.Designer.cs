﻿// <auto-generated />
using System;
using Domain.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Domain.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20250312085215_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Models.BlacklistedIpAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("IpAddress")
                        .HasMaxLength(39)
                        .HasColumnType("nvarchar(39)");

                    b.Property<Guid>("ScrapingLogId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DeletedAt");

                    b.HasIndex("ScrapingLogId");

                    b.ToTable("BlacklistedIpAddresses");
                });

            modelBuilder.Entity("Domain.Models.MalwareSignature", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ScrapingLogId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Signature")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.HasKey("Id");

                    b.HasIndex("DeletedAt");

                    b.HasIndex("ScrapingLogId");

                    b.ToTable("MalwareSignatures");
                });

            modelBuilder.Entity("Domain.Models.ScrapingLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DeletedAt");

                    b.ToTable("ScrapingLogs");
                });

            modelBuilder.Entity("Domain.Models.YaraRule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Rule")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ScrapingLogId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DeletedAt");

                    b.HasIndex("ScrapingLogId");

                    b.ToTable("YaraRules");
                });

            modelBuilder.Entity("Domain.Models.BlacklistedIpAddress", b =>
                {
                    b.HasOne("Domain.Models.ScrapingLog", "ScrapingLog")
                        .WithMany()
                        .HasForeignKey("ScrapingLogId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ScrapingLog");
                });

            modelBuilder.Entity("Domain.Models.MalwareSignature", b =>
                {
                    b.HasOne("Domain.Models.ScrapingLog", "ScrapingLog")
                        .WithMany()
                        .HasForeignKey("ScrapingLogId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ScrapingLog");
                });

            modelBuilder.Entity("Domain.Models.YaraRule", b =>
                {
                    b.HasOne("Domain.Models.ScrapingLog", "ScrapingLog")
                        .WithMany()
                        .HasForeignKey("ScrapingLogId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ScrapingLog");
                });
#pragma warning restore 612, 618
        }
    }
}
