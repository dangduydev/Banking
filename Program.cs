using System.Collections.Generic;
using System;
using System.Linq;
using LINQtoCSV;
using System.IO;
using System.Text;

namespace Bank
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] ListCard =
            {
                "Napas Success", "Napas Success Plus", "VisaStandard", "VisaGold",
                "MastercardGold", "MastercardPlatinum", "JCBGold", "JCBUltimate",
            };
            string[] ListLoan =
            {
                "Unsecured Loan", "Mortage Loan", "Overdraft Loan", "Installment Loan"
            };
            string[] ListSaving =
            {
                "Demand Deposit", "Time Deposit"
            };
            SortedList<string, Client> listClient = new SortedList<string, Client>();
            SortedList<string, DomesticDebitCard> listDebit = new SortedList<string, DomesticDebitCard>();
            SortedList<string, InternationalCreditCard> listCredit = new SortedList<string, InternationalCreditCard>();
            SortedList<string, DemandDeposit> listSavingDemandDeposit = new SortedList<string, DemandDeposit>();
            SortedList<string, TimeDeposit> listSavingTimeDeposit = new SortedList<string, TimeDeposit>();
            SortedList<string, Loans> listLoan = new SortedList<string, Loans>();
            List<TransactionHistory> listHistory = new List<TransactionHistory>();
            List<DebtHistory> listDebt = new List<DebtHistory>();
            List<Function> AllCard = new List<Function>();
            ReadCvsFileDatabase(listClient, AllCard, listHistory, listDebt);
            CreateAllCardListOld(listClient, AllCard, listHistory, listDebt, listDebit, listCredit, listSavingDemandDeposit, listSavingTimeDeposit, listLoan);
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu(listClient, listHistory, listDebit, listCredit, listSavingDemandDeposit, listSavingTimeDeposit, listLoan, listDebt, AllCard);
            }
            // bug dòng  1595
            CreateListAllCardListNew(listClient, AllCard, listDebit, listCredit, listSavingDemandDeposit, listSavingTimeDeposit, listLoan);
            WriteCsvFileDatabase(listHistory, listDebt, AllCard, listClient);
        }
        public static bool CheckAccountNumber(string accountnumber, SortedList<string, DomesticDebitCard> listDebit, SortedList<string, InternationalCreditCard> listCredit,
        SortedList<string, DemandDeposit> listSavingDemandDeposit, SortedList<string, TimeDeposit> listSavingTimeDeposit)
        {
            var listaccountnumberdebit = from tmp in listDebit where tmp.Key == accountnumber select tmp;
            var listaccountnumbercredit = from tmp in listCredit where tmp.Key == accountnumber select tmp;
            var listaccountnumbersavingdemand = from tmp in listSavingDemandDeposit where tmp.Key == accountnumber select tmp;
            var listaccountnumbersavingtime = from tmp in listSavingTimeDeposit where tmp.Key == accountnumber select tmp;
            return (listaccountnumbercredit.Count() + listaccountnumberdebit.Count() + listaccountnumbersavingdemand.Count() + listaccountnumbersavingtime.Count() == 0);
        }
        static void ReadCvsFileDatabase(SortedList<string, Client> listClient, List<Function> AllCard, List<TransactionHistory> listHistory, List<DebtHistory> listDebt)
        {
            ReadCvsFileDatabaseClients(listClient);
            ReadCvsFileDatabaseAllCard(AllCard);
            ReadCvsFileDatabaseHistory(listHistory);
            ReadCvsFileDatabaseDebt(listDebt);
        }
        static void CreateAllCardListOld(SortedList<string, Client> listClient, List<Function> AllCard, List<TransactionHistory> listHistory, List<DebtHistory> listDebt, SortedList<string, DomesticDebitCard> listDebit, SortedList<string, InternationalCreditCard> listCredit, SortedList<string, DemandDeposit> listSavingDemandDeposit, SortedList<string, TimeDeposit> listSavingTimeDeposit, SortedList<string, Loans> listLoan)
        {
            CreateAllCardListOldDebits(listClient, listDebit, AllCard, listHistory);
            CreateAllCardListOldCredits(listClient, listCredit, AllCard, listHistory, listDebt);
            CreateAllCardListOldSavingDemandDeposit(listClient, listSavingDemandDeposit, AllCard, listHistory);
            CreateAllCardListOldSavingTimeDeposit(listClient, listSavingTimeDeposit, AllCard, listHistory);
            CreateAllCardListOldLoans(listClient, listLoan, AllCard, listHistory, listDebt);
        }
        static void CreateListAllCardListNew(SortedList<string, Client> listClient, List<Function> AllCard, SortedList<string, DomesticDebitCard> listDebit, SortedList<string, InternationalCreditCard> listCredit, SortedList<string, DemandDeposit> listSavingDemandDeposit, SortedList<string, TimeDeposit> listSavingTimeDeposit, SortedList<string, Loans> listLoan)
        {
            AllCard.Clear();
            CreateListAllCardListNewDebit(listClient, AllCard, listDebit);
            CreateListAllCardListNewCredit(listClient, AllCard, listCredit);
            CreateListAllCardListNewSavingDemandDeposit(listClient, AllCard, listSavingDemandDeposit);
            CreateListAllCardListNewSavingTimeDeposit(listClient, AllCard, listSavingTimeDeposit);
            CreateListAllCardListNewLoans(listClient, AllCard, listLoan);
        }
        static void WriteCsvFileDatabase(List<TransactionHistory> listHistory, List<DebtHistory> listDebt, List<Function> AllCard, SortedList<string, Client> listClient)
        {
            WriteCsvFileDatabaseHistory(listHistory);
            WriteCsvFileDatabaseClients(listClient);
            WriteCsvFileDatabaseAllCard(AllCard);
            WriteCsvFileDatabaseDebt(listDebt);
        }
        static void ReadCvsFileDatabaseClients(SortedList<string, Client> listClient)
        {
            var csvFileDes = new CsvFileDescription
            {
                FirstLineHasColumnNames = true,
                IgnoreMissingColumns = true,
                //IgnoreUnknownColumns = true,
                SeparatorChar = ',',
                UseFieldIndexForReadingData = false
            };
            var csvContext = new CsvContext();
            var persons = csvContext.Read<Client>("databaseclients.csv", csvFileDes);
            foreach (var p in persons)
            {
                listClient.Add(p.ID, p);
            }
        }
        static void ReadCvsFileDatabaseAllCard(List<Function> Allcard)
        {
            var csvFileDes = new CsvFileDescription
            {
                FirstLineHasColumnNames = true,
                IgnoreMissingColumns = true,
                //IgnoreUnknownColumns = true,
                SeparatorChar = ',',
                UseFieldIndexForReadingData = false
            };
            var csvContext = new CsvContext();
            var persons = csvContext.Read<Function>("databaseallcard.csv", csvFileDes);
            foreach (var p in persons)
            {
                Allcard.Add(p);
            }
        }
        static void ReadCvsFileDatabaseHistory(List<TransactionHistory> histories)
        {
            var csvFileDes = new CsvFileDescription
            {
                FirstLineHasColumnNames = true,
                IgnoreMissingColumns = true,
                //IgnoreUnknownColumns = true,
                SeparatorChar = ',',
                UseFieldIndexForReadingData = false
            };
            var csvContext = new CsvContext();
            var persons = csvContext.Read<TransactionHistory>("databasehistory.csv", csvFileDes);
            foreach (var p in persons)
            {
                histories.Add(p);
            }
        }
        static void ReadCvsFileDatabaseDebt(List<DebtHistory> listDebt)
        {
            var csvFileDes = new CsvFileDescription
            {
                FirstLineHasColumnNames = true,
                IgnoreMissingColumns = true,
                //IgnoreUnknownColumns = true,
                SeparatorChar = ',',
                UseFieldIndexForReadingData = false
            };
            var csvContext = new CsvContext();
            var persons = csvContext.Read<DebtHistory>("databasedebt.csv", csvFileDes);
            foreach (var p in persons)
            {
                listDebt.Add(p);
            }
        }
        static void WriteCsvFileDatabaseClients(SortedList<string, Client> listClient)
        {

            var csvFileDes = new CsvFileDescription
            {
                FirstLineHasColumnNames = true,
                SeparatorChar = ',',
            };
            var csvContext = new CsvContext();
            csvContext.Write(listClient.Values, "databaseclients.csv", csvFileDes);
        }
        static void WriteCsvFileDatabaseAllCard(List<Function> AllCard)
        {

            var csvFileDes = new CsvFileDescription
            {
                FirstLineHasColumnNames = true,

                SeparatorChar = ',',
            };
            var csvContext = new CsvContext();
            csvContext.Write(AllCard, "databaseallcard.csv", csvFileDes);
        }
        static void WriteCsvFileDatabaseHistory(List<TransactionHistory> histories)
        {
            var csvFileDes = new CsvFileDescription
            {
                FirstLineHasColumnNames = true,

                SeparatorChar = ',',
            };
            var csvContext = new CsvContext();
            //var epersons = csvContext.Read<Person>("person.csv", csvFileDes);
            csvContext.Write(histories, "databasehistory.csv", csvFileDes);
        }
        static void WriteCsvFileDatabaseDebt(List<DebtHistory> listDebt)
        {
            var csvFileDes = new CsvFileDescription
            {
                FirstLineHasColumnNames = true,

                SeparatorChar = ',',
            };
            var csvContext = new CsvContext();
            csvContext.Write(listDebt, "databasedebt.csv", csvFileDes);
        }
        public static bool CreateClient(string name, string address, DateTime dateofbirth, string id, string position, string nationality, string workplace, bool sex, string phonenumber, DateTime issueddate, decimal income, string branch, string password, SortedList<string, Client> listClient)
        {
            var tmp = from ID in listClient.Keys where ID == id select ID;
            if (tmp.Count() > 0)
            {
                return false;
            }
            Client A = new Client(name, address, dateofbirth,
         id, position, nationality, workplace,
         sex, phonenumber, issueddate, income, branch, password);
            listClient.Add(A.ID, A);
            return true;
        }
        public static bool CreateDebitCard(string TypeCard, string accountnumber, string pin, Client A,
         SortedList<string, DomesticDebitCard> listDebit, List<TransactionHistory> listHistory)
        {
            string[] ListCard =
                  {
                "Napas Success", "Napas Success Plus"
            };
            if (TypeCard == ListCard[0])
            {
                DomesticDebitCard X = new NapasSuccessCard(TypeCard, accountnumber, A.ID, pin, listHistory, A, "");
                listDebit.Add(X.AccountNumber, X);
            }
            else
            {
                DomesticDebitCard X = new NapasSuccessPlusCard(TypeCard, accountnumber, A.ID, pin, listHistory, A, "");
                listDebit.Add(X.AccountNumber, X);
            }
            return true;
        }
        public static bool CreateCreditCard(string TypeCard, string accountnumber, string pin, Client A,
         SortedList<string, InternationalCreditCard> listCredit, List<TransactionHistory> listHistory, List<DebtHistory> listDebt)
        {
            string[] ListCard =
            {
                "VisaStandard", "VisaGold",
                "MastercardGold", "MastercardPlatinum", "JCBGold", "JCBUltimate",
            };
            if (TypeCard == ListCard[0])
            {
                InternationalCreditCard Ax = new VisaStandard(TypeCard, accountnumber, A.ID, pin, listHistory, listDebt, A, "");
                listCredit.Add(Ax.AccountNumber, Ax);
            }
            else
            if (TypeCard == ListCard[1])
            {
                InternationalCreditCard B = new VisaGold(TypeCard, accountnumber, A.ID, pin, listHistory, listDebt, A, "");
                listCredit.Add(B.AccountNumber, B);
            }
            else
            if (TypeCard == ListCard[2])
            {
                InternationalCreditCard B = new MastercardGold(TypeCard, accountnumber, A.ID, pin, listHistory, listDebt, A, "");
                listCredit.Add(B.AccountNumber, B);
            }
            else
            if (TypeCard == ListCard[3])
            {
                InternationalCreditCard B = new MastercardPlatinum(TypeCard, accountnumber, A.ID, pin, listHistory, listDebt, A, "");
                listCredit.Add(B.AccountNumber, B);
            }
            else
            if (TypeCard == ListCard[4])
            {
                InternationalCreditCard B = new JCBGold(TypeCard, accountnumber, A.ID, pin, listHistory, listDebt, A, "");
                listCredit.Add(B.AccountNumber, B);
            }
            else
            if (TypeCard == ListCard[5])
            {
                InternationalCreditCard B = new JCBUltimate(TypeCard, accountnumber, A.ID, pin, listHistory, listDebt, A, "");
                listCredit.Add(B.AccountNumber, B);
            }
            return true;
        }
        public static bool CreateSavingAccountDemandDeposit(string Typeofsaving, string accountsaving, decimal savingdeposit,
         int term, Client A, SortedList<string, DemandDeposit> listSavingDemandDeposit, List<TransactionHistory> listHistory)
        {
            DemandDeposit Ax = new DemandDeposit(Typeofsaving, accountsaving, "", A.ID, savingdeposit, 0, listHistory, A);
            if (Ax.CheckMinimum() == true && Ax.Term == 0 && Ax.Maximun >= savingdeposit)
            {
                listSavingDemandDeposit.Add(Ax.AccountNumber, Ax);
                return true;
            }
            return false;
        }
        public static bool CreateSavingAccountTimeDeposit(string Typeofsaving, string accountsaving, decimal savingdeposit,
         int term, int typeSaving, Client A, SortedList<string, TimeDeposit> listSavingTimeDeposit,
          List<TransactionHistory> listHistory)
        {
            TimeDeposit B = new TimeDeposit(Typeofsaving, accountsaving, "", A.ID, savingdeposit, term, typeSaving, listHistory, A);
            if (B.CheckMinimum() == true && B.Term != 0)
            {
                listSavingTimeDeposit.Add(B.AccountNumber, B);
                return true;
            }
            return false;
        }
        public static bool CreateLoans(string type, string accountnumber, Client A, decimal Debt, int months,
         List<TransactionHistory> histories, List<DebtHistory> listDebt,
          SortedList<string, Loans> listLoan)
        {
            string[] ListLoan = { "Unsecured Loan", "Mortage Loan", "Overdraft Loan", "Installment Loan" };
            if (type == ListLoan[0])
            {
                Loans UL = new UnsecuredLoan(type, accountnumber, A.ID, A, "", histories, listDebt,
                    Debt, months, A.Fico);
                listLoan.Add(type, UL);
            }
            if (type == ListLoan[2])
            {
                Loans L = new OverdraftLoan(type, accountnumber, A.ID, A, "", months, Debt, histories, listDebt, A.Income);
                listLoan.Add(type, L);
            }
            if (type == ListLoan[3])
            {
                Loans L = new InstallmentLoan(type, accountnumber, A.ID, A, "", months, Debt, histories, listDebt, A.Income);
                listLoan.Add(type, L);
            }
            return true;
        }
        public static bool CreateMortageLoan(Client A, string accountnumber, decimal Debt, int months, decimal totalasset,
         List<TransactionHistory> histories, List<DebtHistory> listDebt, SortedList<string, Loans> listLoan)
        {
            string[] ListLoan = { "Unsecured Loan", "Mortage Loan", "Overdraft Loan", "Installment Loan" };
            Loans L = new MortageLoan(ListLoan[1], "", A.ID, A, "", months, Debt, histories, listDebt, totalasset);
            listLoan.Add(ListLoan[1], L);
            return true;
        }
        public static string CreateTimeDescriptions(DateTime Time)
        {
            return Time.Year.ToString() + "/" + Time.Month.ToString() + "/" + Time.Day.ToString();
        }
        public static void CreateListAllCardListNewDebit(SortedList<string, Client> listClient, List<Function> AllCard, SortedList<string, DomesticDebitCard> listDebit)
        {
            foreach (var person in listClient)
            {
                var listDebitNewCard = from tmp in listDebit where tmp.Value.ID == person.Value.ID select tmp;

                foreach (var cardDebit in listDebitNewCard)
                {
                    string descriptions = cardDebit.Value.Pin + " "
                    + cardDebit.Value.CardBalance.ToString() + " " + CreateTimeDescriptions(cardDebit.Value.StartTime) + " " + CreateTimeDescriptions(cardDebit.Value.AnnualFeesYear) + " " + cardDebit.Value.StatusCard.ToString();//pin -> cardblance -> StartedDay -> AnnualFeesYear -> Status
                    Function tmp = new Function(cardDebit.Value.Type, cardDebit.Value.AccountNumber, cardDebit.Value.ID, person.Value, descriptions);
                    AllCard.Add(tmp);
                }
            }
        }
        public static void CreateAllCardListOldDebits(SortedList<string, Client> listClient, SortedList<string, DomesticDebitCard> listDebit, List<Function> AllCard, List<TransactionHistory> listHistory)
        {
            string[] ListCard =
           {
                "Napas Success", "Napas Success Plus"
            };
            var ListOldDebits = from card in AllCard
                                where card.Type == ListCard[0] || card.Type == ListCard[1]
                                select card;
            // phần debit
            foreach (var OldDebits in ListOldDebits)
            {
                string[] Properties = OldDebits.Description.Split(' ');
                CreateDebitCard(OldDebits.Type, OldDebits.AccountNumber, Properties[0], listClient[OldDebits.ID], listDebit, listHistory);
                listDebit[OldDebits.AccountNumber].CardBalance = decimal.Parse(Properties[1]);
                listDebit[OldDebits.AccountNumber].StartTime = Convert.ToDateTime(Properties[2]);
                listDebit[OldDebits.AccountNumber].AnnualFeesYear = Convert.ToDateTime(Properties[3]);
                listDebit[OldDebits.AccountNumber].StatusCard = bool.Parse(Properties[4]);
            }
        }
        public static void CreateListAllCardListNewCredit(SortedList<string, Client> listClient, List<Function> AllCard, SortedList<string, InternationalCreditCard> listCredit)
        {
            foreach (var person in listClient)
            {
                var listCreditNewCard = from tmp in listCredit where tmp.Value.ID == person.Value.ID select tmp;

                foreach (var cardCredit in listCreditNewCard)
                {
                    string descriptions = cardCredit.Value.Pin + " "
                    + cardCredit.Value.CardBalance.ToString() + " " + CreateTimeDescriptions(cardCredit.Value.StartTime) + " " + CreateTimeDescriptions(cardCredit.Value.AnnualFeesYear) + " " + cardCredit.Value.StatusCard.ToString();//pin -> cardblance -> StartedDay -> Status
                    Function tmp = new Function(cardCredit.Value.Type, cardCredit.Value.AccountNumber, cardCredit.Value.ID, person.Value, descriptions);
                    AllCard.Add(tmp);
                }
            }
        }
        public static void CreateListAllCardListNewSavingDemandDeposit(SortedList<string, Client> listClient, List<Function> AllCard, SortedList<string, DemandDeposit> listSavingDemandDeposit)
        {
            foreach (var person in listClient)
            {
                var listNewSavingDemandDeposit = from tmp in listSavingDemandDeposit where tmp.Value.ID == person.Value.ID select tmp;
                foreach (var item in listNewSavingDemandDeposit)
                {
                    string descriptions = item.Value.SavingDeposit.ToString() + " " + item.Value.Interest.ToString() + " " + item.Value.StartedDate.ToString();//số tiền gửi > tiề-n lãi tích lũy > StartedDay
                    Function tmp = new Function(item.Value.Type, item.Value.AccountNumber, item.Value.ID, person.Value, descriptions);
                    AllCard.Add(tmp);
                }
            }
        }
        public static void CreateListAllCardListNewSavingTimeDeposit(SortedList<string, Client> listClient, List<Function> AllCard, SortedList<string, TimeDeposit> listSavingTimeDeposit)
        {
            foreach (var person in listClient)
            {
                var listNewSavingTimeDeposit = from tmp in listSavingTimeDeposit where tmp.Value.ID == person.Value.ID select tmp;
                foreach (var item in listNewSavingTimeDeposit)
                {
                    string descriptions = item.Value.SavingDeposit.ToString() + " " + item.Value.Term.ToString() + " " + item.Value.TypeSaving.ToString() + " " + item.Value.Interest.ToString() + " " + item.Value.StartedDate.ToString();//số tiền gửi -> kỳ hạn -> loại chuyển tiền > tiề-n lãi tích lũy > StartedDay
                    Function tmp = new Function(item.Value.Type, item.Value.AccountNumber, item.Value.ID, person.Value, descriptions);
                    AllCard.Add(tmp);
                }
            }
        }
        public static void CreateListAllCardListNewLoans(SortedList<string, Client> listClient, List<Function> AllCard, SortedList<string, Loans> listLoan)
        {
            string[] ListLoan = { "Unsecured Loan", "Mortage Loan", "Overdraft Loan", "Installment Loan" };
            foreach (var person in listClient)
            {
                string descriptions = "";
                var listNewLoans = from tmp in listLoan where tmp.Value.ID == person.Value.ID select tmp;
                foreach (var item in listNewLoans)
                {
                    if (item.Value.Type == ListLoan[0])
                    {
                        descriptions = item.Value.Debt.ToString() + " " + item.Value.Months.ToString();  //Debt > Months
                    }
                    if (item.Value.Type == ListLoan[1])
                    {
                        descriptions = item.Value.Debt.ToString() + " " + item.Value.Months.ToString() + " " + item.Value.Limit.ToString(); //Debt + Month + totalasset
                    }
                    if (item.Value.Type == ListLoan[2] || item.Value.Type == ListLoan[3])
                    {
                        descriptions = item.Value.Debt.ToString() + " " + item.Value.Months.ToString();    //Debt -> Months
                    }
                    descriptions += " " + item.Value.DateTimeloan.ToString(); // += DateTimeloan
                    Function tmp = new Function(item.Value.Type, item.Value.AccountNumber, item.Value.ID, person.Value, descriptions);
                    AllCard.Add(tmp);
                }
            }
        }
        public static void CreateAllCardListOldCredits(SortedList<string, Client> listClient, SortedList<string, InternationalCreditCard> listCredit, List<Function> AllCard, List<TransactionHistory> listHistory, List<DebtHistory> listDebt)
        {
            string[] ListCard =
           {
                "VisaStandard", "VisaGold",
                "MastercardGold", "MastercardPlatinum", "JCBGold", "JCBUltimate",
            };
            var ListOldCredits = from card in AllCard
                                 where card.Type == ListCard[0] || card.Type == ListCard[1] || card.Type == ListCard[2]
                                 || card.Type == ListCard[3] || card.Type == ListCard[4] || card.Type == ListCard[5]
                                 select card;
            // phần Credit
            foreach (var OldCredits in ListOldCredits)
            {
                string[] Properties = OldCredits.Description.Split(' ');
                CreateCreditCard(OldCredits.Type, OldCredits.AccountNumber, Properties[0], listClient[OldCredits.ID], listCredit, listHistory, listDebt);
                listCredit[OldCredits.AccountNumber].CardBalance = decimal.Parse(Properties[1]);
                listCredit[OldCredits.AccountNumber].StartTime = Convert.ToDateTime(Properties[2]);
                listCredit[OldCredits.AccountNumber].AnnualFeesYear = Convert.ToDateTime(Properties[3]);
                listCredit[OldCredits.AccountNumber].StatusCard = bool.Parse(Properties[4]);
            }
        }
        public static void CreateAllCardListOldSavingDemandDeposit(SortedList<string, Client> listClient, SortedList<string, DemandDeposit> listSavingDemandDeposit, List<Function> AllCard, List<TransactionHistory> listHistory)
        {
            string[] ListSaving =
             {
                "Demand Deposit", "Time Deposit"
            };
            var ListOldSavingDemandDeposit = from saving in AllCard
                                             where saving.Type == ListSaving[0]
                                             select saving;
            // phần saving demand
            foreach (var OldSaving in ListOldSavingDemandDeposit)
            {
                string[] Properties = OldSaving.Description.Split(' ');
                CreateSavingAccountDemandDeposit(ListSaving[0], OldSaving.AccountNumber, decimal.Parse(Properties[0]), 0, listClient[OldSaving.ID], listSavingDemandDeposit, listHistory);
                listSavingDemandDeposit[OldSaving.AccountNumber].Interest = decimal.Parse(Properties[1]);
                listSavingDemandDeposit[OldSaving.AccountNumber].StartedDate = DateTime.Parse(Properties[2]);
            }
        }
        public static void CreateAllCardListOldSavingTimeDeposit(SortedList<string, Client> listClient, SortedList<string, TimeDeposit> listSavingTimeDeposit, List<Function> AllCard, List<TransactionHistory> listHistory)
        {
            string[] ListSaving =
             {
                "Demand Deposit", "Time Deposit"
            };
            var ListOldSavingTimeDeposit = from saving in AllCard
                                           where saving.Type == ListSaving[1]
                                           select saving;
            // phần saving demand
            foreach (var OldSaving in ListOldSavingTimeDeposit)
            {
                string[] Properties = OldSaving.Description.Split(' ');
                CreateSavingAccountTimeDeposit(ListSaving[1], OldSaving.AccountNumber, decimal.Parse(Properties[0]), int.Parse(Properties[1]), int.Parse(Properties[2]), listClient[OldSaving.ID], listSavingTimeDeposit, listHistory);
                listSavingTimeDeposit[OldSaving.AccountNumber].Interest = decimal.Parse(Properties[3]);
                listSavingTimeDeposit[OldSaving.AccountNumber].StartedDate = DateTime.Parse(Properties[4]);
            }
        }
        public static void CreateAllCardListOldLoans(SortedList<string, Client> listClient, SortedList<string, Loans> listLoan, List<Function> AllCard, List<TransactionHistory> listHistory, List<DebtHistory> listDebt)
        {
            string[] ListLoan = { "Unsecured Loan", "Mortage Loan", "Overdraft Loan", "Installment Loan" };

            var ListOldLoans = from card in AllCard
                               where card.Type == ListLoan[0] || card.Type == ListLoan[1] || card.Type == ListLoan[2]
                               || card.Type == ListLoan[3]
                               select card;
            // phần Credit
            foreach (var OldLoan in ListOldLoans)
            {
                string[] Properties = OldLoan.Description.Split(' ');
                if (OldLoan.Type == ListLoan[0])
                {
                    CreateLoans(OldLoan.Type, OldLoan.AccountNumber, listClient[OldLoan.ID], decimal.Parse(Properties[0]), int.Parse(Properties[1]), listHistory, listDebt, listLoan);
                    listLoan[OldLoan.Type].DateTimeloan = DateTime.Parse(Properties[2]);
                }
                if (OldLoan.Type == ListLoan[1])
                {
                    CreateMortageLoan(listClient[OldLoan.ID], OldLoan.AccountNumber, decimal.Parse(Properties[0]), int.Parse(Properties[1]), decimal.Parse(Properties[2]), listHistory, listDebt, listLoan);
                    listLoan[OldLoan.Type].DateTimeloan = DateTime.Parse(Properties[3]);
                }
                if (OldLoan.Type == ListLoan[2] || OldLoan.Type == ListLoan[3])
                {
                    CreateLoans(OldLoan.Type, OldLoan.AccountNumber, listClient[OldLoan.ID], decimal.Parse(Properties[0]), int.Parse(Properties[1]), listHistory, listDebt, listLoan);
                    listLoan[OldLoan.Type].DateTimeloan = DateTime.Parse(Properties[2]);
                }
            }
        }
        // code methods liên kết nhiều thẻ
        public static bool CreditPayment(DomesticDebitCard cardDebit, InternationalCreditCard cardCredit, decimal x, SortedList<string, DomesticDebitCard> listDebit, SortedList<string, InternationalCreditCard> listCredit)
        {
            return listDebit[cardDebit.AccountNumber].Payment(x, cardDebit.Pin, "Thanh toan no tin dung") == true && listCredit[cardCredit.AccountNumber].Payment(x);
        }
        public static bool SavingReceiveTimeDeposit(DomesticDebitCard cardDebit, TimeDeposit SavingTimeDesposit, SortedList<string, DomesticDebitCard> listDebit, SortedList<string, TimeDeposit> listSavingTimeDeposit)
        {
            decimal tmp = listSavingTimeDeposit[SavingTimeDesposit.AccountNumber].AutoUpdateSavingDeposit();
            if (tmp == -1)
            {
                return false;
            }
            else
            {
                listDebit[cardDebit.AccountNumber].ReceiveMoney(tmp, "Nhan tien tu so tiet kiem " + SavingTimeDesposit.AccountNumber);
                return true;
            }
        }
        public static bool TransfersATM(string pin, DomesticDebitCard Receiver, DomesticDebitCard Sender, decimal x, SortedList<string, DomesticDebitCard> listDebit)
        {
            var lists = from account in listDebit where account.Key == Receiver.AccountNumber select account;
            return listDebit[Receiver.AccountNumber].ReceiveMoney(x, "") && lists.Count() > 0 && listDebit[Sender.AccountNumber].TransfersATM(x, pin);  //vì chuyển khoản tại ATM nên ko có nội dung giao dịch
        }
        public static bool TransfersPOS(DomesticDebitCard Receiver, decimal x, string content, SortedList<string, DomesticDebitCard> listDebit)
        {
            var lists = from account in listDebit.Keys where account == Receiver.AccountNumber select account;
            if (lists.Count() > 0 && listDebit[Receiver.AccountNumber].TransfersPOS(x, content))
            {
                listDebit[Receiver.AccountNumber].ReceiveMoney(x, content);
                return true;
            }
            return false;
        }
        public static bool LoansPayment(DomesticDebitCard cardDebit, decimal x, Loans debts, SortedList<string, DomesticDebitCard> listDebit, SortedList<string, Loans> listLoan)
        {
            foreach (var item in listLoan)
            {
                if (item.Value.Type == debts.Type && listDebit[cardDebit.AccountNumber].Payment(x, cardDebit.Pin, $"Thanh toán khoản vay {debts.Type}"))
                {
                    listLoan[debts.Type].Paymentloans(x);
                    return true;
                }
            }
            return false;
        }
        public static bool LoansTransfer(Loans Loan, DomesticDebitCard cardDebit, decimal x, SortedList<string, DomesticDebitCard> listDebit)
        {
            if (Loan.Type != "Overdraft Loan")
            {
                return false;
            }
            var card = from tmp in listDebit where tmp.Key == cardDebit.AccountNumber select tmp;
            if (card.Count() > 0)
            {
                Loan.Transferloans(x);
                listDebit[cardDebit.AccountNumber].ReceiveMoney(x, "Vay thau chi");
                return true;
            }
            return false;
        }
        public static bool XuatLichSuGD(string accountnumber, DateTime start, DateTime end, List<TransactionHistory> listHistory, List<Function> AllCard)
        {
            var accountnumberallcard = from tmp in AllCard where tmp.AccountNumber == accountnumber select tmp;
            if (accountnumberallcard.Count() > 1)
            {
                return false;
            }
            Console.WriteLine("\nLICH SU GIAO DICH ");
            var historiesLists = from ls in listHistory
                                 where ls.AccountNumber == accountnumber && ls.DayTrading <= end && ls.DayTrading >= start
                                 select ls;
            foreach (TransactionHistory item in historiesLists)
            {
                item.XuatLichSuGD();
                Console.WriteLine();
            }
            return true;
        }
        public static bool XuatLichSuDebt(string accountnumber, DateTime start, DateTime end, List<DebtHistory> listDebt, List<Function> AllCard)
        {
            var accountnumberallcard = from tmp in AllCard where tmp.AccountNumber == accountnumber select tmp;
            if (accountnumberallcard.Count() > 1)
            {
                return false;
            }
            Console.WriteLine("\nLICH SU NO ");
            var historiesLists = from ls in listDebt
                                 where ls.AccountNumber == accountnumber && ls.SettlementDate <= end && ls.SettlementDate >= start
                                 select ls;
            foreach (var item in historiesLists)
            {
                item.XuatLichSuNo();
                Console.WriteLine();
            }
            return true;
        }
        // code menu
        private static bool MainMenu(SortedList<string, Client> listClient, List<TransactionHistory> listHistory,
SortedList<string, DomesticDebitCard> listDebit, SortedList<string, InternationalCreditCard> listCredit, SortedList<string, DemandDeposit> listSavingDemandDeposit, SortedList<string, TimeDeposit> listSavingTimeDeposit, SortedList<string, Loans> listLoan, List<DebtHistory> listDebt, List<Function> AllCard)
        {
            Console.Clear();
            Console.WriteLine("\n---Main Menu---");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Sign Up");
            Console.WriteLine("2) Sign In");
            Console.WriteLine("3) Forgot Password");
            Console.WriteLine("4) Exit");
            Console.Write("\r\nSelect an option: ");
            switch (Console.ReadLine())
            {
                case "0":
                    Console.Clear();
                    MainMenu(listClient, listHistory, listDebit, listCredit, listSavingDemandDeposit, listSavingTimeDeposit, listLoan, listDebt, AllCard);
                    return true;
                case "1":
                    SignUp(listClient);
                    return true;
                case "2":
                    SignIn(listClient, listHistory, listDebit, listCredit, listSavingDemandDeposit, listSavingTimeDeposit, listLoan, listDebt, AllCard);
                    return true;
                case "3":
                    ForgotPassword(listClient);
                    return true;
                case "4":
                    return false;
                default:
                    return true;
            }
        }
        private static bool SignUp(SortedList<string, Client> listClient)
        {
            bool Sex;
            string nameuser;
            string password;
            Console.WriteLine("\nEnter your personal information");
            Console.Write("Name: "); string name = Console.ReadLine();
            Console.Write("Address: "); string address = Console.ReadLine();
            Console.Write("Date of birth: "); string date = Console.ReadLine();
            string[] dateofbirth = date.Split('/');
            DateTime DateOfBirth = new DateTime(day: int.Parse(dateofbirth[0]), month: int.Parse(dateofbirth[1]), year: int.Parse(dateofbirth[0]));
            Console.Write("ID: "); string id = Console.ReadLine();
            Console.Write("Issued date: "); string issued = Console.ReadLine();
            string[] issueddate = issued.Split('/');
            DateTime IssuedDate = new DateTime(day: int.Parse(issueddate[0]), month: int.Parse(issueddate[1]), year: int.Parse(issueddate[0]));
            Console.Write("Postion: "); string position = Console.ReadLine();
            Console.Write("Nationality: "); string nationality = Console.ReadLine();
            Console.Write("Work Place: "); string workplace = Console.ReadLine();
            Console.Write("Sex: "); string sex = Console.ReadLine();
            if (sex == "nam") { Sex = true; }
            else { Sex = false; }
            Console.Write("Phone number: "); string phonenumber = Console.ReadLine();
            Console.Write("Income: "); decimal income = decimal.Parse(Console.ReadLine());
            Console.Write("Branch: "); string branch = Console.ReadLine();
            do
            {
                Console.Write("Password: "); password = Console.ReadLine();
            } while (password.Length < 6);
            if (CreateClient(name, address, DateOfBirth, id, position, nationality, workplace, Sex, phonenumber,
                        IssuedDate, income, branch, password, listClient))
            {
                Console.WriteLine("Tao tai khoan thanh cong!");
            }
            return true;
        }
        private static bool SignIn(SortedList<string, Client> listClient, List<TransactionHistory> listHistory, SortedList<string, DomesticDebitCard> listDebit, SortedList<string, InternationalCreditCard> listCredit, SortedList<string, DemandDeposit> listSavingDemandDeposit, SortedList<string, TimeDeposit> listSavingTimeDeposit, SortedList<string, Loans> listLoan, List<DebtHistory> listDebt, List<Function> AllCard)
        {
            string nameuser;
            string password;
            do
            {
                Console.Write("ID: "); nameuser = Console.ReadLine();
                Console.Write("Password: "); password = Console.ReadLine();
            } while (password.Length < 6);
            var user = from tmp in listClient.Keys where tmp == nameuser select tmp;
            if (user.Count() > 0 && password == listClient[nameuser].Password && listClient[nameuser].StatusClient == true)
            {
                Console.WriteLine("Logged in successfully!");
                ClientMenu(listClient[nameuser], listClient, listHistory, listDebit, listCredit, listSavingDemandDeposit, listSavingTimeDeposit, listLoan, listDebt, AllCard);
            }
            return true;
        }
        private static bool ForgotPassword(SortedList<string, Client> listClient)
        {
            Console.Write("ID: ");
            string id = Console.ReadLine();
            Console.Write("Phone number:");
            string phonenumber = Console.ReadLine();
            var account = from tmp in listClient.Values where tmp.ID == id && tmp.PhoneNumber == phonenumber select tmp;
            if (account.Count() > 0)
            {
                Console.Write("New password: ");
                string newpassword = Console.ReadLine();
                listClient[id].ChangePassword(listClient[id].Password, newpassword);
                Console.WriteLine("Change password successfully");
                return true;
            }
            return false;
        }
        private static bool ClientMenu(Client A, SortedList<string, Client> listClient, List<TransactionHistory> listHistory, SortedList<string, DomesticDebitCard> listDebit, SortedList<string, InternationalCreditCard> listCredit, SortedList<string, DemandDeposit> listSavingDemandDeposit, SortedList<string, TimeDeposit> listSavingTimeDeposit, SortedList<string, Loans> listLoan, List<DebtHistory> listDebt, List<Function> AllCard)
        {
            // thiếu hàm update fico ?????
            var listDebitA = from cdd in listDebit where cdd.Value.ID == A.ID select cdd;
            foreach (var item in listDebitA)
            {
                item.Value.ResetLimit();
                item.Value.MonthlyInterest();
            }
            Console.WriteLine("\n---Client Menu---");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Debit Option");
            Console.WriteLine("2) Credit Option");
            Console.WriteLine("3) Saving Option");
            Console.WriteLine("4) Loans Option");
            Console.WriteLine("5) Change Password");
            Console.WriteLine("6) Export Information");
            Console.WriteLine("7) Lock Account");
            Console.WriteLine("8) Log Out");
            Console.Write("\r\nSelect an option: ");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    MenuOptionDebit(A, listClient, listHistory, listDebit, listCredit, listLoan, AllCard);
                    return true;
                case "2":
                    Console.Clear();
                    MenuOptionCredit(A, listClient, listHistory, listCredit, AllCard, listDebt);
                    return true;
                case "3":
                    Console.Clear();
                    MenuOptionSaving(A, listHistory, listSavingDemandDeposit, listSavingTimeDeposit, AllCard);
                    return true;
                case "4":
                    Console.Clear();
                    MenuOptionLoan(A, listClient, listHistory, listDebt, listDebit, listLoan, AllCard);
                    return true;
                case "5":
                    Console.Clear();
                    MenuChangePassword(A);
                    return true;
                case "6":
                    Console.Clear();
                    A.ExportInformation();
                    ClientMenu(A, listClient, listHistory, listDebit, listCredit, listSavingDemandDeposit, listSavingTimeDeposit,
                    listLoan, listDebt, AllCard);
                    return true;
                case "7":
                    Console.Clear();
                    A.LockClient();
                    return true;
                case "8":
                    return false;
                default:
                    Console.Clear();
                    ClientMenu(A, listClient, listHistory, listDebit, listCredit, listSavingDemandDeposit, listSavingTimeDeposit, listLoan, listDebt, AllCard);
                    return true;
            }
        }
        private static bool MenuChangePassword(Client A)
        {
            Console.Write("Old Password: "); string OldPassword = Console.ReadLine();
            Console.Write("New Password: "); string NewPassWord = Console.ReadLine();
            if (A.ChangePassword(OldPassword, NewPassWord))
            {
                Console.WriteLine("Change password successfully");
                return true;
            }
            else
            {
                Console.WriteLine("You have failed to change your password");
                return false;
            }
        }
        private static bool MenuExporttransactionhistory(Client A, List<Function> AllCard, List<TransactionHistory> listHistory)
        {
            Console.Write("Account Number: "); string accountnumber = Console.ReadLine();
            var accountnumberallcard = from tmp in AllCard where tmp.AccountNumber == accountnumber && tmp.ID == A.ID select tmp;
            if (accountnumberallcard.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.WriteLine("Start Time: "); string datestart = Console.ReadLine();
            string[] dates = datestart.Split('/');
            DateTime tmp1 = new DateTime(day: int.Parse(dates[0]), month: int.Parse(dates[1]), year: int.Parse(dates[2]));
            Console.WriteLine("End Time: "); string dateend = Console.ReadLine();
            string[] datess = dateend.Split('/');
            DateTime tmp2 = new DateTime(day: int.Parse(datess[0]), month: int.Parse(datess[1]), year: int.Parse(datess[2]));
            XuatLichSuGD(accountnumber, tmp1, tmp2, listHistory, AllCard);
            return true;
        }
        private static bool MenuExportDebthistory(Client A, List<Function> AllCard, List<DebtHistory> listDebt)
        {
            Console.Write("Account Number: "); string accountnumber = Console.ReadLine();
            var accountnumberallcard = from tmp in AllCard where tmp.AccountNumber == accountnumber && tmp.ID == A.ID select tmp;
            if (accountnumberallcard.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.WriteLine("Start Time: "); string datestart = Console.ReadLine();
            string[] dates = datestart.Split('/');
            DateTime tmp1 = new DateTime(day: int.Parse(dates[0]), month: int.Parse(dates[1]), year: int.Parse(dates[2]));
            Console.WriteLine("End Time: "); string dateend = Console.ReadLine();
            string[] datess = dateend.Split('/');
            DateTime tmp2 = new DateTime(day: int.Parse(datess[0]), month: int.Parse(datess[1]), year: int.Parse(datess[2]));
            XuatLichSuDebt(accountnumber, tmp1, tmp2, listDebt, AllCard);
            return true;
        }
        private static bool MenuOptionDebit(Client A, SortedList<string, Client> listClient, List<TransactionHistory> listHistory, SortedList<string, DomesticDebitCard> listDebit,
        SortedList<string, InternationalCreditCard> listCredit, SortedList<string, Loans> listLoan, List<Function> AllCard)
        {
            string[] ListCard =
            {
                "Napas Success", "Napas Success Plus"
            };
            var listcard = from tmp in listDebit where tmp.Value.ID == A.ID && tmp.Value.Type == ListCard[0] || (tmp.Value.Type == ListCard[1]) select tmp;
            foreach (var item in listcard)
            {
                item.Value.ResetLimit();
                item.Value.MonthlyInterest();
                item.Value.AutomaticDeductionOfAnnualFee();
            }
            Console.WriteLine("\n---Debit Menu---");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Create Card Debit");
            Console.WriteLine("2) AccountRecharge");
            Console.WriteLine("3) WithdrawATM");
            Console.WriteLine("4) WithdrawPOS");
            Console.WriteLine("5) TransfersATM");
            Console.WriteLine("6) TransfersPOS");
            Console.WriteLine("7) ForeignCurrencyTrading");
            Console.WriteLine("8) Export card information");
            Console.WriteLine("9) Export transaction history");
            Console.WriteLine("10) Payment Credit");
            Console.WriteLine("11) Payment Loan");
            Console.WriteLine("12) Lock Card");
            Console.Write("\r\nSelect an option: ");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    MenuCreateDebitCard(A, listClient, listHistory, listDebit, AllCard);
                    WriteCsvFileDatabaseHistory(listHistory);
                    MenuOptionDebit(A, listClient, listHistory, listDebit, listCredit, listLoan, AllCard);
                    return true;
                case "2":
                    Console.Clear();
                    MenuAccountRechargeDebit(listDebit);
                    WriteCsvFileDatabaseHistory(listHistory);
                    MenuOptionDebit(A, listClient, listHistory, listDebit, listCredit, listLoan, AllCard);
                    return true;
                case "3":
                    Console.Clear();
                    MenuWithDrawATMDebit(A, listDebit);
                    WriteCsvFileDatabaseHistory(listHistory);
                    MenuOptionDebit(A, listClient, listHistory, listDebit, listCredit, listLoan, AllCard);
                    return true;
                case "4":
                    Console.Clear();
                    MenuWithDrawPOSDebit(A, listDebit);
                    WriteCsvFileDatabaseHistory(listHistory);
                    MenuOptionDebit(A, listClient, listHistory, listDebit, listCredit, listLoan, AllCard);
                    return true;
                case "5":
                    Console.Clear();
                    MenuTransfersATMDebit(A, listDebit);
                    WriteCsvFileDatabaseHistory(listHistory);
                    MenuOptionDebit(A, listClient, listHistory, listDebit, listCredit, listLoan, AllCard);
                    return true;
                case "6":
                    Console.Clear();
                    MenuTransfersPOSDebit(A, listDebit);
                    WriteCsvFileDatabaseHistory(listHistory);
                    MenuOptionDebit(A, listClient, listHistory, listDebit, listCredit, listLoan, AllCard);
                    return true;
                case "7":
                    Console.Clear();
                    MenuForeignCurrencyTradingDebit(A, listDebit);
                    WriteCsvFileDatabaseHistory(listHistory);
                    MenuOptionDebit(A, listClient, listHistory, listDebit, listCredit, listLoan, AllCard);
                    return true;
                case "8":
                    Console.Clear();
                    MenuExportcardinformationDebit(A, listDebit);
                    MenuOptionDebit(A, listClient, listHistory, listDebit, listCredit, listLoan, AllCard);
                    return true;
                case "9":
                    Console.Clear();
                    MenuExporttransactionhistory(A, AllCard, listHistory);
                    MenuOptionDebit(A, listClient, listHistory, listDebit, listCredit, listLoan, AllCard);
                    return true;
                case "10":
                    MenuPaymentCreditDebit(A, listCredit, listDebit);
                    WriteCsvFileDatabaseHistory(listHistory);
                    MenuOptionDebit(A, listClient, listHistory, listDebit, listCredit, listLoan, AllCard);
                    return true;
                case "11":
                    MenuPaymentLoanDebit(listDebit, listLoan, AllCard);
                    WriteCsvFileDatabaseHistory(listHistory);
                    MenuOptionDebit(A, listClient, listHistory, listDebit, listCredit, listLoan, AllCard);
                    return true;
                case "12":
                    Console.Clear();
                    MenuLockCardDebit(A, listDebit);
                    MenuOptionDebit(A, listClient, listHistory, listDebit, listCredit, listLoan, AllCard);
                    return true;
                default:
                    Console.Clear();
                    MenuOptionDebit(A, listClient, listHistory, listDebit, listCredit, listLoan, AllCard);
                    return true;
            }
        }
        private static bool MenuCreateDebitCard(Client A, SortedList<string, Client> listClient, List<TransactionHistory> listHistory, SortedList<string, DomesticDebitCard> listDebit, List<Function> AllCard)
        {
            string[] ListCard =
            {
                "Napas Success", "Napas Success Plus"
            };
            string card = "";
            Console.WriteLine("1) Napas Success\n2) Napas Success Plus");
            Console.Write("Your choose: ");
            switch (Console.ReadLine())
            {
                case "1":
                    card = ListCard[0];
                    break;
                case "2":
                    card = ListCard[1];
                    break;
                default:
                    MenuCreateDebitCard(A, listClient, listHistory, listDebit, AllCard);
                    return false;
            }
            Console.Write("Account Number: "); string accountnumber = Console.ReadLine();
            var tmp = from cdd in AllCard where cdd.AccountNumber == accountnumber || (cdd.Type == card && cdd.ID == A.ID) select cdd;
            if (tmp.Count() >= 1)
            {
                Console.WriteLine("Account number already exists");
                return false;
            }
            Console.Write("Pin: "); string pin = Console.ReadLine();
            if (CreateDebitCard(card, accountnumber, pin, A, listDebit, listHistory))
            {
                Console.WriteLine("Create Successfully");
                CreateListAllCardListNewDebit(listClient, AllCard, listDebit);
                WriteCsvFileDatabaseAllCard(AllCard);
                return true;
            }
            Console.WriteLine("Failed");
            return false;
        }
        private static bool MenuAccountRechargeDebit(SortedList<string, DomesticDebitCard> listDebit)
        {
            Console.Write("Account number: "); string accountnumber = Console.ReadLine();
            var account = from tmp in listDebit where tmp.Key == accountnumber select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Amount you want to deposit: "); decimal x = decimal.Parse(Console.ReadLine());
            if (listDebit[accountnumber].AccountRecharge(x, "Nap tien"))
            {
                Console.WriteLine("Transtation successfully");
                return true;
            }
            Console.WriteLine("Fault");
            return false;
        }
        private static bool MenuWithDrawPOSDebit(Client A, SortedList<string, DomesticDebitCard> listDebit)
        {
            Console.Write("Account number: "); string accountnumber = Console.ReadLine();
            var account = from tmp in listDebit where tmp.Key == accountnumber && tmp.Value.ID == A.ID select tmp;

            if (account.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Amount you want to withdraw: "); decimal x = decimal.Parse(Console.ReadLine());
            if (!listDebit[accountnumber].WithdrawPOS(x))
            {
                Console.WriteLine("Fault");
                return false;
            }
            Console.WriteLine("Transation successfully");
            return true;
        }
        private static bool MenuWithDrawATMDebit(Client A, SortedList<string, DomesticDebitCard> listDebit)
        {
            Console.Write("Account number: "); string accountnumber = Console.ReadLine();
            var account = from tmp in listDebit where tmp.Key == accountnumber && tmp.Value.ID == A.ID select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Pin: "); string pin = Console.ReadLine();
            Console.Write("Amount you want to withdraw: "); decimal x = decimal.Parse(Console.ReadLine());
            if (!listDebit[accountnumber].WithdrawATM(x, pin))
            {
                Console.WriteLine("Fault");
                return false;
            }
            Console.WriteLine("Transation successfully");
            return true;
        }
        private static bool MenuTransfersATMDebit(Client A, SortedList<string, DomesticDebitCard> listDebit)
        {
            Console.Write("Your account number: "); string sender = Console.ReadLine();
            var account = from tmp in listDebit where tmp.Key == sender && tmp.Value.ID == A.ID select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Recipient account number: "); string receiver = Console.ReadLine();
            var tmp1 = from tmp in listDebit where tmp.Key == receiver select tmp;
            if (tmp1.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Amount you want to send: "); decimal x = decimal.Parse(Console.ReadLine());
            Console.Write("Pin: "); string pin = Console.ReadLine();
            if (TransfersATM(pin, listDebit[receiver], listDebit[sender], x, listDebit))
            {
                Console.WriteLine("Transation successfully");
                return true;
            }
            Console.WriteLine("Fault");
            return false;
        }
        private static bool MenuTransfersPOSDebit(Client A, SortedList<string, DomesticDebitCard> listDebit)
        {
            Console.Write("Recipient account number: "); string receiver = Console.ReadLine();
            var tmp1 = from tmp in listDebit where tmp.Key == receiver && tmp.Value.ID == A.ID select tmp;
            if (tmp1.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Amount you want to send: "); decimal x = decimal.Parse(Console.ReadLine());
            Console.Write("Content: "); string content = Console.ReadLine();
            if (TransfersPOS(listDebit[receiver], x, content, listDebit))
            {
                Console.WriteLine("Transation successfully");
                return true;
            }
            return false;
        }
        private static bool MenuForeignCurrencyTradingDebit(Client A, SortedList<string, DomesticDebitCard> listDebit)
        {
            string[] ListTypeCurrency =
            {
                "VND","AUD","CAD","CHF","EUR","GBP","HKD","JPY","SGD","USD"
            };
            Console.Write("Account number: "); string accountnumber = Console.ReadLine();
            var account = from tmp in listDebit where tmp.Key == accountnumber && tmp.Value.ID == A.ID select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.WriteLine("1) VND");
            Console.WriteLine("2) AUD");
            Console.WriteLine("3) CAD");
            Console.WriteLine("4) CHF");
            Console.WriteLine("5) EUR");
            Console.WriteLine("6) GBP");
            Console.WriteLine("7) HKD");
            Console.WriteLine("8) JPY");
            Console.WriteLine("9) SGB");
            Console.WriteLine("10) USD");
            int choose;
            do
            {
                Console.Write("Your choose: "); choose = int.Parse(Console.ReadLine());
            }
            while (choose > 10 || choose < 1);
            Console.Write("Amount you want to convert to foreign currency: "); decimal x = decimal.Parse(Console.ReadLine());
            if (listDebit[accountnumber].ForeignCurrencyTrading(x, ListTypeCurrency[choose - 1]))
            {
                Console.WriteLine("Transation successfully");
                return true;
            }
            Console.WriteLine("Fault");
            return false;
        }
        private static bool MenuExportcardinformationDebit(Client A, SortedList<string, DomesticDebitCard> listDebit)
        {
            Console.Write("Account number: "); string accountnumber = Console.ReadLine();
            var account = from tmp in listDebit where tmp.Key == accountnumber && tmp.Value.ID == A.ID select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            listDebit[accountnumber].XuatThongTinThe();
            return true;
        }
        private static bool MenuLockCardDebit(Client A, SortedList<string, DomesticDebitCard> listDebit)
        {
            Console.Write("Account Number: "); string accountnumber = Console.ReadLine();
            var acn = from cdd in listDebit where cdd.Key == accountnumber && cdd.Value.ID == A.ID select cdd;
            if (acn.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            listDebit[accountnumber].LockCard();
            return true;
        }
        private static bool MenuPaymentLoanDebit(SortedList<string, DomesticDebitCard> listDebit, SortedList<string, Loans> listLoan, List<Function> AllCard)
        {
            Console.Write("Account number Loan: "); string accountnumber = Console.ReadLine();
            var accountnumberallcard = from tmp in AllCard where tmp.AccountNumber == accountnumber select tmp;
            if (accountnumberallcard.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Account number Debit: "); string accountnumberdebit = Console.ReadLine();
            var account = from tmp in listDebit where tmp.Key == accountnumberdebit select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Amount you want to pay: "); decimal x = decimal.Parse(Console.ReadLine());
            if (LoansPayment(listDebit[accountnumberdebit], x, listLoan[accountnumber], listDebit, listLoan))
            {
                Console.WriteLine("Pay successfully");
                return true;
            }
            return false;
        }
        private static bool MenuOptionCredit(Client A, SortedList<string, Client> listClient, List<TransactionHistory> listHistory, SortedList<string, InternationalCreditCard> listCredit, List<Function> AllCard, List<DebtHistory> listDebt)
        {
            // lúc nào cũng gọi
            string[] ListCard =
            {
                "VisaStandard", "VisaGold",
                "MastercardGold", "MastercardPlatinum", "JCBGold", "JCBUltimate"
            };
            var listCardCredit = from tmp in listCredit
                                 where tmp.Value.ID == A.ID && (tmp.Value.Type == ListCard[0] || tmp.Value.Type == ListCard[1] || tmp.Value.Type == ListCard[2] || tmp.Value.Type == ListCard[3] || tmp.Value.Type == ListCard[4] || tmp.Value.Type == ListCard[5])
                                 select tmp;
            foreach (var item in listCardCredit)
            {
                item.Value.UpdateEveryMonth();
                item.Value.AutomaticDeductionOfAnnualFee();
                item.Value.CheckBadDebit();
                item.Value.CheckDebitToLockCard();
                item.Value.CalInterest();
                item.Value.CalMiniumPayment();
            }
            Console.WriteLine("\n---Credit Menu---");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Create Card Credit");
            Console.WriteLine("2) WithdrawATM");
            Console.WriteLine("3) WithdrawPOS");
            Console.WriteLine("4) ForeignCurrencyTrading");
            Console.WriteLine("5) ChangeLimitCredit");
            Console.WriteLine("6) Export card information");
            Console.WriteLine("7) Export transaction history");
            Console.WriteLine("8) Export debt history");
            Console.WriteLine("9) Lock Card");
            Console.Write("\r\nSelect an option: ");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    MenuCreateCreditCard(A, listClient, listHistory, listCredit, AllCard, listDebt);
                    MenuOptionCredit(A, listClient, listHistory, listCredit, AllCard, listDebt);
                    return true;
                case "2":
                    Console.Clear();
                    MenuWithDrawATMCredit(A, listCredit);
                    WriteCsvFileDatabaseHistory(listHistory);
                    MenuOptionCredit(A, listClient, listHistory, listCredit, AllCard, listDebt);
                    return true;
                case "3":
                    Console.Clear();
                    MenuWithDrawPOSCredit(A, listCredit);
                    WriteCsvFileDatabaseHistory(listHistory);
                    MenuOptionCredit(A, listClient, listHistory, listCredit, AllCard, listDebt);
                    return true;
                case "4":
                    Console.Clear();
                    MenuForeignCurrencyTradingCredit(A, listCredit);
                    WriteCsvFileDatabaseHistory(listHistory);
                    MenuOptionCredit(A, listClient, listHistory, listCredit, AllCard, listDebt);
                    return true;
                case "5":
                    Console.Clear();
                    MenuChangeLimitCredit(A, listCredit);
                    MenuOptionCredit(A, listClient, listHistory, listCredit, AllCard, listDebt);
                    return true;
                case "6":
                    Console.Clear();
                    MenuExportcardinformationCredit(A, listCredit);
                    MenuOptionCredit(A, listClient, listHistory, listCredit, AllCard, listDebt);
                    return true;
                case "7":
                    Console.Clear();
                    MenuExporttransactionhistory(A, AllCard, listHistory);
                    MenuOptionCredit(A, listClient, listHistory, listCredit, AllCard, listDebt);
                    return true;
                case "8":
                    Console.Clear();
                    MenuExportDebthistory(A, AllCard, listDebt);
                    MenuOptionCredit(A, listClient, listHistory, listCredit, AllCard, listDebt);
                    return true;
                case "9":
                    Console.Clear();
                    MenuLockCardCredit(A, listCredit);
                    MenuOptionCredit(A, listClient, listHistory, listCredit, AllCard, listDebt);
                    return true;
                default:
                    Console.Clear();
                    MenuOptionCredit(A, listClient, listHistory, listCredit, AllCard, listDebt);
                    return true;
            }
        }
        private static bool MenuCreateCreditCard(Client A, SortedList<string, Client> listClient, List<TransactionHistory> listHistory, SortedList<string, InternationalCreditCard> listCredit, List<Function> AllCard, List<DebtHistory> listDebt)
        {
            string[] ListCard =
            {
                "VisaStandard", "VisaGold",
                "MastercardGold", "MastercardPlatinum", "JCBGold", "JCBUltimate"
            };
            string card = "";
            Console.WriteLine("1) VisaStandard\n2) VisaGold\n3) MastercardGold\n4) MastercardPlatinum\n5) JCBGold\n6) JCBUltimate");
            Console.Write("Your choose: ");
            switch (Console.ReadLine())
            {
                case "1":
                    card = ListCard[0];
                    break;
                case "2":
                    card = ListCard[1];
                    break;
                case "3":
                    card = ListCard[2];
                    break;
                case "4":
                    card = ListCard[3];
                    break;
                case "5":
                    card = ListCard[4];
                    break;
                case "6":
                    card = ListCard[5];
                    break;
                default:
                    MenuCreateCreditCard(A, listClient, listHistory, listCredit, AllCard, listDebt);
                    return false;
            }
            Console.Write("Account Number: "); string accountnumber = Console.ReadLine();
            var tmp = from cdd in AllCard where cdd.AccountNumber == accountnumber || (cdd.Type == card && cdd.ID == A.ID) select cdd;
            if (tmp.Count() >= 1)
            {
                Console.WriteLine("Account number already exists");
                return false;
            }
            Console.Write("Pin: "); string pin = Console.ReadLine();
            if (CreateCreditCard(card, accountnumber, pin, A, listCredit, listHistory, listDebt))
            {
                Console.WriteLine("Create Successfully");
                CreateListAllCardListNewCredit(listClient, AllCard, listCredit);
                WriteCsvFileDatabaseAllCard(AllCard);
                return true;
            }
            Console.WriteLine("Failed");
            return false;
        }
        private static bool MenuWithDrawATMCredit(Client A, SortedList<string, InternationalCreditCard> listCredit)
        {
            Console.Write("Account number: "); string accountnumber = Console.ReadLine();
            var account = from tmp in listCredit where tmp.Key == accountnumber && tmp.Value.ID == A.ID select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Pin: "); string pin = Console.ReadLine();
            Console.Write("Amount you want to withdraw: "); decimal x = decimal.Parse(Console.ReadLine());
            if (!listCredit[accountnumber].WithdrawATM(x, pin))
            {
                Console.WriteLine("Fault");
                return false;
            }
            Console.WriteLine("Transation successfully");
            return true;
        }
        private static bool MenuWithDrawPOSCredit(Client A, SortedList<string, InternationalCreditCard> listCredit)
        {
            Console.Write("Account number: "); string accountnumber = Console.ReadLine();
            var account = from tmp in listCredit where tmp.Key == accountnumber && tmp.Value.ID == A.ID select tmp;

            if (account.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Amount you want to withdraw: "); decimal x = decimal.Parse(Console.ReadLine());
            if (!listCredit[accountnumber].WithdrawPOS(x))
            {
                Console.WriteLine("Fault");
                return false;
            }
            Console.WriteLine("Transation successfully");
            return true;
        }
        private static bool MenuForeignCurrencyTradingCredit(Client A, SortedList<string, InternationalCreditCard> listCredit)
        {
            string[] ListTypeCurrency =
            {
                "VND","AUD","CAD","CHF","EUR","GBP","HKD","JPY","SGD","USD"
            };
            Console.Write("Account number: "); string accountnumber = Console.ReadLine();
            var account = from tmp in listCredit where tmp.Key == accountnumber && tmp.Value.ID == A.ID select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.WriteLine("1) VND");
            Console.WriteLine("2) AUD");
            Console.WriteLine("3) CAD");
            Console.WriteLine("4) CHF");
            Console.WriteLine("5) EUR");
            Console.WriteLine("6) GBP");
            Console.WriteLine("7) HKD");
            Console.WriteLine("8) JPY");
            Console.WriteLine("9) SGB");
            Console.WriteLine("10) USD");
            int choose;
            do
            {
                Console.Write("Your choose: "); choose = int.Parse(Console.ReadLine());
            }
            while (choose > 10 || choose < 1);
            Console.Write("Amount you want to convert to foreign currency: "); decimal x = decimal.Parse(Console.ReadLine());
            if (listCredit[accountnumber].ForeignCurrencyTrading(x, ListTypeCurrency[choose - 1]))
            {
                Console.WriteLine("Transation successfully");
                return true;
            }
            Console.WriteLine("Fault");
            return false;
        }
        private static bool MenuExportcardinformationCredit(Client A, SortedList<string, InternationalCreditCard> listCredit)
        {
            Console.Write("Account number: "); string accountnumber = Console.ReadLine();
            var account = from tmp in listCredit where tmp.Key == accountnumber && tmp.Value.ID == A.ID select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            listCredit[accountnumber].XuatThongTinThe();
            return true;
        }
        private static bool MenuLockCardCredit(Client A, SortedList<string, InternationalCreditCard> listCredit)
        {
            Console.Write("Account Number: "); string accountnumber = Console.ReadLine();
            var acn = from cdd in listCredit where cdd.Key == accountnumber && cdd.Value.ID == A.ID select cdd;
            if (acn.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            listCredit[accountnumber].LockCard();
            Console.WriteLine(listCredit[accountnumber].StatusCard);
            return true;
        }
        private static bool MenuChangeLimitCredit(Client A, SortedList<string, InternationalCreditCard> listCredit)
        {
            Console.Write("Account number: "); string accountnumber = Console.ReadLine();
            var account = from tmp in listCredit where tmp.Key == accountnumber && tmp.Value.ID == A.ID select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("New InCome: "); decimal newIncome = decimal.Parse(Console.ReadLine());
            Console.Write("New CreditLimit: "); decimal newCreditLimit = decimal.Parse(Console.ReadLine());
            if (listCredit[accountnumber].ChangeLimitCredit(newCreditLimit, newIncome))
            {
                Console.WriteLine("Change Credit Limit successful");
                return true;
            }
            Console.WriteLine("Not enough condition to change Credit Limit successful");
            return false;
        }
        private static bool MenuPaymentCreditDebit(Client A, SortedList<string, InternationalCreditCard> listCredit, SortedList<string, DomesticDebitCard> listDebit)
        {
            Console.Write("Your account number: "); string accountnumberdebit = Console.ReadLine();
            var account1 = from tmp in listDebit where tmp.Key == accountnumberdebit select tmp;
            if (account1.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Acount number Credit: "); string accountnumbercredit = Console.ReadLine();
            var account2 = from tmp in listCredit where tmp.Key == accountnumbercredit select tmp;
            if (account2.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Amount you want to pay: "); decimal x = decimal.Parse(Console.ReadLine());
            Console.Write("Pin Debit: "); string pin = Console.ReadLine();
            if (CreditPayment(listDebit[accountnumberdebit], listCredit[accountnumbercredit], x, listDebit, listCredit))
            {
                Console.WriteLine("Payment successfully");
                return true;
            }
            Console.WriteLine("Fault");
            return false;
        }
        private static bool MenuOptionSaving(Client A, List<TransactionHistory> listHistory, SortedList<string, DemandDeposit> listSavingDemandDeposit, SortedList<string, TimeDeposit> listSavingTimeDeposit, List<Function> AllCard)
        {
            // lúc nào cũng gọi
            string[] ListSaving =
{
                "Demand Deposit", "Time Deposit"
            };

            Console.WriteLine("\n---Saving Menu---");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Create Card Saving");
            Console.WriteLine("2) SavingDemandDeposit Option ");
            Console.WriteLine("3) SavingTimeDeposit Option ");
            Console.Write("\r\nSelect an option: ");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    MenuCreateSaving(A, listHistory, listSavingDemandDeposit, listSavingTimeDeposit);
                    MenuOptionSaving(A, listHistory, listSavingDemandDeposit, listSavingTimeDeposit, AllCard);
                    return true;
                case "2":
                    Console.Clear();
                    MenuOptionSavingDemandDeposit(A, listHistory, listSavingDemandDeposit, AllCard);
                    return true;
                case "3":
                    Console.Clear();
                    MenuOptionSavingTimeDeposit(A, listHistory, listSavingTimeDeposit, AllCard);
                    return true;
                default:
                    return true;
            }
        }
        private static bool MenuCreateSaving(Client A, List<TransactionHistory> listHistory, SortedList<string,
             DemandDeposit> listSavingDemandDeposit, SortedList<string, TimeDeposit> listSavingTimeDeposit)
        {
            string[] ListSaving =
            {
                "Demand Deposit", "Time Deposit"
            };
            string saving = "";
            Console.WriteLine("1) Demand Deposit\n2) Time Deposit");
            Console.Write("Your choose: ");
            switch (Console.ReadLine())
            {
                case "1":
                    saving = ListSaving[0];
                    break;
                case "2":
                    saving = ListSaving[1];
                    break;
            }
            Console.Write("Account Number: "); string accountnumber = Console.ReadLine();
            var acn1 = from cdd in listSavingDemandDeposit where cdd.Key == accountnumber select cdd;
            var acn2 = from cdd in listSavingTimeDeposit where cdd.Key == accountnumber select cdd;
            if (acn1.Count() + acn2.Count() > 0)
            {
                Console.WriteLine("Account number already exists");
                return false;
            }
            Console.Write("Term: "); int term = int.Parse(Console.ReadLine());
            Console.Write("Saving Deposit: "); decimal savingdeposit = decimal.Parse(Console.ReadLine());
            if (saving == ListSaving[0])
            {
                CreateSavingAccountDemandDeposit(saving, accountnumber, savingdeposit, term, A, listSavingDemandDeposit, listHistory);
                listSavingDemandDeposit[accountnumber].Init();
            }
            else    //ListSaving[1]
            {
                Console.WriteLine("Type Saving:");
                Console.WriteLine("1) Tu dong tat ton khi toi han");
                Console.WriteLine("2) Tu dong gia han goc va lai");
                Console.WriteLine("3) Tu dong gia han goc");
                Console.Write("Your choose: ");
                int typeSaving = 0;
                switch (Console.ReadLine())
                {
                    case "1":
                        typeSaving = 1;
                        break;
                    case "2":
                        typeSaving = 2;
                        break;
                    case "3":
                        typeSaving = 3;
                        break;
                }
                CreateSavingAccountTimeDeposit(saving, accountnumber, savingdeposit, term, typeSaving, A, listSavingTimeDeposit, listHistory);
                listSavingDemandDeposit[accountnumber].Init();
            }
            return true;
        }

        private static bool MenuOptionSavingDemandDeposit(Client A, List<TransactionHistory> listHistory, SortedList<string, DemandDeposit> listSavingDemandDeposit, List<Function> AllCard)
        {
            // luc nao cũng chạy UpdateDepositLimit
            Console.WriteLine("\n---SavingDemandDeposit Menu---");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) WithDrawPos");
            Console.WriteLine("2) RechargeMoney");
            Console.WriteLine("3) Export Saving information");
            Console.WriteLine("4) Export Saving history");

            Console.Write("\r\nSelect an option: ");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    MenuWithDrawPOSSavingDemandDeposit(listSavingDemandDeposit);
                    WriteCsvFileDatabaseHistory(listHistory);
                    MenuOptionSavingDemandDeposit(A, listHistory, listSavingDemandDeposit, AllCard);
                    return true;
                case "2":
                    Console.Clear();
                    MenuAccountRechargeSavingDemandDeposit(listSavingDemandDeposit);
                    WriteCsvFileDatabaseHistory(listHistory);
                    MenuOptionSavingDemandDeposit(A, listHistory, listSavingDemandDeposit, AllCard);
                    return true;
                case "3":
                    Console.Clear();
                    MenuExportcardinformationSavingDemandDeposit(listSavingDemandDeposit);
                    MenuOptionSavingDemandDeposit(A, listHistory, listSavingDemandDeposit, AllCard);
                    return true;
                case "4":
                    Console.Clear();
                    MenuExporttransactionhistory(A, AllCard, listHistory);
                    MenuOptionSavingDemandDeposit(A, listHistory, listSavingDemandDeposit, AllCard);
                    return true;
                default:
                    return true;
            }
        }
        private static bool MenuWithDrawPOSSavingDemandDeposit(SortedList<string, DemandDeposit> listSavingDemandDeposit)
        {
            Console.Write("Saving number: "); string accountnumber = Console.ReadLine();
            var account = from tmp in listSavingDemandDeposit where tmp.Key == accountnumber select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Amount you want to withdraw: "); decimal x = decimal.Parse(Console.ReadLine());
            if (listSavingDemandDeposit[accountnumber].WithDrawPos(x))
            {
                Console.WriteLine("Transaction Successfuly");
                return true;
            }
            Console.WriteLine("Transaction Failed");
            return false;
        }
        private static bool MenuAccountRechargeSavingDemandDeposit(SortedList<string, DemandDeposit> listSavingDemandDeposit)
        {
            Console.Write("Account Saving: "); string accountnumber = Console.ReadLine();
            var account = from tmp in listSavingDemandDeposit where tmp.Key == accountnumber select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Account Saving does not exits");
                return false;
            }
            Console.Write("Amount you want to deposit: "); decimal x = decimal.Parse(Console.ReadLine());
            if (listSavingDemandDeposit[accountnumber].RechargeMoney(x))
            {
                Console.WriteLine("Transaaction Successfuly");
                return true;
            }
            Console.WriteLine("Transaction Failed");
            return false;
        }
        private static bool MenuExportcardinformationSavingDemandDeposit(SortedList<string, DemandDeposit> listSavingDemandDeposit)
        {
            Console.Write("Saving number: "); string accountnumber = Console.ReadLine();
            var account = from tmp in listSavingDemandDeposit where tmp.Key == accountnumber select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Saving number does not exits");
                return false;
            }
            listSavingDemandDeposit[accountnumber].XuatThongTinSo();
            return true;
        }
        private static bool MenuOptionSavingTimeDeposit(Client A, List<TransactionHistory> listHistory, SortedList<string, TimeDeposit> listSavingTimeDeposit, List<Function> AllCard)
        {
            // luc nao cũng chạy AutoUpdateSavingDeposit duybugggg
            Console.WriteLine("\n---SavingTimeDeposit Menu---");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) WithdrawPos");// duy bug (Xong)
            Console.WriteLine("2) Export Saving information");
            Console.WriteLine("3) Export transaction history");
            Console.Write("\r\nSelect an option: ");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    MenuWithDrawPOSSavingTimeDeposit(listSavingTimeDeposit);
                    WriteCsvFileDatabaseHistory(listHistory);
                    MenuOptionSavingTimeDeposit(A, listHistory, listSavingTimeDeposit, AllCard);
                    return true;
                case "2":
                    Console.Clear();
                    MenuOptionSavingTimeDeposit(A, listHistory, listSavingTimeDeposit, AllCard);
                    return true;
                case "3":
                    Console.Clear();
                    MenuExporttransactionhistory(A, AllCard, listHistory);
                    MenuOptionSavingTimeDeposit(A, listHistory, listSavingTimeDeposit, AllCard);
                    return true;
                default:
                    return true;
            }
        }
        private static bool MenuWithDrawPOSSavingTimeDeposit(SortedList<string, TimeDeposit> listSavingTimeDeposit)
        {
            Console.Write("Saving number: "); string accountnumber = Console.ReadLine();
            var account = from tmp in listSavingTimeDeposit where tmp.Key == accountnumber select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Amount you want to withdraw: "); decimal x = decimal.Parse(Console.ReadLine());
            Console.Write("New term: "); int newterm = int.Parse(Console.ReadLine());
            if (listSavingTimeDeposit[accountnumber].Withdraw(x, newterm))
            {
                Console.WriteLine("Transaction Successfuly");
                return true;
            }
            Console.WriteLine("Transaction Failed");
            return false;
        }
        private static bool MenuExportcardinformationSavingTimeDeposit(SortedList<string, TimeDeposit> listSavingTimeDeposit)
        {
            Console.Write("Saving number: "); string accountnumber = Console.ReadLine();
            var account = from tmp in listSavingTimeDeposit where tmp.Key == accountnumber select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Saving number does not exits");
                return false;
            }
            listSavingTimeDeposit[accountnumber].XuatThongTinSo();
            return true;
        }
        private static bool MenuOptionLoan(Client A, SortedList<string, Client> listClient, List<TransactionHistory> listHistory, List<DebtHistory> listDebt, SortedList<string, DomesticDebitCard> listDebit, SortedList<string, Loans> listLoan, List<Function> AllCard)
        {
            // lúc nào cũng gọi
            Console.WriteLine("\n---Loan Menu---");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Create Loan");
            Console.WriteLine("2) Transfer Loan");
            Console.Write("\r\nSelect an option: ");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    MenuCreateLoan(A, listHistory, listDebt, listLoan, AllCard);
                    CreateListAllCardListNewLoans(listClient, AllCard, listLoan);
                    WriteCsvFileDatabaseAllCard(AllCard);
                    MenuOptionLoan(A, listClient, listHistory, listDebt, listDebit, listLoan, AllCard);
                    return true;
                case "2":
                    Console.Clear();
                    MenuTransferLoan(listDebit, listLoan, AllCard);
                    MenuOptionLoan(A, listClient, listHistory, listDebt, listDebit, listLoan, AllCard);
                    return true;

                default:
                    return false;
            }
        }
        private static bool MenuCreateLoan(Client A, List<TransactionHistory> listHistory, List<DebtHistory> listDebt,
        SortedList<string, Loans> listLoan, List<Function> AllCard)
        {
            string[] ListLoan = { "Unsecured Loan", "Mortage Loan", "Overdraft Loan", "Installment Loan" };
            string loan = "";
            Console.WriteLine("1) Unsecured Loan\n2) Mortage Loan\n3) Overdraft Loan\n4) Installment Loan");
            Console.Write("Your choose: ");
            Console.Write("Loan Account Number: "); string accountnumber = Console.ReadLine();
            switch (Console.ReadLine())
            {
                case "1":
                    loan = ListLoan[0];
                    break;
                case "2":
                    loan = ListLoan[1];
                    break;
                case "3":
                    loan = ListLoan[2];
                    break;
                case "4":
                    loan = ListLoan[3];
                    break;
            }
            var accountnumberallcard = from tmp in AllCard where tmp.AccountNumber == accountnumber || (tmp.ID == A.ID && tmp.Type == loan) select tmp;
            if (accountnumberallcard.Count() >= 1)
            {
                Console.WriteLine("Account number already exists");
                return false;
            }
            Console.Write("Amount you want to borrow: "); decimal Debt = decimal.Parse(Console.ReadLine());
            Console.Write("Term: "); int months = int.Parse(Console.ReadLine());
            if (loan == ListLoan[0] || loan == ListLoan[3])
            {
                CreateLoans(loan, accountnumber, A, Debt, months, listHistory, listDebt, listLoan);
                listLoan[accountnumber].Init();
            }
            else if (loan == ListLoan[2])
            {
                CreateLoans(loan, accountnumber, A, Debt, months, listHistory, listDebt, listLoan);
                listLoan[accountnumber].Transferloans(Debt);
            }
            else
            {
                Console.Write("Total Asset: "); decimal totalasset = decimal.Parse(Console.ReadLine());
                CreateMortageLoan(A, accountnumber, Debt, months, totalasset, listHistory, listDebt, listLoan);
                listLoan[accountnumber].Init();
            }
            return true;
        }
        private static bool MenuTransferLoan(SortedList<string, DomesticDebitCard> listDebit, SortedList<string, Loans> listLoan, List<Function> AllCard)
        {
            Console.Write("Loan Account Number: "); string accountnumber = Console.ReadLine();
            var accountnumberallcard = from tmp in AllCard where tmp.AccountNumber == accountnumber select tmp;
            if (accountnumberallcard.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Account number Debit: "); string accountnumberdebit = Console.ReadLine();
            var account = from tmp in listDebit where tmp.Key == accountnumberdebit select tmp;
            if (account.Count() == 0)
            {
                Console.WriteLine("Account number does not exits");
                return false;
            }
            Console.Write("Amount you want to borrow: "); decimal x = decimal.Parse(Console.ReadLine());
            if (LoansTransfer(listLoan[accountnumber], listDebit[accountnumberdebit], x, listDebit))
            {
                Console.WriteLine("Loan successfully");
                return true;
            }
            return false;
        }
    }
}