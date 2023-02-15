﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230202191803_AddVirtualServiceToApplication")]
    partial class AddVirtualServiceToApplication
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.Models.Admin", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AdminRoleId")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsFirstLogin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LoginToken")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LoginTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("AdminRoleId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("Core.Models.AdminRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ManageApplication")
                        .HasColumnType("bit");

                    b.Property<bool>("ManageCountry")
                        .HasColumnType("bit");

                    b.Property<bool>("ManageIcons")
                        .HasColumnType("bit");

                    b.Property<bool>("ManageService")
                        .HasColumnType("bit");

                    b.Property<bool>("ManageServiceCategory")
                        .HasColumnType("bit");

                    b.Property<bool>("ManageSponsor")
                        .HasColumnType("bit");

                    b.Property<bool>("ManageUsers")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ReadApplication")
                        .HasColumnType("bit");

                    b.Property<bool>("ReadCountry")
                        .HasColumnType("bit");

                    b.Property<bool>("ReadIcon")
                        .HasColumnType("bit");

                    b.Property<bool>("ReadIcons")
                        .HasColumnType("bit");

                    b.Property<bool>("ReadService")
                        .HasColumnType("bit");

                    b.Property<bool>("ReadServiceCategory")
                        .HasColumnType("bit");

                    b.Property<bool>("ReadSponsor")
                        .HasColumnType("bit");

                    b.Property<bool>("ReadUsers")
                        .HasColumnType("bit");

                    b.Property<bool>("WriteIcon")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("AdminRoles");
                });

            modelBuilder.Entity("Core.Models.AdminVerification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AdminId")
                        .HasColumnType("bigint");

                    b.Property<Guid>("EmailVerificationToken")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EmailVerificationTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("bit");

                    b.Property<Guid>("ResetPasswordToken")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ResetPasswordTokenExpiry")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("AdminVerification");
                });

            modelBuilder.Entity("Core.Models.Application", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<long>("CountryDistrictId")
                        .HasColumnType("bigint");

                    b.Property<long>("CountryId")
                        .HasColumnType("bigint");

                    b.Property<long>("DistrictStateId")
                        .HasColumnType("bigint");

                    b.Property<bool>("HasMeeting")
                        .HasColumnType("bit");

                    b.Property<bool>("Intrested")
                        .HasColumnType("bit");

                    b.Property<bool>("IsApprovedForPublish")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCalled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("MeetingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CountryDistrictId");

                    b.HasIndex("CountryId");

                    b.HasIndex("DistrictStateId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("Core.Models.ApplicationFollowUp", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ApplicationId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("ApplicationFollowUps");
                });

            modelBuilder.Entity("Core.Models.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("HomeImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("IconId")
                        .HasColumnType("bigint");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("IconId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Core.Models.CategoryDistrict", b =>
                {
                    b.Property<long>("CountryDistrictId")
                        .HasColumnType("bigint");

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.HasKey("CountryDistrictId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("CategoryDistricts");
                });

            modelBuilder.Entity("Core.Models.CategoryFilter", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<string>("EnumKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("CategoryFilters");
                });

            modelBuilder.Entity("Core.Models.Country", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("IconId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Suffix")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("IconId");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Core.Models.CountryDistrict", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CountryId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("CountryDistricts");
                });

            modelBuilder.Entity("Core.Models.DistrictState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CountryDistrictId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountryDistrictId");

                    b.ToTable("DistrictStates");
                });

            modelBuilder.Entity("Core.Models.Icon", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Svg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Icons");
                });

            modelBuilder.Entity("Core.Models.Service", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ApplicationId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasPackage")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSubscribed")
                        .HasColumnType("bit");

                    b.Property<decimal>("Lat")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Lng")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Quote")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SearchImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("YoutubeVideoId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId")
                        .IsUnique();

                    b.ToTable("Services");
                });

            modelBuilder.Entity("Core.Models.ServiceGallery", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<long>("ServiceId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId");

                    b.ToTable("ServiceGalleries");
                });

            modelBuilder.Entity("Core.Models.ServicePackage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("HasPrice")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<string>("PackageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("ServiceId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId");

                    b.ToTable("ServicePackages");
                });

            modelBuilder.Entity("Core.Models.ServicePackageAttribute", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ServicePackageId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ServicePackageId");

                    b.ToTable("ServicePackageAttributes");
                });

            modelBuilder.Entity("Core.Models.ServiceSocialMedia", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("IconId")
                        .HasColumnType("bigint");

                    b.Property<long>("ServiceId")
                        .HasColumnType("bigint");

                    b.Property<string>("SocialMediaLink")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IconId");

                    b.HasIndex("ServiceId");

                    b.ToTable("ServiceSocialMedias");
                });

            modelBuilder.Entity("Core.Models.Admin", b =>
                {
                    b.HasOne("Core.Models.AdminRole", "AdminRole")
                        .WithMany()
                        .HasForeignKey("AdminRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AdminRole");
                });

            modelBuilder.Entity("Core.Models.AdminVerification", b =>
                {
                    b.HasOne("Core.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("Core.Models.Application", b =>
                {
                    b.HasOne("Core.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Models.CountryDistrict", "CountryDistrict")
                        .WithMany()
                        .HasForeignKey("CountryDistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Models.DistrictState", "DistrictState")
                        .WithMany()
                        .HasForeignKey("DistrictStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Country");

                    b.Navigation("CountryDistrict");

                    b.Navigation("DistrictState");
                });

            modelBuilder.Entity("Core.Models.ApplicationFollowUp", b =>
                {
                    b.HasOne("Core.Models.Application", "Application")
                        .WithMany("ApplicationFollowUps")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("Core.Models.Category", b =>
                {
                    b.HasOne("Core.Models.Icon", "Icon")
                        .WithMany()
                        .HasForeignKey("IconId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Icon");
                });

            modelBuilder.Entity("Core.Models.CategoryDistrict", b =>
                {
                    b.HasOne("Core.Models.Category", "Category")
                        .WithMany("CategoryDistricts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Models.CountryDistrict", "CountryDistrict")
                        .WithMany("CategoryDistricts")
                        .HasForeignKey("CountryDistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("CountryDistrict");
                });

            modelBuilder.Entity("Core.Models.CategoryFilter", b =>
                {
                    b.HasOne("Core.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Core.Models.Country", b =>
                {
                    b.HasOne("Core.Models.Icon", "Icon")
                        .WithMany()
                        .HasForeignKey("IconId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Icon");
                });

            modelBuilder.Entity("Core.Models.CountryDistrict", b =>
                {
                    b.HasOne("Core.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Core.Models.DistrictState", b =>
                {
                    b.HasOne("Core.Models.CountryDistrict", "CountryDistrict")
                        .WithMany()
                        .HasForeignKey("CountryDistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CountryDistrict");
                });

            modelBuilder.Entity("Core.Models.Service", b =>
                {
                    b.HasOne("Core.Models.Application", "Application")
                        .WithOne("Service")
                        .HasForeignKey("Core.Models.Service", "ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("Core.Models.ServiceGallery", b =>
                {
                    b.HasOne("Core.Models.Service", "Service")
                        .WithMany("ServiceGallery")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");
                });

            modelBuilder.Entity("Core.Models.ServicePackage", b =>
                {
                    b.HasOne("Core.Models.Service", "Service")
                        .WithMany("ServicePackage")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");
                });

            modelBuilder.Entity("Core.Models.ServicePackageAttribute", b =>
                {
                    b.HasOne("Core.Models.ServicePackage", "ServicePackage")
                        .WithMany()
                        .HasForeignKey("ServicePackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServicePackage");
                });

            modelBuilder.Entity("Core.Models.ServiceSocialMedia", b =>
                {
                    b.HasOne("Core.Models.Icon", "Icon")
                        .WithMany()
                        .HasForeignKey("IconId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Models.Service", "Service")
                        .WithMany("ServiceSocialMedia")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Icon");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("Core.Models.Application", b =>
                {
                    b.Navigation("ApplicationFollowUps");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("Core.Models.Category", b =>
                {
                    b.Navigation("CategoryDistricts");
                });

            modelBuilder.Entity("Core.Models.CountryDistrict", b =>
                {
                    b.Navigation("CategoryDistricts");
                });

            modelBuilder.Entity("Core.Models.Service", b =>
                {
                    b.Navigation("ServiceGallery");

                    b.Navigation("ServicePackage");

                    b.Navigation("ServiceSocialMedia");
                });
#pragma warning restore 612, 618
        }
    }
}
