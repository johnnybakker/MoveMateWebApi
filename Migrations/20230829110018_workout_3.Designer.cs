﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoveMateWebApi.Database;

#nullable disable

namespace MoveMateWebApi.Migrations
{
    [DbContext(typeof(MoveMateDbContext))]
    [Migration("20230829110018_workout_3")]
    partial class workout_3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity(".EnumEntity<MoveMateWebApi.Models.Data.WorkoutType>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("WorkoutTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "RUNNING"
                        },
                        new
                        {
                            Id = 2,
                            Name = "WALKING"
                        });
                });

            modelBuilder.Entity("MoveMateWebApi.Models.Data.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FirebaseToken")
                        .HasColumnType("longtext");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("MoveMateWebApi.Models.Data.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("ToUserId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ToUserId");

                    b.HasIndex("UserId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("MoveMateWebApi.Models.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MoveMateWebApi.Models.Data.Workout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Workouts");
                });

            modelBuilder.Entity("MoveMateWebApi.Models.Data.WorkoutData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("Time")
                        .HasColumnType("bigint");

                    b.Property<int>("WorkoutId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutId");

                    b.ToTable("WorkoutData");
                });

            modelBuilder.Entity("MoveMateWebApi.Models.Data.Session", b =>
                {
                    b.HasOne("MoveMateWebApi.Models.Data.User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MoveMateWebApi.Models.Data.Subscription", b =>
                {
                    b.HasOne("MoveMateWebApi.Models.Data.User", "ToUser")
                        .WithMany("Subscribers")
                        .HasForeignKey("ToUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoveMateWebApi.Models.Data.User", "User")
                        .WithMany("Subscriptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ToUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MoveMateWebApi.Models.Data.Workout", b =>
                {
                    b.HasOne(".EnumEntity<MoveMateWebApi.Models.Data.WorkoutType>", "TypeEntity")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoveMateWebApi.Models.Data.User", "User")
                        .WithMany("Workouts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TypeEntity");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MoveMateWebApi.Models.Data.WorkoutData", b =>
                {
                    b.HasOne("MoveMateWebApi.Models.Data.Workout", "Workout")
                        .WithMany("Data")
                        .HasForeignKey("WorkoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workout");
                });

            modelBuilder.Entity("MoveMateWebApi.Models.Data.User", b =>
                {
                    b.Navigation("Sessions");

                    b.Navigation("Subscribers");

                    b.Navigation("Subscriptions");

                    b.Navigation("Workouts");
                });

            modelBuilder.Entity("MoveMateWebApi.Models.Data.Workout", b =>
                {
                    b.Navigation("Data");
                });
#pragma warning restore 612, 618
        }
    }
}
