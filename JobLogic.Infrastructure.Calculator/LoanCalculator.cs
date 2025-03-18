using System;

namespace JobLogic.Infrastructure.Calculator
{
    // Indicates when payments are due when calling financial methods.
    public enum DueDate
    {
        // Falls at the end of the date interval
        EndOfPeriod = 0,
        // Falls at the beginning of the date interval
        BegOfPeriod = 1
    }

    public interface ILoanCalculator
    {
        LoanRateEntity CalculateRate(double orderAmount, double apr, int numberOfYears, double deposit = 0);
    }

    public class LoanCalculator : ILoanCalculator
    {
        static bool IsNegative(double number) => number < 0;
        static bool IsZero(double number) => number == 0;
        readonly string depositeExceptionMessage = "{0} cannot be greater than {1}: ";

        public LoanRateEntity CalculateRate(double orderAmount, double apr, int numberOfYears, double deposit = 0)
        {
            if (IsNegative(orderAmount)) throw new ArgumentOutOfRangeException("orderAmount");
            if (IsNegative(apr) || IsZero(apr) || apr > 100) throw new ArgumentOutOfRangeException("apr");
            if (IsNegative(numberOfYears) || IsZero(numberOfYears)) throw new ArgumentOutOfRangeException("numberOfYears");
            if (IsNegative(deposit)) throw new ArgumentOutOfRangeException("deposit");
            if (deposit > orderAmount) throw new ArgumentOutOfRangeException("deposit", string.Format(depositeExceptionMessage, nameof(deposit), nameof(orderAmount)));

            var numberOfMonth = numberOfYears * 12;

            double monthlyPaymentRoundedDownTo2Digits = 0, interestCharged = 0, totalAmountPayable = 0;

            if (!IsZero(orderAmount))
            {
                var creditAmount = orderAmount - deposit;

                var xMonthlyFormula = 1 + (apr / 100);
                var yMonthlyFormula = 1 / (double)12;

                var monthlyRate = Math.Pow(xMonthlyFormula, yMonthlyFormula) - 1;
                var monthlyPayment = -Pmt(monthlyRate, numberOfMonth, creditAmount, 0, DueDate.EndOfPeriod);
                var multiplier = monthlyPayment / creditAmount;

                monthlyPaymentRoundedDownTo2Digits = Math.Truncate((multiplier * creditAmount) * 100) / 100;
                totalAmountPayable = Math.Round(monthlyPaymentRoundedDownTo2Digits * numberOfMonth, 2);
                interestCharged = Math.Round(totalAmountPayable - creditAmount, 2);
            }

            var response = new LoanRateEntity
            {
                OrderAmount = orderAmount,
                TermsInYear = numberOfYears,
                TermsInMonth = numberOfMonth,
                MonthlyPayments = monthlyPaymentRoundedDownTo2Digits,
                InterestCharged = interestCharged,
                TotalAmountPayable = totalAmountPayable,
                Representative = apr,
            };
            return response;
        }

        private static double Pmt(double Rate, double NPer, double PV, double FV = 0, DueDate Due = 0)
        {
            double fV;
            double num;
            if (Rate != 0)
            {
                num = (Due == DueDate.EndOfPeriod ? 1 : 1 + Rate);
                var num1 = Math.Pow(Rate + 1, NPer);
                fV = (-FV - PV * num1) / (num * (num1 - 1)) * Rate;
            }
            else
            {
                fV = (-FV - PV) / NPer;
            }
            return fV;
        }
    }
}
