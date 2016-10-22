using System;
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


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<PrivilegeModel>()
                 .HasOne(pt => pt.Fee)
                 .WithMany(p => p.PrivilegeModels)
                 .HasForeignKey(f => f.MembershipFeeFoeignKey)
                 .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);

            builder.Entity<TransferPayment>()
                .HasOne(p => p.Fee)
                .WithMany(i => i.TransferPayments)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);

            builder.Entity<CashModel>()
                .HasOne(pt => pt.Fee)
                .WithMany(p => p.CashPayments)
                .HasForeignKey(pt => pt.MembershipFeeId)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);

            builder.Entity<FeeStateChangeModel>()
                .HasOne(pt => pt.Fee)
                .WithMany(p => p.FeeStateChanges)
                .HasForeignKey(pt => pt.MembershipFeeForeignKey)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<CashModel> CashModel { get; set; }

        public DbSet<TransferPayment> TransferPayments { get; set; }

        public DbSet<PendingPaymentModel> Payments { get; set; }

        public DbSet<Privilege> Privileges { get; set; }

        public DbSet<PrivilegeModel> PrivilegeModels { get; set; }

        public DbSet<MembershipFee> MembershipFees { get; set; }

        public DbSet<FeeAmountChangeModel> FeeAmountChangeModels { get; set; }

        public DbSet<FeeStateChangeModel> FeeStateChanges { get; set; }

    }
}
