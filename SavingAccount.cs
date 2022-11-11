public class SavingAccount :Function
{
    // So tien gui toi thieu doi voi moi loai
    // Cac loai ky han
    public enum TypeOfTerm
    {
        T0 = 0,
        T2 = 4,
        T5 = 5,
        T7 = 6,
        T10 = 7,
        T12 = 8,
        T14 = 9,
        T16 = 10,
    }
    // Loai tien te
    private decimal minium;
    public decimal Minium { get; set; }
    // Tai khoan thu huong: la so tai khoan cua khach hang
    // Doi voi khach hang khong co Payment Account van co the mo tai khoan tiet kiem,khong bat buoc phai co tai khoan thanh tona tai ngan hang
    // Ngay bat dau mo tai khoan tiet kiem
    private DateTime starteddate;//Ngày bắt đầu 
    public DateTime StartedDate { get; set; }
    // Lai suat 1 nam
    private decimal interestpercent;
    public decimal InterestPercent { get; set; }
    // So tien gui
    private decimal savingdeposit;
    public decimal SavingDeposit { get; set; }
    private int term;
    public int Term{ get; set; }

    private DateTime duedate;
    public DateTime DueDate{ get; set; }

    private decimal interest;// tổng tiền lãi đã tích lũy
    public decimal Interest
    { get; set; }
    public List<TransactionHistory> Histories = new List<TransactionHistory>();
    public SavingAccount(string type,string accountnumber,string description,string id,decimal savingdeposit, int term,List<TransactionHistory> histories,Client A) : base(type,accountnumber,id,A,description)
    {
        StartedDate = DateTime.Now;   //note
        SavingDeposit = savingdeposit;
        Term = term;
        Histories = histories;
        DueDate = StartedDate.AddMonths(term);
    }

    ~SavingAccount()
    {

    }
    public DateTime FindDueDate(int Term, DateTime StartedDate)
    {
        DateTime x = StartedDate.AddMonths(Term);
        return x;

    }
    public bool CheckMinimum()
    {
        return (SavingDeposit > Minium);
    }
    public decimal FindInterestPercent(int Term)
    {
        decimal x;
        switch (Term)
        {
            case 0:
                {
                    x = (decimal)0.01;
                    break;
                }
            case 1:
            case 2:
                {
                    x = (decimal)TypeOfTerm.T2 / 100;
                    break;
                }
            case 3:
            case 4:
            case 5:
                {
                    x = (decimal)TypeOfTerm.T5 / 100;
                    break;
                }
            case 6:
            case 7:
                {
                    x = (decimal)TypeOfTerm.T7 / 100;
                    break;
                }
            case 8:
            case 9:
            case 10:
                {
                    x = (decimal)TypeOfTerm.T10 / 100;
                    break;
                }
            case 11:
            case 12:
                {
                    x = (decimal)TypeOfTerm.T12 / 100;
                    break;
                }
            case 13:
            case 14:
                {
                    x = (decimal)TypeOfTerm.T14 / 100;
                    break;
                }
            case 15:
            case 16:
                {
                    x = (decimal)TypeOfTerm.T16 / 100;
                    break;
                }
            default:
                {
                    x = (decimal)TypeOfTerm.T16 / 100;
                    break;
                }

        }
        return x;
    }
    // Xuaatrs thông tin của cái sổ
}