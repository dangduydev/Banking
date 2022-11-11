using System;
using System.Security.Cryptography.X509Certificates;

class InternationalCreditCard : PaymentCard
{
    private decimal cardbalance;
    public decimal CardBalance
    {
        get; set;
    }
    private decimal Creditlimit;
    #region CreditLimit
    //Hạn mức giao dịch tín dụng
    #endregion
    public decimal CreditLimit
    {
        get; set;
    }
    private decimal poslimit;
    #region CreditLimit
    //Hạn mức giao dịch tại pos atm
    #endregion
    public decimal POSLimit
    {
        get; set;
    }
    private decimal atmlimit;
    #region ATMLimit
    //Hạn mức giao dịch tại pos atm
    #endregion
    public decimal ATMLimit
    {
        get; set;
    }
    private decimal Ipolimit;
    #region Initial Public Offering
    //Phát hành công khai lần đầu, còn gọi là IPO
    #endregion
    public decimal IPOLimit
    {
        get; set;
    }
    private decimal Poglimit;
    #region Payment of good
    //Thanh toán hàng hóa/dịch vụ
    #endregion
    public decimal POGLimit
    {
        get; set;
    }

    private decimal internetlimit;
    #region Deal Internet/Moto
    //Giao dịch Internet/MOTO
    #endregion
    public decimal InternetLimit
    {
        get; set;
    }

    private double creditinterestrate = 1.08;// lãi suất
    public double CreditInterestRate
    {
        get; set;
    }

