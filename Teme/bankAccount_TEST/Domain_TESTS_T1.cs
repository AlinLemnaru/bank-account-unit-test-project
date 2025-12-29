using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//------------------------------------------------------------
//----------------------Domain TESTS T1-----------------------
//------------------------------------------------------------

namespace bankAccount
{
    [TestFixture]
    [Category("Domain")]
    [Category("Deposit method - Boundary Value Analysis tests(Tema_1)")]
    public class Domain_TESTS_T1
    {
        //------------------Variables-------------------

        // Variable used for single account testing
        private BankAccount? testAccount;

        // Variables used for source and destination accounts       
        private BankAccount? sourceAccount;
        private BankAccount? destinationAccount;

        //-------------Setup and teardown---------------

        [SetUp]
        public void Setup_Preconditions()
        {
            // Clear accounts before each test
            testAccount = null;
            sourceAccount = null;
            destinationAccount = null;
        }

        [TearDown]
        public void Teardown_Postconditions()
        {
            // Clear accounts before each test
            testAccount = null;
            sourceAccount = null;
            destinationAccount = null;
        }

        //-------------------Tests----------------------

        /// <summary>
        /// T1.1 - ON (On Boundary): Test deposit of smallest valid amount (0.01M)
        /// Boundary value testing: Testing at the exact minimum valid boundary
        /// </summary>
        [Test]
        [Category("Domain-ON")]
        [Description("ON: Deposit smallest valid amount (0.01M) - boundary value")]
        public void T1_1_Deposit_ON_SmallestValidAmount_ShouldSucceed()
        {
            // Arrange
            testAccount = new BankAccount(1000M);
            decimal initialBalance = testAccount.Balance;
            decimal depositAmount = 0.01M; // Smallest valid amount

            // Act
            testAccount.Deposit(depositAmount);

            // Assert - Verify balance updated correctly
            decimal expectedBalance = initialBalance + depositAmount; // 1000 + 0.01 = 1000.01
            Assert.That(testAccount.Balance, Is.EqualTo(expectedBalance), $"Balance should be {expectedBalance} after deposit but it is {testAccount.Balance}");

            // Assert - Verify transaction recorded correctly
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(1), "Transaction history should contain 1 transaction after deposit.");

            // Assert - Verify transaction details
            var transaction = testAccount.TransactionHistory[0];
            Assert.That(transaction.Amount, Is.EqualTo(depositAmount), $"Transaction amount should be {depositAmount} but it is {transaction.Amount}");
            Assert.That(transaction.Description, Is.EqualTo($"Deposit ({depositAmount} {testAccount.AccountCurrency})"), 
                $"Transaction description should be 'Deposit' but it is '{transaction.Description}'");

            // Assert - Verify transaction date is recent (within last minute)
            Assert.That(transaction.TransactionDate, Is.GreaterThan(DateTime.Now.AddMinutes(-1)), "Transaction date should be recent.");

            // Success message
            TestContext.WriteLine($"T1.1: Deposit of {depositAmount} succeeded - Balance: {testAccount.Balance}");
        }

        /// <summary>
        /// T1.2 - IN1 (Inside Boundary): Test deposit of small valid amount (50M)
        /// Boundary value testing: Testing inside the valid range
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN1: Deposit small valid amount (50M) - inside boundary")]
        public void T1_2_Deposit_IN1_SmallAmount_ShouldSucceed()
        {
            // Arrange
            testAccount = new BankAccount(1000M);
            decimal initialBalance = testAccount.Balance;
            decimal depositAmount = 50M;

            // Act
            testAccount.Deposit(depositAmount);

            // Assert - Verify balance increased correctly
            decimal expectedBalance = initialBalance + depositAmount; // 1000 + 50 = 1050
            Assert.That(testAccount.Balance, Is.EqualTo(expectedBalance), $"Balance should be {expectedBalance} after depositing {depositAmount}");

            // Assert - Verify transaction history was recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(1), "TransactionHistory should contain exactly 1 transaction after deposit");

            // Assert - Verify transaction details
            var transaction = testAccount.TransactionHistory[0];
            Assert.That(transaction.Amount, Is.EqualTo(depositAmount), $"Transaction amount should be {depositAmount}");
            Assert.That(transaction.Description, Is.EqualTo($"Deposit ({depositAmount} {testAccount.AccountCurrency})"),
                $"Transaction description should be 'Deposit ({depositAmount} {testAccount.AccountCurrency})' but it is '{transaction.Description}'");

            // Success message
            Assert.Pass($"T1.2: Deposit of {depositAmount} succeeded - Balance: {testAccount.Balance}");
        }

        /// <summary>
        /// T1.3 - IN2 (Inside Boundary): Test deposit of large valid amount (10000M)
        /// Boundary value testing: Testing inside the valid range with larger amount
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN2: Deposit large valid amount (10000M) - inside boundary")]
        public void T1_3_Deposit_IN2_LargeAmount_ShouldSucceed()
        {
            // Arrange
            testAccount = new BankAccount(1000M);
            decimal initialBalance = testAccount.Balance;
            decimal depositAmount = 10000M;

            // Act
            testAccount.Deposit(depositAmount);

            // Assert - Verify balance increased correctly
            decimal expectedBalance = initialBalance + depositAmount; // 1000 + 10000 = 11000
            Assert.That(testAccount.Balance, Is.EqualTo(expectedBalance), $"Balance should be {expectedBalance} after depositing {depositAmount}");

            // Assert - Verify transaction history was recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(1), "TransactionHistory should contain exactly 1 transaction after deposit");

            // Assert - Verify transaction details
            var transaction = testAccount.TransactionHistory[0];
            Assert.That(transaction.Amount, Is.EqualTo(depositAmount), $"Transaction amount should be {depositAmount}");
            Assert.That(transaction.Description, Is.EqualTo($"Deposit ({depositAmount} {testAccount.AccountCurrency})"),
                $"Transaction description should be 'Deposit ({depositAmount} {testAccount.AccountCurrency})' but it is '{transaction.Description}'");

            // Success message
            Assert.Pass($"T1.3: Deposit of {depositAmount} succeeded - Balance: {testAccount.Balance}");
        }

        /// <summary>
        /// T1.4 - OFF1 (Outside Boundary): Test deposit of zero amount (0M)
        /// Boundary value testing: Testing below minimum valid boundary
        /// Expected: Should throw ArgumentException
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF1: Deposit zero amount (0M) - outside boundary (below minimum)")]
        public void T1_4_Deposit_OFF1_ZeroAmount_ShouldThrowArgumentException()
        {
            // Arrange
            testAccount = new BankAccount(1000M);
            decimal depositAmount = 0M;

            // Act & Assert - Deposit of 0 should throw ArgumentException
            var exception = Assert.Throws<ArgumentException>(() =>
                testAccount.Deposit(depositAmount), "Deposit of zero should throw ArgumentException");

            // Assert - Verify exception message contains helpful info
            Assert.That(exception.Message, Does.Contain("positive").IgnoreCase, "Exception message should mention that amount must be positive");

            // Assert - Verify balance was NOT changed
            Assert.That(testAccount.Balance, Is.EqualTo(1000M), "Balance should remain unchanged when deposit throws exception");

            // Assert - Verify no transaction was recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(0), "No transaction should be recorded when deposit fails");

            // Success message
            Assert.Pass($"T1.4: Deposit of {depositAmount} correctly threw ArgumentException");
        }

        /// <summary>
        /// T1.5 - OFF2 (Outside Boundary): Test deposit of negative amount (-100M)
        /// Boundary value testing: Testing outside valid boundary (negative value)
        /// Expected: Should throw ArgumentException
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF2: Deposit negative amount (-100M) - outside boundary (negative)")]
        public void T1_5_Deposit_OFF2_NegativeAmount_ShouldThrowArgumentException()
        {
            // Arrange
            testAccount = new BankAccount(1000M);
            decimal depositAmount = -100M;

            // Act & Assert - Deposit of negative should throw ArgumentException
            var exception = Assert.Throws<ArgumentException>(() =>
                testAccount.Deposit(depositAmount), "Deposit of negative amount should throw ArgumentException");

            // Assert - Verify exception message contains helpful info
            Assert.That(exception.Message, Does.Contain("positive").IgnoreCase, "Exception message should mention that amount must be positive");

            // Assert - Verify balance was NOT changed
            Assert.That(testAccount.Balance, Is.EqualTo(1000M), "Balance should remain unchanged when deposit throws exception");

            // Assert - Verify no transaction was recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(0), "No transaction should be recorded when deposit fails");

            // Success message
            Assert.Pass($"T1.5: Deposit of {depositAmount} correctly threw ArgumentException");
        }

        /// <summary>
        /// T1.6 - ON (On Boundary): Test withdraw to exact minimum balance (0.1M)
        /// Boundary value testing: Withdraw exactly enough to leave minBalance
        /// Account has 1000M, minBalance is 0.1M, so can withdraw 999.9M
        /// </summary>
        [Test]
        [Category("Domain-ON")]
        [Description("ON: Withdraw to exact minimum balance - boundary value")]
        public void T1_6_Withdraw_ON_ExactToMinBalance_ShouldSucceed()
        {
            // Arrange
            testAccount = new BankAccount(1000M);
            decimal minBalance = testAccount.MinBalance; // 0.1M
            decimal withdrawAmount = 1000M - minBalance; // 999.9M
            decimal expectedBalance = minBalance; // 0.1M

            // Act
            testAccount.Withdraw(withdrawAmount);

            // Assert - Verify balance is now exactly at minBalance
            Assert.That(testAccount.Balance, Is.EqualTo(expectedBalance), $"Balance should be {expectedBalance} (minBalance) after withdrawing {withdrawAmount}");

            // Assert - Verify transaction history was recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(1), "TransactionHistory should contain exactly 1 transaction after withdrawal");

            // Assert - Verify transaction details
            var transaction = testAccount.TransactionHistory[0];
            Assert.That(transaction.Amount, Is.EqualTo(withdrawAmount), $"Transaction amount should be {withdrawAmount}");
            Assert.That(transaction.Description, Is.EqualTo($"Withdraw ({withdrawAmount} {testAccount.AccountCurrency})"),
                $"Transaction description should be 'Withdraw ({withdrawAmount} {testAccount.AccountCurrency})' but it is '{transaction.Description}'");

            // Success message
            Assert.Pass($"T1.6: Withdraw of {withdrawAmount} succeeded - Balance at minBalance: {testAccount.Balance}");
        }

