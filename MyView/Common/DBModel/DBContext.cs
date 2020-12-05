using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Common.DBModel
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MDriver> MDriver { get; set; }
        public virtual DbSet<MFilecategory> MFilecategory { get; set; }
        public virtual DbSet<MFiletype> MFiletype { get; set; }
        public virtual DbSet<MFolder> MFolder { get; set; }
        public virtual DbSet<TFileinfo> TFileinfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(AppSettingsHelper.Configuration.GetConnectionString("MyDbConnect"), x => x.ServerVersion("8.0.17-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MDriver>(entity =>
            {
                entity.HasKey(e => new { e.DriverId, e.DriverName })
                    .HasName("PRIMARY");

                entity.ToTable("M_DRIVER");

                entity.Property(e => e.DriverId)
                    .HasColumnName("DRIVER_ID")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.DriverName)
                    .HasColumnName("DRIVER_NAME")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreateDatetime).HasColumnName("CREATE_DATETIME");

                entity.Property(e => e.CreateIp)
                    .HasColumnName("CREATE_IP")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreateName)
                    .IsRequired()
                    .HasColumnName("CREATE_NAME")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FilesysType)
                    .IsRequired()
                    .HasColumnName("FILESYS_TYPE")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SortKey)
                    .HasColumnName("SORT_KEY")
                    .HasColumnType("int(10)");

                entity.Property(e => e.UpdateDatetime).HasColumnName("UPDATE_DATETIME");

                entity.Property(e => e.UpdateIp)
                    .HasColumnName("UPDATE_IP")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateName)
                    .IsRequired()
                    .HasColumnName("UPDATE_NAME")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<MFilecategory>(entity =>
            {
                entity.HasKey(e => new { e.CategoryId, e.CategoryName })
                    .HasName("PRIMARY");

                entity.ToTable("M_FILECATEGORY");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CATEGORY_ID")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CategoryName)
                    .HasColumnName("CATEGORY_NAME")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreateDatetime).HasColumnName("CREATE_DATETIME");

                entity.Property(e => e.CreateIp)
                    .HasColumnName("CREATE_IP")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreateName)
                    .IsRequired()
                    .HasColumnName("CREATE_NAME")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SortKey)
                    .HasColumnName("SORT_KEY")
                    .HasColumnType("int(10)");

                entity.Property(e => e.UpdateDatetime).HasColumnName("UPDATE_DATETIME");

                entity.Property(e => e.UpdateIp)
                    .HasColumnName("UPDATE_IP")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateName)
                    .IsRequired()
                    .HasColumnName("UPDATE_NAME")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<MFiletype>(entity =>
            {
                entity.HasKey(e => new { e.TypeId, e.TypeName })
                    .HasName("PRIMARY");

                entity.ToTable("M_FILETYPE");

                entity.Property(e => e.TypeId)
                    .HasColumnName("TYPE_ID")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.TypeName)
                    .HasColumnName("TYPE_NAME")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreateDatetime).HasColumnName("CREATE_DATETIME");

                entity.Property(e => e.CreateIp)
                    .HasColumnName("CREATE_IP")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreateName)
                    .IsRequired()
                    .HasColumnName("CREATE_NAME")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                //entity.Property(e => e.SortKey)
                //    .HasColumnName("SORT_KEY")
                //    .HasColumnType("int(10)");

                entity.Property(e => e.ThumbPath)
                    .HasColumnName("THUMB_PATH")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateDatetime).HasColumnName("UPDATE_DATETIME");

                entity.Property(e => e.UpdateIp)
                    .HasColumnName("UPDATE_IP")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateName)
                    .IsRequired()
                    .HasColumnName("UPDATE_NAME")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<MFolder>(entity =>
            {
                entity.HasKey(e => new { e.FolderId, e.FolderName, e.FolderLevel, e.ParentId, e.DriverId })
                    .HasName("PRIMARY");

                entity.ToTable("M_FOLDER");

                entity.Property(e => e.FolderId)
                    .HasColumnName("FOLDER_ID")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FolderName)
                    .HasColumnName("FOLDER_NAME")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FolderLevel)
                    .HasColumnName("FOLDER_LEVEL")
                    .HasColumnType("tinyint(3)");

                entity.Property(e => e.ParentId)
                    .HasColumnName("PARENT_ID")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.DriverId)
                    .HasColumnName("DRIVER_ID")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreateDatetime).HasColumnName("CREATE_DATETIME");

                entity.Property(e => e.CreateIp)
                    .HasColumnName("CREATE_IP")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreateName)
                    .IsRequired()
                    .HasColumnName("CREATE_NAME")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SortKey)
                    .HasColumnName("SORT_KEY")
                    .HasColumnType("int(10)");

                entity.Property(e => e.UpdateDatetime).HasColumnName("UPDATE_DATETIME");

                entity.Property(e => e.UpdateIp)
                    .HasColumnName("UPDATE_IP")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateName)
                    .IsRequired()
                    .HasColumnName("UPDATE_NAME")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<TFileinfo>(entity =>
            {
                entity.HasKey(e => new { e.FileId, e.FileName, e.MappingKey })
                    .HasName("PRIMARY");

                entity.ToTable("T_FILEINFO");

                entity.Property(e => e.FileId)
                    .HasColumnName("FILE_ID")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FileName)
                    .HasColumnName("FILE_NAME")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.MappingKey)
                    .HasColumnName("MAPPING_KEY")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreateDatetime).HasColumnName("CREATE_DATETIME");

                entity.Property(e => e.CreateIp)
                    .HasColumnName("CREATE_IP")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreateName)
                    .IsRequired()
                    .HasColumnName("CREATE_NAME")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasColumnName("FILE_PATH")
                    .HasColumnType("varchar(3000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.TypeId)
                    .IsRequired()
                    .HasColumnName("TYPE_ID")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateDatetime).HasColumnName("UPDATE_DATETIME");

                entity.Property(e => e.UpdateIp)
                    .HasColumnName("UPDATE_IP")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdateName)
                    .IsRequired()
                    .HasColumnName("UPDATE_NAME")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