    private decimal moneywithdrawn;
    public decimal MoneyWithDrawn
    {
        get; set;
    }
    private DateTime lasttrading;
    public DateTime LastTrading
    {
        get; set;
    }// ngày giao dịch cuối
    private bool baddebit = false;
    public bool BadDebit
    {
        get; set;
    }
    private decimal minimumpayment;
    public decimal Minimumpayment
    {
        get; set;
    }
    private DateTime nointerestpaymentdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15);
    /*
     ngay thanh toan du no ():hạn thanh toán cuối cùng cho bạn "trả nợ" để không bị tính 
     lãi suất số tiền đã chi tiêu và không phải chịu phí phạt trả chậm theo quy định của ngân hàng.
     */
    public DateTime NoInterestPaymentDate
    {
        get; set;
    }
    public List<DebtHistory> DebtHistoryList = new List<DebtHistory>();// lịch sử nợ
    public InternationalCreditCard(string type, string accountnumber, string id, string pin, List<TransactionHistory> Histories, List<DebtHistory> ddebtHistoryList, Client A, string description) : base(type, accountnumber, id, pin, Histories, A, description)
    {
        DebtHistoryList = ddebtHistoryList;
    }
    // update các thuộc tính qua mỗi tháng
    public void UpdateEveryMonth()
    {
        if (DateTime.Now.Day == 1)
        {
            this.MoneyWithDrawn = 0;
            this.NoInterestPaymentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15);
            for (int i = 0; i < this.DebtHistoryList.Count; i++)
            {
                DebtHistory tmp = this.DebtHistoryList[i];
                if (tmp.IsPayment == false && tmp.NoInterestPaymentDate < DateTime.Now)
                {
                    this.BadDebit = true;
                }

            }
        }

    }
    // rút tiền tại atm
    public bool WithdrawATM(decimal x, string pinn)
    {
        if (pinn != this.Pin) return false;


        if (x + this.MoneyWithDrawn <= this.CreditLimit &&
            StatusCard == true && x <= this.ATMLimit)
        {
            CardBalance -= x + 1100;

            if (LastTrading.Month == DateTime.Now.Month)
            {
                this.MoneyWithDrawn += x;
            }
            if (LastTrading.Month < DateTime.Now.Month)
            {
                this.MoneyWithDrawn = x;

            }
            LastTrading = DateTime.Now;
            TransactionHistory transactionHistory = new TransactionHistory(DateTime.Now, this.AccountNumber, true, this.Type
                , this.StatusCard, "Rút tại atm", x, this.CardBalance, this.ID);
            Histories.Add(transactionHistory);
            return true;
        }

        return false;
    }
    // rút tiền tại quầy pos
    public bool WithdrawPOS(decimal x, string pinn)
    {
        if (pinn != this.Pin) return false;
        if (x + this.MoneyWithDrawn <= this.CreditLimit && StatusCard == true &&
            x <= this.POSLimit)
        {
            CardBalance -= x;
            if (LastTrading.Month == DateTime.Now.Month)
            {
                this.MoneyWithDrawn += x;
            }
            if (LastTrading.Month < DateTime.Now.Month)
            {
                this.MoneyWithDrawn = x;

            }
            LastTrading = DateTime.Now;
            TransactionHistory transactionHistory = new TransactionHistory(DateTime.Now, this.AccountNumber, true, this.Type
                , this.StatusCard, "Rút tại pos", x, this.CardBalance, this.ID);
            Histories.Add(transactionHistory);
            return true;
        }

        return false;
    }

    // đổi tiền ngoại tệ
    public bool ForeignCurrencyTrading(decimal x, string typecurrrency)
    {
        string[] ListTypeCurrency =
            {
                "VND","AUD","CAD","CHF","EUR","GBP","HKD","JPY","SGD","USD"
            };
        if (typecurrrency == ListTypeCurrency[0]) x *= 1;
        else
            if (typecurrrency == ListTypeCurrency[1]) x *= 15478;
        else
            if (typecurrrency == ListTypeCurrency[2]) x *= 17910;
        else
            if (typecurrrency == ListTypeCurrency[3]) x *= 24385;
        else
            if (typecurrrency == ListTypeCurrency[4]) x *= 24072;
        else
            if (typecurrrency == ListTypeCurrency[5]) x *= 27985;
        else
            if (typecurrrency == ListTypeCurrency[6]) x *= 3096;
        else
            if (typecurrrency == ListTypeCurrency[7]) x *= 16566;
        else
            if (typecurrrency == ListTypeCurrency[8]) x *= 17254;
        else
            if (typecurrrency == ListTypeCurrency[9]) x *= 24610;
        if (x + this.MoneyWithDrawn <= this.CreditLimit && StatusCard == true &&
            x <= this.POSLimit)
        {
            CardBalance -= x;
            if (LastTrading.Month == DateTime.Now.Month)
            {
                this.MoneyWithDrawn += x;
            }
            if (LastTrading.Month < DateTime.Now.Month)
            {
                this.MoneyWithDrawn = x;

            }
            LastTrading = DateTime.Now;
            TransactionHistory transactionHistory = new TransactionHistory(DateTime.Now, this.AccountNumber, true, this.Type
                , this.StatusCard, "Chuyển đổi và rút ngoại tệ tại POS", x, this.CardBalance, this.ID);
            Histories.Add(transactionHistory);
            return true;
        }
        return false;
    }
    // tự động trừ tiền thường niên
    public void AutomaticDeductionOfAnnualFee()
    {
        if (DateTime.Now == this.AnnualFeesYear.AddYears(1))
        {
            CardBalance += this.AnnualFees;
            this.AnnualFeesYear = new DateTime(this.StartTime.Year + 1, this.StartTime.Month, this.StartTime.Day);
        }
    }

    public decimal FindCreditCardBalance()//Tính số dư nợ (FindCreditCardBalance)
    {
        return CardBalance;
    }
    public void SoDuKhaDung()
    {
        Console.WriteLine(CardBalance);
    }
    //Xuất thông tin số dư nợ thẻ tín dụng cần thanh toán mỗi tháng (PrintMontlyStatement)
    public void PrintMontlyStatement()
    {
        Console.WriteLine(this.CalMiniumPayment() / 5 * 100);
    }
    public decimal CalMiniumPayment()//Tính khoản thanh toán tối thiểu (MiniumPayment)
    {
        decimal kq = 0;
        DateTime C = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);// ngày đầu của tháng hiện tại
        DateTime D = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        D.AddMonths(1);
        D.AddDays(-1);// ngày cuối của tháng hiện tại

        // chỉ tính được vào ngày cuối cùng của tháng hiện tại
        if (DateTime.Now.Day <= D.Day) return 0;

        var list = from tmp in Histories where tmp.TransactionType == false && tmp.DayTrading <= C && tmp.DayTrading >= D select tmp;
        foreach (var tmp in list)
        {
            kq += tmp.TransactionMoney;
        }
        DebtHistory tmpp = new DebtHistory(D, kq, this.ID, this.AccountNumber, this.Type);
        this.DebtHistoryList.Add(tmpp);
        kq = kq / 100 * 5;
        return kq;
    }

    public decimal CalPaymentMoney()// tính tiền đã thanh toán trong tháng
    {
        decimal kq = 0;
        DateTime C = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);// ngày đầu của tháng hiện tại
        DateTime D = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        D.AddMonths(1);
        D.AddDays(-1);// ngày cuối của tháng hiện tại

        // chỉ tính được vào ngày cuối cùng của tháng hiện tại
        int i;
        var list = from tmp in Histories where tmp.DayTrading >= C && tmp.DayTrading <= D && tmp.TransactionType == true select tmp;
        foreach (var tmp in list)
        {
            kq += tmp.TransactionMoney;
        }
        return kq;
    }
    // kiểm tra nợ xấu
    public void CheckBadDebit()
    {
        decimal K = this.DebtHistoryList[this.DebtHistoryList.Count].Miniumpayment;
        if (this.CalPaymentMoney() >= K && this.BadDebit == false)
        {
            this.BadDebit = false;
        }
        else
        {
            this.BadDebit = true;
        }

    }
    // thanh toán hạn mức tính dụng
    public bool Payment(decimal x)
    {
        if (StatusCard == false) return false;
        this.CardBalance += x;
        TransactionHistory transactionHistory = new TransactionHistory(DateTime.Now, AccountNumber, false, Type, StatusCard
            , "Thanh toán nơ tín dụng", x, CardBalance, this.ID);
        Histories.Add(transactionHistory);

        for (int i = 0; i < this.DebtHistoryList.Count; i++)
        {
            DebtHistory tmp = DebtHistoryList[i];
            if (tmp.Maximumpayment > 0 && tmp.IsPayment == false)
            {
                if (x > tmp.Maximumpayment)
                {
                    x -= tmp.Maximumpayment;
                    tmp.IsPayment = true;
                    if (tmp.NoInterestPaymentDate < DateTime.Now) tmp.Ispayontime = true;
                    DebtHistoryList[i] = tmp;
                }
                else
                if (x == tmp.Maximumpayment)
                {
                    x = 0;
                    tmp.IsPayment = true;
                    if (tmp.NoInterestPaymentDate < DateTime.Now) tmp.Ispayontime = true;
                    DebtHistoryList[i] = tmp;
                    break;
                }
                else
                if (x < tmp.Maximumpayment)
                {
                    x = 0;
                    DebtHistoryList[i] = tmp;
                    break;
                }

            }

        }

        return true;

    }
    public decimal CalInterestInOneMonth(int month, int year)
    // tính lãi suất trong 1 tháng hiện tại month = datetime.now.month \\ year = datetime.now.year
    {
        DateTime C = new DateTime(year, month, 1);// ngày đầu của tháng 
        DateTime D = new DateTime(year, month, 1);
        D.AddMonths(1);
        D.AddDays(-1);// ngày cuối của tháng 
        decimal kq = 0;
        for (int i = 0; i < this.DebtHistoryList.Count; i++)
        {
            DebtHistory tmp = this.DebtHistoryList[i];
            if (tmp.NoInterestPaymentDate > D) break;
            if (tmp.NoInterestPaymentDate <= D && tmp.NoInterestPaymentDate >= C && tmp.IsPayment == false)
            {
                kq += tmp.Maximumpayment / 100 * 5;
            }
        }
        return kq;
    }
    public void CalInterest()// tính lãi suất trong cả thẻ từ lúc bắt đầu để update
    {
        decimal kq = 0;
        DateTime C = new DateTime(this.StartTime.Year, this.StartTime.Month, 1);// ngày đầu của tháng 
        DateTime D = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        D.AddMonths(-1);
        for (int i = 0; i < this.DebtHistoryList.Count; i++)
        {
            DebtHistory tmp = this.DebtHistoryList[i];
            if (tmp.NoInterestPaymentDate > D) break;
            if (tmp.NoInterestPaymentDate <= D && tmp.NoInterestPaymentDate >= C && tmp.IsPayment == false)
            {
                kq += tmp.Maximumpayment / 100 * 5;
            }

        }
        this.CardBalance += kq + this.CalInterestInOneMonth(DateTime.Now.Month, DateTime.Now.Year);
        DebtHistory k = new(DateTime.Now, kq + this.CalInterestInOneMonth(DateTime.Now.Month, DateTime.Now.Year), this.ID, this.AccountNumber, this.Type);
        this.DebtHistoryList.Add(k);

    }


    public bool ChangeLimitCredit(int x, int fico, decimal income)
    {
        CheckBadDebit();
        if ((this.BadDebit == true && fico <= -100) || (x > income * 5))
            return false;
        TimeSpan days = DateTime.Now - this.StartTime;
        if (days.Days * 2 < 365) return false;
        this.CreditLimit = x;
        return true;
    }
    public void CheckDebitToLockCard()
    {
        var list = from tmp in this.DebtHistoryList where tmp.IsPayment == false && tmp.SettlementDate.AddDays(90) <= DateTime.Now select tmp;
        if (list.Count() > 0) this.BadDebit = true;
    }
    public void XuatThongTinThe()
    {
        Console.WriteLine("Type: " + this.Type);
        Console.WriteLine("Account Number: " + this.AccountNumber);
        Console.WriteLine("Card Balance: " + this.CardBalance);
    }

    ~InternationalCreditCard()
    { }
}
class VisaStandard : InternationalCreditCard
{
    public VisaStandard(string type, string accountnumber, string id, string pin, List<TransactionHistory> Histories, List<DebtHistory> ddebtHistoryList, Client A, string description) : base(type, accountnumber, id, pin, Histories, ddebtHistoryList, A, description)
    {
        this.AnnualFees = 100000;

        this.CreditLimit = 30000000;
        this.POSLimit = this.CreditLimit / 2;
        this.POGLimit = 30000000;
        this.ATMLimit = 15000000;
        this.InternetLimit = 5000000;
        this.IPOLimit = 100000;

        Type = "VisaStandard";

        this.CreditInterestRate = 0;
        this.Duedate = this.StartTime;
        this.Minimumpayment = 0;

    }
    ~VisaStandard()
    { }

}
class VisaGold : InternationalCreditCard
{
    //Miễn phí bảo hiểm tai nạn chủ thẻ trên phạm vi toàn cầu với số tiền bảo hiểm lên tới 15 triệu đồng
    public VisaGold(string type, string accountnumber, string id, string pin, List<TransactionHistory> Histories, List<DebtHistory> ddebtHistoryList, Client A, string description) : base(type, accountnumber, id, pin, Histories, ddebtHistoryList, A, description)
    {
        this.AnnualFees = 0;

        this.CreditLimit = 100000000;
        this.POSLimit = this.CreditLimit / 2;
        this.POGLimit = 100000000;
        this.ATMLimit = 50000000;
        this.InternetLimit = 5000000;
        this.IPOLimit = 200000;

        Type = "VisaGold";

        this.CreditInterestRate = 0;
        this.Duedate = this.StartTime;
        this.Minimumpayment = 0;

    }
    ~VisaGold()
    { }
}

