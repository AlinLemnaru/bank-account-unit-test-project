using NuGet.Frameworks;
using NUnit.Framework;
using System;
using System.Security.Principal;

//------------------------------------------------------------
//-------------Bank Account Tests - Preliminary---------------
//------------------------------------------------------------

namespace bankAccount
{
    [TestFixture]
    [Category("Preliminary")]
    [Description("Tests for BankAccount constructor initialization and basic setup")]
    public class Preliminary_TESTS
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
        /// P1: Verify that the default constructor initializes all fields correctly.
        /// </summary>
        [Test]
        [Category("Setup")]
        [Description("Verifies Balance=0, AccountID is GUID, InterestRate=0.02, Currency=RON, TransactionHistory is empty")]
        public void P1_Ctor_DefaultConstructor_ShouldInitializeWithZeroBalance()
        {
            // Arrange & Act
            testAccount = new BankAccount();

            // Assert - Verify Balance
            Assert.That(testAccount.Balance, Is.EqualTo(0M), "Balance should be initialized to 0");

            // Assert - Verify AccountID is a valid GUID
            Assert.That(testAccount.AccountID, Is.Not.Null, "AccountID should not be null");
            Assert.That(testAccount.AccountID, Is.Not.Empty, "AccountID should not be empty");
            Assert.DoesNotThrow(() => Guid.Parse(testAccount.AccountID), "AccountID should be a valid GUID");

            // Assert - Verify InterestRate
            Assert.That(testAccount.InterestRate, Is.EqualTo(0.02M), "InterestRate should be initialized to 0.02");

            // Assert - Verify Currency
            Assert.That(testAccount.AccountCurrency, Is.EqualTo(Currency.RON), "Currency should be initialized to RON");

            // Assert - Verify TransactionHistory is empty
            Assert.That(testAccount.TransactionHistory, Is.Not.Null, "TransactionHistory should not be null");
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(0), "TransactionHistory should be empty upon initialization");

