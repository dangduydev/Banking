
public class TimeDeposit : SavingAccount
{
    private int typesaving;// phương thức 
    public int TypeSaving
    {
        get; set;
    }
    public TimeDeposit(string type, string accountnumber,string description,string id, decimal savingdeposit, int term, int typesaving,List<TransactionHistory> histories,Client A): base(type,accountnumber,description,id,savingdeposit,term,histories,A)
    {
        Minium = 1000000;
        TypeSaving = typesaving;
    }
    public void Init()
    {
        TransactionHistory tmp1 = new TransactionHistory(this.StartedDate, AccountNumber, false, Type, true,
                "Gui tiet kiem"
                , SavingDeposit, SavingDeposit,this.ID);
        Histories.Add(tmp1);
    }
    public bool Withdraw(decimal x, int newterm)
    {
        if (DateTime.Now < this.DueDate)
        {
            // có bắt đầu kỳ hạn mới hay ko 
            // có cập nhật lại
            if (x <= SavingDeposit)
            {
                SavingDeposit -= x;// chuyển tiền vào debitcard
                this.Interest = 0;
                if (newterm > 0)
                {
                    Expire(newterm);
                }
                TransactionHistory tmp = new TransactionHistory(DateTime.Now, AccountNumber, true, Type, true, "Rút tiền tiết kiêm" +
                " tại quầy POS", x, SavingDeposit,this.ID);
                Histories.Add(tmp);
                return true;
            }
            return false;
        }
        else
        {
            this.AutoUpdateSavingDeposit();
            if (x <= SavingDeposit)
            {
                SavingDeposit -= x;// chuyển tiền vào debitcard
                this.Interest = 0;
                // lưu lịch sử giao dịch
                TransactionHistory tmp = new TransactionHistory(DateTime.Now, AccountNumber, true, Type, true, "Rút tiền tiết kiêm" +
                " tại quầy POS", x, SavingDeposit,this.ID);
                Histories.Add(tmp);
                return true;
            }
            return false;
        }
    }
    // Dao han
    public void Expire(int term)
    {
        this.InterestPercent = FindInterestPercent(term);
        this.StartedDate = DateTime.Now;
        this.DueDate = this.StartedDate.AddMonths(term);
    }
    /*
      Nếu tự động gia hạn gốc và lãi: thì cứ qua 1 lần của kỳ hạn cả
      tiền gốc và lãi sẽ gộp lại sẽ tự động tiếp tục 1 lần kỳ hạn nữa với mứ
      c lãi suất như trước
     */
    public decimal CalInterestMoney()
    {
        return this.FindInterestPercent(this.Term) * this.SavingDeposit;
    }
    public decimal AutoUpdateSavingDeposit()// tự cộng tiền lãi vào debit card
    {
        if (DueDate.Month == DateTime.Now.Month && DueDate.Day == DateTime.Now.Day && DueDate.Year == DateTime.Now.Year)
        {
            this.Interest += CalInterestMoney();
            if (TypeSaving == 2)
            {
                decimal tmpp = CalInterestMoney(); // ban đầu 
                SavingDeposit += tmpp;
                TransactionHistory tmp1 = new TransactionHistory(DateTime.Now, AccountNumber, false, Type, true,
                    "Tự động gia hạn tiền gốc và lãi khi hết kỳ hạn"
                    , tmpp, SavingDeposit,this.ID);
                Histories.Add(tmp1);
                // lưu lại lịch sử giao dịch
                this.DueDate.AddMonths(this.Term);
                return 0;
            }
            if (TypeSaving == 1)
            {
                SavingDeposit += CalInterestMoney();
                // lưu lại lịch sử giao dịch
                decimal tmp = SavingDeposit;
                SavingDeposit = 0;
                this.Term = 0;
                TransactionHistory tmp2 = new TransactionHistory(DateTime.Now, AccountNumber, true, Type, true,
                    "Tự động rút hết tiền"
                    , tmp, SavingDeposit,this.ID);
                Histories.Add(tmp2);
                return tmp;
            }
            if (TypeSaving == 3)
            {
                // lưu lại lịch sử giao dịch
                decimal tmp = CalInterestMoney();
                this.DueDate.AddMonths(this.Term);
                TransactionHistory tmp3 = new TransactionHistory(DateTime.Now, AccountNumber, true, Type, true,
                    "Tự động rút hết tiền"
                    , tmp, SavingDeposit,this.ID);
                Histories.Add(tmp3);
                return CalInterestMoney();
            }
        }
        return -1;
    }
}