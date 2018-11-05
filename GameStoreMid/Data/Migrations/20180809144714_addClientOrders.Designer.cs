﻿// <auto-generated />
using GameStoreMid.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace GameStoreMid.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180809144714_addClientOrders")]
    partial class addClientOrders
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GameStoreMid.Models.Address", b =>
                {
                    b.Property<int>("AddressID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("Street");

                    b.Property<int?>("ZipCode");

                    b.HasKey("AddressID");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("GameStoreMid.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int>("AddressID");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("AddressID");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("GameStoreMid.Models.BrowsingHistory", b =>
                {
                    b.Property<int>("BrowsingHistroyID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<int>("ProductID");

                    b.Property<string>("UserName");

                    b.Property<DateTime>("Viewed");

                    b.HasKey("BrowsingHistroyID");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("ProductID");

                    b.ToTable("BrowsingHistory");
                });

            modelBuilder.Entity("GameStoreMid.Models.Deal", b =>
                {
                    b.Property<int>("DealID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<double>("PercentageDiscount");

                    b.HasKey("DealID");

                    b.ToTable("Deal");
                });

            modelBuilder.Entity("GameStoreMid.Models.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<DateTime>("ExpectedDate");

                    b.Property<DateTime>("OrderDate");

                    b.Property<double>("Total");

                    b.HasKey("OrderID");

                    b.ToTable("Order");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Order");
                });

            modelBuilder.Entity("GameStoreMid.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Cost");

                    b.Property<int?>("DealID");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("ProductDescription");

                    b.Property<string>("ProductName")
                        .IsRequired();

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<int>("TotalQuantity");

                    b.HasKey("ProductID");

                    b.HasIndex("DealID");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("GameStoreMid.Models.ProductOrder", b =>
                {
                    b.Property<int>("ProductID");

                    b.Property<int>("OrderID");

                    b.Property<int?>("OrderID1");

                    b.Property<int?>("ProductID1");

                    b.Property<int?>("Quantity");

                    b.HasKey("ProductID", "OrderID");

                    b.HasIndex("OrderID");

                    b.HasIndex("OrderID1");

                    b.HasIndex("ProductID1");

                    b.ToTable("ProductOrder");
                });

            modelBuilder.Entity("GameStoreMid.Models.ProductTag", b =>
                {
                    b.Property<int>("ProductID");

                    b.Property<int>("TagID");

                    b.Property<int?>("ProductID1");

                    b.Property<int?>("TagID1");

                    b.HasKey("ProductID", "TagID");

                    b.HasIndex("ProductID1");

                    b.HasIndex("TagID");

                    b.HasIndex("TagID1");

                    b.ToTable("ProductTag");
                });

            modelBuilder.Entity("GameStoreMid.Models.Review", b =>
                {
                    b.Property<int>("ReviewID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserID")
                        .IsRequired();

                    b.Property<string>("Content")
                        .HasMaxLength(50);

                    b.Property<DateTime>("PostDate");

                    b.Property<int>("ProductID");

                    b.Property<int>("Rate");

                    b.HasKey("ReviewID");

                    b.HasIndex("ApplicationUserID");

                    b.HasIndex("ProductID");

                    b.ToTable("Review");
                });

            modelBuilder.Entity("GameStoreMid.Models.Tag", b =>
                {
                    b.Property<int>("TagID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("TagID");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("GameStoreMid.Models.ClientOrder", b =>
                {
                    b.HasBaseType("GameStoreMid.Models.Order");

                    b.Property<string>("ApplicationUserID");

                    b.HasIndex("ApplicationUserID");

                    b.ToTable("ClientOrder");

                    b.HasDiscriminator().HasValue("ClientOrder");
                });

            modelBuilder.Entity("GameStoreMid.Models.ApplicationUser", b =>
                {
                    b.HasOne("GameStoreMid.Models.Address", "Address")
                        .WithMany("ApplicationUsers")
                        .HasForeignKey("AddressID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GameStoreMid.Models.BrowsingHistory", b =>
                {
                    b.HasOne("GameStoreMid.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("BrowsingHistories")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("GameStoreMid.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GameStoreMid.Models.Product", b =>
                {
                    b.HasOne("GameStoreMid.Models.Deal", "Deal")
                        .WithMany("Products")
                        .HasForeignKey("DealID");
                });

            modelBuilder.Entity("GameStoreMid.Models.ProductOrder", b =>
                {
                    b.HasOne("GameStoreMid.Models.Order")
                        .WithMany("ProductOrders")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GameStoreMid.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderID1");

                    b.HasOne("GameStoreMid.Models.Product")
                        .WithMany("ProductOrders")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GameStoreMid.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID1");
                });

            modelBuilder.Entity("GameStoreMid.Models.ProductTag", b =>
                {
                    b.HasOne("GameStoreMid.Models.Product")
                        .WithMany("ProductTags")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GameStoreMid.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID1");

                    b.HasOne("GameStoreMid.Models.Tag")
                        .WithMany("ProductTags")
                        .HasForeignKey("TagID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GameStoreMid.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagID1");
                });

            modelBuilder.Entity("GameStoreMid.Models.Review", b =>
                {
                    b.HasOne("GameStoreMid.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GameStoreMid.Models.Product", "Product")
                        .WithMany("Reviews")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("GameStoreMid.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("GameStoreMid.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GameStoreMid.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("GameStoreMid.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GameStoreMid.Models.ClientOrder", b =>
                {
                    b.HasOne("GameStoreMid.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserID");
                });
#pragma warning restore 612, 618
        }
    }
}
