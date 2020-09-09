using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FeatureDBPortal.Server.Data.Models.RA
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RdFeatureAvailabilities> RdFeatureAvailabilities { get; set; }
        public virtual DbSet<RdFeatures> RdFeatures { get; set; }
        public virtual DbSet<RdModels> RdModels { get; set; }
        public virtual DbSet<RdVersions> RdVersions { get; set; }
        public virtual DbSet<TestRdFeatureAvail> TestRdFeatureAvail { get; set; }
        public virtual DbSet<TestRdFeatures> TestRdFeatures { get; set; }
        public virtual DbSet<TestRdModels> TestRdModels { get; set; }
        public virtual DbSet<TestRdVersions> TestRdVersions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseOracle("");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "RDREP");

            modelBuilder.Entity<RdFeatureAvailabilities>(entity =>
            {
                entity.HasKey(e => new { e.FavMajorversion, e.FavModelcode, e.FavFeaturePartnumber })
                    .HasName("RD_FEATURE_AVAILABILITIES_PK");

                entity.ToTable("RD_FEATURE_AVAILABILITIES");

                entity.HasIndex(e => new { e.FavFeaturecode, e.FavFeatureBosName })
                    .HasName("FAV_FEATURECODE_BOS_NAME");

                entity.HasIndex(e => new { e.FavMajorversion, e.FavModelcode, e.FavFeaturePartnumber })
                    .HasName("RD_FEATURE_AVAILABILITIES_PK")
                    .IsUnique();

                entity.Property(e => e.FavMajorversion)
                    .HasColumnName("FAV_MAJORVERSION")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.FavModelcode)
                    .HasColumnName("FAV_MODELCODE")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.FavFeaturePartnumber)
                    .HasColumnName("FAV_FEATURE_PARTNUMBER")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.FavFeatureBosName)
                    .HasColumnName("FAV_FEATURE_BOS_NAME")
                    .HasColumnType("VARCHAR2(100)");

                entity.Property(e => e.FavFeaturecode)
                    .IsRequired()
                    .HasColumnName("FAV_FEATURECODE")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.FavTimestamp)
                    .HasColumnName("FAV_TIMESTAMP")
                    .HasColumnType("DATE");

                entity.Property(e => e.FavUser)
                    .HasColumnName("FAV_USER")
                    .HasColumnType("VARCHAR2(100)");

                entity.HasOne(d => d.FavFeaturecodeNavigation)
                    .WithMany(p => p.RdFeatureAvailabilities)
                    .HasForeignKey(d => d.FavFeaturecode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RD_FEATURE_AVAILABILITIES_R01");

                entity.HasOne(d => d.FavMajorversionNavigation)
                    .WithMany(p => p.RdFeatureAvailabilities)
                    .HasForeignKey(d => d.FavMajorversion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RD_FEATURE_AVAILABILITIES_R02");

                entity.HasOne(d => d.FavModelcodeNavigation)
                    .WithMany(p => p.RdFeatureAvailabilities)
                    .HasForeignKey(d => d.FavModelcode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RD_FEATURE_AVAILABILITIES_R03");
            });

            modelBuilder.Entity<RdFeatures>(entity =>
            {
                entity.HasKey(e => e.FeaCode)
                    .HasName("RD_FEATURES_PK");

                entity.ToTable("RD_FEATURES");

                entity.HasIndex(e => e.FeaCategory)
                    .HasName("FEA_CATEGORY");

                entity.HasIndex(e => e.FeaCode)
                    .HasName("RD_FEATURES_PK")
                    .IsUnique();

                entity.HasIndex(e => e.FeaName)
                    .HasName("RD_FEATURES_U01")
                    .IsUnique();

                entity.Property(e => e.FeaCode)
                    .HasColumnName("FEA_CODE")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.FeaCategory)
                    .HasColumnName("FEA_CATEGORY")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.FeaName)
                    .IsRequired()
                    .HasColumnName("FEA_NAME")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.FeaTimestamp)
                    .HasColumnName("FEA_TIMESTAMP")
                    .HasColumnType("DATE");

                entity.Property(e => e.FeaUser)
                    .HasColumnName("FEA_USER")
                    .HasColumnType("VARCHAR2(100)");
            });

            modelBuilder.Entity<RdModels>(entity =>
            {
                entity.HasKey(e => e.ModCode)
                    .HasName("RD_MODELS_PK");

                entity.ToTable("RD_MODELS");

                entity.HasIndex(e => e.ModCode)
                    .HasName("RD_MODELS_PK")
                    .IsUnique();

                entity.HasIndex(e => e.ModName)
                    .HasName("UNIQUE_MOD_NAME")
                    .IsUnique();

                entity.HasIndex(e => e.ModType)
                    .HasName("MOD_TYPE");

                entity.Property(e => e.ModCode)
                    .HasColumnName("MOD_CODE")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.ModName)
                    .IsRequired()
                    .HasColumnName("MOD_NAME")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.ModTimestamp)
                    .HasColumnName("MOD_TIMESTAMP")
                    .HasColumnType("DATE");

                entity.Property(e => e.ModType)
                    .HasColumnName("MOD_TYPE")
                    .HasColumnType("VARCHAR2(3)");

                entity.Property(e => e.ModUser)
                    .HasColumnName("MOD_USER")
                    .HasColumnType("VARCHAR2(100)");
            });

            modelBuilder.Entity<RdVersions>(entity =>
            {
                entity.HasKey(e => e.VerMajor)
                    .HasName("RD_VERSIONS_PK");

                entity.ToTable("RD_VERSIONS");

                entity.HasIndex(e => e.VerIslatest)
                    .HasName("VER_ISLATEST");

                entity.HasIndex(e => e.VerMajor)
                    .HasName("RD_VERSIONS_PK")
                    .IsUnique();

                entity.Property(e => e.VerMajor)
                    .HasColumnName("VER_MAJOR")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.VerIslatest)
                    .HasColumnName("VER_ISLATEST")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.VerTimestamp)
                    .HasColumnName("VER_TIMESTAMP")
                    .HasColumnType("DATE");

                entity.Property(e => e.VerUser)
                    .HasColumnName("VER_USER")
                    .HasColumnType("VARCHAR2(100)");
            });

            modelBuilder.Entity<TestRdFeatureAvail>(entity =>
            {
                entity.HasKey(e => new { e.TfavModelcode, e.TfavFeaturePartnumber, e.TfavMajorversion })
                    .HasName("TEST_RD_FEATURE_AVAIL_PK");

                entity.ToTable("TEST_RD_FEATURE_AVAIL");

                entity.HasIndex(e => new { e.TfavMajorversion, e.TfavModelcode, e.TfavFeaturePartnumber })
                    .HasName("TEST_RD_FEATURE_AVAIL_U01")
                    .IsUnique();

                entity.Property(e => e.TfavModelcode)
                    .HasColumnName("TFAV_MODELCODE")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.TfavFeaturePartnumber)
                    .HasColumnName("TFAV_FEATURE_PARTNUMBER")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.TfavMajorversion)
                    .HasColumnName("TFAV_MAJORVERSION")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.TfavFeatureBosName)
                    .HasColumnName("TFAV_FEATURE_BOS_NAME")
                    .HasColumnType("VARCHAR2(100)");

                entity.Property(e => e.TfavFeaturecode)
                    .IsRequired()
                    .HasColumnName("TFAV_FEATURECODE")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.TfavTimestamp)
                    .HasColumnName("TFAV_TIMESTAMP")
                    .HasColumnType("DATE");

                entity.Property(e => e.TfavUser)
                    .HasColumnName("TFAV_USER")
                    .HasColumnType("VARCHAR2(100)");

                entity.HasOne(d => d.TfavFeaturecodeNavigation)
                    .WithMany(p => p.TestRdFeatureAvail)
                    .HasForeignKey(d => d.TfavFeaturecode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TEST_RD_FEATURE_AVAIL_R01");

                entity.HasOne(d => d.TfavMajorversionNavigation)
                    .WithMany(p => p.TestRdFeatureAvail)
                    .HasForeignKey(d => d.TfavMajorversion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TEST_RD_FEATURE_AVAIL_R02");

                entity.HasOne(d => d.TfavModelcodeNavigation)
                    .WithMany(p => p.TestRdFeatureAvail)
                    .HasForeignKey(d => d.TfavModelcode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C00326169");
            });

            modelBuilder.Entity<TestRdFeatures>(entity =>
            {
                entity.HasKey(e => e.TfeaCode)
                    .HasName("TEST_RD_FEATURES_PK");

                entity.ToTable("TEST_RD_FEATURES");

                entity.HasIndex(e => e.TfeaCode)
                    .HasName("TEST_RD_FEATURES_PK")
                    .IsUnique();

                entity.HasIndex(e => e.TfeaName)
                    .HasName("TEST_RD_FEATURES_U01")
                    .IsUnique();

                entity.Property(e => e.TfeaCode)
                    .HasColumnName("TFEA_CODE")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.TfeaCategory)
                    .HasColumnName("TFEA_CATEGORY")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.TfeaName)
                    .IsRequired()
                    .HasColumnName("TFEA_NAME")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.TfeaTimestamp)
                    .HasColumnName("TFEA_TIMESTAMP")
                    .HasColumnType("DATE");

                entity.Property(e => e.TfeaUser)
                    .HasColumnName("TFEA_USER")
                    .HasColumnType("VARCHAR2(100)");
            });

            modelBuilder.Entity<TestRdModels>(entity =>
            {
                entity.HasKey(e => e.TmodCode)
                    .HasName("TEST_RD_MODELS_PK");

                entity.ToTable("TEST_RD_MODELS");

                entity.HasIndex(e => e.TmodCode)
                    .HasName("TEST_RD_MODELS_PK")
                    .IsUnique();

                entity.HasIndex(e => e.TmodName)
                    .HasName("TEST_UNIQUE_MOD_NAME")
                    .IsUnique();

                entity.Property(e => e.TmodCode)
                    .HasColumnName("TMOD_CODE")
                    .HasColumnType("VARCHAR2(20)");

                entity.Property(e => e.TmodName)
                    .IsRequired()
                    .HasColumnName("TMOD_NAME")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.TmodTimestamp)
                    .HasColumnName("TMOD_TIMESTAMP")
                    .HasColumnType("DATE");

                entity.Property(e => e.TmodType)
                    .HasColumnName("TMOD_TYPE")
                    .HasColumnType("VARCHAR2(3)");

                entity.Property(e => e.TmodUser)
                    .HasColumnName("TMOD_USER")
                    .HasColumnType("VARCHAR2(100)");
            });

            modelBuilder.Entity<TestRdVersions>(entity =>
            {
                entity.HasKey(e => e.TverMajor)
                    .HasName("TEST_RD_VERSIONS_PK");

                entity.ToTable("TEST_RD_VERSIONS");

                entity.HasIndex(e => e.TverMajor)
                    .HasName("TEST_RD_VERSIONS_PK")
                    .IsUnique();

                entity.Property(e => e.TverMajor)
                    .HasColumnName("TVER_MAJOR")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.TverIslatest)
                    .HasColumnName("TVER_ISLATEST")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.TverTimestamp)
                    .HasColumnName("TVER_TIMESTAMP")
                    .HasColumnType("DATE");

                entity.Property(e => e.TverUser)
                    .HasColumnName("TVER_USER")
                    .HasColumnType("VARCHAR2(100)");
            });

            modelBuilder.HasSequence("ISEQ$$_5890272");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
