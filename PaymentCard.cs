public class PaymentCard : Function
{
    private string pin;
    public string Pin
    { get; set; }
    private bool statuscard;
    public bool StatusCard
    { get; set; }
    private DateTime duedate;
    public DateTime Duedate
    {
        get; set;
    }
    private decimal Annualfees;
    #region Annual fees
    //Phí thường niên
    #endregion
    public decimal AnnualFees
    {
        get; set;
    }
    private DateTime Annualfeesyear;

    public DateTime AnnualFeesYear
    {
        get; set;
    }
    public DateTime StartTime;
    public List<TransactionHistory> Histories = new List<TransactionHistory>();
    public Client AA;
    public PaymentCard(string type,string accountnumber,string id,string pin,List<TransactionHistory> histories,Client A,string description) : base(type,accountnumber,id,A,description)
    {
        Pin = pin;
        StatusCard = true;
        AA = A;
        StartTime = DateTime.Now;
        AnnualFeesYear = DateTime.Now;
        this.Histories = histories;
    }
    public bool CheckAccountStatus()
    {
        return StatusCard;
    }
    public void LockCard()
    {
        this.StatusCard = false;
    }
    public void OpenCard()
    {
        this.StatusCard = true;
    }
    ~PaymentCard()
    { }
}