namespace JobLogic.Infrastructure.Calculator
{
    public class LoanRateEntity
    {
        public double Representative { get; set; }
        public double OrderAmount { get; set; }
        public double MonthlyPayments { get; set; }
        public double TotalAmountPayable { get; set; }
        public double InterestCharged { get; set; }
        public int TermsInYear { get; set; }
        public int TermsInMonth { get; set; }
    }
}
