﻿// <auto-generated />
using System;
using GraphQLApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GraphQLApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231103172310_MoneyRelatedModelsAdded")]
    partial class MoneyRelatedModelsAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.24")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GraphQLApi.Models.AcademyFacilityModel", b =>
                {
                    b.Property<Guid>("AcademyId")
                        .HasColumnType("uuid");

                    b.Property<int>("FacilitiesQuality")
                        .HasColumnType("integer");

                    b.Property<int>("ManagerQuality")
                        .HasColumnType("integer");

                    b.Property<string>("SecondTeamName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AcademyId");

                    b.ToTable("AcademyFacilities");
                });

            modelBuilder.Entity("GraphQLApi.Models.LeagueModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("GraphQLApi.Models.LogoModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("IconId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MainColor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SecondaryColor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Logos");
                });

            modelBuilder.Entity("GraphQLApi.Models.ProfitModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Season")
                        .HasColumnType("integer");

                    b.Property<double?>("Stadium")
                        .HasColumnType("double precision");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TeamModelId")
                        .HasColumnType("uuid");

                    b.Property<double?>("Transfers")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("TeamModelId");

                    b.ToTable("Profits");
                });

            modelBuilder.Entity("GraphQLApi.Models.ScoresModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Draws")
                        .HasColumnType("integer");

                    b.Property<string>("Form")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("LeagueModelId")
                        .HasColumnType("uuid");

                    b.Property<int>("Lost")
                        .HasColumnType("integer");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.Property<int>("Season")
                        .HasColumnType("integer");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TeamModelId")
                        .HasColumnType("uuid");

                    b.Property<int>("Wins")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LeagueModelId");

                    b.HasIndex("TeamModelId");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("GraphQLApi.Models.ShirtModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsSecond")
                        .HasColumnType("boolean");

                    b.Property<string>("MainColor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SecondaryColor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TeamModelId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TeamModelId");

                    b.ToTable("Shirts");
                });

            modelBuilder.Entity("GraphQLApi.Models.SpendingModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double?>("Salaries")
                        .HasColumnType("double precision");

                    b.Property<int>("Season")
                        .HasColumnType("integer");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TeamModelId")
                        .HasColumnType("uuid");

                    b.Property<double?>("Transfers")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("TeamModelId");

                    b.ToTable("Spendings");
                });

            modelBuilder.Entity("GraphQLApi.Models.StadiumModel", b =>
                {
                    b.Property<Guid>("StadiumId")
                        .HasColumnType("uuid");

                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<int>("FansExtrasQuality")
                        .HasColumnType("integer");

                    b.Property<int>("SeatQuality")
                        .HasColumnType("integer");

                    b.Property<string>("StadiumName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("StadiumId");

                    b.ToTable("Stadiums");
                });

            modelBuilder.Entity("GraphQLApi.Models.TeamModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("LogoId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("GraphQLApi.Models.UserPreferencesModel", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<bool>("BottomMenu")
                        .HasColumnType("boolean");

                    b.Property<string>("Mode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NavbarColor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("UserPreferences");
                });

            modelBuilder.Entity("GraphQLApi.Models.LogoModel", b =>
                {
                    b.HasOne("GraphQLApi.Models.TeamModel", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");
                });

            modelBuilder.Entity("GraphQLApi.Models.ProfitModel", b =>
                {
                    b.HasOne("GraphQLApi.Models.TeamModel", null)
                        .WithMany("Profits")
                        .HasForeignKey("TeamModelId");
                });

            modelBuilder.Entity("GraphQLApi.Models.ScoresModel", b =>
                {
                    b.HasOne("GraphQLApi.Models.LeagueModel", null)
                        .WithMany("Scores")
                        .HasForeignKey("LeagueModelId");

                    b.HasOne("GraphQLApi.Models.TeamModel", null)
                        .WithMany("Scores")
                        .HasForeignKey("TeamModelId");
                });

            modelBuilder.Entity("GraphQLApi.Models.ShirtModel", b =>
                {
                    b.HasOne("GraphQLApi.Models.TeamModel", null)
                        .WithMany("Shirts")
                        .HasForeignKey("TeamModelId");
                });

            modelBuilder.Entity("GraphQLApi.Models.SpendingModel", b =>
                {
                    b.HasOne("GraphQLApi.Models.TeamModel", null)
                        .WithMany("Spendings")
                        .HasForeignKey("TeamModelId");
                });

            modelBuilder.Entity("GraphQLApi.Models.LeagueModel", b =>
                {
                    b.Navigation("Scores");
                });

            modelBuilder.Entity("GraphQLApi.Models.TeamModel", b =>
                {
                    b.Navigation("Profits");

                    b.Navigation("Scores");

                    b.Navigation("Shirts");

                    b.Navigation("Spendings");
                });
#pragma warning restore 612, 618
        }
    }
}
