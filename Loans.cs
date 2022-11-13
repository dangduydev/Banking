public class Loans : Function
{
    private decimal limit, percent, loansBadDebit, numberofpayment, debt; //
    private int term, months;
    private DateTime datetimeloan, datepayment, datepayeverymonth; //payment - day payment, Term - limit month loan
    private bool isbaddebt;
    private int numberofloans;
    private bool isLoan;
    private decimal sotiendavay;
    public int Months { get; set; }
    public bool Isbaddebt { get; set; }// có phải nợ xấu hay ko
    public decimal Debt { get; set; }// dư nợ
    public decimal kq { get; set; }
    public int Term { get; set; }// kỳ hạn
    public decimal Limit { get; set; }// giới hạn vay
    public decimal Percent { get; set; }// lãi suất
    public DateTime DateTimeloan { get; set; }// ngày bắt đầu vay
    public DateTime DatePayment { get; set; }// ngày phải trả để k khoá thẻ
    public DateTime Datepayeverymonth { get; set; }//ngày phải trả mỗi tháng trong kỳ hạn
    public decimal LoansBadDebit { get; set; }// số tiền nợ xấu
    public List<DebtHistory> LoansHistoryDebitList = new List<DebtHistory>();// lịch sử ghi nợ
    public List<TransactionHistory> Histories = new List<TransactionHistory>();
    public Loans(string type, string accountnumber, string id,
        Client A, string description, int months, decimal Debt,
        List<TransactionHistory> histories, List<DebtHistory> listDebt)
                : base(type, accountnumber, id, A, description)
    {
        DateTimeloan = DateTime.Now;
        Isbaddebt = false;
        Histories = histories;
        LoansHistoryDebitList = listDebt;
        Months = months;
        DatePayment = DateTimeloan.AddMonths(months);
        isLoan = false;
    }
    public bool Init()
    {
        this.Debt = Debt;
        if (CheckLoanLimit() && CheckMonthLimit())
        {
            TransactionHistory loansTransaction = new TransactionHistory(DateTime.Now, Type, false, Type, true, "Vay", Debt, Debt, this.ID);// lịch sử giao dịch
            Histories.Add(loansTransaction);
            // lưu lại lịch sử nợ
            decimal kq = Debt / Convert.ToDecimal(Months) * (1 + Percent);
            for (int i = 0; i < this.Months; i++)
            {
                DebtHistory tmp = new(DateTimeloan.AddMonths(i + 1), kq, this.ID, "", this.Type);
                LoansHistoryDebitList.Add(tmp);
            }
            isLoan = true;
            return true;
        }
        return false;
    }
    public bool CheckLoanLimit()
    {
        return (this.Debt <= this.Limit);
    }
    public decimal CheckBadDebt() //nợ xấu có nghĩa là số tiền còn lại trong kỳ chưa thanh toán
    {
        return 0;
    }

    public bool CheckMonthLimit()
    {
        return (this.Months <= this.Term);
    }
    public int CheckFico(double Fico)
    {
        int type;
        if (Fico >= 7.85 && Fico <= 10) { type = 1; }
        else if (Fico >= 6.7 && Fico < 7.85) { type = 2; }
        else { type = 3; }
        return type;
    }
    public int CheckIncome(decimal Income)
    {
        int type;
        if (Income >= 50000000) { type = 1; }
        else if (Income >= 30000000 && Income < 50000000) { type = 2; }
        else { type = 3; }
        return type;
    }
    // vay chuyển khoản
    public bool Transferloans(decimal x)// vay thấu chi
    {
        string[] ListLoan = { "Unsecured Loan", "Mortage Loan", "Overdraft Loan", "Installment Loan" };
        if ((this.Type != ListLoan[2]) && Debt >= 0 && x + Debt < Limit) return false;
        sotiendavay += x;
        Debt += x;
        DatePayment = DateTimeloan.AddMonths(Months);
        TransactionHistory loansTransaction = new TransactionHistory(DateTime.Now, Type, false, Type, true, "Vay thau chi", x, Debt, this.ID);// lịch sử giao dịch
        Histories.Add(loansTransaction);
        // lưu lại lịch sử nợ
        decimal kq = Debt * (1 + Percent);
        DebtHistory tmp = new(DateTimeloan.AddMonths(1), kq, this.ID, "", this.Type);
        LoansHistoryDebitList.Add(tmp);
        isLoan = true;
        return true;
    }
    public void Paymentloans(decimal x)// thanh toán nợ
    {
        Debt -= x;
        TransactionHistory loansTransaction = new TransactionHistory(DateTime.Now, Type, true, Type, true, "Tra no", x, Debt, this.ID);// lịch sử giao dịch
        Histories.Add(loansTransaction);
        for (int i = 0; i < this.LoansHistoryDebitList.Count; i++)
        {
            DebtHistory tmp = this.LoansHistoryDebitList[i];
            if (x == 0) break;
            if (tmp.IsPayment == false && tmp.Maximumpayment > 0 && tmp.NoInterestPaymentDate <= DateTime.Now)
            {
                if (x > tmp.Maximumpayment)
                {
                    tmp.IsPayment = true;
                    tmp.Maximumpayment = 0;
                    x -= tmp.Maximumpayment;
                }
                else
                    if (x == tmp.Maximumpayment)
                {
                    tmp.IsPayment = true;
                    tmp.Maximumpayment = 0;
                    x -= tmp.Maximumpayment;
                }
                else
                    if (x < tmp.Maximumpayment)
                {
                    tmp.Maximumpayment -= x;
                }
            }
        }
        if (x == 0) return;
        for (int i = 0; i < this.LoansHistoryDebitList.Count; i++)
        {
            DebtHistory tmp = this.LoansHistoryDebitList[i];
            if (x == 0) break;
            if (tmp.IsPayment == false && tmp.Maximumpayment > 0 && tmp.NoInterestPaymentDate >= DateTime.Now)
            {
                if (x > tmp.Maximumpayment)
                {
                    tmp.IsPayment = true;
                    tmp.Maximumpayment = 0;
                    x -= tmp.Maximumpayment;
                }
                else
                    if (x == tmp.Maximumpayment)
                {
                    tmp.IsPayment = true;
                    tmp.Maximumpayment = 0;
                    x -= tmp.Maximumpayment;
                }
                else
                    if (x < tmp.Maximumpayment)
                {
                    tmp.Maximumpayment -= x;
                }
            }
        }
        return;
    }
    // phương thức xuất thông tin vay connsole.wrilei.....
    public void Display()
    {
        Console.WriteLine($"Loan Date: {DateTimeloan} - {DatePayment}");
        Console.WriteLine($"Loan Type: {Type}");
        Console.WriteLine($"Loan: {Debt}");
        Console.WriteLine($"Time Loan: {Term}");
    }
    // cộng tiền lãi của những cái chưa trả hết trong lịch sử nợ
    public void CalIntervestAll()
    {
        decimal kq = 0;
        var lists = from tmp in LoansHistoryDebitList where tmp.IsPayment == false && tmp.NoInterestPaymentDate <= DateTime.Now select tmp;
        foreach (var item in lists)
        {
            decimal k = Decimal.Multiply(item.Maximumpayment, this.Percent);
            kq = Decimal.Add(kq, k);
        }
        this.Debt += kq;
        DebtHistory p = new DebtHistory(DateTime.Now, kq, this.ID, "", this.Type);
        this.LoansHistoryDebitList.Add(p);
    }
    // nợ xấu kiểm tra cạt nhật
    public void CheckBadDebit()
    {
        var list = from tmp in this.LoansHistoryDebitList where tmp.IsPayment == false && tmp.NoInterestPaymentDate.AddDays(90) <= DateTime.Now select tmp;
        if (list.Count() > 0) this.Isbaddebt = true;
    }
    ~Loans() { }
}
class UnsecuredLoan : Loans
{
    public UnsecuredLoan(
        string type, string accountnumber, string id, Client A, string description,
        List<TransactionHistory> histories, List<DebtHistory> listDebt,
        decimal Debt, int months, int Fico) : base(type, accountnumber, id, A,
            description, months, Debt, histories, listDebt)
    {
        Type = "Unsecured Loan";
        if (CheckFico(Fico) == 2)
        {
            Limit = 30000000;
            Percent = 0.18m;
            Term = 24;
        }
        else if (CheckFico(Fico) == 1)
        {
            Limit = 100000000;
            Percent = 0.12m;
            Term = 36;
        }
        else
        {
            Limit = 15000000;
            Percent = 0.2m;
            Term = 12;
        }
        this.Debt = Debt;
    }
    ~UnsecuredLoan() { }
}