            // Success message
            Assert.Pass("P1: Default constructor initialized all fields correctly");
        }

        /// <summary>
        /// Test P2: Verifies that constructor with initial balance initializes correctly
        /// </summary>
        [Test]
        [Category("Setup")]
        [Description("Verifies constructor with initial balance sets Balance correctly and other fields as default")]
        public void P2_Ctor_WithInitialBalance_ShouldInitializeCorrectly()
        {
            testAccount = new BankAccount(1000M);

            // Assert - Verify Balance
            Assert.That(testAccount.Balance, Is.EqualTo(1000M), "Balance should be initialized to 1000");

            // Assert - Verify AccountID is a valid GUID
            Assert.That(testAccount.AccountID, Is.Not.Null, "AccountID should not be null");
            Assert.That(testAccount.AccountID, Is.Not.Empty, "AccountID should not be empty");
            Assert.DoesNotThrow(() => Guid.Parse(testAccount.AccountID), "AccountID should be a valid GUID");

            // Assert - Verify InterestRate
            Assert.That(testAccount.InterestRate, Is.EqualTo(0.02M), "InterestRate should be initialized to 0.02");

            // Assert - Verify Currency
            Assert.That(testAccount.AccountCurrency, Is.EqualTo(Currency.RON), "Currency should be initialized to RON");

            // Assert - Verify TransactionHistory is empty
            Assert.That(testAccount.TransactionHistory, Is.Not.Null, "TransactionHistory should not be null");
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(0), "TransactionHistory should be empty upon initialization");

            // Assert - Verify MinBalance
            Assert.That(testAccount.MinBalance, Is.EqualTo(0.1M), "MinBalance should be initialized to 0.1");

            // Assert - Verify DailyWithdrawLimit
            Assert.That(testAccount.DailyWithdrawLimit, Is.EqualTo(5000M), "DailyWithdrawLimit should be initialized to 5000");

            // Success message
            Assert.Pass("P2: Constructor with initial balance initialized all fields correctly");
        }

        /// <summary>
        /// Test P3: Verifies that constructor with converter dependency injection works correctly
        /// </summary>
        [Test]
        [Category("Setup")]
        [Description("Verifies constructor with currency converter dependency injection initializes correctly")]
        public void P3_Ctor_WithConverter_ShouldInitializeWithInjectedDependency()
        {
            // Arrange & Act
            var stubConverter = new CurrencyConverterStub(5M);
            testAccount = new BankAccount(1000M, stubConverter);

            // Assert - Verify Balance
            Assert.That(testAccount.Balance, Is.EqualTo(1000M), "Balance should be initialized to 1000");

            // Assert - Verify AccountID is a valid GUID
            Assert.That(testAccount.AccountID, Is.Not.Null, "AccountID should not be null");
            Assert.That(testAccount.AccountID, Is.Not.Empty, "AccountID should not be empty");
            Assert.DoesNotThrow(() => Guid.Parse(testAccount.AccountID), "AccountID should be a valid GUID");

            // Assert - Verify InterestRate
            Assert.That(testAccount.InterestRate, Is.EqualTo(0.02M), "InterestRate should be initialized to 0.02");

            // Assert - Verify Currency
            Assert.That(testAccount.AccountCurrency, Is.EqualTo(Currency.RON), "Currency should be initialized to RON");

            // Assert - Verify TransactionHistory is empty
            Assert.That(testAccount.TransactionHistory, Is.Not.Null, "TransactionHistory should not be null");
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(0), "TransactionHistory should be empty upon initialization");

            // Assert - Verify MinBalance
            Assert.That(testAccount.MinBalance, Is.EqualTo(0.1M), "MinBalance should be initialized to 0.1");

            // Assert - Verify DailyWithdrawLimit
            Assert.That(testAccount.DailyWithdrawLimit, Is.EqualTo(5000M), "DailyWithdrawLimit should be initialized to 5000");

            // Assert - Verify converter was injected and works
            decimal ronToEurRate = testAccount.ConvertRonToEur(1M);
            Assert.That(ronToEurRate, Is.EqualTo(0.2M), "Converter should convert 1 RON to 0.2 EUR with stub rate of 5 RON/EUR");

            // Success message
            Assert.Pass("P3: Constructor with currency converter dependency injection initialized all fields correctly");
        }

        /// <summary>
        /// Test P4: Verifies that constructor with all parameters initializes correctly
        /// </summary>
        [Test]
        [Category("Setup")]
        [Description("Verifies constructor with all parameters sets all fields correctly")]
        public void P4_Ctor_WithAllParameters_ShouldInitializeAllFields()
        {
            // Arrange
            var stubConverter = new CurrencyConverterStub(5M);
            decimal initialBalance = 1000M;
            decimal customInterestRate = 0.03M;
            Currency accountCurrency = Currency.EUR;

            // Act
            testAccount = new BankAccount(initialBalance, stubConverter, customInterestRate, accountCurrency);

            // Assert - Verify Balance
            Assert.That(testAccount.Balance, Is.EqualTo(1000M), "Balance should be initialized to 1000");

            // Assert - Verify AccountID is a valid GUID
            Assert.That(testAccount.AccountID, Is.Not.Null, "AccountID should not be null");
            Assert.That(testAccount.AccountID, Is.Not.Empty, "AccountID should not be empty");
            Assert.DoesNotThrow(() => Guid.Parse(testAccount.AccountID), "AccountID should be a valid GUID");

            // Assert - Verify InterestRate
            Assert.That(testAccount.InterestRate, Is.EqualTo(0.03M), "InterestRate should be initialized to 0.03");

            // Assert - Verify Currency
            Assert.That(testAccount.AccountCurrency, Is.EqualTo(Currency.EUR), "Currency should be initialized to EUR");

            // Assert - Verify TransactionHistory is empty
            Assert.That(testAccount.TransactionHistory, Is.Not.Null, "TransactionHistory should not be null");
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(0), "TransactionHistory should be empty upon initialization");

            // Assert - Verify MinBalance
            Assert.That(testAccount.MinBalance, Is.EqualTo(0.1M), "MinBalance should be initialized to 0.1");

            // Assert - Verify DailyWithdrawLimit
            Assert.That(testAccount.DailyWithdrawLimit, Is.EqualTo(5000M), "DailyWithdrawLimit should be initialized to 5000");

            // Assert - Verify converter was injected and works
            decimal ronToEurRate = testAccount.ConvertEurToRon(1M);
            Assert.That(ronToEurRate, Is.EqualTo(5M), "Converter should convert 1 EUR to 5 RON with stub rate of 5 RON/EUR");

            // Success message
            Assert.Pass("P4: Constructor with all parameters initialized all fields correctly");
        }

        /// <summary>
        /// Test P5: Verifies that BNR API returns valid exchange rate
        /// NOTE: This test requires internet connection and calls the real BNR API
        /// </summary>
        [Test]
        [Timeout(5000)] // Add a timeout of 5s to prevent hanging if no internet
        [Category("API_Test")]
        [Category("Integration")]
        [Description("Verifies that BNR API returns a valid EUR to RON exchange rate greater than 0 - requires internet connection")]
        public void P5_BnrApi_ShouldReturnPositiveValue()
        {
            // Arrange
            var bnrConverter = new BnrCurrencyConverter(); // Real API converter

            // Act
            decimal rate = 0M;

            // Try to get the rate and catch any exceptions
            try
            {
                rate = bnrConverter.GetEurToRonRate();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to get EUR to RON rate from BNR API: {ex.Message}");
            }

            // Assert - Verify rate is greater than 0
            Assert.That(rate, Is.GreaterThan(0), "EUR to RON rate from BNR API should be greater than 0");

            // Assert - Verify logical range (e.g., between 4 and 6)
            Assert.That(rate, Is.InRange(4M, 6M), "EUR to RON rate from BNR API should be between 4 and 6");

            // Log
            Console.WriteLine($"EUR to RON rate from BNR API: {rate}");

            // Success message
            Assert.Pass($"P5: BNR API returned a valid EUR to RON exchange rate: {rate} RON per 1 EUR");
        }

        /// <summary>
        /// Test P6: Verifies that TransactionHistory is empty immediately after account creation
        /// </summary>
        [Test]
        [Category("Setup")]
        [Description("Verifies that TransactionHistory is empty immediately after account creation")]
        public void P6_TransactionHistory_AfterCreation_ShouldBeEmpty()
        {
            // Arrange & Act
            testAccount = new BankAccount();

            // Assert - Verify TransactionHistory exists but is empty
            Assert.That(testAccount.TransactionHistory, Is.Not.Null, "TransactionHistory should not be null");
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(0), "TransactionHistory should be empty (count == 0) immediately after account creation");
            Assert.That(testAccount.TransactionHistory, Is.Empty, "TransactionHistory should be empty immediately after account creation");

            // Assert - Verify TransactionHistory is of correct type (List)
            Assert.That(testAccount.TransactionHistory, Is.InstanceOf<List<TransactionDetails>>(), 
                "TransactionHistory should be of type List<Transaction>");

            // Test with constructor with initial balance
            testAccount = new BankAccount(1000M);

            Assert.That(testAccount.TransactionHistory, Is.Not.Null, "TransactionHistory should not be null");
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(0), "TransactionHistory should be empty (count == 0) immediately after account creation");
            Assert.That(testAccount.TransactionHistory, Is.Empty, "TransactionHistory should be empty immediately after account creation");

            // Test with constructor with converter
            var stubConverter = new CurrencyConverterStub(5M);
            testAccount = new BankAccount(1000M, stubConverter);

            Assert.That(testAccount.TransactionHistory, Is.Not.Null, "TransactionHistory should not be null");
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(0), "TransactionHistory should be empty (count == 0) immediately after account creation");
            Assert.That(testAccount.TransactionHistory, Is.Empty, "TransactionHistory should be empty immediately after account creation");

            // Test with constructor with all parameters
            testAccount = new BankAccount(1000M, stubConverter, 0.03M, Currency.EUR);

            Assert.That(testAccount.TransactionHistory, Is.Not.Null, "TransactionHistory should not be null");
            Assert.That(testAccount.TransactionHistory.Count, Is.EqualTo(0), "TransactionHistory should be empty (count == 0) immediately after account creation");
            Assert.That(testAccount.TransactionHistory, Is.Empty, "TransactionHistory should be empty immediately after account creation");

            // Success message
            Assert.Pass("P6: TransactionHistory is empty immediately after account creation for all constructors");
        }

        /// <summary>
        /// Test P7: Verifies that each account gets a unique AccountID (GUID collision check)
        /// </summary>
        [Test]
        [Category("Setup")]
        [Description("Verifies that each account gets a unique AccountID (GUID collision check)")]
        public void P7_AccountID_ShouldBeUniqueGuid()
        {
            // Arrange & Act - Create first account
            sourceAccount = new BankAccount(1000M);
            string firstAccountId = sourceAccount.AccountID;

            // Assert - Verify first AccountID is a valid GUID
            Assert.That(firstAccountId, Is.Not.Null, "AccountID should not be null");
            Assert.That(firstAccountId, Is.Not.Empty, "AccountID should not be empty");
            Assert.DoesNotThrow(() => Guid.Parse(firstAccountId), "AccountID should be a valid GUID");

            // Arrange & Act - Create second account
            destinationAccount = new BankAccount(500M);
            string secondAccountId = destinationAccount.AccountID;

            // Assert - Verify second AccountID is a valid GUID
            Assert.That(secondAccountId, Is.Not.Null, "AccountID should not be null");
            Assert.That(secondAccountId, Is.Not.Empty, "AccountID should not be empty");
            Assert.DoesNotThrow(() => Guid.Parse(secondAccountId), "AccountID should be a valid GUID");

            // Assert - Verify that both AccountIDs are unique
            Assert.That(firstAccountId, Is.Not.EqualTo(secondAccountId), "Each account should have a unique AccountID (GUID)");

            // Success message
            Assert.Pass("P7: Each account has a unique AccountID (GUID collision check passed)");
        }
    }
}