public class LoansDebtHistory
{
    private DateTime settlementdate;// ngày kết toán
    public DateTime SettlementDate { get; set; }

    private decimal numberofpayment;
    public decimal NumberOfPayment { get; set; }  // hạn mức cần thanh toán
    private DateTime nointerestpaymentdate;
    /*
     hạn thanh toán cuối cùng cho bạn "trả nợ" để không bị tính 
     lãi suất số tiền đã chi tiêu và không phải chịu lãi trả chậm theo quy định của ngân hàng.
     */
    public DateTime NoInterestPaymentDate { get; set; }
    private bool ispayment;
    public bool IsPayment { get; set; }

    private bool ispayontime;
    public bool Ispayontime { get; set; }
    public LoansDebtHistory() { }
    public LoansDebtHistory(DateTime settlementdate, decimal numberofpayment)
    {
        SettlementDate = settlementdate;
        NumberOfPayment = numberofpayment;
        NoInterestPaymentDate = settlementdate.AddDays(10);
        IsPayment = false;
        Ispayontime = false;
    }
} 
    