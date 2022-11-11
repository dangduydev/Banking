using System.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

public class Client
{
    private string name;
    public string Name { get; set; }
    private DateTime dateofbirth;
    public DateTime DateOfBirth { get; set; }
    private string address;
    public string Address { get; set; }
    private string id;
    public string ID { get; set; }
    private bool sex;
    public bool Sex { get; set; }
    private string workplace;
    public string WorkPlace { get; set; }
    private string position;
    public string Position { get; set; }
    private string nationality;
    public string Nationality { get; set; }
    private string password;
    public string Password { get; set; }
    private long CustomerID = 0;
    private DateTime issueddate;
    public DateTime IssuedDate { get; set; }
    private string phonenumber;
    public string PhoneNumber { get; set; }
    private decimal income;
    public decimal Income { get; set; }
    public bool StatusClient { get; set; }
    private int fico;
    public int Fico { get; set; }
    // Chi nhanh: mot so ngan hang nhu Agribank se thu them phi tu 0.02 - 0.07% neu rut tien khac chi nhanh
    private string branch;
    public string Branch { get; set; }
    public Client()
    { }
    public Client(string name, string address, DateTime dateofbirth,
        string id, string position, string nationality, string workplace,
        bool sex, string phonenumber, DateTime issueddate, decimal income, string branch, string password)
    {
        this.Name = name;
        this.Address = address;
        this.DateOfBirth = dateofbirth;
        this.ID = id;
        this.WorkPlace = workplace;
        this.Position = position;
        this.Nationality = nationality;
        this.Sex = sex;
        this.PhoneNumber = phonenumber;
        this.IssuedDate = issueddate;
        this.Income = income;
        this.Branch = branch;
        this.StatusClient = true;
        this.Password = password;
        if (Income < 5000000)
        {
            Fico = 1;
        }
        else if (Income >= 5000000 && Income < 1000000)
        {
            Fico = 2;
        }
        else if (Income >= 10000000 && Income < 2000000)
        {
            Fico = 4;
        }
        else if (Income >= 20000000 && Income < 3000000)
        {
            Fico = 7;
        }
        else if (Income >= 50000000)
        {
            Fico = 10;
        }
    }
    // ngân hàng tự động khóa thẻ 
    // khách hàng muốn khóa thẻ

    //methods
    public bool AgeCheck()
    {
        #region Hàm AgeCheck
        /*
             Trả về kiểu giá trị bool 
             True: đủ 18 tuổi
             False: ko đủ 18 tuổi
         */
        #endregion
        return (DateTime.Now > DateOfBirth.AddYears(18));
    }
    public bool IDCheck()
    {
        #region Hàm IDCheck
        /*
             Trả về kiểu giá trị bool 
             True: còn hạn
             False: hết hạn
         */
        #endregion
        return issueddate > DateTime.Now;
    }
    public bool ChangePassword(string OldPassword, string NewPassWord)
    {
        if (this.Password == OldPassword)
        {
            this.Password = NewPassWord;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ExportInformation()
    {
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Date of birth: {DateOfBirth.ToString("dd/MM/yyyy")}");
        Console.WriteLine($"Address: {Address}");
        Console.WriteLine($"CCCD/CMND: {Name}");
        // sưa cái set
        //Console.WriteLine($"Sex: {sex}
        Console.WriteLine($"WorkPlace: {WorkPlace}");
        Console.WriteLine($"Position: {Position}");
        Console.WriteLine($"Nationality: {Nationality}");
        Console.WriteLine($"CustomerID: {CustomerID}");
        Console.WriteLine($"Issued Date: {IssuedDate.ToString("dd/MM/yyyy")}");
        //xuất fico
        // xuất các thẻ 
    }


}