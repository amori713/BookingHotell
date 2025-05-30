﻿// <auto-generated />
using System;
using BookingHotell.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookingHotell.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250109020357_SeedData")]
    partial class SeedData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookingHotell.Models.Booking", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingId"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ExtraBeds")
                        .HasColumnType("int");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("BookingId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("RoomId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("BookingHotell.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            CustomerId = 1,
                            Email = "alice@example.com",
                            FirstName = "Alice",
                            LastName = "Smith",
                            PhoneNumber = "123456789"
                        },
                        new
                        {
                            CustomerId = 2,
                            Email = "bob@example.com",
                            FirstName = "Bob",
                            LastName = "Johnson",
                            PhoneNumber = "987654321"
                        },
                        new
                        {
                            CustomerId = 3,
                            Email = "charlie@example.com",
                            FirstName = "Charlie",
                            LastName = "Brown",
                            PhoneNumber = "456789123"
                        },
                        new
                        {
                            CustomerId = 4,
                            Email = "diana@example.com",
                            FirstName = "Diana",
                            LastName = "Jones",
                            PhoneNumber = "789123456"
                        });
                });

            modelBuilder.Entity("BookingHotell.Models.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoomId"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<int>("ExtraBedsAvailable")
                        .HasColumnType("int");

                    b.Property<bool>("HasExtraBedOption")
                        .HasColumnType("bit");

                    b.Property<decimal>("PricePerNight")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RoomType")
                        .HasColumnType("int");

                    b.HasKey("RoomId");

                    b.ToTable("Rooms");

                    b.HasData(
                        new
                        {
                            RoomId = -1,
                            Capacity = 1,
                            ExtraBedsAvailable = 0,
                            HasExtraBedOption = false,
                            PricePerNight = 500m,
                            RoomType = 1
                        },
                        new
                        {
                            RoomId = -2,
                            Capacity = 2,
                            ExtraBedsAvailable = 2,
                            HasExtraBedOption = true,
                            PricePerNight = 1000m,
                            RoomType = 2
                        },
                        new
                        {
                            RoomId = -3,
                            Capacity = 2,
                            ExtraBedsAvailable = 2,
                            HasExtraBedOption = true,
                            PricePerNight = 1200m,
                            RoomType = 2
                        },
                        new
                        {
                            RoomId = -4,
                            Capacity = 1,
                            ExtraBedsAvailable = 0,
                            HasExtraBedOption = false,
                            PricePerNight = 450m,
                            RoomType = 1
                        });
                });

            modelBuilder.Entity("BookingHotell.Models.Booking", b =>
                {
                    b.HasOne("BookingHotell.Models.Customer", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookingHotell.Models.Room", "Room")
                        .WithMany("Bookings")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("BookingHotell.Models.Customer", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("BookingHotell.Models.Room", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
