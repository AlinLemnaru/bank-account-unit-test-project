using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

//------------------------------------------------------------
//---------------------Bank Account Class---------------------
//------------------------------------------------------------

namespace bankAccount
{
    public class BankAccount
    {
        //-------------------Private Fields-------------------

        private decimal balance; // Private field to store the account balance
        private decimal minBalance = 0.1M; // Private field to store the minimum balance
        private ICurrencyConverter currencyConverter; // Currency converter interface
        private string accountID; // Unique account identifier
        private List<TransactionDetails> transactionHistory; // List to store transaction history
        private decimal dailyWithdrawLimit = 5000M; // Daily withdrawal limit in RON (bug fix - initialized with 50000 when 5000 intended)
        private decimal totalWithdrawnToday = 0M; // total amount withdrawn today
        private DateTime lastWithdrawDate = DateTime.MinValue; // date of the last withdrawal
        private decimal interestRate; // interest rate of 2%
        private Currency accountCurrency;

        //--------------------Constructors--------------------

        // Default Constructor to initialize the bank account
        public BankAccount()
        {
            balance = 0; // initial balance
            currencyConverter = new BnrCurrencyConverter(); // using BNR currency converter 
            accountID = Guid.NewGuid().ToString(); // unique account ID
            transactionHistory = new List<TransactionDetails>(); // initialize transaction history
            interestRate = 0.02M; // default interest rate of 2%
            accountCurrency = Currency.RON; // default currency RON
        }

        // Overloaded Constructor to initialize the bank account with a specific balance
        public BankAccount(decimal value)
        {
            balance = value; // initial balance
            currencyConverter = new BnrCurrencyConverter(); // using BNR currency converter 
            accountID = Guid.NewGuid().ToString(); // unique account ID
            transactionHistory = new List<TransactionDetails>(); // initialize transaction history
            interestRate = 0.02M; // default interest rate of 2%
            accountCurrency = Currency.RON; // default currency RON
        }

        // Overloaded Constructor to initialize the bank account with a specific balance and currency converter
        // Dependency Injection(STUB or MOCK)
        public BankAccount(decimal value, ICurrencyConverter converter)
        {
            balance = value; // initial balance
            currencyConverter = converter; // stub or mock for currency converter
            accountID = Guid.NewGuid().ToString(); // unique account ID
            transactionHistory = new List<TransactionDetails>(); // initialize transaction history
            interestRate = 0.02M; // default interest rate of 2%
            accountCurrency = Currency.RON; // default currency RON
        }

        // Overloaded Constructor to initialize the bank account with a specific balance, interest rate, currency converter and Currency (RON/EUR)
        // Dependency Injection(STUB or MOCK)
        public BankAccount(decimal value, ICurrencyConverter converter, decimal interestRateValue, Currency currency)
        {
            balance = value; // initial balance
            currencyConverter = converter; // stub or mock for currency converter
            accountID = Guid.NewGuid().ToString(); // unique account ID
            transactionHistory = new List<TransactionDetails>(); // initialize transaction history
            interestRate = interestRateValue;
            accountCurrency = currency;
        }

        //---------------------Public Methods---------------------

        // Method to deposit money into the bank account
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount should be positive");

            balance += amount; // Update balance

            transactionHistory.Add(new TransactionDetails(DateTime.Now, amount, $"Deposit ({amount} {accountCurrency})")); // Log transaction
        }

        // Private method to reset daily withdraw amount if the date has changed
        private void ResetDailyLimitIfNeeded()
        {
            if (lastWithdrawDate.Date < DateTime.Now.Date)
            {
                totalWithdrawnToday = 0;
            }
        }

        // Private method to withdraw money with option to check daily limit
        private void Withdraw(decimal amount, bool checkDailyLimit)
        {
            // Verify daily limit for withdrawals
            if (checkDailyLimit)
            {
                ResetDailyLimitIfNeeded();

                if (totalWithdrawnToday + amount > dailyWithdrawLimit)
                {
                    throw new InvalidOperationException($"Limit ({dailyWithdrawLimit} {accountCurrency})");
                }

                totalWithdrawnToday += amount;
                lastWithdrawDate = DateTime.Now;
            }

            // Verify sufficient balance after maintaining minimum balance
            if (balance - amount < minBalance)
                throw new NotEnoughFundsException();

            balance -= amount; // Update balance

            transactionHistory.Add(new TransactionDetails(DateTime.Now, amount, $"Withdraw ({amount} {accountCurrency})")); // Log transaction
        }

