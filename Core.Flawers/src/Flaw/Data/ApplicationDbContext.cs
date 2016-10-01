﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Flaw.Models;
using Flaw.Controllers;

namespace Flaw.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, MyIdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Filename=Flaw_dbo.db");
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MembershipFee>()
                 .HasOne(p => p.Privilege)
                 .WithOne(i => i.Fee)
                 .HasForeignKey<Privilege>(b => b.MembershipFeeForeignKey)
                 .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);

            builder.Entity<PaymentModel>()
                .HasOne(p => p.CashPayment)
                .WithOne(i => i.Payment)
                .HasForeignKey<CashModel>(b => b.PaymentModelId)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);

            builder.Entity<PendingPaymentModel>()
                .HasOne(pt => pt.Fee)
                .WithMany(p => p.Payments)
                .HasForeignKey(pt => pt.MembershipFeeForeignKey)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<CashModel> CashModel { get; set; }

        public DbSet<Privilege> Privileges { get; set; }

        public DbSet<MembershipFee> MembershipFees { get; set; }

        public DbSet<PaymentModel> Payments { get; set; }

        public DbSet<FeeAmountChangeModel> FeeAmountChangeModels { get; set; }

        public DbSet<PendingPaymentModel> PendingPayments { get; set; }

    }
}