        /// <summary>
        /// T1.7 - IN1 (Inside Boundary): Test withdraw of small valid amount (100M)
        /// Boundary value testing: Withdraw less than balance, leaves good margin from minBalance
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN1: Withdraw small valid amount (100M) - inside boundary")]
        public void T1_7_Withdraw_IN1_SmallAmount_ShouldSucceed()
        {
            // Arrange
            testAccount = new BankAccount(1000M);
            decimal initialBalance = testAccount.Balance;
            decimal withdrawAmount = 100M;
            decimal expectedBalance = initialBalance - withdrawAmount; // 900M

            // Act
            testAccount.Withdraw(withdrawAmount);

            // Assert - Verify balance decreased correctly
            Assert.That(testAccount.Balance, Is.EqualTo(expectedBalance), $"Balance should be {expectedBalance} after withdrawing {withdrawAmount}");

            // Assert - Verify transaction history was recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(1), "TransactionHistory should contain exactly 1 transaction after withdrawal");

            // Assert - Verify transaction details
            var transaction = testAccount.TransactionHistory[0];
            Assert.That(transaction.Amount, Is.EqualTo(withdrawAmount), $"Transaction amount should be {withdrawAmount}");
            Assert.That(transaction.Description, Is.EqualTo($"Withdraw ({withdrawAmount} {testAccount.AccountCurrency})"),
                $"Transaction description should be 'Withdraw ({withdrawAmount} {testAccount.AccountCurrency})' but it is '{transaction.Description}'");

            // Success message
            Assert.Pass($"T1.7: Withdraw of {withdrawAmount} succeeded - Balance: {testAccount.Balance}");
        }

        /// <summary>
        /// T1.8 - IN2 (Inside Boundary): Test withdraw of medium amount (500M)
        /// Boundary value testing: Withdraw larger amount but still valid
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN2: Withdraw medium valid amount (500M) - inside boundary")]
        public void T1_8_Withdraw_IN2_MediumAmount_ShouldSucceed()
        {
            // Arrange
            testAccount = new BankAccount(1000M);
            decimal initialBalance = testAccount.Balance;
            decimal withdrawAmount = 500M;
            decimal expectedBalance = initialBalance - withdrawAmount; // 500M

            // Act
            testAccount.Withdraw(withdrawAmount);

            // Assert - Verify balance decreased correctly
            Assert.That(testAccount.Balance, Is.EqualTo(expectedBalance), $"Balance should be {expectedBalance} after withdrawing {withdrawAmount}");

            // Assert - Verify transaction history was recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(1), "TransactionHistory should contain exactly 1 transaction after withdrawal");

            // Assert - Verify transaction details
            var transaction = testAccount.TransactionHistory[0];
            Assert.That(transaction.Amount, Is.EqualTo(withdrawAmount), $"Transaction amount should be {withdrawAmount}");
            Assert.That(transaction.Description, Is.EqualTo($"Withdraw ({withdrawAmount} {testAccount.AccountCurrency})"),
                $"Transaction description should be 'Withdraw ({withdrawAmount} {testAccount.AccountCurrency})' but it is '{transaction.Description}'");

            // Success message
            Assert.Pass($"T1.8: Withdraw of {withdrawAmount} succeeded - Balance: {testAccount.Balance}");
        }

        /// <summary>
        /// T1.9 - OFF1 (Outside Boundary): Test withdraw exceeding balance (1500M from 1000M)
        /// Boundary value testing: Withdraw more than available balance
        /// Expected: Should throw NotEnoughFundsException
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF1: Withdraw exceeding balance (1500M from 1000M) - outside boundary")]
        public void T1_9_Withdraw_OFF1_ExceedsBalance_ShouldThrowNotEnoughFundsException()
        {
            // Arrange
            testAccount = new BankAccount(1000M);
            decimal withdrawAmount = 1500M; // More than available

            // Act & Assert - Withdraw exceeding balance should throw NotEnoughFundsException
            var exception = Assert.Throws<NotEnoughFundsException>(() =>
                testAccount.Withdraw(withdrawAmount), "Withdraw exceeding balance should throw NotEnoughFundsException");

            // Assert - Verify balance was NOT changed
            Assert.That(testAccount.Balance, Is.EqualTo(1000M), "Balance should remain unchanged when withdrawal throws exception");

            // Assert - Verify no transaction was recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(0), "No transaction should be recorded when withdrawal fails");

            // Success message
            Assert.Pass($"T1.9: Withdraw of {withdrawAmount} correctly threw NotEnoughFundsException");
        }

        /// <summary>
        /// T1.10 - OFF2 (Outside Boundary): Test withdraw that would go below minBalance
        /// Boundary value testing: Withdraw amount that would violate minBalance requirement
        /// Account has 1000M, minBalance is 0.1M, so cannot withdraw more than 999.9M
        /// Attempting to withdraw 999.95M would leave 0.05M (below 0.1M minBalance)
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF2: Withdraw that would go below minBalance - outside boundary")]
        public void T1_10_Withdraw_OFF2_BelowMinBalance_ShouldThrowNotEnoughFundsException()
        {
            // Arrange
            testAccount = new BankAccount(1000M);
            decimal minBalance = testAccount.MinBalance; // 0.1M
            decimal withdrawAmount = 999.95M; // Would leave 0.05M, which is < minBalance
            decimal expectedRemainingBalance = 1000M - withdrawAmount; // 0.05M

            // Act & Assert - Withdrawal that violates minBalance should throw NotEnoughFundsException
            var exception = Assert.Throws<NotEnoughFundsException>(() =>
                testAccount.Withdraw(withdrawAmount), "Withdraw that would go below minBalance should throw NotEnoughFundsException");

            // Assert - Verify balance was NOT changed
            Assert.That(testAccount.Balance, Is.EqualTo(1000M), "Balance should remain unchanged when withdrawal throws exception");

            // Assert - Verify no transaction was recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(0), "No transaction should be recorded when withdrawal fails");

            // Success message
            Assert.Pass($"T1.10: Withdraw of {withdrawAmount} correctly threw NotEnoughFundsException " +
                $"(would leave {expectedRemainingBalance} < minBalance {minBalance})");
        }

        /// <summary>
        /// T1.11 - ON (On Boundary): Test withdraw at exact daily limit (50000M)
        /// Boundary value testing: Withdraw exactly the daily limit allowed
        /// Account starts with 100000M, daily limit set to 50000M
        /// </summary>
        [Test]
        [Category("Domain-ON")]
        [Description("ON: Withdraw at exact daily limit (50000M) - boundary value")]
        public void T1_11_Withdraw_ON_ExactDailyLimit_ShouldSucceed()
        {
            // Arrange
            testAccount = new BankAccount(100000M);
            testAccount.DailyWithdrawLimit = 50000M; 

            decimal dailyLimit = testAccount.DailyWithdrawLimit; // 50000M
            decimal withdrawAmount = dailyLimit; // Exactly at limit
            decimal expectedBalance = 100000M - withdrawAmount; // 50000M

            // Act
            testAccount.Withdraw(withdrawAmount);

            // Assert - Verify balance decreased correctly
            Assert.That(testAccount.Balance, Is.EqualTo(expectedBalance), $"Balance should be {expectedBalance} after withdrawing {withdrawAmount}");

            // Assert - Verify transaction history was recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(1), "TransactionHistory should contain exactly 1 transaction");

            // Assert - Verify transaction details
            var transaction = testAccount.TransactionHistory[0];
            Assert.That(transaction.Amount, Is.EqualTo(withdrawAmount), $"Transaction amount should be {withdrawAmount}");
            Assert.That(transaction.Description, Is.EqualTo($"Withdraw ({withdrawAmount} {testAccount.AccountCurrency})"),
                $"Transaction description should be 'Withdraw ({withdrawAmount} {testAccount.AccountCurrency})'");

            // Success message
            Assert.Pass($"T1.11: Withdraw of {withdrawAmount} at exact daily limit succeeded - Balance: {testAccount.Balance}");
        }

        /// <summary>
        /// T1.12 - IN1 (Inside Boundary): Test withdraw below daily limit (25000M)
        /// Boundary value testing: Withdraw amount well below the daily limit
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN1: Withdraw below daily limit (25000M) - inside boundary")]
        public void T1_12_Withdraw_IN1_BelowDailyLimit_ShouldSucceed()
        {
            // Arrange
            testAccount = new BankAccount(100000M);
            testAccount.DailyWithdrawLimit = 50000M; 

            decimal withdrawAmount = 25000M; // Well below 50000M limit
            decimal expectedBalance = 100000M - withdrawAmount; // 75000M

            // Act
            testAccount.Withdraw(withdrawAmount);

            // Assert - Verify balance decreased correctly
            Assert.That(testAccount.Balance, Is.EqualTo(expectedBalance), $"Balance should be {expectedBalance} after withdrawing {withdrawAmount}");

            // Assert - Verify transaction history was recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(1), "TransactionHistory should contain exactly 1 transaction");

            // Assert - Verify transaction details
            var transaction = testAccount.TransactionHistory[0];
            Assert.That(transaction.Amount, Is.EqualTo(withdrawAmount), $"Transaction amount should be {withdrawAmount}");

            Assert.That(transaction.Description, Is.EqualTo($"Withdraw ({withdrawAmount} {testAccount.AccountCurrency})"),
                $"Transaction description should be 'Withdraw ({withdrawAmount} {testAccount.AccountCurrency})'");

            // Success message
            Assert.Pass($"T1.12: Withdraw of {withdrawAmount} below daily limit succeeded - Balance: {testAccount.Balance}");
        }

        /// <summary>
        /// T1.13 - IN2 (Inside Boundary): Test multiple withdrawals that total below daily limit
        /// Boundary value testing: Two withdrawals that together don't exceed the daily limit
        /// First: 20000M (OK), Second: 20000M (OK, total = 40000M < 50000M limit)
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN2: Multiple withdrawals that total below daily limit - inside boundary")]
        public void T1_13_Withdraw_IN2_MultipleWithdrawsBelowLimit_ShouldSucceed()
        {
            // Arrange
            testAccount = new BankAccount(100000M);
            testAccount.DailyWithdrawLimit = 50000M; 

            decimal firstWithdraw = 20000M;
            decimal secondWithdraw = 20000M;
            decimal totalWithdrawn = firstWithdraw + secondWithdraw; // 40000M
            decimal expectedBalance = 100000M - totalWithdrawn; // 60000M

            // Act - First withdrawal
            testAccount.Withdraw(firstWithdraw);
            Assert.That(testAccount.Balance, Is.EqualTo(100000M - firstWithdraw), $"After first withdraw: balance should be {100000M - firstWithdraw}");

            // Act - Second withdrawal
            testAccount.Withdraw(secondWithdraw);

            // Assert - Verify final balance after two withdrawals
            Assert.That(testAccount.Balance, Is.EqualTo(expectedBalance), $"Balance should be {expectedBalance} after two withdrawals totaling {totalWithdrawn}");

            // Assert - Verify both transactions recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(2), "TransactionHistory should contain exactly 2 transactions");

            // Assert - Verify first transaction
            var firstTransaction = testAccount.TransactionHistory[0];
            Assert.That(firstTransaction.Amount, Is.EqualTo(firstWithdraw), $"First transaction amount should be {firstWithdraw}");

            // Assert - Verify second transaction
            var secondTransaction = testAccount.TransactionHistory[1];
            Assert.That(secondTransaction.Amount, Is.EqualTo(secondWithdraw), $"Second transaction amount should be {secondWithdraw}");

            // Success message
            Assert.Pass($"T1.13: Multiple withdrawals totaling {totalWithdrawn} below limit (50000M) succeeded - Balance: {testAccount.Balance}");
        }

