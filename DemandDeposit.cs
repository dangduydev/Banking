public class DemandDeposit : SavingAccount 
{
    private DateTime lastTrading;
    public DateTime LastTrading { get; set; }
    private decimal maximun;
    public decimal Maximun { get; set; }
    public DemandDeposit(string type, string accountnumber,string description,string id, decimal savingdeposit, int term,List<TransactionHistory> histories,Client A) : base(type,accountnumber,description,id,savingdeposit,term,histories,A)
        
    {
        Minium = 500000;
        Maximun = 200000000000;
        LastTrading = this.StartedDate;
        this.Term = term;
        
    }
    ~DemandDeposit()
    {

    }
    public void Init()
    {
        TransactionHistory tmp1 = new TransactionHistory(this.StartedDate, this.AccountNumber,false, Type, true, "Goi tiet kiem", SavingDeposit, SavingDeposit,this.ID);
        Histories.Add(tmp1);
    }
    public decimal CalInterestMoneyEveryDay(int days)
    {
        return this.FindInterestPercent(this.Term) * this.SavingDeposit / 365 * days;
    }
    // cật nhật tiền lãi

    public bool WithDrawPos(decimal x)
    {
        if (LastTrading < DateTime.Now)
        {
            TimeSpan Time = DateTime.Now - LastTrading;
            int gapdate = Time.Days;
            decimal tienlai = CalInterestMoneyEveryDay(gapdate);
            SavingDeposit += tienlai;
            TransactionHistory tmplai = new TransactionHistory(DateTime.Now, AccountNumber, false, Type, true, "Tien lai", tienlai, SavingDeposit,this.ID);
            Histories.Add(tmplai);
            Interest += tienlai;
            SavingDeposit += CalInterestMoneyEveryDay(gapdate) - x;
            if (x > this.SavingDeposit) return false;
            TransactionHistory tmp = new TransactionHistory(DateTime.Now, AccountNumber, true, Type, true, "Rút tiền tiết kiêm" +
                " tại quầy POS", x, SavingDeposit,this.ID);
            Histories.Add(tmp);
            LastTrading = DateTime.Now;
            return true;
        }
        return false;

    }
    public bool RechargeMoney(decimal x)
    {
        TimeSpan Time = DateTime.Now - LastTrading;
        int gapdate = Time.Days;
        decimal tienlai = CalInterestMoneyEveryDay(gapdate);
        SavingDeposit += tienlai;
        TransactionHistory tmplai = new TransactionHistory(DateTime.Now, AccountNumber, false, Type, true, "Tien lai", tienlai, SavingDeposit,this.ID);
        Histories.Add(tmplai);
        Interest += tienlai;
        if (x + SavingDeposit > Maximun) return false;
        SavingDeposit += x;
        LastTrading = DateTime.Now;
        TransactionHistory tmp = new TransactionHistory(DateTime.Now, AccountNumber, false, Type, true, "Nạp tiền tiết kiệm tại pos", x, SavingDeposit,this.ID);
        Histories.Add(tmp);
        return true;
        // lưu lịch sử giao dịch
    }
    public void UpdateDepositLimit()
    {
        DateTime C = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);// 4/11/2022 -> C = 2022/11/1
        C.AddMonths(1);// C = 2022/12/1
        C.AddDays(-1);// C = 2022/11/31
        if (DateTime.Now == C)
        {
            if (Interest >= 2000000) Maximun = 30000000;
            if (Interest >= 5000000) Maximun = 50000000;
            if (Interest >= 10000000) Maximun = 80000000;
            if (Interest >= 15000000) Maximun = 100000000;
        }
    }


}