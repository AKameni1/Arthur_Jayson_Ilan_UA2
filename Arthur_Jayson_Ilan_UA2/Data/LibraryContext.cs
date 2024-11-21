using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Models;
using Microsoft.EntityFrameworkCore;

namespace Arthur_Jayson_Ilan_UA2.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public DbSet<UserPermission> UserPermissions { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;    
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Loan> Loans { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<SupportTicket> SupportTickets { get; set; } = null!;
        public DbSet<TicketResponse> TicketResponses { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<FAQ> FAQs { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<Report> Reports { get; set; } = null!;
        public DbSet<ReportParameter> ReportParameters { get; set; } = null!;

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des relations many-to-many pour RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleID, rp.PermissionID });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionID)
                .OnDelete(DeleteBehavior.Cascade);


            // Configurer les relations many-to-many pour UserPermission
            modelBuilder.Entity<UserPermission>()
                .HasKey(up => new { up.UserID, up.PermissionID });

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.PermissionID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration des relations un-à-plusieurs et autres relations

            // User -> AuditLog (One-to-Many)
            modelBuilder.Entity<AuditLog>()
                .HasOne(al => al.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(al => al.UserID)
                .OnDelete(DeleteBehavior.SetNull);

            // Book -> Category (Many-to-One)
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);

            // Loan -> User (Many-to-One)
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.User)
                .WithMany(u => u.Loans)
                .HasForeignKey(l => l.UserID)
                .OnDelete(DeleteBehavior.SetNull);

            // Loan -> Book (Many-to-One)
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookID)
                .OnDelete(DeleteBehavior.SetNull);

            // Reservation -> User (Many-to-One)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Reservation -> Book (Many-to-One)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reservations)
                .HasForeignKey(r => r.BookID)
                .OnDelete(DeleteBehavior.Cascade);

            // SupportTicket -> User (Many-to-One)
            modelBuilder.Entity<SupportTicket>()
                .HasOne(st => st.User)
                .WithMany(u => u.SupportTickets)
                .HasForeignKey(st => st.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // TicketResponse -> SupportTicket (Many-to-One)
            modelBuilder.Entity<TicketResponse>()
                .HasOne(tr => tr.SupportTicket)
                .WithMany(st => st.TicketResponses)
                .HasForeignKey(tr => tr.TicketID)
                .OnDelete(DeleteBehavior.Cascade);

            // TicketResponse -> User (Many-to-One)
            modelBuilder.Entity<TicketResponse>()
                .HasOne(tr => tr.User)
                .WithMany(u => u.TicketResponses)
                .HasForeignKey(tr => tr.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Notification -> User (Many-to-One)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Report -> User (Many-to-One)
            modelBuilder.Entity<Report>()
                .HasOne(r => r.GeneratedByUser)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.GeneratedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // ReportParameter -> Report (Many-to-One)
            modelBuilder.Entity<ReportParameter>()
                .HasOne(rp => rp.Report)
                .WithMany(r => r.ReportParameters)
                .HasForeignKey(rp => rp.ReportID)
                .OnDelete(DeleteBehavior.Cascade);


            // SupportTicket: Set default values or configurations if necessary

            // Configuration des contraintes uniques

            // Unique pour ISBN dans Book
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique();

            // Unique pour CategoryName dans Category
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CategoryName)
                .IsUnique();


            // Configuration des relations et des contraintes
            modelBuilder.Entity<User>()
                        .HasOne(u => u.RoleEntity)
                        .WithMany(r => r.Users)
                        .HasForeignKey(u => u.RoleID)
                        .OnDelete(DeleteBehavior.Restrict);


            // Configuration des indices uniques

            // Unique pour Username et Email dans User
            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Username)
                        .IsUnique();

            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();

            modelBuilder.Entity<Role>()
                        .HasIndex(r => r.Name)
                        .IsUnique();

            // Configuration des enums pour être stockés en tant que chaînes de caractères
            //modelBuilder.Entity<User>()
            //    .Property(u => u.Role)
            //    .HasConversion<string>();

            modelBuilder.Entity<Book>()
                .Property(b => b.Availability)
                .HasConversion<string>();

            modelBuilder.Entity<Loan>()
                .Property(l => l.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Reservation>()
                .Property(r => r.Status)
                .HasConversion<string>();

            modelBuilder.Entity<SupportTicket>()
                .Property(st => st.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Report>()
                .Property(r => r.ReportType)
                .HasConversion<string>();
        }
    }
}
