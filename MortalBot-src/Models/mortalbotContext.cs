using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoreBot.Models
{
    public partial class mortalbotContext : DbContext
    {
        public mortalbotContext()
        {
        }

        public mortalbotContext(DbContextOptions<mortalbotContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Acciones> Acciones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:emellado.database.windows.net,1433;Initial Catalog=mortal-bot;Persist Security Info=False;User ID=sqladmin;Password=Dreamcatcher90*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acciones>(entity =>
            {
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifedDate).HasColumnType("datetime");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });
        }
    }
}
