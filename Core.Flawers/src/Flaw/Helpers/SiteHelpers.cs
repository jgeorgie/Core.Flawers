using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flaw.Data;
using Flaw.Models;
using Microsoft.EntityFrameworkCore;

namespace Flaw.Helpers
{
    public static class SiteHelpers
    {
        public static void AddPaymentsForFee(int membershipFeeId, ApplicationDbContext _context)
        {
            var membershipFee = _context.MembershipFees.FirstOrDefault(fee => fee.Id == membershipFeeId);
            if (membershipFee.Periodicity == FeePeriodicity.Year)
            {
                int month = membershipFee.Start.Month;
                for (int i = 1; i <= 12; i++)
                {
                    PendingPaymentModel payment;
                    if (i == 1 && membershipFee.Start.Day > 1)
                    {
                        int days = DateTime.DaysInMonth(membershipFee.Start.Year, membershipFee.Start.Month);
                        double differense = Math.Floor((membershipFee.Start.Day - 1) * membershipFee.MonthlyPay / days);
                        membershipFee.LeftOver -= differense;
                        payment = new PendingPaymentModel()
                        {
                            MembershipFeeForeignKey = membershipFee.Id,
                            Amount = (membershipFee.MonthlyPay - differense),
                            PaymentDeadline = membershipFee.Start.AddMonths(i),
                            Status = PaymentStatus.Pending,
                            DepositOrDebt = -(membershipFee.MonthlyPay - differense)
                        };
                    }
                    else
                    {
                        payment = new PendingPaymentModel()
                        {
                            MembershipFeeForeignKey = membershipFee.Id,
                            Amount = membershipFee.MonthlyPay,
                            PaymentDeadline = membershipFee.Start.AddMonths(i),
                            Status = PaymentStatus.Pending,
                            DepositOrDebt = -membershipFee.MonthlyPay
                        };
                    }

                    _context.Payments.Add(payment);

                }
                _context.Update(membershipFee);
            }
            else
            {
                int month = membershipFee.Start.AddMonths(1).Month;
                var payment = new PendingPaymentModel()
                {
                    MembershipFeeForeignKey = membershipFee.Id,
                    Amount = Math.Floor(membershipFee.RealAmount / 12),
                    PaymentDeadline = new DateTime(DateTime.Now.Year, month, 15),
                    Status = PaymentStatus.Pending
                };

                _context.Payments.Add(payment);
            }
            _context.SaveChanges();
        }


        public static void ReCountPayments(int feeId, CashModel cashModel, TransferPayment transferPayment,
            ApplicationDbContext _context)
        {
            var payments =
                _context.Payments.AsNoTracking()
                    .Where(p => p.MembershipFeeForeignKey == feeId && p.Status != PaymentStatus.Paused)
                    .OrderBy(p => p.PaymentDeadline)
                    .ToList();

            foreach (var payment in payments)
            {
                payment.Status = PaymentStatus.Pending;
                payment.DepositOrDebt = -payment.Amount;
            }

            if (cashModel != null)
            {
                double amount = cashModel.Amount;
                foreach (PendingPaymentModel payment in payments)
                {
                    if (amount >= Math.Abs(payment.DepositOrDebt.Value))
                    {
                        payment.Status = PaymentStatus.Payed;
                        payment.CashPaymentForeignKey = cashModel.Id;
                        payment.PayedOn = cashModel.Date;
                        amount -= Math.Abs(payment.DepositOrDebt.Value);
                        payment.DepositOrDebt = 0;

                        _context.Update(payment);

                        if (amount == 0)
                        {
                            break;
                        }
                    }
                    else
                    {
                        payment.DepositOrDebt += amount;

                        _context.Update(payment);
                        break;
                    }
                }
                _context.SaveChanges();
            }
            else if (transferPayment != null)
            {
                double amount = transferPayment.Amount;

                foreach (PendingPaymentModel payment in payments)
                {
                    if (amount >= Math.Abs(payment.DepositOrDebt.Value))
                    {
                        payment.Status = PaymentStatus.Payed;
                        payment.TransferPaymentForeignKey = transferPayment.Id;
                        payment.PayedOn = transferPayment.Date;
                        amount -= Math.Abs(payment.DepositOrDebt.Value);
                        payment.DepositOrDebt = 0;

                        _context.Update(payment);


                        if (amount == 0)
                        {
                            break;
                        }
                    }
                    else
                    {
                        payment.DepositOrDebt += amount;

                        _context.Update(payment);
                        break;
                    }
                }
                _context.SaveChanges();
            }
        }


    }
}
