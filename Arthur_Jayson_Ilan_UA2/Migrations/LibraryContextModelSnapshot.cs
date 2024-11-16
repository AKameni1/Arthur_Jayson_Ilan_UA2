﻿// <auto-generated />
using System;
using Arthur_Jayson_Ilan_UA2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Arthur_Jayson_Ilan_UA2.Migrations
{
    [DbContext(typeof(LibraryContext))]
    partial class LibraryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.AuditLog", b =>
                {
                    b.Property<int>("AuditID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("AuditID"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Entity")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("AuditID");

                    b.HasIndex("UserID");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Book", b =>
                {
                    b.Property<int>("BookID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("BookID"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Availability")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<int?>("PublishedYear")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("BookID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("ISBN")
                        .IsUnique();

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("CategoryID"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("CategoryID");

                    b.HasIndex("CategoryName")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.FAQ", b =>
                {
                    b.Property<int>("FAQID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("FAQID"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("FAQID");

                    b.ToTable("FAQs");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Loan", b =>
                {
                    b.Property<int>("LoanID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("LoanID"));

                    b.Property<int?>("BookID")
                        .HasColumnType("int");

                    b.Property<bool>("DueNotificationSent")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("LoanID");

                    b.HasIndex("BookID");

                    b.HasIndex("UserID");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Notification", b =>
                {
                    b.Property<int>("NotificationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("NotificationID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsRead")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("NotificationID");

                    b.HasIndex("UserID");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Permission", b =>
                {
                    b.Property<int>("PermissionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("PermissionID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("PermissionID");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Report", b =>
                {
                    b.Property<int>("ReportID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ReportID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("GeneratedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("GeneratedBy")
                        .HasColumnType("int");

                    b.Property<string>("ReportPath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ReportType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("ReportID");

                    b.HasIndex("GeneratedBy");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.ReportParameter", b =>
                {
                    b.Property<int>("ParameterID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ParameterID"));

                    b.Property<string>("ParameterName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ParameterValue")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("ReportID")
                        .HasColumnType("int");

                    b.HasKey("ParameterID");

                    b.HasIndex("ReportID");

                    b.ToTable("ReportParameters");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Reservation", b =>
                {
                    b.Property<int>("ReservationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ReservationID"));

                    b.Property<int>("BookID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("ReservationEndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ReservationID");

                    b.HasIndex("BookID");

                    b.HasIndex("UserID");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Role", b =>
                {
                    b.Property<int>("RoleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("RoleID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("RoleID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.RolePermission", b =>
                {
                    b.Property<int>("RoleID")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<int>("PermissionID")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.HasKey("RoleID", "PermissionID");

                    b.HasIndex("PermissionID");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.SupportTicket", b =>
                {
                    b.Property<int>("TicketID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("TicketID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("TicketID");

                    b.HasIndex("UserID");

                    b.ToTable("SupportTickets");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.TicketResponse", b =>
                {
                    b.Property<int>("ResponseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ResponseID"));

                    b.Property<DateTime>("ResponseDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ResponseText")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TicketID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ResponseID");

                    b.HasIndex("TicketID");

                    b.HasIndex("UserID");

                    b.ToTable("TicketResponses");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("UserID"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsSuperAdmin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("RoleID")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("UserID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleID");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.UserPermission", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<int>("PermissionID")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.HasKey("UserID", "PermissionID");

                    b.HasIndex("PermissionID");

                    b.ToTable("UserPermissions");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.AuditLog", b =>
                {
                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.User", "User")
                        .WithMany("AuditLogs")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("User");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Book", b =>
                {
                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.Category", "Category")
                        .WithMany("Books")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Loan", b =>
                {
                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.Book", "Book")
                        .WithMany("Loans")
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.User", "User")
                        .WithMany("Loans")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Notification", b =>
                {
                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Report", b =>
                {
                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.User", "GeneratedByUser")
                        .WithMany("Reports")
                        .HasForeignKey("GeneratedBy")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("GeneratedByUser");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.ReportParameter", b =>
                {
                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.Report", "Report")
                        .WithMany("ReportParameters")
                        .HasForeignKey("ReportID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Report");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Reservation", b =>
                {
                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.Book", "Book")
                        .WithMany("Reservations")
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.User", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.RolePermission", b =>
                {
                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.SupportTicket", b =>
                {
                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.User", "User")
                        .WithMany("SupportTickets")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.TicketResponse", b =>
                {
                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.SupportTicket", "SupportTicket")
                        .WithMany("TicketResponses")
                        .HasForeignKey("TicketID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.User", "User")
                        .WithMany("TicketResponses")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SupportTicket");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.User", b =>
                {
                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.Role", null)
                        .WithMany("Users")
                        .HasForeignKey("RoleID");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.UserPermission", b =>
                {
                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.Permission", "Permission")
                        .WithMany("UserPermissions")
                        .HasForeignKey("PermissionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Arthur_Jayson_Ilan_UA2.Models.User", "User")
                        .WithMany("UserPermissions")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Book", b =>
                {
                    b.Navigation("Loans");

                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Category", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Permission", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("UserPermissions");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Report", b =>
                {
                    b.Navigation("ReportParameters");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.Role", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.SupportTicket", b =>
                {
                    b.Navigation("TicketResponses");
                });

            modelBuilder.Entity("Arthur_Jayson_Ilan_UA2.Models.User", b =>
                {
                    b.Navigation("AuditLogs");

                    b.Navigation("Loans");

                    b.Navigation("Notifications");

                    b.Navigation("Reports");

                    b.Navigation("Reservations");

                    b.Navigation("SupportTickets");

                    b.Navigation("TicketResponses");

                    b.Navigation("UserPermissions");
                });
#pragma warning restore 612, 618
        }
    }
}
