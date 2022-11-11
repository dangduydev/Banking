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
            string[] ListLoan = { "Unsecured Loan", "Mortage Loan", "Overdraft Loan", "Installment Loan" };
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
            ReadCvsFileDatabaseClients(listClient);

            ReadCvsFileDatabaseAllCard(AllCard);

            ReadCvsFileDatabaseHistory(listHistory);

            CreateAllCardListOldDebits(listClient, listDebit, AllCard, listHistory);
            CreateAllCardListOldCredits(listClient, listCredit, AllCard, listHistory, listDebt);
            CreateAllCardListOldSavingDemandDeposit(listClient, listSavingDemandDeposit, AllCard, listHistory);
            CreateAllCardListOldSavingTimeDeposit(listClient, listSavingTimeDeposit, AllCard, listHistory);
            CreateAllCardListOldLoans(listClient, listLoan, AllCard, listHistory, listDebt);

            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu(listClient);
            }

            //Loans
            // CreateLoans(ListLoan[0], listClient["1"], 1000000, 6, listHistory, listDebt, listLoan);
            // CreateLoans(ListLoan[2], listClient["1"], 2000000, 6, listHistory, listDebt, listLoan);
            // CreateLoans(ListLoan[3], listClient["1"], 3000000, 6, listHistory, listDebt, listLoan);
            // CreateMortageLoan(listClient["1"], 4000000,6,10000000,listHistory, listDebt,listLoan);
            // listLoan[ListLoan[0]].Transferloans(1000000,6);
            // listLoan[ListLoan[1]].Transferloans(2000000,6);
            // listLoan[ListLoan[2]].Transferloans(3000000,6);
            // listLoan[ListLoan[3]].Transferloans(4000000,6);

            //clear
            AllCard.Clear();
            CreateListAllCardNewListDebit(listClient, AllCard, listDebit);// đi chung
            CreateListAllCardNewListCredit(listClient, AllCard, listCredit);
            CreateListAllCardNewListSavingDemandDeposit(listClient, AllCard, listSavingDemandDeposit);
            CreateListAllCardNewListSavingTimeDeposit(listClient, AllCard, listSavingTimeDeposit);
            CreateListAllCardNewListLoans(listClient, AllCard, listLoan);
            WriteCsvFileNewAllCard(AllCard);// đi chung

            //Console.WriteLine(AllCard.Count());
            WriteCsvFileDatabaseHistory(listHistory);
            WriteCsvFileDatabaseClients(listClient);

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
                //Console.WriteLine(p.Name);
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
                //Console.WriteLine(p.Name);
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
            //var epersons = csvContext.Read<Person>("person.csv", csvFileDes);
            csvContext.Write(listClient.Values, "databaseclients.csv", csvFileDes);
        }
        static void WriteCsvFileNewAllCard(List<Function> AllCard)
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
        public static bool CreateClient(string name, string address, DateTime dateofbirth,
        string id, string position, string nationality, string workplace,
        bool sex, string phonenumber, DateTime issueddate, decimal income, string branch, string password, SortedList<string, Client> listClient)
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
        public static bool CreateDebitCard(string TypeCard, string accountnumber, string pin, Client A, SortedList<string, DomesticDebitCard> listDebit, List<TransactionHistory> listHistory)
        {
            string[] ListCard =
                  {
                "Napas Success", "Napas Success Plus"
            };
            var tmp = from nnb in listDebit where nnb.Value.Type == TypeCard && nnb.Value.ID == A.ID select nnb;
            if (tmp.Count() > 0) return false;
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
        public static bool CreateCreditCard(string TypeCard, string accountnumber, string pin, Client A, SortedList<string, InternationalCreditCard> listCredit, List<TransactionHistory> listHistory, List<DebtHistory> listDebt)
        {
            string[] ListCard =
            {
                "VisaStandard", "VisaGold",
                "MastercardGold", "MastercardPlatinum", "JCBGold", "JCBUltimate",
            };
            var tmp = from nnb in listCredit where nnb.Value.Type == TypeCard && nnb.Value.ID == A.ID select nnb;
            if (tmp.Count() > 0) return false;
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
        public static bool CreateSavingAccountDemandDeposit(string Typeofsaving, string accountsaving, decimal savingdeposit, int term, Client A, SortedList<string, DemandDeposit> listSavingDemandDeposit, List<TransactionHistory> listHistory)
        {
            DemandDeposit Ax = new DemandDeposit(Typeofsaving, accountsaving, "", A.ID, savingdeposit, 0, listHistory, A);
            if (Ax.CheckMinimum() == true && Ax.Term == 0 && Ax.Maximun >= savingdeposit)
            {
                listSavingDemandDeposit.Add(Ax.AccountNumber, Ax);
                return true;
            }
            return false;
        }
        public static bool CreateSavingAccountTimeDeposit(string Typeofsaving, string accountsaving, decimal savingdeposit, int term, int typeSaving,
         Client A, SortedList<string, TimeDeposit> listSavingTimeDeposit, List<TransactionHistory> listHistory)
        {
            TimeDeposit B = new TimeDeposit(Typeofsaving, accountsaving, "", A.ID, savingdeposit, term, typeSaving, listHistory, A);
            if (B.CheckMinimum() == true && B.Term != 0)
            {
                listSavingTimeDeposit.Add(B.AccountNumber, B);
                return true;
            }
            return false;
        }
        public static bool CreateLoans(string type, Client A,
        decimal Debt, int months, List<TransactionHistory> histories, List<DebtHistory> listDebt, SortedList<string, Loans> listLoan)
        {
            string[] ListLoan = { "Unsecured Loan", "Mortage Loan", "Overdraft Loan", "Installment Loan" };
            if (type == ListLoan[0])
            {
                Loans UL = new UnsecuredLoan(type, "", A.ID, A, "", histories, listDebt,
                    Debt, months, A.Fico);
                listLoan.Add(type, UL);
            }
            if (type == ListLoan[2])
            {
                Loans L = new OverdraftLoan(type, "", A.ID, A, "", histories, listDebt, A.Income);
                listLoan.Add(type, L);
            }
            if (type == ListLoan[3])
            {
                Loans L = new InstallmentLoan(type, "", A.ID, A, "", histories, listDebt, A.Income);
                listLoan.Add(type, L);
            }
            return true;
        }
        public static bool CreateMortageLoan(Client A,
        decimal Debt, int months, decimal totalasset, List<TransactionHistory> histories, List<DebtHistory> listDebt, SortedList<string, Loans> listLoan)
        {
            string[] ListLoan = { "Unsecured Loan", "Mortage Loan", "Overdraft Loan", "Installment Loan" };
            Loans L = new MortageLoan(ListLoan[1], "", A.ID, A, "", histories, listDebt, totalasset);
            listLoan.Add(ListLoan[1], L);
            return true;
        }
        public static void XuatLichSuGD(Client A, string type, DateTime start, DateTime end, List<TransactionHistory> listHistory)
        {

            Console.WriteLine("\nLICH SU GIAO DICH " + type);
            var historiesLists = from ls in listHistory
                                 where ls.ID == A.ID && ls.DayTrading <= end && ls.DayTrading >= start
            && ls.Type == type
                                 select ls;
            foreach (TransactionHistory item in historiesLists)
            {
                item.XuatLichSuGD();
                Console.WriteLine();
            }
            //Console.WriteLine(historiesLists.Count());
        }
        public static void CreateListAllCardNewListDebit(SortedList<string, Client> listClient, List<Function> AllCard, SortedList<string, DomesticDebitCard> listDebit)
        {
            foreach (var person in listClient)
            {
                var listDebitNewCard = from tmp in listDebit where tmp.Value.ID == person.Value.ID select tmp;

                foreach (var cardDebit in listDebitNewCard)
                {
                    string descriptions = cardDebit.Value.Pin + " "
                    + cardDebit.Value.CardBalance.ToString() + " " + cardDebit.Value.StartTime.ToString();//pin -> cardblance -> StartedDay
                    Function tmp = new Function(cardDebit.Value.Type, cardDebit.Value.AccountNumber, cardDebit.Value.ID, person.Value, descriptions);
                    AllCard.Add(tmp);

                }
            }
        }
        public static void CreateAllCardListOldDebits(SortedList<string, Client> listClient,
        SortedList<string, DomesticDebitCard> listDebit, List<Function> AllCard, List<TransactionHistory> listHistory)
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
                listDebit[OldDebits.AccountNumber].StartTime = DateTime.Parse(Properties[2]);
            }

        }

        public static void CreateListAllCardNewListCredit(SortedList<string, Client> listClient, List<Function> AllCard, SortedList<string, InternationalCreditCard> listCredit)
        {
            foreach (var person in listClient)
            {
                var listCreditNewCard = from tmp in listCredit where tmp.Value.ID == person.Value.ID select tmp;

                foreach (var cardCredit in listCreditNewCard)
                {
                    string descriptions = cardCredit.Value.Pin + " "
                    + cardCredit.Value.CardBalance.ToString() + " " + cardCredit.Value.StartTime.ToString();//pin -> cardblance -> StartedDay
                    Function tmp = new Function(cardCredit.Value.Type, cardCredit.Value.AccountNumber, cardCredit.Value.ID, person.Value, descriptions);
                    AllCard.Add(tmp);

                }
            }
        }
        public static void CreateListAllCardNewListSavingDemandDeposit(SortedList<string, Client> listClient, List<Function> AllCard, SortedList<string, DemandDeposit> listSavingDemandDeposit)
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
        //listSavingTimeDeposit
        public static void CreateListAllCardNewListSavingTimeDeposit(SortedList<string, Client> listClient, List<Function> AllCard, SortedList<string, TimeDeposit> listSavingTimeDeposit)
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
        //Loans
        public static void CreateListAllCardNewListLoans(SortedList<string, Client> listClient, List<Function> AllCard, SortedList<string, Loans> listLoan)
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
                        descriptions = item.Value.Debt.ToString() + " " + item.Value.Term.ToString();  //Debt > Term (months)
                    }
                    if (item.Value.Type == ListLoan[1])
                    {
                        descriptions = item.Value.Limit.ToString(); //Limit (totalasset)
                    }
                    descriptions += " " + item.Value.DateTimeloan.ToString(); // += DateTimeloan
                    Function tmp = new Function(item.Value.Type, item.Value.AccountNumber, item.Value.ID, person.Value, descriptions);
                    AllCard.Add(tmp);
                }
            }
        }
        public static void CreateAllCardListOldCredits(SortedList<string, Client> listClient,
        SortedList<string, InternationalCreditCard> listCredit, List<Function> AllCard, List<TransactionHistory> listHistory, List<DebtHistory> listDebt)
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
                listCredit[OldCredits.AccountNumber].StartTime = DateTime.Parse(Properties[2]);
            }

        }

        public static void CreateAllCardListOldSavingDemandDeposit(SortedList<string, Client> listClient,
        SortedList<string, DemandDeposit> listSavingDemandDeposit, List<Function> AllCard, List<TransactionHistory> listHistory)
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
        //listSavingTimeDeposit
        public static void CreateAllCardListOldSavingTimeDeposit(SortedList<string, Client> listClient,
        SortedList<string, TimeDeposit> listSavingTimeDeposit, List<Function> AllCard, List<TransactionHistory> listHistory)
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
        public static void CreateAllCardListOldLoans(SortedList<string, Client> listClient,
        SortedList<string, Loans> listLoan, List<Function> AllCard, List<TransactionHistory> listHistory, List<DebtHistory> listDebt)
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
                    CreateLoans(OldLoan.Type, listClient[OldLoan.ID], decimal.Parse(Properties[0]), int.Parse(Properties[1]), listHistory, listDebt, listLoan);
                    listLoan[OldLoan.Type].DateTimeloan = DateTime.Parse(Properties[2]);
                }
                if (OldLoan.Type == ListLoan[1])
                {
                    CreateMortageLoan(listClient[OldLoan.ID], 0, 0, decimal.Parse(Properties[0]), listHistory, listDebt, listLoan);
                    listLoan[OldLoan.Type].DateTimeloan = DateTime.Parse(Properties[1]);
                }
                if (OldLoan.Type == ListLoan[2] || OldLoan.Type == ListLoan[3])
                {
                    CreateLoans(OldLoan.Type, listClient[OldLoan.ID], 0, 0, listHistory, listDebt, listLoan);
                    listLoan[OldLoan.Type].DateTimeloan = DateTime.Parse(Properties[0]);
                }
            }

        }
        public static void CreditPayment(string accountnumbercredit, string accountnumberdebit, string pindebit,
                decimal x, SortedList<string, DomesticDebitCard> listDebit, SortedList<string, InternationalCreditCard> listCredit)
        {
            var lists = from account in listCredit.Keys where account == accountnumbercredit select account;
            if (lists.Count() > 0 && listDebit[accountnumberdebit].Payment(x, pindebit, "Thanh toan no tin dung"))
            {
                listCredit[accountnumbercredit].Payment(x);
            }
        }
        public static bool SavingReceiveTimeDeposit(string accountnumbersaving, string accountnumberdebit, SortedList<string, DomesticDebitCard> listDebit,
                SortedList<string, TimeDeposit> listSavingTimeDeposit)
        {
            decimal tmp = listSavingTimeDeposit[accountnumbersaving].AutoUpdateSavingDeposit();
            if (tmp == -1)
            {
                return false;
            }
            else
            {
                listDebit[accountnumberdebit].ReceiveMoney(tmp, "Nhan tien tu so tiet kiem " + accountnumbersaving);
                return true;
            }
            return false;
        }
        public static void TransfersATM(string accountnumberofRemitter, string pin,
                            string accountnumberofReceiver, decimal x, SortedList<string, DomesticDebitCard> listDebit)
        {
            var lists = from account in listDebit.Keys where account == accountnumberofReceiver select account;
            if (lists.Count() > 0 && listDebit[accountnumberofRemitter].TransfersATM(x, pin))
            {
                listDebit[accountnumberofReceiver].ReceiveMoney(x, "");  //vì chuyển khoản tại ATM nên ko có nội dung giao dịch
            }
        }
        public static void LoansPayment(string accountnumber, string pin, decimal x, string TypeLoans, SortedList<string, DomesticDebitCard> listDebit, SortedList<string, Loans> listLoan)
        {
            foreach (var item in listLoan)
            {
                if (item.Value.Type == TypeLoans && listDebit[accountnumber].Payment(x, pin, $"Thanh toán khoản vay {TypeLoans}"))
                {
                    listLoan[TypeLoans].Paymentloans(x);
                }
            }
        }
        private static bool MainMenu(SortedList<string, Client> listClient)
        {
            // Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("0) Clear");
            Console.WriteLine("1) Sign Up");
            Console.WriteLine("2) Sign In");
            Console.WriteLine("3) Forgot Password");
            Console.WriteLine("4) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "0":
                    Console.Clear();
                    MainMenu(listClient);
                    return true;
                case "1":
                    SignUp(listClient);
                    return true;
                case "2":
                    SignIn(listClient);
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
            Console.WriteLine("Enter your personal information");
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
                //MenuClient
                // MainMenu(listClient);
            }
            // Console.ReadKey();
            return true;
        }
        private static bool SignIn(SortedList<string, Client> listClient)
        {
            string nameuser;
            string password;
            do
            {
                Console.Write("Nameuser: "); nameuser = Console.ReadLine();
                Console.Write("Password: "); password = Console.ReadLine();
            } while (password.Length < 6);
            var user = from tmp in listClient.Keys where tmp == nameuser select tmp;
            if (user.Count() > 0 && password == listClient[nameuser].Password)
            {
                Console.WriteLine("Dang nhap thanh cong!");
                MainMenu(listClient);
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
    }
}