class MortageLoan : Loans
{
    public MortageLoan(string type, string accountnumber, string id, Client A,
        string description, int months, decimal Debt,
        List<TransactionHistory> histories, List<DebtHistory> listDebt, decimal totalasset)
                    : base(type, accountnumber, id, A, description, months, Debt, histories, listDebt)
    {
        Limit = totalasset;
        Percent = 0.125m;
        Term = 60;
        Type = "Mortage Loan";
        this.Debt = Debt;

    }
    ~MortageLoan() { }
}

class OverdraftLoan : Loans
{
    public OverdraftLoan(string type, string accountnumber, string id,
        Client A, string description, int months, decimal Debt,
        List<TransactionHistory> histories, List<DebtHistory> listDebt, decimal income)
                : base(type, accountnumber, id, A, description, months, Debt, histories, listDebt)
    {
        Limit = 5 * income;
        Percent = 0.1m;
        Term = 1;
        Months = 1;
        Type = "Overdraft Loan";
        DatePayment = DateTimeloan.AddMonths(1);
    }
    ~OverdraftLoan() { }
}

class InstallmentLoan : Loans
{
    public InstallmentLoan(string type, string accountnumber,
        string id, Client A, string description, int months, decimal Debt,
        List<TransactionHistory> histories, List<DebtHistory> listDebt, decimal Income)
                : base(type, accountnumber, id, A, description, months, Debt, histories, listDebt)
    {
        Type = "Installment Loan";
        DateTimeloan = DateTime.Now;
        if (CheckIncome(Income) == 1)
        {
            Limit = 500000000;
            Percent = 0.15m;
            Term = 36;
        }
        else if (CheckIncome(Income) == 2)
        {
            Limit = 300000000;
            Percent = 0.17m;
            Term = 24;
        }
        else
        {
            Limit = 100000000;
            Percent = 0.2m;
            Term = 12;
        }
        this.Debt = Debt;
    }
    ~InstallmentLoan() { }
}