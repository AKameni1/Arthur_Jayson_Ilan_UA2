using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Arthur_Jayson_Ilan_UA2.Model;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditlog> Auditlogs { get; set; } = null!;

    public virtual DbSet<Book> Books { get; set; } = null!;

    public virtual DbSet<Category> Categories { get; set; } = null!;

    public virtual DbSet<Faq> Faqs { get; set; } = null!;

    public virtual DbSet<Loan> Loans { get; set; } = null!;
    
    public virtual DbSet<Notification> Notifications { get; set; } = null!;

    public virtual DbSet<Permission> Permissions { get; set; } = null!;

    public virtual DbSet<Report> Reports { get; set; } = null!;

    public virtual DbSet<Reportparameter> Reportparameters { get; set; } = null!;

    public virtual DbSet<Reservation> Reservations { get; set; } = null!;

    public virtual DbSet<Role> Roles { get; set; } = null!;

    public virtual DbSet<Supportticket> Supporttickets { get; set; } = null!;

    public virtual DbSet<Ticketresponse> Ticketresponses { get; set; } = null!;

    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=library.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditlog>(entity =>
        {
            entity.HasKey(e => e.AuditId);

            entity.ToTable("auditlog");

            entity.Property(e => e.AuditId).HasColumnName("AuditID");
            entity.Property(e => e.CreatedAt).HasColumnType("DATETIME");
            entity.Property(e => e.UpdateAt).HasColumnType("DATETIME");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Auditlogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("book");

            entity.HasIndex(e => e.Isbn, "IX_book_ISBN").IsUnique();

            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.Availability).HasDefaultValue("Available");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Isbn).HasColumnName("ISBN");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("category");

            entity.HasIndex(e => e.CategoryName, "IX_category_CategoryName").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
        });

        modelBuilder.Entity<Faq>(entity =>
        {
            entity.ToTable("faq");

            entity.Property(e => e.Faqid).HasColumnName("FAQID");
            entity.Property(e => e.CreatedAt).HasColumnType("DATETIME");
            entity.Property(e => e.UpdatedAt).HasColumnType("DATETIME");
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.ToTable("loan");

            entity.Property(e => e.LoanId).HasColumnName("LoanID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.DueNotificationSent).HasDefaultValue(0);
            entity.Property(e => e.EndDate).HasColumnType("DATETIME");
            entity.Property(e => e.ReturnDate).HasColumnType("DATETIME");
            entity.Property(e => e.StartDate).HasColumnType("DATETIME");
            entity.Property(e => e.Status).HasDefaultValue("Borrowed");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Book).WithMany(p => p.Loans)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.User).WithMany(p => p.Loans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("notification");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("DATE");
            entity.Property(e => e.IsRead).HasDefaultValue(0);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("permission");

            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.ToTable("report");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.GeneratedAt).HasColumnType("DATETIME");

            entity.HasOne(d => d.GeneratedByNavigation).WithMany(p => p.Reports)
                .HasForeignKey(d => d.GeneratedBy)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Reportparameter>(entity =>
        {
            entity.HasKey(e => e.ParameterId);

            entity.ToTable("reportparameter");

            entity.Property(e => e.ParameterId).HasColumnName("ParameterID");
            entity.Property(e => e.ReportId).HasColumnName("ReportID");

            entity.HasOne(d => d.Report).WithMany(p => p.Reportparameters)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.ToTable("reservation");

            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.ReservationDate).HasColumnType("DATETIME");
            entity.Property(e => e.ReservationEndDate).HasColumnType("DATETIME");
            entity.Property(e => e.Status).HasDefaultValue("Reserved");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Book).WithMany(p => p.Reservations).HasForeignKey(d => d.BookId);

            entity.HasOne(d => d.User).WithMany(p => p.Reservations).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasMany(d => d.Permissions).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "Rolepermission",
                    r => r.HasOne<Permission>().WithMany().HasForeignKey("PermissionId"),
                    l => l.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                    j =>
                    {
                        j.HasKey("RoleId", "PermissionId");
                        j.ToTable("rolepermission");
                        j.IndexerProperty<int>("RoleId").HasColumnName("RoleID");
                        j.IndexerProperty<int>("PermissionId").HasColumnName("PermissionID");
                    });
        });

        modelBuilder.Entity<Supportticket>(entity =>
        {
            entity.HasKey(e => e.TicketId);

            entity.ToTable("supportticket");

            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.CreatedAt).HasColumnType("DATETIME");
            entity.Property(e => e.Status).HasDefaultValue("Open");
            entity.Property(e => e.UpdatedAt).HasColumnType("DATETIME");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Supporttickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Ticketresponse>(entity =>
        {
            entity.HasKey(e => e.ResponseId);

            entity.ToTable("ticketresponse");

            entity.Property(e => e.ResponseId).HasColumnName("ResponseID");
            entity.Property(e => e.ResponseDate).HasColumnType("DATETIME");
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Ticketresponses)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.User).WithMany(p => p.Ticketresponses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "IX_user_Email").IsUnique();

            entity.HasIndex(e => e.Username, "IX_user_Username").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreationDate).HasColumnType("DATETIME");
            entity.Property(e => e.IsActive).HasDefaultValue(1);
            entity.Property(e => e.LateReturnCount).HasDefaultValue(0);
            entity.Property(e => e.LoanCount).HasDefaultValue(0);
            entity.Property(e => e.LoanLimit).HasDefaultValue(3);
            entity.Property(e => e.LoanSuspendedUntil).HasColumnType("DATETIME");
            entity.Property(e => e.PenaltyPoints).HasDefaultValue(0);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(d => d.Permissions).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Userpermission",
                    r => r.HasOne<Permission>().WithMany().HasForeignKey("PermissionId"),
                    l => l.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "PermissionId");
                        j.ToTable("userpermission");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserID");
                        j.IndexerProperty<int>("PermissionId").HasColumnName("PermissionID");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