        // Method to withdraw money from the bank account
        public void Withdraw(decimal amount)
        {
            Withdraw(amount, true); // Call private withdraw method with daily limit check
        }

        // Method to calculate interest for a given number of days
        public decimal CalculateInterest(int daysCount)
        {
            if (daysCount <= 0)
                throw new ArgumentException("Days number should be a positive number");

            if (interestRate <= 0)
                throw new InvalidOperationException("Interest rate must be positive");

            decimal interest = balance * interestRate * (daysCount / 365.0M);
            interest = Math.Round(interest, 2, MidpointRounding.AwayFromZero); // get only two decimal places

            return interest;
        }

        // Method to apply interest to the bank account for a given number of days
        public void ApplyInterest(int daysCount)
        {
            decimal interest = CalculateInterest(daysCount);
            balance += interest;

            transactionHistory.Add(new TransactionDetails(DateTime.Now, interest, 
                $"Interest Applied ({interest} for {daysCount} days -> {balance} {accountCurrency} )")); // Log transaction
        }

        // Method to transfer funds to another bank account
        public void TransferFunds(BankAccount destination, decimal amount)
        {
            if (amount <= 0)
                throw new NotEnoughFundsException();

            if (amount > balance )
                throw new NotEnoughFundsException();

            if (destination.accountCurrency != this.accountCurrency)
                throw new InvalidOperationException("Cannot transfer between accounts of different currencies.");

            if (balance - amount >= minBalance)
            {
                destination.Deposit(amount); // Add to destination account
                Withdraw(amount, false); // Withdraw from source account without checking daily limit
            }
            else
                throw new NotEnoughFundsException();

            transactionHistory.Add(new TransactionDetails(DateTime.Now, amount, $"Transfer to Account {destination.accountID}"));
            destination.transactionHistory.Add(new TransactionDetails(DateTime.Now, amount, $"Transfer from Account {this.accountID}"));
        }

        // Method to transfer funds while maintaining minimum balance
        public BankAccount TransferMinFunds(BankAccount destination, decimal amount)
        {
            // Tranfer amount must be positive
            if (amount <= 0)
                throw new NotEnoughFundsException();

            if (amount > balance)
                throw new NotEnoughFundsException();

            if (destination.accountCurrency != this.accountCurrency)
                throw new InvalidOperationException("Cannot transfer between accounts of different currencies.");

            // Verify sufficient balance after maintaining minimum balance
            if (balance - amount >= minBalance)
            {
                destination.Deposit(amount); // Add to destination account
                Withdraw(amount, false); // Withdraw from source account without checking daily limit
            }
            else
            {
                destination.Deposit(balance - minBalance); // Add to destination account
                Withdraw(balance - minBalance, false); // Withdraw from source account without checking daily limit
            }

            transactionHistory.Add(new TransactionDetails(DateTime.Now, amount, $"Transfer to Account {destination.accountID}"));
            destination.transactionHistory.Add(new TransactionDetails(DateTime.Now, amount, $"Transfer from Account {this.accountID}"));

            return destination; // Return destination account
        }

        // Method to convert RON to EUR using the currency converter
        public decimal ConvertRonToEur(decimal amountRon)
        {
            if (amountRon <= 0)
                throw new ArgumentException("Amount should be positive");

            if (accountCurrency != Currency.RON)
                throw new InvalidOperationException("Account currency must be RON to convert to EUR");

            decimal eurToRonRate = currencyConverter.GetEurToRonRate();

            if (eurToRonRate < 4 || eurToRonRate > 6)
                throw new ArgumentException("Rate should be logically between 4 and 6");

            return amountRon / eurToRonRate;
        }

