public class Function
{
    private string type;
    public string Type { get; set; }
    private string accountnumber;
    public string AccountNumber { get; set; }
    private string description;
    public string Description { get; set; }
    private string id;
    public string ID { get; set; }

    //public List<TransactionHistory> Histories = new List<TransactionHistory>();

    public Function(string type, string accountnumber, string id,Client A,string description)
    {
        this.AccountNumber = accountnumber;
        this.Type = type;
        this.ID = A.ID;
        this.Description = description;
    }
    public Function() { }
    public void Xuat()
    {
        Console.WriteLine($"Type: {Type}");
        Console.WriteLine($"AccountNumber: {AccountNumber}");
        Console.WriteLine($"ID: {ID}");
        Console.WriteLine($"Description: {Description}");
        Console.WriteLine("------------------");
    }
}