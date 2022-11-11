public class TransactionHistory 
{
    // lịch sử giao dịch
    private DateTime daytrading;
    public DateTime DayTrading { get; set; }// tg giao dịch

    private bool transactiontype;// loại giao dịch
    public bool TransactionType { get; set; }
    /*
     true chuyển tiền 
     false nhận tiền 
     */
    private string id;
    public string ID { get; set; }
    private bool statuscard;
    public bool StatusCard { get; set; }
    private decimal transactionmoney;
    public decimal TransactionMoney { get; set; }

    private decimal cardbalance;
    public decimal CardBalance { get; set; }

    private string transactioncontent;
    public string TransactionContent { get; set; }

    private string accountnumber;
    public string AccountNumber { get; set; }

    private string type;
    public string Type { get; set; }

    public TransactionHistory() { }
    public TransactionHistory(DateTime daytrading, string accountnumber, bool transactiontype,
        string typecard, bool statuscard, string transactioncontent,
        decimal transactionmoney, decimal cardbalance, string id)
    {
        DayTrading = daytrading;
        AccountNumber = accountnumber;
        TransactionType = transactiontype;
        Type = typecard;
        StatusCard = statuscard;
        TransactionContent = transactioncontent;
        TransactionMoney = transactionmoney;
        CardBalance = cardbalance;
        ID = id;

        //DayTrading,TransactionType,StatusCard,TransactionMoney,CardBalance,TransactionContent,Type,AccountNumber,Histories
    }
    public void XuatLichSuGD()
    {
        if (TransactionType)
        {
            Console.WriteLine($"Day trading: {DayTrading.ToString("dd/MM/yyyy")}");
            Console.WriteLine("Time trading: {0}:{1}:{2}", DayTrading.Hour, DayTrading.Minute, DayTrading.Second);
            Console.WriteLine($"Transaction content: {TransactionContent}");
            Console.WriteLine($"Transaction Money: -{TransactionMoney}");
            Console.WriteLine($"Balance after transaction: {CardBalance}");
        }
        else
        {
            Console.WriteLine($"Day trading: {DayTrading.ToString("dd/MM/yyyy")}");
            Console.WriteLine("Time trading: {0}:{1}:{2}", DayTrading.Hour, DayTrading.Minute, DayTrading.Second);
            Console.WriteLine($"Transaction content: {TransactionContent}");
            Console.WriteLine($"Transaction Money: +{TransactionMoney}");
            Console.WriteLine($"Balance after transaction: {CardBalance}");
        }
    }
}