        /// <summary>
        /// T1.14 - OFF1 (Outside Boundary): Test single withdrawal exceeding daily limit
        /// Boundary value testing: Attempt to withdraw more than the daily limit in one transaction
        /// Account daily limit is 50000M, attempting to withdraw 60000M
        /// Expected: Should throw InvalidOperationException
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF1: Single withdrawal exceeding daily limit (60000M > 50000M) - outside boundary")]
        public void T1_14_Withdraw_OFF1_ExceedsDailyLimit_ShouldThrowInvalidOperationException()
        {
            // Arrange
            testAccount = new BankAccount(100000M);
            testAccount.DailyWithdrawLimit = 50000M; 

            decimal dailyLimit = testAccount.DailyWithdrawLimit; // 50000M
            decimal withdrawAmount = 60000M; // Exceeds 50000M limit

            // Act & Assert - Withdrawal exceeding daily limit should throw InvalidOperationException
            var exception = Assert.Throws<InvalidOperationException>(() =>
                testAccount.Withdraw(withdrawAmount), "Withdrawal exceeding daily limit should throw InvalidOperationException");

            // Assert - Verify exception message contains helpful info
            Assert.That(exception.Message, Does.Contain("limit").IgnoreCase, "Exception message should mention daily limit");

            // Assert - Verify balance was NOT changed
            Assert.That(testAccount.Balance, Is.EqualTo(100000M), "Balance should remain unchanged when withdrawal throws exception");

            // Assert - Verify no transaction was recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(0), "No transaction should be recorded when withdrawal fails");

            // Success message
            Assert.Pass($"T1.14: Withdraw of {withdrawAmount} exceeding daily limit {dailyLimit} correctly threw InvalidOperationException");
        }

        /// <summary>
        /// T1.15 - OFF2 (Outside Boundary): Test cumulative withdrawals exceeding daily limit
        /// Boundary value testing: Two withdrawals where cumulative total exceeds the daily limit
        /// First: 40000M (OK), Second: 15000M (NOT OK, total = 55000M > 50000M limit)
        /// Expected: Second withdrawal should throw InvalidOperationException
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF2: Cumulative withdrawals exceeding daily limit - outside boundary")]
        public void T1_15_Withdraw_OFF2_SecondWithdrawExceedsLimit_ShouldThrowInvalidOperationException()
        {
            // Arrange
            testAccount = new BankAccount(100000M);
            testAccount.DailyWithdrawLimit = 50000M; 

            decimal dailyLimit = testAccount.DailyWithdrawLimit; // 50000M
            decimal firstWithdraw = 40000M; // OK (< 50000M)
            decimal secondWithdraw = 15000M; // NOT OK (40000 + 15000 = 55000 > 50000M)
            decimal totalWithdraw = firstWithdraw + secondWithdraw; // 55000M

            // Act - First withdrawal should succeed
            testAccount.Withdraw(firstWithdraw);
            Assert.That(testAccount.Balance, Is.EqualTo(100000M - firstWithdraw), 
                $"After first withdraw: balance should be {100000M - firstWithdraw} but it is {testAccount.Balance}");

            // Act & Assert - Second withdrawal should fail
            var exception = Assert.Throws<InvalidOperationException>(() =>
                testAccount.Withdraw(secondWithdraw), "Second withdrawal exceeding cumulative daily limit should throw InvalidOperationException");

            // Assert - Verify exception message
            Assert.That(exception.Message, Does.Contain("limit").IgnoreCase, "Exception message should mention daily limit");

            // Assert - Verify balance remains after first withdrawal only
            Assert.That(testAccount.Balance, Is.EqualTo(100000M - firstWithdraw), 
                $"Balance should be {100000M - firstWithdraw} but it is {testAccount.Balance} (only first withdrawal applied, second failed)");

            // Assert - Verify only first transaction was recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(1), 
                "TransactionHistory should contain only 1 transaction (the successful first withdrawal)");

            // Assert - Verify first transaction details
            var transaction = testAccount.TransactionHistory[0];
            Assert.That(transaction.Amount, Is.EqualTo(firstWithdraw), $"Transaction amount should be {firstWithdraw}");

