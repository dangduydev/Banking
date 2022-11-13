public class DebtHistory
{
    private string id;
    public string ID { get; set; }
    private bool statuscard;
    public bool StatusCard { get; set; }
    private string accountnumber;
    public string AccountNumber { get; set; }
    private string type;
    public string Type { get; set; }
    // nợ tín dụng
    private DateTime settlementdate;// ngày kết toán
    public DateTime SettlementDate { get; set; }
    private decimal miniumpayment;
    public decimal Miniumpayment { get; set; }  // hạn mức tối thiểu cần thanh toán
    private decimal maximumpayment;
    public decimal Maximumpayment { get; set; }  // hạn mức cần thanh toán
    private DateTime nointerestpaymentdate;
    /*
     hạn thanh toán cuối cùng cho bạn "trả nợ" để không bị tính 
     lãi suất số tiền đã chi tiêu và không phải chịu lãi trả chậm theo quy định của ngân hàng.
     */
    public DateTime NoInterestPaymentDate
    {
        get; set;
    }
    private bool ispayment;
    public bool IsPayment { get; set; }

    private bool ispayontime;
    public bool Ispayontime { get; set; }
    public DebtHistory() { }
    public DebtHistory(DateTime settlementdate, decimal maximumpayment,string id,string accountnumber,string type)
    {
        SettlementDate = settlementdate;
        Maximumpayment = maximumpayment;
        Miniumpayment = Maximumpayment/20;
        NoInterestPaymentDate = settlementdate.AddDays(15);
        IsPayment = false;
        Ispayontime = false;
        ID = id;
        AccountNumber = accountnumber;
        Type = type;
    }
    public void XuatLichSuNo()
    {
            Console.WriteLine($"Account Number: {AccountNumber}");
            Console.WriteLine($"Day trading: {SettlementDate.ToString("dd/MM/yyyy")}");
            Console.WriteLine("Time trading: {0}:{1}:{2}", SettlementDate.Hour, SettlementDate.Minute, SettlementDate.Second);
            Console.WriteLine($"Amount owed Money: +{Maximumpayment}");
    }
}