        // Method to convert EUR to RON using the currency converter
        public decimal ConvertEurToRon(decimal amountEur)
        {
            if (amountEur <= 0)
                throw new ArgumentException("Amount should be positive");

            if (accountCurrency != Currency.EUR)
                throw new InvalidOperationException("Account currency must be EUR to convert to RON");

            decimal eurToRonRate = currencyConverter.GetEurToRonRate();

            if (eurToRonRate < 4 || eurToRonRate > 6)
                throw new ArgumentException("Rate should be logically between 4 and 6");

            return amountEur * eurToRonRate;
        }

        // Method to transfer RON to EUR between two bank accounts
        public void TransferRonToEur(BankAccount destination, decimal amountRon)
        {
            if (amountRon <= 0)
                throw new ArgumentException("Amount should be positive");

            // Verify if source has enough funds for maintaining minimum balance
            if (balance - amountRon < minBalance)
                throw new NotEnoughFundsException();

            if (accountCurrency != Currency.RON || destination.accountCurrency != Currency.EUR)
                throw new InvalidOperationException("Invalid account currencies for RON -> EUR transfer");

            decimal amountEur = ConvertRonToEur(amountRon);

            Withdraw(amountRon, false);

            destination.Deposit(amountEur);

            transactionHistory.Add(new TransactionDetails(DateTime.Now, amountRon, $"Transfer to Account {destination.accountID} RON -> EUR"));
            destination.transactionHistory.Add(new TransactionDetails(DateTime.Now, amountEur, $"Transfer from Account {this.accountID}"));
        }

        // Method to transfer EUR to RON between two bank accounts
        public void TransferEurToRon(BankAccount destination, decimal amountEur)
        {
            if (amountEur <= 0)
                throw new ArgumentException("Amount should be positive");

            // Verify if source has enough funds for maintaining minimum balance
            if (balance - amountEur < minBalance)
                throw new NotEnoughFundsException();

            if (accountCurrency != Currency.EUR || destination.accountCurrency != Currency.RON)
                throw new InvalidOperationException("Invalid account currencies for EUR -> RON transfer");

            decimal amountRon = ConvertEurToRon(amountEur);

            Withdraw(amountEur, false);

            destination.Deposit(amountRon);

            transactionHistory.Add(new TransactionDetails(DateTime.Now, amountEur, $"Transfer to Account {destination.accountID} EUR -> RON"));
            destination.transactionHistory.Add(new TransactionDetails(DateTime.Now, amountRon, $"Transfer from Account {this.accountID}"));
        }

        // Method to get the transaction history as a formatted string
        public string GetTransactionHistory()
        {
            StringBuilder history = new StringBuilder();
            history.AppendLine("Date\t\t\tAmount\tDescription");
            history.AppendLine("--------------------------------------------------");

            foreach (var transaction in transactionHistory)
            {
                history.AppendLine($"{transaction.TransactionDate}\t{transaction.Amount}\t{transaction.Description}");
            }

            return history.ToString();
        }

        // Method to clear the transaction history
        public void ClearTransactionHistory()
        {
            transactionHistory.Clear();
        }

        // Method to generate a summary of the bank account
        public string GetAccountSummary()
        {
            StringBuilder summary = new StringBuilder();
            summary.AppendLine($"Account ID: {accountID}");
            summary.AppendLine($"Balance: {balance} {accountCurrency}");
            summary.AppendLine($"Minimum Balance: {minBalance} {accountCurrency}");
            summary.AppendLine($"Daily Withdraw Limit: {dailyWithdrawLimit} {accountCurrency}");

            return summary.ToString();
        }

        //---------------------Get & Set---------------------

        public decimal Balance
        {
            get { return balance; }
        }

        public decimal MinBalance
        {
            get { return minBalance; }
        }

        public string AccountID
        {
            get { return accountID; }
        }

        public List<TransactionDetails> TransactionHistory
        {
            get { return new List<TransactionDetails>(transactionHistory); }
        }

        public decimal DailyWithdrawLimit
        {
            get { return dailyWithdrawLimit; }
            set { dailyWithdrawLimit = value; }
        }

        public Currency AccountCurrency
        {
            get { return accountCurrency; }
        }

        public decimal InterestRate
        {
            get { return interestRate; }
        }
    }
}