            // Success message
            Assert.Pass($"T1.15: Second withdrawal correctly blocked - would exceed daily limit (attempted total: {totalWithdraw}, limit: {dailyLimit})");
        }

        /// <summary>
        /// T1.16 - ON (On Boundary): Test TransferFunds that leaves source at exact minBalance
        /// Boundary value testing: Transfer exactly enough to leave minBalance in source
        /// Source starts with 1000M, minBalance is 0.1M, so can transfer 999.9M
        /// </summary>
        [Test]
        [Category("Domain-ON")]
        [Description("ON: TransferFunds leaving source at exact minBalance - boundary value")]
        public void T1_16_TransferFunds_ON_ExactToMinBalance_ShouldSucceed()
        {
            // Arrange
            sourceAccount = new BankAccount(1000M);
            destinationAccount = new BankAccount(0M);

            decimal sourceMinBalance = sourceAccount.MinBalance; // 0.1M
            decimal transferAmount = 1000M - sourceMinBalance; // 999.9M
            decimal expectedSourceBalance = sourceMinBalance; // 0.1M
            decimal expectedDestinationBalance = transferAmount; // 999.9M

            // Act
            sourceAccount.TransferFunds(destinationAccount, transferAmount);

            // Assert - Verify source balance is now exactly at minBalance
            Assert.That(sourceAccount.Balance, Is.EqualTo(expectedSourceBalance),
                $"Source balance should be {expectedSourceBalance} (minBalance) after TransferFunds");

            // Assert - Verify destination balance increased
            Assert.That(destinationAccount.Balance, Is.EqualTo(expectedDestinationBalance),
                $"Destination balance should be {expectedDestinationBalance} after receiving TransferFunds");

            // Assert - Verify source transaction was recorded
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(2), "Source account should have 2 transaction");

            // Assert - Verify destination transaction was recorded
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(2), "Destination account should have 2 transaction");

            // Assert - Verify source transaction details
            var sourceTransaction = sourceAccount.TransactionHistory[0];
            var sourceTransaction1 = sourceAccount.TransactionHistory[1];
            Assert.That(sourceTransaction.Amount, Is.EqualTo(transferAmount), $"Source transaction amount should be {transferAmount}");
            Assert.That(sourceTransaction.Description, Does.Contain("Withdraw").IgnoreCase, "Source transaction should indicate a transfer");
            Assert.That(sourceTransaction1.Amount, Is.EqualTo(transferAmount), $"Source transaction amount should be {transferAmount}");
            Assert.That(sourceTransaction1.Description, Does.Contain("Transfer").IgnoreCase, "Source transaction should indicate a transfer");

            // Assert - Verify destination transaction details
            var destTransaction = destinationAccount.TransactionHistory[0];
            var destTransactio1 = destinationAccount.TransactionHistory[1];
            Assert.That(destTransaction.Amount, Is.EqualTo(transferAmount), $"Destination transaction amount should be {transferAmount}");
            Assert.That(destTransaction.Description, Does.Contain("Deposit").IgnoreCase, "Destination transaction should indicate a transfer");
            Assert.That(destTransactio1.Amount, Is.EqualTo(transferAmount), $"Destination transaction amount should be {transferAmount}");
            Assert.That(destTransactio1.Description, Does.Contain("Transfer").IgnoreCase, "Destination transaction should indicate a transfer");

            // Success message
            Assert.Pass($"T1.16 [TransferFunds]: Transfer of {transferAmount} succeeded - Source at minBalance: " +
                $"{sourceAccount.Balance}, Destination: {destinationAccount.Balance}");
        }

        /// <summary>
        /// T1.17 - IN1: Small transfer
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN1: Small TransferFunds - inside boundary")]
        public void T1_17_TransferFunds_IN1_SmallAmount_ShouldSucceed()
        {
            // Arrange
            sourceAccount = new BankAccount(1000M);
            destinationAccount = new BankAccount(500M);

            decimal transferAmount = 100M;
            decimal expectedSourceBalance = 900M;
            decimal expectedDestinationBalance = 600M;

            // Act
            sourceAccount.TransferFunds(destinationAccount, transferAmount);

            // Assert
            Assert.That(sourceAccount.Balance, Is.EqualTo(expectedSourceBalance));
            Assert.That(destinationAccount.Balance, Is.EqualTo(expectedDestinationBalance));

            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(2), "Source: Withdraw + Transfer");
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(2), "Destination: Deposit + Transfer received");

            // Success message
            Assert.Pass($"T1.17 [TransferFunds]: Small transfer of {transferAmount} succeeded");
        }

        /// <summary>
        /// T1.18 - IN2: Large transfer
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN2: Large TransferFunds - inside boundary")]
        public void T1_18_TransferFunds_IN2_LargeAmount_ShouldSucceed()
        {
            // Arrange
            sourceAccount = new BankAccount(10000M);
            destinationAccount = new BankAccount(0M);

            decimal transferAmount = 5000M;
            decimal expectedSourceBalance = 5000M;
            decimal expectedDestinationBalance = 5000M;

            // Act
            sourceAccount.TransferFunds(destinationAccount, transferAmount);

            // Assert
            Assert.That(sourceAccount.Balance, Is.EqualTo(expectedSourceBalance));
            Assert.That(destinationAccount.Balance, Is.EqualTo(expectedDestinationBalance));

            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(2));
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(2));

            // Success message
            Assert.Pass($"T1.18 [TransferFunds]: Large transfer of {transferAmount} succeeded");
        }

        /// <summary>
        /// T1.19 - OFF1: Transfer exceeding source balance
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF1: TransferFunds exceeding source balance - outside boundary")]
        public void T1_19_TransferFunds_OFF1_ExceedsBalance_ShouldThrowNotEnoughFundsException()
        {
            // Arrange
            sourceAccount = new BankAccount(1000M);
            destinationAccount = new BankAccount(0M);
            decimal transferAmount = 1500M;

            // Act & Assert
            Assert.Throws<NotEnoughFundsException>(() =>
                sourceAccount.TransferFunds(destinationAccount, transferAmount));

            // Verify NO transactions recorded 
            Assert.That(sourceAccount.Balance, Is.EqualTo(1000M), "Source balance changed on failure");
            Assert.That(destinationAccount.Balance, Is.EqualTo(0M), "Destination balance changed on failure");
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(0), "Source should have NO transactions on failed transfer");
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(0), "Destination should have NO transactions on failed transfer");

            // Success message
            Assert.Pass($"T1.19 [TransferFunds]: Correctly threw NotEnoughFundsException");
        }

        /// <summary>
        /// T1.20 - OFF2: Transfer violating minBalance
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF2: TransferFunds violates minBalance - outside boundary")]
        public void T1_20_TransferFunds_OFF2_WouldLeaveBelowMinBalance_ShouldThrowNotEnoughFundsException()
        {
            // Arrange
            sourceAccount = new BankAccount(1000M);
            destinationAccount = new BankAccount(0M);
            decimal transferAmount = 999.95M;

            // Act & Assert
            Assert.Throws<NotEnoughFundsException>(() =>
                sourceAccount.TransferFunds(destinationAccount, transferAmount));

            // Verify NO transactions 
            Assert.That(sourceAccount.Balance, Is.EqualTo(1000M));
            Assert.That(destinationAccount.Balance, Is.EqualTo(0M));
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(0));
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(0));

            // Success message
            Assert.Pass($"T1.20 [TransferFunds]: Blocked transfer violating minBalance");
        }

        /// <summary>
        /// T1.16 - ON (On Boundary): Test TransferMinFunds that leaves source at exact minBalance
        /// Source has 1000M, requesting transfer of 999.9M
        /// Since balance (1000) - amount (999.9) >= minBalance (0.1), full transfer succeeds
        /// Result: Source = 0.1M (minBalance), Destination receives full 999.9M
        /// </summary>
        [Test]
        [Category("Domain-ON")]
        [Description("ON: TransferMinFunds leaving source at exact minBalance - boundary value")]
        public void T1_16_TransferMinFunds_ON_ExactToMinBalance_ShouldSucceed()
        {
            // Arrange
            sourceAccount = new BankAccount(1000M);
            destinationAccount = new BankAccount(0M);

            decimal sourceMinBalance = sourceAccount.MinBalance; // 0.1M
            decimal transferAmount = 999.9M; // Exactly to minBalance
            decimal expectedSourceBalance = sourceMinBalance; // 0.1M
            decimal expectedDestinationBalance = transferAmount; // 999.9M (full amount)

            // Act
            sourceAccount.TransferMinFunds(destinationAccount, transferAmount);

            // Assert - Verify balances
            Assert.That(sourceAccount.Balance, Is.EqualTo(expectedSourceBalance),
                $"Source balance should be {expectedSourceBalance} (minBalance) after TransferMinFunds");
            Assert.That(destinationAccount.Balance, Is.EqualTo(expectedDestinationBalance),
                $"Destination balance should be {expectedDestinationBalance} (full requested amount)");

            // Assert - Both accounts have 2 transactions
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(2),
                "Source should have 2 transactions: Withdraw + Transfer log");
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(2),
                "Destination should have 2 transactions: Deposit + Transfer received log");

            // Verify source transactions
            var sourceWithdraw = sourceAccount.TransactionHistory[0];
            Assert.That(sourceWithdraw.Amount, Is.EqualTo(transferAmount),
                $"Source withdraw amount should be {transferAmount}");
            Assert.That(sourceWithdraw.Description, Does.Contain("Withdraw").IgnoreCase,
                "First source transaction should be Withdraw");

            var sourceTransfer = sourceAccount.TransactionHistory[1];
            Assert.That(sourceTransfer.Amount, Is.EqualTo(transferAmount),
                $"Source transfer log amount should be {transferAmount}");
            Assert.That(sourceTransfer.Description, Does.Contain("Transfer").IgnoreCase,
                "Second source transaction should be Transfer log");

            // Verify destination transactions
            var destDeposit = destinationAccount.TransactionHistory[0];
            Assert.That(destDeposit.Amount, Is.EqualTo(transferAmount),
                $"Destination deposit amount should be {transferAmount}");
            Assert.That(destDeposit.Description, Does.Contain("Deposit").IgnoreCase,
                "First destination transaction should be Deposit");

            var destTransfer = destinationAccount.TransactionHistory[1];
            Assert.That(destTransfer.Amount, Is.EqualTo(transferAmount),
                $"Destination transfer log amount should be {transferAmount}");
            Assert.That(destTransfer.Description, Does.Contain("Transfer").IgnoreCase,
                "Second destination transaction should be Transfer received log");

            // Success message
            Assert.Pass($"T1.16 [TransferMinFunds]: Transfer of {transferAmount} succeeded - Source: {sourceAccount.Balance}, Dest: {destinationAccount.Balance}");
        }

        /// <summary>
        /// T1.17 - IN1 (Inside Boundary): Test small TransferMinFunds
        /// Source has 1000M, requesting transfer of 100M
        /// Since balance (1000) - amount (100) = 900 >= minBalance (0.1), full transfer succeeds
        /// Result: Source = 900M, Destination receives full 100M
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN1: Small TransferMinFunds - inside boundary")]
        public void T1_17_TransferMinFunds_IN1_SmallAmount_ShouldSucceed()
        {
            // Arrange
            sourceAccount = new BankAccount(1000M);
            destinationAccount = new BankAccount(500M);

            decimal transferAmount = 100M;
            decimal expectedSourceBalance = 900M;
            decimal expectedDestinationBalance = 600M; // 500 + 100

            // Act
            sourceAccount.TransferMinFunds(destinationAccount, transferAmount);

            // Assert - Verify balances
            Assert.That(sourceAccount.Balance, Is.EqualTo(expectedSourceBalance),
                $"Source balance should be {expectedSourceBalance}");
            Assert.That(destinationAccount.Balance, Is.EqualTo(expectedDestinationBalance),
                $"Destination balance should be {expectedDestinationBalance}");

            // Assert - Both accounts have 2 transactions
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(2),
                "Source: Withdraw + Transfer");
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(2),
                "Destination: Deposit + Transfer received");

            // Success message
            Assert.Pass($"T1.17 [TransferMinFunds]: Small transfer of {transferAmount} succeeded - Source: {sourceAccount.Balance}, Dest: {destinationAccount.Balance}");
        }

        /// <summary>
        /// T1.18 - IN2 (Inside Boundary): Test large TransferMinFunds
        /// Source has 10000M, requesting transfer of 5000M
        /// Since balance (10000) - amount (5000) = 5000 >= minBalance (0.1), full transfer succeeds
        /// Result: Source = 5000M, Destination receives full 5000M
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN2: Large TransferMinFunds - inside boundary")]
        public void T1_18_TransferMinFunds_IN2_LargeAmount_ShouldSucceed()
        {
            // Arrange
            sourceAccount = new BankAccount(10000M);
            destinationAccount = new BankAccount(0M);

            decimal transferAmount = 5000M;
            decimal expectedSourceBalance = 5000M;
            decimal expectedDestinationBalance = 5000M;

            // Act
            sourceAccount.TransferMinFunds(destinationAccount, transferAmount);

            // Assert - Verify balances
            Assert.That(sourceAccount.Balance, Is.EqualTo(expectedSourceBalance),
                $"Source balance should be {expectedSourceBalance}");
            Assert.That(destinationAccount.Balance, Is.EqualTo(expectedDestinationBalance),
                $"Destination balance should be {expectedDestinationBalance}");

            // Assert - Both accounts have 2 transactions
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(2));
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(2));

            // Success message
            Assert.Pass($"T1.18 [TransferMinFunds]: Large transfer of {transferAmount} succeeded - Source: {sourceAccount.Balance}, Dest: {destinationAccount.Balance}");
        }

        /// <summary>
        /// T1.19 - OFF1 (Outside Boundary): Test TransferMinFunds exceeding source balance
        /// Source has 1000M, requesting transfer of 1500M
        /// Since requested amount (1500) > balance (1000), throws NotEnoughFundsException
        /// Note: Even with minBalance adjustment, can't transfer more than you have
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF1: TransferMinFunds exceeding source balance - outside boundary")]
        public void T1_19_TransferMinFunds_OFF1_ExceedsBalance_ShouldThrowNotEnoughFundsException()
        {
            // Arrange
            sourceAccount = new BankAccount(1000M);
            destinationAccount = new BankAccount(0M);
            decimal transferAmount = 1500M; // More than source has

            // Act & Assert - Transfer exceeding balance should throw NotEnoughFundsException
            var exception = Assert.Throws<NotEnoughFundsException>(() =>
                sourceAccount.TransferMinFunds(destinationAccount, transferAmount),
                "TransferMinFunds exceeding source balance should throw NotEnoughFundsException");

            // Assert - Verify NO transactions recorded (atomic operation)
            Assert.That(sourceAccount.Balance, Is.EqualTo(1000M),
                "Source balance should remain unchanged when transfer throws exception");
            Assert.That(destinationAccount.Balance, Is.EqualTo(0M),
                "Destination balance should remain unchanged when transfer throws exception");
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(0),
                "Source should have NO transactions on failed transfer");
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(0),
                "Destination should have NO transactions on failed transfer");

            // Success message
            Assert.Pass($"T1.19 [TransferMinFunds]: Transfer of {transferAmount} correctly threw NotEnoughFundsException - cannot exceed balance");
        }

        /// <summary>
        /// T1.20 - OFF2 (Outside Boundary): Test TransferMinFunds that would violate minBalance
        /// Source has 1000M, requesting transfer of 999.95M
        /// Since balance (1000) - amount (999.95) = 0.05 < minBalance (0.1), 
        /// TransferMinFunds ADJUSTS and transfers (999.95 - 0.1) = 999.85M instead
        /// Result: Source = 0.1M (protected), Destination receives 999.85M (adjusted amount)
        /// This is KEY DIFFERENCE from other transfer methods!
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF2: TransferMinFunds auto-adjusts to protect minBalance - outside boundary")]
        public void T1_20_TransferMinFunds_OFF2_AutoAdjustsToProtectMinBalance_ShouldSucceed()
        {
            // Arrange
            sourceAccount = new BankAccount(1000M);
            destinationAccount = new BankAccount(0M);

            decimal sourceMinBalance = sourceAccount.MinBalance; // 0.1M
            decimal requestedAmount = 999.95M; // Would leave 0.05M < minBalance
            decimal actualTransferAmount = sourceAccount.Balance - sourceMinBalance; // 999.9M (auto-adjusted)
            decimal expectedSourceBalance = sourceMinBalance; // 0.1M (protected!)
            decimal expectedDestinationBalance = actualTransferAmount; // 999.85M (adjusted, not requested)

            // Act - TransferMinFunds auto-adjusts to protect minBalance
            sourceAccount.TransferMinFunds(destinationAccount, requestedAmount);

            // Assert - Verify source protected at minBalance
            Assert.That(sourceAccount.Balance, Is.EqualTo(expectedSourceBalance),
                $"Source balance should be {expectedSourceBalance} (minBalance protected)");

            // Assert - Verify destination received ADJUSTED amount (not requested amount)
            Assert.That(destinationAccount.Balance, Is.EqualTo(expectedDestinationBalance),
                $"Destination should receive {expectedDestinationBalance} (auto-adjusted: requested {requestedAmount} - minBalance {sourceMinBalance})");

            // Assert - Verify destination did NOT receive full requested amount
            Assert.That(destinationAccount.Balance, Is.Not.EqualTo(requestedAmount),
                $"Destination should NOT receive full requested amount ({requestedAmount}) due to minBalance protection");

            // Assert - Both accounts have 2 transactions (transfer succeeded with adjustment)
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(2),
                "Source should have 2 transactions: Withdraw + Transfer log");
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(2),
                "Destination should have 2 transactions: Deposit + Transfer received log");

            // Success message
            Assert.Pass($"T1.20 [TransferMinFunds]: AUTO-ADJUSTED transfer succeeded - Requested: {requestedAmount}, Actually transferred: {actualTransferAmount} to protect minBalance");
        }

        /// <summary>
        /// T1.16 - ON (On Boundary): Test TransferRonToEur leaving RON source at exact minBalance
        /// Source: RON account with 1000M RON
        /// Destination: EUR account with 0 EUR
        /// Transfer: 999.9M RON -> 199.98 EUR (999.9 ÷ 5 = 199.98)
        /// Result: Source at 0.1M RON (minBalance), Destination has 199.98 EUR
        /// </summary>
        [Test]
        [Category("Domain-ON")]
        [Description("ON: TransferRonToEur leaving RON source at exact minBalance - boundary value")]
        public void T1_16_TransferRonToEur_ON_ExactToMinBalance_ShouldSucceed()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);

            sourceAccount = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);
            destinationAccount = new BankAccount(0M, stubConverter, 0.02M, Currency.EUR);

            decimal sourceMinBalance = sourceAccount.MinBalance; // 0.1M
            decimal transferRon = 1000M - sourceMinBalance; // 999.9M RON
            decimal expectedSourceBalance = sourceMinBalance; // 0.1M RON
            decimal expectedDestinationBalance = transferRon / 5.0M; // 199.98M EUR (999.9 ÷ 5)

            // Act
            sourceAccount.TransferRonToEur(destinationAccount, transferRon);

            // Assert - Verify RON source at minBalance
            Assert.That(sourceAccount.Balance, Is.EqualTo(expectedSourceBalance),
                $"RON source balance should be {expectedSourceBalance} (minBalance) after TransferRonToEur");

            // Assert - Verify EUR destination increased by converted amount
            Assert.That(destinationAccount.Balance, Is.EqualTo(expectedDestinationBalance),
                $"EUR destination balance should be {expectedDestinationBalance} after receiving conversion");

            // Assert - Verify currencies maintained
            Assert.That(sourceAccount.AccountCurrency, Is.EqualTo(Currency.RON),
                "Source should remain RON currency");
            Assert.That(destinationAccount.AccountCurrency, Is.EqualTo(Currency.EUR),
                "Destination should remain EUR currency");

            // Assert - Both accounts have 2 transactions
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(2),
                "RON source should have 2 transactions: Withdraw + Transfer log");
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(2),
                "EUR destination should have 2 transactions: Deposit + Transfer received log");

            // Verify source transactions
            var sourceWithdraw = sourceAccount.TransactionHistory[0];
            Assert.That(sourceWithdraw.Amount, Is.EqualTo(transferRon),
                $"Source withdraw should be {transferRon} RON");
            Assert.That(sourceWithdraw.Description, Does.Contain("Withdraw").IgnoreCase);

            var sourceTransfer = sourceAccount.TransactionHistory[1];
            Assert.That(sourceTransfer.Amount, Is.EqualTo(transferRon),
                $"Source transfer log should show {transferRon} RON");
            Assert.That(sourceTransfer.Description, Does.Contain("Transfer").IgnoreCase);

            // Verify destination transactions
            var destDeposit = destinationAccount.TransactionHistory[0];
            Assert.That(destDeposit.Amount, Is.EqualTo(expectedDestinationBalance),
                $"Destination deposit should be {expectedDestinationBalance} EUR (converted)");
            Assert.That(destDeposit.Description, Does.Contain("Deposit").IgnoreCase);

            var destTransfer = destinationAccount.TransactionHistory[1];
            Assert.That(destTransfer.Amount, Is.EqualTo(expectedDestinationBalance),
                $"Destination transfer log should show {expectedDestinationBalance} EUR");
            Assert.That(destTransfer.Description, Does.Contain("Transfer").IgnoreCase);

            // Success message
            Assert.Pass($"T1.16 [TransferRonToEur]: {transferRon}M RON -> {expectedDestinationBalance}M EUR at minBalance boundary");
        }

        /// <summary>
        /// T1.17 - IN1 (Inside Boundary): Test small TransferRonToEur
        /// Source: RON account with 1000M RON
        /// Destination: EUR account with 200M EUR
        /// Transfer: 500M RON -> 100M EUR (500 ÷ 5 = 100)
        /// Result: Source = 500M RON, Destination = 300M EUR (200 + 100)
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN1: Small TransferRonToEur - inside boundary")]
        public void T1_17_TransferRonToEur_IN1_SmallAmount_ShouldSucceed()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);

            sourceAccount = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);
            destinationAccount = new BankAccount(200M, stubConverter, 0.02M, Currency.EUR);

            decimal transferRon = 500M;
            decimal convertedEur = transferRon / 5.0M; // 100M EUR
            decimal expectedSourceBalance = 500M; // 1000 - 500
            decimal expectedDestinationBalance = 300M; // 200 + 100

            // Act
            sourceAccount.TransferRonToEur(destinationAccount, transferRon);

            // Assert - Verify balances
            Assert.That(sourceAccount.Balance, Is.EqualTo(expectedSourceBalance),
                $"RON source balance should be {expectedSourceBalance}");
            Assert.That(destinationAccount.Balance, Is.EqualTo(expectedDestinationBalance),
                $"EUR destination balance should be {expectedDestinationBalance}");

            // Assert - Both accounts have 2 transactions
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(2),
                "RON source: Withdraw + Transfer");
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(2),
                "EUR destination: Deposit + Transfer received");

            // Success message
            Assert.Pass($"T1.17 [TransferRonToEur]: {transferRon}M RON -> {convertedEur}M EUR conversion succeeded");
        }

        /// <summary>
        /// T1.18 - IN2 (Inside Boundary): Test large TransferRonToEur
        /// Source: RON account with 5000M RON
        /// Destination: EUR account with 0M EUR
        /// Transfer: 2500M RON -> 500M EUR (2500 ÷ 5 = 500)
        /// Result: Source = 2500M RON, Destination = 500M EUR
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN2: Large TransferRonToEur - inside boundary")]
        public void T1_18_TransferRonToEur_IN2_LargeAmount_ShouldSucceed()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);

            sourceAccount = new BankAccount(5000M, stubConverter, 0.02M, Currency.RON);
            destinationAccount = new BankAccount(0M, stubConverter, 0.02M, Currency.EUR);

            decimal transferRon = 2500M;
            decimal convertedEur = transferRon / 5.0M; // 500M EUR
            decimal expectedSourceBalance = 2500M; // 5000 - 2500
            decimal expectedDestinationBalance = 500M; // 0 + 500

            // Act
            sourceAccount.TransferRonToEur(destinationAccount, transferRon);

            // Assert - Verify balances
            Assert.That(sourceAccount.Balance, Is.EqualTo(expectedSourceBalance),
                $"RON source balance should be {expectedSourceBalance}");
            Assert.That(destinationAccount.Balance, Is.EqualTo(expectedDestinationBalance),
                $"EUR destination balance should be {expectedDestinationBalance}");

            // Assert - Both accounts have 2 transactions
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(2));
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(2));

            // Success message
            Assert.Pass($"T1.18 [TransferRonToEur]: Large {transferRon}M RON -> {convertedEur}M EUR conversion succeeded");
        }

        /// <summary>
        /// T1.19 - OFF1 (Outside Boundary): Test TransferRonToEur exceeding RON source balance
        /// Source: RON account with 1000M RON
        /// Destination: EUR account with 0M EUR
        /// Attempt to transfer: 1500M RON (more than source has)
        /// Expected: Should throw NotEnoughFundsException
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF1: TransferRonToEur exceeding RON source balance - outside boundary")]
        public void T1_19_TransferRonToEur_OFF1_ExceedsBalance_ShouldThrowNotEnoughFundsException()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);

            sourceAccount = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);
            destinationAccount = new BankAccount(0M, stubConverter, 0.02M, Currency.EUR);

            decimal transferRon = 1500M; // More than source RON has

            // Act & Assert - Transfer exceeding RON balance should throw NotEnoughFundsException
            var exception = Assert.Throws<NotEnoughFundsException>(() =>
                sourceAccount.TransferRonToEur(destinationAccount, transferRon),
                "TransferRonToEur exceeding RON balance should throw NotEnoughFundsException");

            // Assert - Verify NO changes (atomic operation)
            Assert.That(sourceAccount.Balance, Is.EqualTo(1000M),
                "RON source balance should remain unchanged on failure");
            Assert.That(destinationAccount.Balance, Is.EqualTo(0M),
                "EUR destination balance should remain unchanged on failure");

            // Assert - Verify NO transactions recorded
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(0),
                "Source should have NO transactions on failed transfer");
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(0),
                "Destination should have NO transactions on failed transfer");

            // Success message
            Assert.Pass($"T1.19 [TransferRonToEur]: Transfer of {transferRon}M RON correctly threw NotEnoughFundsException");
        }

        /// <summary>
        /// T1.20 - OFF2 (Outside Boundary): Test TransferRonToEur violating RON minBalance
        /// Source: RON account with 1000M RON, minBalance = 0.1M
        /// Destination: EUR account with 0M EUR
        /// Attempt to transfer: 999.95M RON (would leave 0.05M < minBalance)
        /// Expected: Should throw NotEnoughFundsException
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF2: TransferRonToEur violates RON minBalance - outside boundary")]
        public void T1_20_TransferRonToEur_OFF2_WouldLeaveBelowMinBalance_ShouldThrowNotEnoughFundsException()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);

            sourceAccount = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);
            destinationAccount = new BankAccount(0M, stubConverter, 0.02M, Currency.EUR);

            decimal sourceMinBalance = sourceAccount.MinBalance; // 0.1M
            decimal transferRon = 999.95M; // Would leave 0.05M < minBalance
            decimal remainingBalance = 1000M - transferRon; // 0.05M

            // Act & Assert - Transfer violating minBalance should throw NotEnoughFundsException
            var exception = Assert.Throws<NotEnoughFundsException>(() =>
                sourceAccount.TransferRonToEur(destinationAccount, transferRon),
                "TransferRonToEur violating minBalance should throw NotEnoughFundsException");

            // Assert - Verify NO changes (atomic operation)
            Assert.That(sourceAccount.Balance, Is.EqualTo(1000M),
                "RON source balance should remain unchanged on failure");
            Assert.That(destinationAccount.Balance, Is.EqualTo(0M),
                "EUR destination balance should remain unchanged on failure");

            // Assert - Verify NO transactions recorded
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(0),
                "Source should have NO transactions when transfer fails");
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(0),
                "Destination should have NO transactions when transfer fails");

            // Success message
            Assert.Pass($"T1.20 [TransferRonToEur]: Transfer blocked - would leave RON source with {remainingBalance} < minBalance {sourceMinBalance}");
        }

        /// <summary>
        /// T1.16 - ON (On Boundary): Test TransferEurToRon leaving EUR source at exact minBalance
        /// Source: EUR account with 200M EUR
        /// Destination: RON account with 0 RON
        /// Transfer: 199.9M EUR -> 999.5M RON (199.9 × 5 = 999.5)
        /// Result: Source at 0.1M EUR (minBalance), Destination has 999.5M RON
        /// </summary>
        [Test]
        [Category("Domain-ON")]
        [Description("ON: TransferEurToRon leaving EUR source at exact minBalance - boundary value")]
        public void T1_16_TransferEurToRon_ON_ExactToMinBalance_ShouldSucceed()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);

            sourceAccount = new BankAccount(200M, stubConverter, 0.02M, Currency.EUR);
            destinationAccount = new BankAccount(0M, stubConverter, 0.02M, Currency.RON);

            decimal sourceMinBalance = sourceAccount.MinBalance; // 0.1M
            decimal transferEur = 200M - sourceMinBalance; // 199.9M EUR
            decimal expectedSourceBalance = sourceMinBalance; // 0.1M EUR
            decimal expectedDestinationBalance = transferEur * 5.0M; // 999.5M RON (199.9 × 5)

            // Act
            sourceAccount.TransferEurToRon(destinationAccount, transferEur);

            // Assert - Verify EUR source at minBalance
            Assert.That(sourceAccount.Balance, Is.EqualTo(expectedSourceBalance),
                $"EUR source balance should be {expectedSourceBalance} (minBalance) after TransferEurToRon");

            // Assert - Verify RON destination increased by converted amount
            Assert.That(destinationAccount.Balance, Is.EqualTo(expectedDestinationBalance),
                $"RON destination balance should be {expectedDestinationBalance} after receiving conversion");

            // Assert - Verify currencies maintained
            Assert.That(sourceAccount.AccountCurrency, Is.EqualTo(Currency.EUR),
                "Source should remain EUR currency");
            Assert.That(destinationAccount.AccountCurrency, Is.EqualTo(Currency.RON),
                "Destination should remain RON currency");

            // Assert - Both accounts have 2 transactions
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(2),
                "EUR source should have 2 transactions: Withdraw + Transfer log");
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(2),
                "RON destination should have 2 transactions: Deposit + Transfer received log");

            // Verify source transactions
            var sourceWithdraw = sourceAccount.TransactionHistory[0];
            Assert.That(sourceWithdraw.Amount, Is.EqualTo(transferEur),
                $"Source withdraw should be {transferEur} EUR");
            Assert.That(sourceWithdraw.Description, Does.Contain("Withdraw").IgnoreCase);

            var sourceTransfer = sourceAccount.TransactionHistory[1];
            Assert.That(sourceTransfer.Amount, Is.EqualTo(transferEur),
                $"Source transfer log should show {transferEur} EUR");
            Assert.That(sourceTransfer.Description, Does.Contain("Transfer").IgnoreCase);

            // Verify destination transactions
            var destDeposit = destinationAccount.TransactionHistory[0];
            Assert.That(destDeposit.Amount, Is.EqualTo(expectedDestinationBalance),
                $"Destination deposit should be {expectedDestinationBalance} RON (converted)");
            Assert.That(destDeposit.Description, Does.Contain("Deposit").IgnoreCase);

            var destTransfer = destinationAccount.TransactionHistory[1];
            Assert.That(destTransfer.Amount, Is.EqualTo(expectedDestinationBalance),
                $"Destination transfer log should show {expectedDestinationBalance} RON");
            Assert.That(destTransfer.Description, Does.Contain("Transfer").IgnoreCase);

            // Success message
            Assert.Pass($"T1.16 [TransferEurToRon]: {transferEur}M EUR -> {expectedDestinationBalance}M RON at minBalance boundary");
        }

        /// <summary>
        /// T1.17 - IN1 (Inside Boundary): Test small TransferEurToRon
        /// Source: EUR account with 200M EUR
        /// Destination: RON account with 1000M RON
        /// Transfer: 100M EUR -> 500M RON (100 × 5 = 500)
        /// Result: Source = 100M EUR, Destination = 1500M RON (1000 + 500)
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN1: Small TransferEurToRon - inside boundary")]
        public void T1_17_TransferEurToRon_IN1_SmallAmount_ShouldSucceed()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);

            sourceAccount = new BankAccount(200M, stubConverter, 0.02M, Currency.EUR);
            destinationAccount = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);

            decimal transferEur = 100M;
            decimal convertedRon = transferEur * 5.0M; // 500M RON
            decimal expectedSourceBalance = 100M; // 200 - 100
            decimal expectedDestinationBalance = 1500M; // 1000 + 500

            // Act
            sourceAccount.TransferEurToRon(destinationAccount, transferEur);

            // Assert - Verify balances
            Assert.That(sourceAccount.Balance, Is.EqualTo(expectedSourceBalance),
                $"EUR source balance should be {expectedSourceBalance}");
            Assert.That(destinationAccount.Balance, Is.EqualTo(expectedDestinationBalance),
                $"RON destination balance should be {expectedDestinationBalance}");

            // Assert - Both accounts have 2 transactions
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(2),
                "EUR source: Withdraw + Transfer");
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(2),
                "RON destination: Deposit + Transfer received");

            // Success message
            Assert.Pass($"T1.17 [TransferEurToRon]: {transferEur}M EUR -> {convertedRon}M RON conversion succeeded");
        }

        /// <summary>
        /// T1.18 - IN2 (Inside Boundary): Test large TransferEurToRon
        /// Source: EUR account with 300M EUR
        /// Destination: RON account with 0M RON
        /// Transfer: 150M EUR -> 750M RON (150 × 5 = 750)
        /// Result: Source = 150M EUR, Destination = 750M RON
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN2: Large TransferEurToRon - inside boundary")]
        public void T1_18_TransferEurToRon_IN2_LargeAmount_ShouldSucceed()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);

            sourceAccount = new BankAccount(300M, stubConverter, 0.02M, Currency.EUR);
            destinationAccount = new BankAccount(0M, stubConverter, 0.02M, Currency.RON);

            decimal transferEur = 150M;
            decimal convertedRon = transferEur * 5.0M; // 750M RON
            decimal expectedSourceBalance = 150M; // 300 - 150
            decimal expectedDestinationBalance = 750M; // 0 + 750

            // Act
            sourceAccount.TransferEurToRon(destinationAccount, transferEur);

            // Assert - Verify balances
            Assert.That(sourceAccount.Balance, Is.EqualTo(expectedSourceBalance),
                $"EUR source balance should be {expectedSourceBalance}");
            Assert.That(destinationAccount.Balance, Is.EqualTo(expectedDestinationBalance),
                $"RON destination balance should be {expectedDestinationBalance}");

            // Assert - Both accounts have 2 transactions
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(2));
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(2));

            // Success message
            Assert.Pass($"T1.18 [TransferEurToRon]: Large {transferEur}M EUR -> {convertedRon}M RON conversion succeeded");
        }

        /// <summary>
        /// T1.19 - OFF1 (Outside Boundary): Test TransferEurToRon exceeding EUR source balance
        /// Source: EUR account with 200M EUR
        /// Destination: RON account with 0M RON
        /// Attempt to transfer: 300M EUR (more than source has)
        /// Expected: Should throw NotEnoughFundsException
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF1: TransferEurToRon exceeding EUR source balance - outside boundary")]
        public void T1_19_TransferEurToRon_OFF1_ExceedsBalance_ShouldThrowNotEnoughFundsException()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);

            sourceAccount = new BankAccount(200M, stubConverter, 0.02M, Currency.EUR);
            destinationAccount = new BankAccount(0M, stubConverter, 0.02M, Currency.RON);

            decimal transferEur = 300M; // More than source EUR has

            // Act & Assert - Transfer exceeding EUR balance should throw NotEnoughFundsException
            var exception = Assert.Throws<NotEnoughFundsException>(() =>
                sourceAccount.TransferEurToRon(destinationAccount, transferEur),
                "TransferEurToRon exceeding EUR balance should throw NotEnoughFundsException");

            // Assert - Verify NO changes (atomic operation)
            Assert.That(sourceAccount.Balance, Is.EqualTo(200M),
                "EUR source balance should remain unchanged on failure");
            Assert.That(destinationAccount.Balance, Is.EqualTo(0M),
                "RON destination balance should remain unchanged on failure");

            // Assert - Verify NO transactions recorded
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(0),
                "Source should have NO transactions on failed transfer");
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(0),
                "Destination should have NO transactions on failed transfer");

            // Success message
            Assert.Pass($"T1.19 [TransferEurToRon]: Transfer of {transferEur}M EUR correctly threw NotEnoughFundsException");
        }

        /// <summary>
        /// T1.20 - OFF2 (Outside Boundary): Test TransferEurToRon violating EUR minBalance
        /// Source: EUR account with 200M EUR, minBalance = 0.1M
        /// Destination: RON account with 0M RON
        /// Attempt to transfer: 199.95M EUR (would leave 0.05M < minBalance)
        /// Expected: Should throw NotEnoughFundsException
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF2: TransferEurToRon violates EUR minBalance - outside boundary")]
        public void T1_20_TransferEurToRon_OFF2_WouldLeaveBelowMinBalance_ShouldThrowNotEnoughFundsException()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);

            sourceAccount = new BankAccount(200M, stubConverter, 0.02M, Currency.EUR);
            destinationAccount = new BankAccount(0M, stubConverter, 0.02M, Currency.RON);

            decimal sourceMinBalance = sourceAccount.MinBalance; // 0.1M
            decimal transferEur = 199.95M; // Would leave 0.05M < minBalance
            decimal wouldLeave = 200M - transferEur; // 0.05M

            // Act & Assert - Transfer violating minBalance should throw NotEnoughFundsException
            var exception = Assert.Throws<NotEnoughFundsException>(() =>
                sourceAccount.TransferEurToRon(destinationAccount, transferEur),
                "TransferEurToRon violating minBalance should throw NotEnoughFundsException");

            // Assert - Verify NO changes (atomic operation)
            Assert.That(sourceAccount.Balance, Is.EqualTo(200M),
                "EUR source balance should remain unchanged on failure");
            Assert.That(destinationAccount.Balance, Is.EqualTo(0M),
                "RON destination balance should remain unchanged on failure");

            // Assert - Verify NO transactions recorded
            Assert.That(sourceAccount.TransactionHistory.Count, Is.EqualTo(0),
                "Source should have NO transactions when transfer fails");
            Assert.That(destinationAccount.TransactionHistory.Count, Is.EqualTo(0),
                "Destination should have NO transactions when transfer fails");

            // Success message
            Assert.Pass($"T1.20 [TransferEurToRon]: Transfer blocked - would leave EUR source with {wouldLeave} < minBalance {sourceMinBalance}");
        }

        /// <summary>
        /// T1.21 - ON (On Boundary): Test AddInterest for one day (minimum valid days)
        /// Boundary value testing: Testing at exact minimum valid boundary (1 day)
        /// Account: 1000M balance, 0.02 (2%) interest rate
        /// Calculation: 1000 + (1000 × 0.02 × 1/365) = 1000.0548M
        /// Formula: Interest = 1000 × 0.02 × (1/365) = 0.0548M
        /// </summary>
        [Test]
        [Category("Domain-ON")]
        [Description("ON: AddInterest for one day (minimum boundary) - 1/365 of annual rate")]
        public void T1_21_AddInterest_ON_OneDay_ShouldCalculateCorrectly()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);

            decimal initialBalance = 1000M;
            decimal interestRate = 0.02M; // 2% annual
            int days = 1; // One day (minimum valid)

            decimal interestAmount = initialBalance * interestRate * (days / 365M);
            interestAmount = Math.Round(interestAmount, 2, MidpointRounding.AwayFromZero);
            decimal expectedBalance = initialBalance + interestAmount; // 1000.0548M

            testAccount = new BankAccount(initialBalance, stubConverter, interestRate, Currency.RON);

            // Verify initial state
            Assert.That(testAccount.Balance, Is.EqualTo(initialBalance),
                "Initial balance should be 1000M");
            Assert.That(testAccount.InterestRate, Is.EqualTo(interestRate),
                "Interest rate should be 0.02");

            // Act
            testAccount.ApplyInterest(days);

            // Assert - Verify balance increased by interest
            Assert.That(testAccount.Balance, Is.EqualTo(expectedBalance),
                $"Balance should be {expectedBalance}M after applying interest for {days} day(s)");

            // Assert - Verify interest amount is correct
            decimal expectedInterest = 0.05M; // 1000 × 0.02 × (1/365)
            decimal actualInterest = testAccount.Balance - initialBalance;
            Assert.That(actualInterest, Is.EqualTo(expectedInterest).Within(0.0001M),
                $"Interest amount should be approximately {expectedInterest}M");

            // Assert - Verify transaction recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.GreaterThan(0),
                "Transaction history should record interest");

            // Verify interest transaction
            var interestTransaction = testAccount.TransactionHistory[testAccount.TransactionHistory.Count - 1];
            Assert.That(interestTransaction.Description, Does.Contain("Interest").IgnoreCase,
                "Transaction should indicate interest application");

            // Success message
            Assert.Pass($"T1.21 [AddInterest]: {days} day at {interestRate} rate -> Balance: {testAccount.Balance}M (Interest: {actualInterest}M)");
        }

        /// <summary>
        /// T1.22 - IN1 (Inside Boundary): Test AddInterest for 30 days
        /// Boundary value testing: Inside valid range (typical month)
        /// Account: 1000M balance, 0.02 (2%) interest rate
        /// Calculation: 1000 + (1000 × 0.02 × 30/365) ≈ 1001.64M
        /// Formula: Interest = 1000 × 0.02 × (30/365) ≈ 1.64M
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN1: AddInterest for 30 days (typical month) - inside boundary")]
        public void T1_22_AddInterest_IN1_ThirtyDays_ShouldCalculateCorrectly()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);

            decimal initialBalance = 1000M;
            decimal interestRate = 0.02M; // 2% annual
            int days = 30; // Thirty days

            decimal interestAmount = initialBalance * interestRate * (days / 365M);
            interestAmount = Math.Round(interestAmount, 2, MidpointRounding.AwayFromZero);
            decimal expectedBalance = initialBalance + interestAmount; // 1001.64M

            testAccount = new BankAccount(initialBalance, stubConverter, interestRate, Currency.RON);

            // Verify initial state
            Assert.That(testAccount.Balance, Is.EqualTo(initialBalance), "Initial balance should be 1000M");
            Assert.That(testAccount.InterestRate, Is.EqualTo(interestRate), "Interest rate should be 0.02");

            // Act
            testAccount.ApplyInterest(days);

            // Assert - Verify balance increased by interest
            Assert.That(testAccount.Balance, Is.EqualTo(expectedBalance), $"Balance should be {expectedBalance}M after applying interest for {days} day(s)");

            // Assert - Verify interest amount is correct
            decimal expectedInterest = 1.64M; // 1000 × 0.02 × (30/365) rounded
            decimal actualInterest = testAccount.Balance - initialBalance;
            Assert.That(actualInterest, Is.EqualTo(expectedInterest).Within(0.01M), $"Interest amount should be approximately {expectedInterest}M");

            // Assert - Verify transaction recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.GreaterThan(0), "Transaction history should record interest");

            // Verify interest transaction
            var interestTransaction = testAccount.TransactionHistory[testAccount.TransactionHistory.Count - 1];
            Assert.That(interestTransaction.Description, Does.Contain("Interest").IgnoreCase, "Transaction should indicate interest application");

            // Success message
            Assert.Pass($"T1.22 [AddInterest]: {days} days at {interestRate} rate -> Balance: {testAccount.Balance}M (Interest: {actualInterest}M)");
        }

        /// <summary>
        /// T1.23 - IN2 (Inside Boundary): Test AddInterest for 365 days (full year)
        /// Boundary value testing: Inside valid range (full annual period)
        /// Account: 1000M balance, 0.02 (2%) interest rate
        /// Calculation: 1000 + (1000 × 0.02 × 365/365) = 1020M
        /// Formula: Interest = 1000 × 0.02 × (365/365) = 20M
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN2: AddInterest for 365 days (full year) - inside boundary")]
        public void T1_23_AddInterest_IN2_FullYear365Days_ShouldCalculateCorrectly()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);

            decimal initialBalance = 1000M;
            decimal interestRate = 0.02M; // 2% annual
            int days = 365; // Full year

            decimal interestAmount = initialBalance * interestRate * (days / 365M);
            interestAmount = Math.Round(interestAmount, 2, MidpointRounding.AwayFromZero);
            decimal expectedBalance = initialBalance + interestAmount; // 1020M

            testAccount = new BankAccount(initialBalance, stubConverter, interestRate, Currency.RON);

            // Verify initial state
            Assert.That(testAccount.Balance, Is.EqualTo(initialBalance), "Initial balance should be 1000M");
            Assert.That(testAccount.InterestRate, Is.EqualTo(interestRate), "Interest rate should be 0.02");

            // Act
            testAccount.ApplyInterest(days);

            // Assert - Verify balance increased by interest
            Assert.That(testAccount.Balance, Is.EqualTo(expectedBalance), $"Balance should be {expectedBalance}M after applying interest for {days} day(s)");

            // Assert - Verify interest amount is correct
            decimal expectedInterest = 20M; // 1000 × 0.02 × (365/365)
            decimal actualInterest = testAccount.Balance - initialBalance;
            Assert.That(actualInterest, Is.EqualTo(expectedInterest).Within(0.01M), $"Interest amount should be {expectedInterest}M");

            // Assert - Verify transaction recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.GreaterThan(0), "Transaction history should record interest");

            // Verify interest transaction
            var interestTransaction = testAccount.TransactionHistory[testAccount.TransactionHistory.Count - 1];
            Assert.That(interestTransaction.Description, Does.Contain("Interest").IgnoreCase, "Transaction should indicate interest application");

            // Success message
            Assert.Pass($"T1.23 [AddInterest]: {days} days (full year) at {interestRate} rate -> Balance: {testAccount.Balance}M (Interest: {actualInterest}M)");
        }

        /// <summary>
        /// T1.24 - OFF1 (Outside Boundary): Test AddInterest with zero days
        /// Boundary value testing: At boundary but invalid (no time passed)
        /// Expected: Should throw ArgumentException (days must be positive)
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF1: AddInterest with zero days - outside boundary (invalid)")]
        public void T1_24_AddInterest_OFF1_ZeroDays_ShouldThrowArgumentException()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);

            decimal initialBalance = 1000M;
            decimal interestRate = 0.02M;
            int days = 0; // Zero days (invalid)

            testAccount = new BankAccount(initialBalance, stubConverter, interestRate, Currency.RON);

            // Verify initial state
            Assert.That(testAccount.Balance, Is.EqualTo(initialBalance),
                "Initial balance should be 1000M");

            // Act & Assert - ApplyInterest with zero days should throw ArgumentException
            var exception = Assert.Throws<ArgumentException>(() =>
                testAccount.ApplyInterest(days), "ApplyInterest with zero days should throw ArgumentException");

            // Assert - Verify exception message contains helpful info
            Assert.That(exception.Message, Does.Contain("positive").IgnoreCase
                .And.Contain("days").IgnoreCase, "Exception message should mention days must be positive");

            // Assert - Verify balance unchanged
            Assert.That(testAccount.Balance, Is.EqualTo(initialBalance), "Balance should remain unchanged when ApplyInterest throws exception");

            // Assert - Verify no transaction recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(0), "No interest transaction should be recorded on failure");

            // Success message
            Assert.Pass($"T1.24 [AddInterest]: Zero days correctly threw ArgumentException");
        }

        /// <summary>
        /// T1.25 - OFF2 (Outside Boundary): Test AddInterest with negative days
        /// Boundary value testing: Outside valid boundary (negative time)
        /// Expected: Should throw ArgumentException (days must be positive)
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF2: AddInterest with negative days (-10) - outside boundary (invalid)")]
        public void T1_25_AddInterest_OFF2_NegativeDays_ShouldThrowArgumentException()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);

            decimal initialBalance = 1000M;
            decimal interestRate = 0.02M;
            int days = -10; // Negative days (invalid)

            testAccount = new BankAccount(initialBalance, stubConverter, interestRate, Currency.RON);

            // Verify initial state
            Assert.That(testAccount.Balance, Is.EqualTo(initialBalance), "Initial balance should be 1000M");

            // Act & Assert - ApplyInterest with negative days should throw ArgumentException
            var exception = Assert.Throws<ArgumentException>(() =>
                testAccount.ApplyInterest(days), "ApplyInterest with negative days should throw ArgumentException");

            // Assert - Verify exception message
            Assert.That(exception.Message, Does.Contain("positive").IgnoreCase
                .And.Contain("days").IgnoreCase, "Exception message should mention days must be positive");

            // Assert - Verify balance unchanged
            Assert.That(testAccount.Balance, Is.EqualTo(initialBalance), "Balance should remain unchanged when ApplyInterest throws exception");

            // Assert - Verify no transaction recorded
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(0), "No interest transaction should be recorded on failure");

            // Success message
            Assert.Pass($"T1.25 [AddInterest]: Negative days correctly threw ArgumentException");
        }

        /// <summary>
        /// T1.26 - ON (On Boundary): Test ConvertRonToEur with smallest valid amount (0.01 RON)
        /// Boundary value testing: Testing at exact minimum valid boundary
        /// Conversion rate: 1 EUR = 5 RON, so 0.01 RON = 0.002 EUR
        /// </summary>
        [Test]
        [Category("Domain-ON")]
        [Description("ON: ConvertRonToEur smallest valid amount (0.01 RON) - boundary value")]
        public void T1_26_ConvertRonToEur_ON_SmallestValidAmount_ShouldSucceed()
        {
            // Arrange
            BankAccount ronAccount;
            CurrencyConverterStub stubConverter = new CurrencyConverterStub(5M);


            ronAccount = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);
            decimal amountRon = 0.01M;
            decimal expectedEur = amountRon / 5.0M; // 0.002 EUR

            // Act
            decimal result = ronAccount.ConvertRonToEur(amountRon);

            // Assert - Verify conversion result
            Assert.That(result, Is.EqualTo(expectedEur),
                $"Converting {amountRon}M RON should return {expectedEur}M EUR");
            Assert.That(result, Is.GreaterThan(0M),
                "Conversion result should be positive");
            Assert.That(result, Is.LessThan(amountRon),
                "EUR amount should be less than RON amount (due to 5:1 ratio)");

            // Success message
            Assert.Pass($"T1.26 [ConvertRonToEur]: {amountRon}M RON -> {result}M EUR at minimum boundary");
        }

        /// <summary>
        /// T1.27 - IN1 (Inside Boundary): Test ConvertRonToEur with small amount (100 RON)
        /// Boundary value testing: Inside valid range
        /// Conversion: 100 RON ÷ 5 = 20 EUR
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN1: ConvertRonToEur small amount (100 RON) - inside boundary")]
        public void T1_27_ConvertRonToEur_IN1_SmallAmount_ShouldSucceed()
        {
            // Arrange
            BankAccount ronAccount;
            CurrencyConverterStub stubConverter = new CurrencyConverterStub(5M);

            ronAccount = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);
            decimal amountRon = 100M;
            decimal expectedEur = 20M; // 100 ÷ 5

            // Act
            decimal result = ronAccount.ConvertRonToEur(amountRon);

            // Assert
            Assert.That(result, Is.EqualTo(expectedEur),
                $"Converting {amountRon}M RON should return {expectedEur}M EUR");
            Assert.That(result, Is.GreaterThan(0M),
                "Result should be positive");

            // Success message
            Assert.Pass($"T1.27 [ConvertRonToEur]: {amountRon}M RON -> {result}M EUR");
        }

        /// <summary>
        /// T1.28 - IN2 (Inside Boundary): Test ConvertRonToEur with large amount (1000 RON)
        /// Boundary value testing: Inside valid range with larger value
        /// Conversion: 1000 RON ÷ 5 = 200 EUR
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN2: ConvertRonToEur large amount (1000 RON) - inside boundary")]
        public void T1_28_ConvertRonToEur_IN2_LargeAmount_ShouldSucceed()
        {
            // Arrange
            BankAccount ronAccount;
            CurrencyConverterStub stubConverter = new CurrencyConverterStub(5M);

            ronAccount = new BankAccount(10000M, stubConverter, 0.02M, Currency.RON);
            decimal amountRon = 1000M;
            decimal expectedEur = 200M; // 1000 ÷ 5

            // Act
            decimal result = ronAccount.ConvertRonToEur(amountRon);

            // Assert
            Assert.That(result, Is.EqualTo(expectedEur),
                $"Converting {amountRon}M RON should return {expectedEur}M EUR");

            // Success message
            Assert.Pass($"T1.28 [ConvertRonToEur]: {amountRon}M RON -> {result}M EUR");
        }

        /// <summary>
        /// T1.29 - OFF1 (Outside Boundary): Test ConvertRonToEur with zero amount
        /// Boundary value testing: Below minimum valid boundary
        /// Expected: Should throw ArgumentException
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF1: ConvertRonToEur zero amount (0 RON) - outside boundary")]
        public void T1_29_ConvertRonToEur_OFF1_ZeroAmount_ShouldThrowArgumentException()
        {
            // Arrange
            BankAccount ronAccount;
            CurrencyConverterStub stubConverter = new CurrencyConverterStub(5M);

            ronAccount = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);
            decimal amountRon = 0M;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                ronAccount.ConvertRonToEur(amountRon),
                "ConvertRonToEur with zero amount should throw ArgumentException");

            // Assert - Verify exception message
            Assert.That(exception.Message, Does.Contain("positive").IgnoreCase,
                "Exception message should mention amount must be positive");

            // Success message
            Assert.Pass($"T1.29 [ConvertRonToEur]: Zero amount correctly threw ArgumentException");
        }

        /// <summary>
        /// T1.30 - OFF2 (Outside Boundary): Test ConvertRonToEur with negative amount
        /// Boundary value testing: Outside valid boundary (negative)
        /// Expected: Should throw ArgumentException
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF2: ConvertRonToEur negative amount (-100 RON) - outside boundary")]
        public void T1_30_ConvertRonToEur_OFF2_NegativeAmount_ShouldThrowArgumentException()
        {
            // Arrange
            BankAccount ronAccount;
            CurrencyConverterStub stubConverter = new CurrencyConverterStub(5M);

            ronAccount = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);
            decimal amountRon = -100M;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                ronAccount.ConvertRonToEur(amountRon),
                "ConvertRonToEur with negative amount should throw ArgumentException");

            // Assert - Verify exception message
            Assert.That(exception.Message, Does.Contain("positive").IgnoreCase,
                "Exception message should mention amount must be positive");

            // Success message
            Assert.Pass($"T1.30 [ConvertRonToEur]: Negative amount correctly threw ArgumentException");
        }

        /// <summary>
        /// T1.31 - ON (On Boundary): Test ConvertEurToRon with smallest valid amount (0.01 EUR)
        /// Boundary value testing: Testing at exact minimum valid boundary
        /// Conversion rate: 1 EUR = 5 RON, so 0.01 EUR = 0.05 RON
        /// </summary>
        [Test]
        [Category("Domain-ON")]
        [Description("ON: ConvertEurToRon smallest valid amount (0.01 EUR) - boundary value")]
        public void T1_31_ConvertEurToRon_ON_SmallestValidAmount_ShouldSucceed()
        {
            // Arrange
            BankAccount eurAccount;
            CurrencyConverterStub stubConverter = new CurrencyConverterStub(5M);

            eurAccount = new BankAccount(200M, stubConverter, 0.02M, Currency.EUR);
            decimal amountEur = 0.01M;
            decimal expectedRon = amountEur * 5.0M; // 0.05 RON

            // Act
            decimal result = eurAccount.ConvertEurToRon(amountEur);

            // Assert - Verify conversion result
            Assert.That(result, Is.EqualTo(expectedRon),
                $"Converting {amountEur}M EUR should return {expectedRon}M RON");
            Assert.That(result, Is.GreaterThan(0M),
                "Conversion result should be positive");
            Assert.That(result, Is.GreaterThan(amountEur),
                "RON amount should be greater than EUR amount (due to 5:1 ratio)");

            // Success message
            Assert.Pass($"T1.31 [ConvertEurToRon]: {amountEur}M EUR -> {result}M RON at minimum boundary");
        }

        /// <summary>
        /// T1.32 - IN1 (Inside Boundary): Test ConvertEurToRon with small amount (20 EUR)
        /// Boundary value testing: Inside valid range
        /// Conversion: 20 EUR × 5 = 100 RON
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN1: ConvertEurToRon small amount (20 EUR) - inside boundary")]
        public void T1_32_ConvertEurToRon_IN1_SmallAmount_ShouldSucceed()
        {
            // Arrange
            BankAccount eurAccount;
            CurrencyConverterStub stubConverter = new CurrencyConverterStub(5M);

            eurAccount = new BankAccount(200M, stubConverter, 0.02M, Currency.EUR);
            decimal amountEur = 20M;
            decimal expectedRon = 100M; // 20 × 5

            // Act
            decimal result = eurAccount.ConvertEurToRon(amountEur);

            // Assert
            Assert.That(result, Is.EqualTo(expectedRon),
                $"Converting {amountEur}M EUR should return {expectedRon}M RON");
            Assert.That(result, Is.GreaterThan(0M),
                "Result should be positive");

            // Success message
            Assert.Pass($"T1.32 [ConvertEurToRon]: {amountEur}M EUR -> {result}M RON");
        }

        /// <summary>
        /// T1.33 - IN2 (Inside Boundary): Test ConvertEurToRon with large amount (200 EUR)
        /// Boundary value testing: Inside valid range with larger value
        /// Conversion: 200 EUR × 5 = 1000 RON
        /// </summary>
        [Test]
        [Category("Domain-IN")]
        [Description("IN2: ConvertEurToRon large amount (200 EUR) - inside boundary")]
        public void T1_33_ConvertEurToRon_IN2_LargeAmount_ShouldSucceed()
        {
            // Arrange
            BankAccount eurAccount;
            CurrencyConverterStub stubConverter = new CurrencyConverterStub(5M);

            eurAccount = new BankAccount(500M, stubConverter, 0.02M, Currency.EUR);
            decimal amountEur = 200M;
            decimal expectedRon = 1000M; // 200 × 5

            // Act
            decimal result = eurAccount.ConvertEurToRon(amountEur);

            // Assert
            Assert.That(result, Is.EqualTo(expectedRon),
                $"Converting {amountEur}M EUR should return {expectedRon}M RON");

            // Success message
            Assert.Pass($"T1.33 [ConvertEurToRon]: {amountEur}M EUR -> {result}M RON");
        }

        /// <summary>
        /// T1.34 - OFF1 (Outside Boundary): Test ConvertEurToRon with zero amount
        /// Boundary value testing: Below minimum valid boundary
        /// Expected: Should throw ArgumentException
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF1: ConvertEurToRon zero amount (0 EUR) - outside boundary")]
        public void T1_34_ConvertEurToRon_OFF1_ZeroAmount_ShouldThrowArgumentException()
        {
            // Arrange
            BankAccount eurAccount;
            CurrencyConverterStub stubConverter = new CurrencyConverterStub(5M);

            eurAccount = new BankAccount(200M, stubConverter, 0.02M, Currency.EUR);
            decimal amountEur = 0M;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                eurAccount.ConvertEurToRon(amountEur),
                "ConvertEurToRon with zero amount should throw ArgumentException");

            // Assert - Verify exception message
            Assert.That(exception.Message, Does.Contain("positive").IgnoreCase,
                "Exception message should mention amount must be positive");

            // Success message
            Assert.Pass($"T1.34 [ConvertEurToRon]: Zero amount correctly threw ArgumentException");
        }

        /// <summary>
        /// T1.35 - OFF2 (Outside Boundary): Test ConvertEurToRon with negative amount
        /// Boundary value testing: Outside valid boundary (negative)
        /// Expected: Should throw ArgumentException
        /// </summary>
        [Test]
        [Category("Domain-OFF")]
        [Description("OFF2: ConvertEurToRon negative amount (-20 EUR) - outside boundary")]
        public void T1_35_ConvertEurToRon_OFF2_NegativeAmount_ShouldThrowArgumentException()
        {
            // Arrange
            BankAccount eurAccount;
            CurrencyConverterStub stubConverter = new CurrencyConverterStub(5M);

            eurAccount = new BankAccount(200M, stubConverter, 0.02M, Currency.EUR);
            decimal amountEur = -20M;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                eurAccount.ConvertEurToRon(amountEur),
                "ConvertEurToRon with negative amount should throw ArgumentException");

            // Assert - Verify exception message
            Assert.That(exception.Message, Does.Contain("positive").IgnoreCase,
                "Exception message should mention amount must be positive");

            // Success message
            Assert.Pass($"T1.35 [ConvertEurToRon]: Negative amount correctly threw ArgumentException");
        }
    }
}
