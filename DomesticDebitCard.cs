
class DomesticDebitCard : PaymentCard
{
    protected decimal hanMucRutTienTrenNgay;
    protected decimal hanMucChuyenKhoanTrenNgayCungNH;
    protected decimal hanMucChuyenKhoanTrenNgayKhacNH;
    protected decimal hanMucRutTienTrenLanKhacNH;
    protected decimal hanMucChuyenKhoanTrenLan;
    protected decimal hanMucRutTienTrenLanCungNH;
    private decimal internetlimit;
    public decimal InternetLimit { get; set; }
    public decimal CardBalance;
    protected static decimal SoTienDaRut;
    protected static decimal SoTienDaChuyen;
    private DateTime Last;

    public DomesticDebitCard(string type, string accountnumber, string id, string pin, List<TransactionHistory> Histories, Client A, string description) : base(type, accountnumber, id, pin, Histories, A, description)
    {
        CardBalance = 50000;
    }
    public bool AccountRecharge(decimal x, string transactioncontent)// nạp tiền vô thẻ 
    {
        if (!StatusCard)
        {
            return false;
        }
        CardBalance += x;
        TransactionHistory transactionHistory = new TransactionHistory(DateTime.Now, this.AccountNumber, false, Type, StatusCard, "Nap tien", x, CardBalance, this.ID);
        this.Histories.Add(transactionHistory);
        return true;
    }
    public void ResetLimit() //Kiểm tra lần giao dịch cuối cùng đã qua ngày mới chưa
    {
        if (Last.Day < DateTime.Now.Day)
        {
            SoTienDaChuyen = 0;
            SoTienDaRut = 0;
        }
    }
    public bool WithdrawATM(decimal x, string pin)
    {
        ResetLimit();
        if (pin != this.Pin)
        {
            return false;
        }
        if (CheckBalanceMinimun(x) && x < hanMucRutTienTrenLanCungNH && x + SoTienDaRut < hanMucRutTienTrenNgay
        && StatusCard == true)
        {
            CardBalance -= x;
            SoTienDaRut += x;
            Last = DateTime.Now;
            TransactionHistory transactionHistory = new TransactionHistory(DateTime.Now, this.AccountNumber, true, Type, StatusCard, "Rut tien tai ATM", x, CardBalance, this.ID);
            this.Histories.Add(transactionHistory);
            CardBalance -= 1100;   //1100 là phí rút tại ATM
            TransactionHistory phi = new TransactionHistory(DateTime.Now, this.AccountNumber, true, Type, StatusCard, "Phi rut tien", 1100, CardBalance, this.ID);
            this.Histories.Add(phi);
            return true;
        }
        return false;
    }
    public bool WithdrawPOS(decimal x)
    {
        if (CheckBalanceMinimun(x) && StatusCard == true)
        {
            CardBalance -= x;
            TransactionHistory transactionHistory = new TransactionHistory(DateTime.Now, this.AccountNumber, true, Type, StatusCard, "Rut tien tai POS", x, CardBalance, this.ID);
            this.Histories.Add(transactionHistory);
            return true;
        }
        return false;
    }
    public bool TransfersATM(decimal x, string pin)
    {
        ResetLimit();
        if (pin != this.Pin)
        {
            return false;
        }
        if (CheckBalanceMinimun(x) && StatusCard == true && x < hanMucChuyenKhoanTrenLan
            && x + SoTienDaChuyen < hanMucChuyenKhoanTrenNgayCungNH)
        {
            CardBalance -= x;
            Last = DateTime.Now;
            SoTienDaChuyen += x;
            TransactionHistory transactionHistory = new TransactionHistory(DateTime.Now, this.AccountNumber, true, Type, StatusCard, "", x, CardBalance, this.ID);
            this.Histories.Add(transactionHistory);
            return true;
        }
        return false;
    }
    public bool TransfersPOS(decimal x, string transactioncontent)
    {
        if (CheckBalanceMinimun(x))// && StatusCard == true)
        {
            CardBalance -= x;
            TransactionHistory transactionHistory = new TransactionHistory(DateTime.Now, this.AccountNumber, true, Type, StatusCard, transactioncontent, x, CardBalance, this.ID);
            this.Histories.Add(transactionHistory);
            return true;
        }
        return false;
    }
    public bool ReceiveMoney(decimal x, string transactioncontent)
    {
        if (!StatusCard)
        {
            return false;
        }
        CardBalance += x;
        TransactionHistory transactionHistory = new TransactionHistory(DateTime.Now, this.AccountNumber, false, Type, StatusCard, transactioncontent, x, CardBalance, this.ID);
        this.Histories.Add(transactionHistory);
        return true;
    }
    public bool CheckBalanceMinimun(decimal money)
    {
        return CardBalance - money >= 50000;
    }
    public bool Payment(decimal x, string pin, string transactioncontent)
    {
        if (pin != this.Pin)
        {
            return false;
        }
        if (CheckBalanceMinimun(x) && StatusCard == true)
        {
            CardBalance -= x;
            TransactionHistory transactionHistory = new TransactionHistory(DateTime.Now, this.AccountNumber, true, Type, StatusCard, transactioncontent, x, CardBalance, this.ID);
            this.Histories.Add(transactionHistory);
            return true;
        }
        return false;
    }
    public void MonthlyInterest()
    {
        DateTime C = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);// ngày đầu của tháng hiện tại
        DateTime D = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        D.AddMonths(1);
        D.AddDays(-1);// ngày cuối của tháng hiện tại
        if (DateTime.Now == D)
        {
            decimal x = CardBalance * 0.01M;
            CardBalance += x;
            TransactionHistory transactionHistory = new TransactionHistory(DateTime.Now, this.AccountNumber, false, Type, StatusCard, "lai hang thang", x, CardBalance, this.ID);
            this.Histories.Add(transactionHistory);
        }
    }
    public void AutomaticDeductionOfAnnualFee()
    {
        if (DateTime.Now == this.AnnualFeesYear.AddYears(1))
        {
            CardBalance -= this.AnnualFees;
            this.AnnualFeesYear = new DateTime(this.StartTime.Year + 1, this.StartTime.Month, this.StartTime.Day);
            TransactionHistory transactionHistory = new TransactionHistory(DateTime.Now, this.AccountNumber, true, this.Type
               , this.StatusCard, "Trừ phí thường niên", this.AnnualFees, this.CardBalance, this.ID);
            Histories.Add(transactionHistory);
        }
    }
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
        if (CheckBalanceMinimun(x) && StatusCard == true)
        {
            CardBalance -= x;
            TransactionHistory transactionHistory = new TransactionHistory(DateTime.Now, this.AccountNumber, true, Type, StatusCard, "Doi tien ngoai te", x, CardBalance, this.ID);
            this.Histories.Add(transactionHistory);
            return true;
        }
        return false;
    }
    public void XuatThongTinThe()
    {
        Console.WriteLine("Type: " + this.Type);
        Console.WriteLine("Account Number: " + this.AccountNumber);
        Console.WriteLine("Card Balance: " + this.CardBalance);
    }
    ~DomesticDebitCard() { }
}
class NapasSuccessCard : DomesticDebitCard
{
    public NapasSuccessCard(string type, string accountnumber, string id, string pin, List<TransactionHistory> Histories, Client A, string description) : base(type, accountnumber, id, pin, Histories, A, description)
    {
        Type = "Napas Success";
        hanMucRutTienTrenNgay = 25000000;
        hanMucChuyenKhoanTrenNgayCungNH = 50000000;
        hanMucChuyenKhoanTrenNgayKhacNH = 25000000;
        hanMucRutTienTrenLanCungNH = 5000000;
        hanMucRutTienTrenLanKhacNH = 3000000;
        hanMucChuyenKhoanTrenLan = 2500000;
        AnnualFees = 12000;
    }
    ~NapasSuccessCard() { }
}
class NapasSuccessPlusCard : DomesticDebitCard
{
    public NapasSuccessPlusCard(string type, string accountnumber, string id, string pin, List<TransactionHistory> Histories, Client A, string description) : base(type, accountnumber, id, pin, Histories, A, description)
    {
        Type = "Napas Success Plus";
        hanMucRutTienTrenNgay = 50000000;
        hanMucChuyenKhoanTrenNgayCungNH = 100000000;
        hanMucChuyenKhoanTrenNgayKhacNH = 100000000;
        hanMucRutTienTrenLanCungNH = 5000000;
        hanMucRutTienTrenLanKhacNH = 3000000;
        hanMucChuyenKhoanTrenLan = 25000000;
        AnnualFees = 50000;
        InternetLimit = 5000000;
    }
    ~NapasSuccessPlusCard() { }
}