class MastercardGold : InternationalCreditCard
{
    //Miễn phí bảo hiểm tai nạn chủ thẻ trên phạm vi toàn cầu với số tiền bảo hiểm lên tới 15 triệu đồng
    public MastercardGold(string type, string accountnumber, string id, string pin, List<TransactionHistory> Histories, List<DebtHistory> ddebtHistoryList, Client A, string description) : base(type, accountnumber, id, pin, Histories, ddebtHistoryList, A, description)
    {
        this.AnnualFees = 0;

        this.CreditLimit = 100000000;
        this.POSLimit = this.CreditLimit / 2;
        this.POGLimit = 100000000;
        this.ATMLimit = 50000000;
        this.InternetLimit = 5000000;
        this.IPOLimit = 200000;

        Type = "MastercardGold";

        this.CreditInterestRate = 0;
        this.Duedate = this.StartTime;
        this.Minimumpayment = 0;

    }
    ~MastercardGold()
    { }
}
class MastercardPlatinum : InternationalCreditCard
{
    //Miễn phí bảo hiểm tai nạn chủ thẻ trên phạm vi toàn cầu với số tiền bảo hiểm lên tới 100 triệu đồng.
    public MastercardPlatinum(string type, string accountnumber, string id, string pin, List<TransactionHistory> Histories, List<DebtHistory> ddebtHistoryList, Client A, string description) : base(type, accountnumber, id, pin, Histories, ddebtHistoryList, A, description)
    {
        this.AnnualFees = 100000;

        this.CreditLimit = 200000000;
        this.POSLimit = this.CreditLimit / 2;
        this.POGLimit = 200000000;
        this.ATMLimit = 100000000;
        this.InternetLimit = 5000000;
        this.IPOLimit = 300000;

        Type = "MastercardPlatinum";

        this.CreditInterestRate = 0;
        this.Duedate = this.StartTime;
        this.Minimumpayment = 0;

    }
    ~MastercardPlatinum()
    { }
}
class JCBGold : InternationalCreditCard
{

