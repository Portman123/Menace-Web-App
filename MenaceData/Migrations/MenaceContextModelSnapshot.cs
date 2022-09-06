﻿// <auto-generated />
using System;
using MenaceData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MenaceData.Migrations
{
    [DbContext(typeof(MenaceContext))]
    partial class MenaceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Noughts_and_Crosses.AIMenace", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("AIMenace");
                });

            modelBuilder.Entity("Noughts_and_Crosses.AIOptimalMove", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("AIOptimalMove");
                });

            modelBuilder.Entity("Noughts_and_Crosses.AIRandomMove", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("AIRandomMove");
                });

            modelBuilder.Entity("Noughts_and_Crosses.Bead", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<Guid?>("MatchboxId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MatchboxId");

                    b.ToTable("Bead");
                });

            modelBuilder.Entity("Noughts_and_Crosses.BoardPosition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Encoded")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isFullBoard")
                        .HasColumnType("bit");

                    b.Property<bool>("isWinningPosition")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("BoardPosition");
                });

            modelBuilder.Entity("Noughts_and_Crosses.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CurrentBoardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Finished")
                        .HasColumnType("bit");

                    b.Property<Guid>("P1Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("P2Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TurnNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CurrentBoardId");

                    b.HasIndex("P1Id");

                    b.HasIndex("P2Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Noughts_and_Crosses.GameHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("GameHistories");
                });

            modelBuilder.Entity("Noughts_and_Crosses.Matchbox", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AIMenaceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BoardPositionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AIMenaceId");

                    b.HasIndex("BoardPositionId");

                    b.ToTable("Matchbox");
                });

            modelBuilder.Entity("Noughts_and_Crosses.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Draws")
                        .HasColumnType("int");

                    b.Property<int>("Losses")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Wins")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Player");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Player");
                });

            modelBuilder.Entity("Noughts_and_Crosses.Turn", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("GameHistoryId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GameHistoryId");

                    b.ToTable("Turn");
                });

            modelBuilder.Entity("Noughts_and_Crosses.PlayerHumanOnWeb", b =>
                {
                    b.HasBaseType("Noughts_and_Crosses.Player");

                    b.HasDiscriminator().HasValue("PlayerHumanOnWeb");
                });

            modelBuilder.Entity("Noughts_and_Crosses.PlayerMenace", b =>
                {
                    b.HasBaseType("Noughts_and_Crosses.Player");

                    b.Property<Guid>("MenaceEngineId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("MenaceEngineId");

                    b.HasDiscriminator().HasValue("PlayerMenace");
                });

            modelBuilder.Entity("Noughts_and_Crosses.PlayerOptimal", b =>
                {
                    b.HasBaseType("Noughts_and_Crosses.Player");

                    b.Property<Guid>("OptimalEngineId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("OptimalEngineId");

                    b.HasDiscriminator().HasValue("PlayerOptimal");
                });

            modelBuilder.Entity("Noughts_and_Crosses.PlayerRandom", b =>
                {
                    b.HasBaseType("Noughts_and_Crosses.Player");

                    b.Property<Guid>("RandomEngineId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("RandomEngineId");

                    b.HasDiscriminator().HasValue("PlayerRandom");
                });

            modelBuilder.Entity("Noughts_and_Crosses.Bead", b =>
                {
                    b.HasOne("Noughts_and_Crosses.Matchbox", null)
                        .WithMany("Beads")
                        .HasForeignKey("MatchboxId");
                });

            modelBuilder.Entity("Noughts_and_Crosses.Game", b =>
                {
                    b.HasOne("Noughts_and_Crosses.BoardPosition", "CurrentBoard")
                        .WithMany()
                        .HasForeignKey("CurrentBoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Noughts_and_Crosses.Player", "P1")
                        .WithMany()
                        .HasForeignKey("P1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Noughts_and_Crosses.Player", "P2")
                        .WithMany()
                        .HasForeignKey("P2Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentBoard");

                    b.Navigation("P1");

                    b.Navigation("P2");
                });

            modelBuilder.Entity("Noughts_and_Crosses.Matchbox", b =>
                {
                    b.HasOne("Noughts_and_Crosses.AIMenace", null)
                        .WithMany("Matchboxes")
                        .HasForeignKey("AIMenaceId");

                    b.HasOne("Noughts_and_Crosses.BoardPosition", "BoardPosition")
                        .WithMany()
                        .HasForeignKey("BoardPositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BoardPosition");
                });

            modelBuilder.Entity("Noughts_and_Crosses.Turn", b =>
                {
                    b.HasOne("Noughts_and_Crosses.GameHistory", null)
                        .WithMany("TurnHistory")
                        .HasForeignKey("GameHistoryId");
                });

            modelBuilder.Entity("Noughts_and_Crosses.PlayerMenace", b =>
                {
                    b.HasOne("Noughts_and_Crosses.AIMenace", "MenaceEngine")
                        .WithMany()
                        .HasForeignKey("MenaceEngineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MenaceEngine");
                });

            modelBuilder.Entity("Noughts_and_Crosses.PlayerOptimal", b =>
                {
                    b.HasOne("Noughts_and_Crosses.AIOptimalMove", "OptimalEngine")
                        .WithMany()
                        .HasForeignKey("OptimalEngineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OptimalEngine");
                });

            modelBuilder.Entity("Noughts_and_Crosses.PlayerRandom", b =>
                {
                    b.HasOne("Noughts_and_Crosses.AIRandomMove", "RandomEngine")
                        .WithMany()
                        .HasForeignKey("RandomEngineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RandomEngine");
                });

            modelBuilder.Entity("Noughts_and_Crosses.AIMenace", b =>
                {
                    b.Navigation("Matchboxes");
                });

            modelBuilder.Entity("Noughts_and_Crosses.GameHistory", b =>
                {
                    b.Navigation("TurnHistory");
                });

            modelBuilder.Entity("Noughts_and_Crosses.Matchbox", b =>
                {
                    b.Navigation("Beads");
                });
#pragma warning restore 612, 618
        }
    }
}
