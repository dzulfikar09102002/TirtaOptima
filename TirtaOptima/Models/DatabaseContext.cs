using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace TirtaOptima.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActionType> ActionTypes { get; set; }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<CollectionStrategy> CollectionStrategies { get; set; }

    public virtual DbSet<Criteria> Criterias { get; set; }

    public virtual DbSet<Debt> Debts { get; set; }

    public virtual DbSet<DebtsManagement> DebtsManagements { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<FahpCalculation> FahpCalculations { get; set; }

    public virtual DbSet<Leader> Leaders { get; set; }

    public virtual DbSet<Letter> Letters { get; set; }

    public virtual DbSet<LetterCategory> LetterCategories { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PsoResult> PsoResults { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<StrategyResult> StrategyResults { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Village> Villages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = configuration?.GetConnectionString("MySQL") ?? "server=localhost;database=tirtaoptima;user=root";

        optionsBuilder.UseMySql(connectionString, ServerVersion.Parse("8.0.28-mysql"));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<ActionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("action_types");

            entity.HasIndex(e => e.CreatedBy, "jenis_tindakan_users_FK");

            entity.HasIndex(e => e.UpdatedBy, "jenis_tindakan_users_FK_1");

            entity.HasIndex(e => e.DeletedBy, "jenis_tindakan_users_FK_2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ActionTypeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("jenis_tindakan_users_FK");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.ActionTypeDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("jenis_tindakan_users_FK_2");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ActionTypeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("jenis_tindakan_users_FK_1");
        });

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("collections");

            entity.HasIndex(e => e.PiutangId, "penagihan_piutang_FK");

            entity.HasIndex(e => e.StatusId, "penagihan_status_FK");

            entity.HasIndex(e => e.SuratId, "penagihan_surat_FK");

            entity.HasIndex(e => e.CreatedBy, "penagihan_users_FK");

            entity.HasIndex(e => e.UpdatedBy, "penagihan_users_FK_1");

            entity.HasIndex(e => e.DeletedBy, "penagihan_users_FK_2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Alasan)
                .HasMaxLength(255)
                .HasColumnName("alasan");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Foto)
                .HasMaxLength(100)
                .HasColumnName("foto");
            entity.Property(e => e.Ket)
                .HasColumnType("text")
                .HasColumnName("ket");
            entity.Property(e => e.NamaPenerima)
                .HasMaxLength(100)
                .HasColumnName("nama_penerima");
            entity.Property(e => e.NotelpPenerima)
                .HasMaxLength(100)
                .HasColumnName("notelp_penerima");
            entity.Property(e => e.PiutangId).HasColumnName("piutang_id");
            entity.Property(e => e.RencanaBayar).HasColumnName("rencana_bayar");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.SuratId).HasColumnName("surat_id");
            entity.Property(e => e.Tanggal)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("tanggal");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CollectionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("penagihan_users_FK");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.CollectionDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("penagihan_users_FK_2");

            entity.HasOne(d => d.Piutang).WithMany(p => p.Collections)
                .HasForeignKey(d => d.PiutangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("penagihan_piutang_FK");

            entity.HasOne(d => d.Status).WithMany(p => p.Collections)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("penagihan_status_FK");

            entity.HasOne(d => d.Surat).WithMany(p => p.Collections)
                .HasForeignKey(d => d.SuratId)
                .HasConstraintName("penagihan_surat_FK");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CollectionUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("penagihan_users_FK_1");
        });

        modelBuilder.Entity<CollectionStrategy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("collection_strategies");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Deskripsi)
                .HasColumnType("text")
                .HasColumnName("deskripsi");
            entity.Property(e => e.Kondisi)
                .HasColumnType("text")
                .HasColumnName("kondisi");
            entity.Property(e => e.NamaStrategi)
                .HasMaxLength(100)
                .HasColumnName("nama_strategi");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<Criteria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("criterias");

            entity.HasIndex(e => e.CreatedBy, "kriteria_users_FK");

            entity.HasIndex(e => e.UpdatedBy, "kriteria_users_FK_1");

            entity.HasIndex(e => e.DeletedBy, "kriteria_users_FK_2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bobot)
                .HasPrecision(10, 6)
                .HasColumnName("bobot");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CriteriaCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kriteria_users_FK");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.CriteriaDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("kriteria_users_FK_2");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CriteriaUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("kriteria_users_FK_1");
        });

        modelBuilder.Entity<Debt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("debts");

            entity.HasIndex(e => e.CreatedBy, "piutang_users_FK");

            entity.HasIndex(e => e.UpdatedBy, "piutang_users_FK_1");

            entity.HasIndex(e => e.DeletedBy, "piutang_users_FK_2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Alamat)
                .HasColumnType("text")
                .HasColumnName("alamat");
            entity.Property(e => e.Bulan).HasColumnName("bulan");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Denda).HasColumnName("denda");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IdPelanggan).HasColumnName("id_pelanggan");
            entity.Property(e => e.JatuhTempo).HasColumnName("jatuh_tempo");
            entity.Property(e => e.Jenis)
                .HasMaxLength(100)
                .HasColumnName("jenis");
            entity.Property(e => e.Ket)
                .HasColumnType("text")
                .HasColumnName("ket");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NoPelanggan)
                .HasMaxLength(100)
                .HasColumnName("no_pelanggan");
            entity.Property(e => e.NoTelepon)
                .HasMaxLength(100)
                .HasColumnName("no_telepon");
            entity.Property(e => e.NomialNonair).HasColumnName("nomial_nonair");
            entity.Property(e => e.NominalAir).HasColumnName("nominal_air");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .HasColumnName("status");
            entity.Property(e => e.Tahun).HasColumnName("tahun");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DebtCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("piutang_users_FK");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.DebtDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("piutang_users_FK_2");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.DebtUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("piutang_users_FK_1");
        });

        modelBuilder.Entity<DebtsManagement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("debts_management");

            entity.HasIndex(e => e.PembayaranId, "pengelolaan_piutang_pembayaran_FK");

            entity.HasIndex(e => e.PiutangId, "pengelolaan_piutang_piutang_FK");

            entity.HasIndex(e => e.StatusId, "pengelolaan_piutang_status_FK");

            entity.HasIndex(e => e.CreatedBy, "pengelolaan_piutang_users_FK");

            entity.HasIndex(e => e.UpdatedBy, "pengelolaan_piutang_users_FK_1");

            entity.HasIndex(e => e.DeletedBy, "pengelolaan_piutang_users_FK_2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Nominal).HasColumnName("nominal");
            entity.Property(e => e.PembayaranId).HasColumnName("pembayaran_id");
            entity.Property(e => e.PiutangId).HasColumnName("piutang_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Tanggal)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("tanggal");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DebtsManagementCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("pengelolaan_piutang_users_FK");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.DebtsManagementDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("pengelolaan_piutang_users_FK_2");

            entity.HasOne(d => d.Pembayaran).WithMany(p => p.DebtsManagements)
                .HasForeignKey(d => d.PembayaranId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pengelolaan_piutang_pembayaran_FK");

            entity.HasOne(d => d.Piutang).WithMany(p => p.DebtsManagements)
                .HasForeignKey(d => d.PiutangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pengelolaan_piutang_piutang_FK");

            entity.HasOne(d => d.Status).WithMany(p => p.DebtsManagements)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pengelolaan_piutang_status_FK");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.DebtsManagementUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("pengelolaan_piutang_users_FK_1");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("districts");

            entity.HasIndex(e => e.CreatedBy, "kecamatan_users_FK");

            entity.HasIndex(e => e.UpdatedBy, "kecamatan_users_FK_1");

            entity.HasIndex(e => e.DeletedBy, "kecamatan_users_FK_2");

            entity.HasIndex(e => e.KodeKec, "kode_kec").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.KodeKec).HasColumnName("kode_kec");
            entity.Property(e => e.Nama)
                .HasMaxLength(100)
                .HasColumnName("nama");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DistrictCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("kecamatan_users_FK");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.DistrictDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("kecamatan_users_FK_2");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.DistrictUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("kecamatan_users_FK_1");
        });

        modelBuilder.Entity<FahpCalculation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("fahp_calculations");

            entity.HasIndex(e => e.KriteriaId, "fahp_perhitungan_kriteria_FK");

            entity.HasIndex(e => e.PiutangId, "fahp_perhitungan_piutang_FK");

            entity.HasIndex(e => e.CreatedBy, "fahp_perhitungan_users_FK");

            entity.HasIndex(e => e.UpdatedBy, "fahp_perhitungan_users_FK_1");

            entity.HasIndex(e => e.DeletedBy, "fahp_perhitungan_users_FK_2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.KriteriaId).HasColumnName("kriteria_id");
            entity.Property(e => e.NilaiFuzzy)
                .HasPrecision(10, 6)
                .HasColumnName("nilai_fuzzy");
            entity.Property(e => e.PiutangId).HasColumnName("piutang_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.FahpCalculationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fahp_perhitungan_users_FK");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.FahpCalculationDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("fahp_perhitungan_users_FK_2");

            entity.HasOne(d => d.Kriteria).WithMany(p => p.FahpCalculations)
                .HasForeignKey(d => d.KriteriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fahp_perhitungan_kriteria_FK");

            entity.HasOne(d => d.Piutang).WithMany(p => p.FahpCalculations)
                .HasForeignKey(d => d.PiutangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fahp_perhitungan_piutang_FK");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.FahpCalculationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fahp_perhitungan_users_FK_1");
        });

        modelBuilder.Entity<Leader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("leaders");

            entity.HasIndex(e => e.UserId, "pimpinan_users_FK");

            entity.HasIndex(e => e.CreatedBy, "pimpinan_users_FK_1");

            entity.HasIndex(e => e.UpdatedBy, "pimpinan_users_FK_2");

            entity.HasIndex(e => e.DeletedBy, "pimpinan_users_FK_3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Signature)
                .HasMaxLength(255)
                .HasDefaultValueSql("'nophoto'")
                .HasColumnName("signature");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LeaderCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("pimpinan_users_FK_1");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.LeaderDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("pimpinan_users_FK_3");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LeaderUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("pimpinan_users_FK_2");

            entity.HasOne(d => d.User).WithMany(p => p.LeaderUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pimpinan_users_FK");
        });

        modelBuilder.Entity<Letter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("letters");

            entity.HasIndex(e => e.TindakanId, "surat_jenis_tindakan_FK");

            entity.HasIndex(e => e.KategoriId, "surat_kategori_surat_FK");

            entity.HasIndex(e => e.PimpinanId, "surat_pimpinan_FK");

            entity.HasIndex(e => e.PiutangId, "surat_piutang_FK");

            entity.HasIndex(e => e.CreatedBy, "surat_users_FK");

            entity.HasIndex(e => e.UpdatedBy, "surat_users_FK_1");

            entity.HasIndex(e => e.DeletedBy, "surat_users_FK_2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.KategoriId).HasColumnName("kategori_id");
            entity.Property(e => e.Ket)
                .HasColumnType("text")
                .HasColumnName("ket");
            entity.Property(e => e.Lampiran).HasColumnName("lampiran");
            entity.Property(e => e.NomorSurat)
                .HasMaxLength(100)
                .HasColumnName("nomor_surat");
            entity.Property(e => e.Note)
                .HasMaxLength(100)
                .HasColumnName("note");
            entity.Property(e => e.PimpinanId).HasColumnName("pimpinan_id");
            entity.Property(e => e.PiutangId).HasColumnName("piutang_id");
            entity.Property(e => e.Sifat)
                .HasMaxLength(100)
                .HasColumnName("sifat");
            entity.Property(e => e.TindakanId).HasColumnName("tindakan_id");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LetterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("surat_users_FK");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.LetterDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("surat_users_FK_2");

            entity.HasOne(d => d.Kategori).WithMany(p => p.Letters)
                .HasForeignKey(d => d.KategoriId)
                .HasConstraintName("surat_kategori_surat_FK");

            entity.HasOne(d => d.Pimpinan).WithMany(p => p.Letters)
                .HasForeignKey(d => d.PimpinanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("surat_pimpinan_FK");

            entity.HasOne(d => d.Piutang).WithMany(p => p.Letters)
                .HasForeignKey(d => d.PiutangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("surat_piutang_FK");

            entity.HasOne(d => d.Tindakan).WithMany(p => p.Letters)
                .HasForeignKey(d => d.TindakanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("surat_jenis_tindakan_FK");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LetterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("surat_users_FK_1");
        });

        modelBuilder.Entity<LetterCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("letter_categories");

            entity.HasIndex(e => e.CreatedBy, "kategori_surat_users_FK");

            entity.HasIndex(e => e.UpdatedBy, "kategori_surat_users_FK_1");

            entity.HasIndex(e => e.DeletedBy, "kategori_surat_users_FK_2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LetterCategoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("kategori_surat_users_FK");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.LetterCategoryDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("kategori_surat_users_FK_2");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LetterCategoryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("kategori_surat_users_FK_1");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("payments");

            entity.HasIndex(e => e.PiutangId, "pembayaran_piutang_FK");

            entity.HasIndex(e => e.CreatedBy, "pembayaran_users_FK");

            entity.HasIndex(e => e.UpdatedBy, "pembayaran_users_FK_1");

            entity.HasIndex(e => e.DeletedBy, "pembayaran_users_FK_2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Alamat)
                .HasColumnType("text")
                .HasColumnName("alamat");
            entity.Property(e => e.Bulan).HasColumnName("bulan");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IdPelanggan).HasColumnName("id_pelanggan");
            entity.Property(e => e.Jenis)
                .HasMaxLength(100)
                .HasColumnName("jenis");
            entity.Property(e => e.Ket)
                .HasColumnType("text")
                .HasColumnName("ket");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NoPelanggan)
                .HasMaxLength(100)
                .HasColumnName("no_pelanggan");
            entity.Property(e => e.NoTelepon)
                .HasMaxLength(100)
                .HasColumnName("no_telepon");
            entity.Property(e => e.NominalBayar).HasColumnName("nominal_bayar");
            entity.Property(e => e.PiutangId).HasColumnName("piutang_id");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .HasColumnName("status");
            entity.Property(e => e.Tahun).HasColumnName("tahun");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PaymentCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("pembayaran_users_FK");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.PaymentDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("pembayaran_users_FK_2");

            entity.HasOne(d => d.Piutang).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PiutangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pembayaran_piutang_FK");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PaymentUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("pembayaran_users_FK_1");
        });

        modelBuilder.Entity<PsoResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pso_results");

            entity.HasIndex(e => e.PiutangId, "piutang_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Iterasi).HasColumnName("iterasi");
            entity.Property(e => e.PiutangId).HasColumnName("piutang_id");
            entity.Property(e => e.Prioritas)
                .HasPrecision(10, 6)
                .HasColumnName("prioritas");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Piutang).WithMany(p => p.PsoResults)
                .HasForeignKey(d => d.PiutangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pso_results_ibfk_1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.HasIndex(e => e.CreatedBy, "roles_users_FK");

            entity.HasIndex(e => e.UpdatedBy, "roles_users_FK_1");

            entity.HasIndex(e => e.DeletedBy, "roles_users_FK_2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RoleCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("roles_users_FK");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.RoleDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("roles_users_FK_2");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.RoleUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("roles_users_FK_1");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("status");

            entity.HasIndex(e => e.CreatedBy, "status_users_FK");

            entity.HasIndex(e => e.UpdatedBy, "status_users_FK_1");

            entity.HasIndex(e => e.DeletedBy, "status_users_FK_2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StatusCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("status_users_FK");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.StatusDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("status_users_FK_2");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.StatusUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("status_users_FK_1");
        });

        modelBuilder.Entity<StrategyResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("strategy_results");

            entity.HasIndex(e => e.PiutangId, "hasil_strategi_piutang_FK");

            entity.HasIndex(e => e.CreatedBy, "hasil_strategi_users_FK");

            entity.HasIndex(e => e.UpdatedBy, "hasil_strategi_users_FK_1");

            entity.HasIndex(e => e.DeletedBy, "hasil_strategi_users_FK_2");

            entity.HasIndex(e => e.PsoId, "pso_id");

            entity.HasIndex(e => e.StrategiId, "strategi_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.PiutangId).HasColumnName("piutang_id");
            entity.Property(e => e.PsoId).HasColumnName("pso_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'pending'")
                .HasColumnType("enum('pending','diterapkan')")
                .HasColumnName("status");
            entity.Property(e => e.StrategiId).HasColumnName("strategi_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StrategyResultCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hasil_strategi_users_FK");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.StrategyResultDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("hasil_strategi_users_FK_2");

            entity.HasOne(d => d.Piutang).WithMany(p => p.StrategyResults)
                .HasForeignKey(d => d.PiutangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hasil_strategi_piutang_FK");

            entity.HasOne(d => d.Pso).WithMany(p => p.StrategyResults)
                .HasForeignKey(d => d.PsoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("strategy_results_ibfk_1");

            entity.HasOne(d => d.Strategi).WithMany(p => p.StrategyResults)
                .HasForeignKey(d => d.StrategiId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("strategy_results_ibfk_2");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.StrategyResultUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("hasil_strategi_users_FK_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.RoleId, "users_roles_FK");

            entity.HasIndex(e => e.CreatedBy, "users_users_FK");

            entity.HasIndex(e => e.UpdatedBy, "users_users_FK_1");

            entity.HasIndex(e => e.DeletedBy, "users_users_FK_2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Alamat)
                .HasMaxLength(255)
                .HasColumnName("alamat");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("gender");
            entity.Property(e => e.Jabatan)
                .HasMaxLength(100)
                .HasColumnName("jabatan");
            entity.Property(e => e.More)
                .HasMaxLength(50)
                .HasDefaultValueSql("'-'")
                .HasColumnName("more")
                .UseCollation("utf8mb4_general_ci");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasDefaultValueSql("'New User'")
                .HasColumnName("name")
                .UseCollation("utf8mb4_general_ci");
            entity.Property(e => e.NikNip)
                .HasMaxLength(20)
                .HasColumnName("nik_nip");
            entity.Property(e => e.NomorTelepon)
                .HasMaxLength(20)
                .HasColumnName("nomor_telepon");
            entity.Property(e => e.Password)
                .HasColumnType("text")
                .HasColumnName("password")
                .UseCollation("utf8mb4_general_ci");
            entity.Property(e => e.Photo)
                .HasMaxLength(255)
                .HasColumnName("photo");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("users_users_FK");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.InverseDeletedByNavigation)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("users_users_FK_2");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_roles_FK");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InverseUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("users_users_FK_1");
        });

        modelBuilder.Entity<Village>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("villages");

            entity.HasIndex(e => e.CreatedBy, "kelurahan_users_FK");

            entity.HasIndex(e => e.UpdatedBy, "kelurahan_users_FK_1");

            entity.HasIndex(e => e.DeletedBy, "kelurahan_users_FK_2");

            entity.HasIndex(e => e.KodeDesa, "kode_desa").IsUnique();

            entity.HasIndex(e => e.KodeKec, "kode_kec");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Jarak).HasColumnName("jarak");
            entity.Property(e => e.Keterangan)
                .HasColumnType("text")
                .HasColumnName("keterangan");
            entity.Property(e => e.KodeDesa).HasColumnName("kode_desa");
            entity.Property(e => e.KodeKec).HasColumnName("kode_kec");
            entity.Property(e => e.Layanan).HasColumnName("layanan");
            entity.Property(e => e.Nama)
                .HasMaxLength(100)
                .HasColumnName("nama");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.VillageCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("kelurahan_users_FK");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.VillageDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("kelurahan_users_FK_2");

            entity.HasOne(d => d.KodeKecNavigation).WithMany(p => p.Villages)
                .HasForeignKey(d => d.KodeKec)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kelurahan_kecamatan_FK");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.VillageUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("kelurahan_users_FK_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