    public JCBGold(string type, string accountnumber, string id, string pin, List<TransactionHistory> Histories, List<DebtHistory> ddebtHistoryList, Client A, string description) : base(type, accountnumber, id, pin, Histories, ddebtHistoryList, A, description)
    {
        this.AnnualFees = 0;

        this.CreditLimit = 100000000;
        this.POSLimit = this.CreditLimit / 2;
        this.POGLimit = 100000000;
        this.ATMLimit = 50000000;
        this.InternetLimit = 5000000;
        this.IPOLimit = 300000;

        Type = "JCBUltimate";

        this.CreditInterestRate = 0;
        this.Duedate = this.StartTime;
        this.Minimumpayment = 0;

    }
    ~JCBGold()
    { }
}
class JCBUltimate : InternationalCreditCard
{

    public JCBUltimate(string type, string accountnumber, string id, string pin, List<TransactionHistory> Histories, List<DebtHistory> ddebtHistoryList, Client A, string description) : base(type, accountnumber, id, pin, Histories, ddebtHistoryList, A, description)
    {
        this.AnnualFees = 0;

        this.CreditLimit = 200000000;
        this.POSLimit = this.CreditLimit / 2;
        this.POGLimit = 200000000;
        this.ATMLimit = 50000000;
        this.InternetLimit = 100000000;
        this.IPOLimit = 0;

        Type = "JCBUltimate";

        this.CreditInterestRate = 0;
        this.Duedate = this.StartTime;
        this.Minimumpayment = 0;

    }
    ~JCBUltimate()
    { }

}