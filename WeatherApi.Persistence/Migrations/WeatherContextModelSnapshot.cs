﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WeatherApi.Persistence;

#nullable disable

namespace WeatherApi.Migrations
{
    [DbContext(typeof(WeatherContext))]
    partial class WeatherContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WeatherApi.Models.City", b =>
                {
                    b.Property<Guid>("IdCity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("IdCity");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("CityData");
                });

            modelBuilder.Entity("WeatherApi.Models.Weather", b =>
                {
                    b.Property<Guid>("IdWeather")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DayTime")
                        .HasColumnType("integer");

                    b.Property<double>("Humidity")
                        .HasColumnType("double precision");

                    b.Property<Guid>("IdCity")
                        .HasColumnType("uuid");

                    b.Property<int>("MaxTemperature")
                        .HasColumnType("integer");

                    b.Property<int>("MinTemperature")
                        .HasColumnType("integer");

                    b.Property<int>("NightTime")
                        .HasColumnType("integer");

                    b.Property<double>("Precipitation")
                        .HasColumnType("double precision");

                    b.Property<double>("WindSpeed")
                        .HasColumnType("double precision");

                    b.HasKey("IdWeather");

                    b.HasIndex("IdCity");

                    b.ToTable("WeatherData");
                });

            modelBuilder.Entity("WeatherApi.Models.Weather", b =>
                {
                    b.HasOne("WeatherApi.Models.City", "City")
                        .WithMany("WeatherDataList")
                        .HasForeignKey("IdCity")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("WeatherApi.Models.City", b =>
                {
                    b.Navigation("WeatherDataList");
                });
#pragma warning restore 612, 618
        }
    }
}