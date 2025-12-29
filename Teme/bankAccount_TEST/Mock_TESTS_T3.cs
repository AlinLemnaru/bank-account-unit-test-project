using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//--------------------------------------------------------------
//-----------------------Mock TESTS T2--------------------------
//--------------------------------------------------------------

namespace bankAccount
{
    [TestFixture]
    [Category("Mock")]
    [Category("Mock tests(Tema_3)")]
    public class Mock_TESTS_T3
    {
        //------------------Variables-------------------

        private BankAccount? testAccount;
        private Mock<ICurrencyConverter>? mockConverter;

        //-------------Setup and teardown---------------
        [SetUp]
        public void Setup_Preconditions()
        {
            testAccount = null;
            mockConverter = null;
        }

        [TearDown]
        public void Teardown_Postconditions()
        {
            testAccount = null;
            mockConverter = null;
        }

        //---------------------Tests----------------------

        /// <summary>
        /// T3.1 - MOCK: Verify ConvertRonToEur calls converter GetEurToRonRate exactly once
        /// Mocks ICurrencyConverter and verifies the method is called
        /// Expected: GetEurToRonRate() called exactly 1 time with correct parameters
        /// </summary>
        [Test]
        [Category("Mock")]
        [Description("T3.1: ConvertRonToEur calls GetEurToRonRate exactly once")]
        public void T3_1_ConvertRonToEur_ShouldCallGetEurToRonRate_ExactlyOnce()
        {
            // Arrange
            mockConverter = new Mock<ICurrencyConverter>();
            mockConverter.Setup(m => m.GetEurToRonRate()).Returns(5M); // Mock returns 5M

            testAccount = new BankAccount(1000M, mockConverter.Object, 0.02M, Currency.RON);
            decimal amountRon = 100M;
            decimal expectedEur = 20M; // 100 / 5

            // Act
            decimal result = testAccount.ConvertRonToEur(amountRon);

            // Assert - Verify conversion result
            Assert.That(result, Is.EqualTo(expectedEur), $"ConvertRonToEur should return {expectedEur}M EUR");

            // Assert - Verify GetEurToRonRate was called exactly once
            mockConverter.Verify(m => m.GetEurToRonRate(), Times.Once(), "GetEurToRonRate should be called exactly once");

            // Success message
            Assert.Pass($"T3.1: GetEurToRonRate() called exactly once, returned {mockConverter.Object.GetEurToRonRate()}M");
        }

        /// <summary>
        /// T3.2 - MOCK: ConvertEurToRon should call GetEurToRonRate exactly once
        /// Create Mock ICurrencyConverter, call ConvertEurToRon(20M)
        /// Expected: Verify GetEurToRonRate() called Times.Once()
        /// </summary>
        [Test]
        [Category("Mock")]
        [Description("T3.2: ConvertEurToRon should call GetEurToRonRate exactly once")]
        public void T3_2_ConvertEurToRon_ShouldCallGetEurToRonRate_ExactlyOnce()
        {
            // Arrange
            mockConverter = new Mock<ICurrencyConverter>();
            mockConverter.Setup(m => m.GetEurToRonRate()).Returns(5M);

            testAccount = new BankAccount(200M, mockConverter.Object, 0.02M, Currency.EUR);

            // Act
            decimal result = testAccount.ConvertEurToRon(20M);

            // Assert - Verify GetEurToRonRate was called exactly once
            mockConverter.Verify(m => m.GetEurToRonRate(), Times.Once(), "GetEurToRonRate should be called exactly once");

            // Assert - Verify correct conversion
            Assert.That(result, Is.EqualTo(100M), "20M EUR × 5 should equal 100M RON");

            // Success message
            Assert.Pass($"T3.2: GetEurToRonRate() called Times.Once()");
        }

        /// <summary>
        /// T3.3 - MOCK: Transfer same currency should NOT call CurrencyConverter
        /// Mock converter, Source(RON).Transfer(Dest(RON), 100M)
        /// Expected: Verify GetEurToRonRate() called Times.Never()
        /// </summary>
        [Test]
        [Category("Mock")]
        [Description("T3.3: Transfer same currency should NOT call CurrencyConverter")]
        public void T3_3_Transfer_SameCurrency_ShouldNOTCallCurrencyConverter()
        {
            // Arrange
            mockConverter = new Mock<ICurrencyConverter>();
            mockConverter.Setup(m => m.GetEurToRonRate()).Returns(5M);

            var sourceRon = new BankAccount(1000M, mockConverter.Object, 0.02M, Currency.RON);
            var destRon = new BankAccount(0M, mockConverter.Object, 0.02M, Currency.RON);

            // Act
            sourceRon.TransferMinFunds(destRon, 100M);

            // Assert - Verify GetEurToRonRate was NOT called
            mockConverter.Verify(m => m.GetEurToRonRate(), Times.Never(), "GetEurToRonRate should NOT be called for same-currency transfer");

            // Assert - Verify balances (no conversion)
            Assert.That(sourceRon.Balance, Is.EqualTo(900M));
            Assert.That(destRon.Balance, Is.EqualTo(100M));

            // Success message
            Assert.Pass($"T3.3: GetEurToRonRate() called Times.Never()");
        }

        /// <summary>
        /// T3.4 - MOCK: Transfer different currency should call GetEurToRonRate once
        /// Mock converter, Source(RON).Transfer(Dest(EUR), 100M)
        /// Expected: Verify GetEurToRonRate() called Times.Once()
        /// </summary>
        [Test]
        [Category("Mock")]
        [Description("T3.4: Transfer different currency should call GetEurToRonRate once")]
        public void T3_4_Transfer_DifferentCurrency_ShouldCallGetEurToRonRate_Once()
        {
            // Arrange
            mockConverter = new Mock<ICurrencyConverter>();
            mockConverter.Setup(m => m.GetEurToRonRate()).Returns(5M);

            var sourceRon = new BankAccount(1000M, mockConverter.Object, 0.02M, Currency.RON);
            var destEur = new BankAccount(0M, mockConverter.Object, 0.02M, Currency.EUR);

            // Act
            sourceRon.TransferRonToEur(destEur, 100M);

            // Assert - Verify GetEurToRonRate was called exactly once
            mockConverter.Verify(m => m.GetEurToRonRate(), Times.Once(), "GetEurToRonRate should be called exactly once for cross-currency transfer");

            // Assert - Verify balances with conversion
            Assert.That(sourceRon.Balance, Is.EqualTo(900M));
            Assert.That(destEur.Balance, Is.EqualTo(20M)); // 100/5

            // Success message
            Assert.Pass($"T3.4: GetEurToRonRate() called Times.Once()");
        }

        /// <summary>
        /// T3.5 - MOCK: Multiple transfers should call converter multiple times
        /// Mock converter, perform 3 transfers with currency conversion
        /// Expected: Verify GetEurToRonRate() called Times.Exactly(3)
        /// </summary>
        [Test]
        [Category("Mock")]
        [Description("T3.5: Multiple transfers should call converter multiple times")]
        public void T3_5_Transfer_MultipleTransfers_ShouldCallConverterMultipleTimes()
        {
            // Arrange
            mockConverter = new Mock<ICurrencyConverter>();
            mockConverter.Setup(m => m.GetEurToRonRate()).Returns(5M);

            var sourceRon = new BankAccount(1000M, mockConverter.Object, 0.02M, Currency.RON);
            var destEur = new BankAccount(0M, mockConverter.Object, 0.02M, Currency.EUR);

            // Act - Perform 3 transfers with currency conversion
            sourceRon.TransferRonToEur(destEur, 100M); // 1st call
            sourceRon.TransferRonToEur(destEur, 50M);  // 2nd call
            sourceRon.TransferRonToEur(destEur, 25M);  // 3rd call

            // Assert - Verify GetEurToRonRate was called exactly 3 times
            mockConverter.Verify(m => m.GetEurToRonRate(), Times.Exactly(3), "GetEurToRonRate should be called exactly 3 times (once per transfer)");

            // Assert - Verify final balances
            Assert.That(sourceRon.Balance, Is.EqualTo(825M)); // 1000 - 100 - 50 - 25
            Assert.That(destEur.Balance, Is.EqualTo(35M)); // (100+50+25)/5

            // Success message
            Assert.Pass($"T3.5: GetEurToRonRate() called Times.Exactly(3)");
        }

        /// <summary>
        /// T3.6 - MOCK: Deposit should NOT call CurrencyConverter (negative test)
        /// Mock converter, call Deposit(100M)
        /// Expected: Verify GetEurToRonRate() called Times.Never()
        /// </summary>
        [Test]
        [Category("Mock")]
        [Description("T3.6: Deposit should NOT call CurrencyConverter (negative test)")]
        public void T3_6_Deposit_ShouldNOTCallCurrencyConverter()
        {
            // Arrange
            mockConverter = new Mock<ICurrencyConverter>();
            mockConverter.Setup(m => m.GetEurToRonRate()).Returns(5M);

            testAccount = new BankAccount(1000M, mockConverter.Object, 0.02M, Currency.RON);

            // Act
            testAccount.Deposit(100M);

            // Assert - Verify GetEurToRonRate was NOT called
            mockConverter.Verify(m => m.GetEurToRonRate(), Times.Never(), "GetEurToRonRate should NOT be called during Deposit");

            // Assert - Verify balance increased
            Assert.That(testAccount.Balance, Is.EqualTo(1100M));

            // Success message
            Assert.Pass($"T3.6: GetEurToRonRate() called Times.Never() (negative test)");
        }

        /// <summary>
        /// T3.7 - MOCK: Withdraw should NOT call CurrencyConverter (negative test)
        /// Mock converter, call Withdraw(100M)
        /// Expected: Verify GetEurToRonRate() called Times.Never()
        /// </summary>
        [Test]
        [Category("Mock")]
        [Description("T3.7: Withdraw should NOT call CurrencyConverter (negative test)")]
        public void T3_7_Withdraw_ShouldNOTCallCurrencyConverter()
        {
            // Arrange
            mockConverter = new Mock<ICurrencyConverter>();
            mockConverter.Setup(m => m.GetEurToRonRate()).Returns(5M);

            testAccount = new BankAccount(1000M, mockConverter.Object, 0.02M, Currency.RON);

            // Act
            testAccount.Withdraw(100M);

            // Assert - Verify GetEurToRonRate was NOT called
            mockConverter.Verify(m => m.GetEurToRonRate(), Times.Never(), "GetEurToRonRate should NOT be called during Withdraw");

            // Assert - Verify balance decreased
            Assert.That(testAccount.Balance, Is.EqualTo(900M));

            // Success message
            Assert.Pass($"T3.7: GetEurToRonRate() called Times.Never() (negative test)");
        }

        /// <summary>
        /// T3.8 - MOCK: AddInterest should NOT call CurrencyConverter (negative test)
        /// Mock converter, call AddInterest(30)
        /// Expected: Verify GetEurToRonRate() called Times.Never()
        /// </summary>
        [Test]
        [Category("Mock")]
        [Description("T3.8: AddInterest should NOT call CurrencyConverter (negative test)")]
        public void T3_8_AddInterest_ShouldNOTCallCurrencyConverter()
        {
            // Arrange
            mockConverter = new Mock<ICurrencyConverter>();
            mockConverter.Setup(m => m.GetEurToRonRate()).Returns(5M);

            testAccount = new BankAccount(1000M, mockConverter.Object, 0.02M, Currency.RON);

            // Act
            testAccount.ApplyInterest(30); // 30 days

            // Assert - Verify GetEurToRonRate was NOT called
            mockConverter.Verify(m => m.GetEurToRonRate(), Times.Never(), "GetEurToRonRate should NOT be called during AddInterest");

            // Assert - Verify balance increased with interest
            Assert.That(testAccount.Balance, Is.GreaterThan(1000M), "Balance should increase after applying interest");

            // Success message
            Assert.Pass($"T3.8: GetEurToRonRate() called Times.Never() (negative test)");
        }

        /// <summary>
        /// T3.9 - MOCK: Multiple conversions should call converter for each
        /// Mock converter, call ConvertRonToEur(100M) twice
        /// Expected: Verify GetEurToRonRate() called Times.Exactly(2)
        /// </summary>
        [Test]
        [Category("Mock")]
        [Description("T3.9: Multiple conversions should call converter for each")]
        public void T3_9_ConvertRonToEur_MultipleConversions_ShouldCallConverterForEach()
        {
            // Arrange
            mockConverter = new Mock<ICurrencyConverter>();
            mockConverter.Setup(m => m.GetEurToRonRate()).Returns(5M);

            testAccount = new BankAccount(1000M, mockConverter.Object, 0.02M, Currency.RON);

            // Act - Call ConvertRonToEur twice
            decimal result1 = testAccount.ConvertRonToEur(100M); // 1st call
            decimal result2 = testAccount.ConvertRonToEur(100M); // 2nd call

            // Assert - Verify GetEurToRonRate was called exactly twice
            mockConverter.Verify(m => m.GetEurToRonRate(), Times.Exactly(2), "GetEurToRonRate should be called exactly twice (once per conversion)");

            // Assert - Verify both conversions are correct
            Assert.That(result1, Is.EqualTo(20M));
            Assert.That(result2, Is.EqualTo(20M));

            // Success message
            Assert.Pass($"T3.9: GetEurToRonRate() called Times.Exactly(2)");
        }

        /// <summary>
        /// T3.10 - MOCK: Transfer with VerifyAll should validate all setup calls
        /// Mock with MockBehavior.Strict, perform transfer
        /// Expected: VerifyAll() passes if all expected calls made
        /// </summary>
        [Test]
        [Category("Mock")]
        [Description("T3.10: Transfer with VerifyAll should validate all setup calls")]
        public void T3_10_Transfer_WithVerifyAll_ShouldValidateAllSetupCalls()
        {
            // Arrange - Use MockBehavior.Strict (all calls must be setup)
            mockConverter = new Mock<ICurrencyConverter>(MockBehavior.Strict);
            mockConverter.Setup(m => m.GetEurToRonRate()).Returns(5M);

            var sourceRon = new BankAccount(1000M, mockConverter.Object, 0.02M, Currency.RON);
            var destEur = new BankAccount(0M, mockConverter.Object, 0.02M, Currency.EUR);

            // Act
            sourceRon.TransferRonToEur(destEur, 100M);

            // Assert - VerifyAll() ensures all setup calls were made
            Assert.DoesNotThrow(() => mockConverter.VerifyAll(), "VerifyAll() should pass if all expected calls were made");

            // Assert - Verify balances
            Assert.That(sourceRon.Balance, Is.EqualTo(900M));
            Assert.That(destEur.Balance, Is.EqualTo(20M));

            // Success message
            Assert.Pass($"T3.10: VerifyAll() passed - all expected calls made");
        }

        /// <summary>
        /// T3.11 - MOCK NEGATIVE: ConvertRonToEur with invalid amounts should NOT call converter
        /// Parametrized test with 5 invalid amounts: zero, negative, very small negative, large negative, decimal.MinValue
        /// Expected: GetEurToRonRate() called Times.Never() for all cases
        /// </summary>
        [Test]
        [Category("Mock-Negative")]
        [Description("T3.11: ConvertRonToEur with invalid amounts should NOT call GetEurToRonRate")]
        [TestCase(0)]           // Zero amount
        [TestCase(-1)]          // Small negative
        [TestCase(-100)]        // Medium negative
        [TestCase(-999999)]     // Large negative
        [TestCase(-0.01)]       // Very small negative
        public void T3_11_ConvertRonToEur_InvalidAmounts_ShouldNOTCallConverter(double invalidAmount)
        {
            // Arrange
            mockConverter = new Mock<ICurrencyConverter>();
            mockConverter.Setup(m => m.GetEurToRonRate()).Returns(5M);

            testAccount = new BankAccount(1000M, mockConverter.Object, 0.02M, Currency.RON);
            decimal invalidAmountDecimal = (decimal)invalidAmount;

            // Act & Assert - Should throw ArgumentException for invalid amount
            var exception = Assert.Throws<ArgumentException>(() =>
                testAccount.ConvertRonToEur(invalidAmountDecimal),
                $"ConvertRonToEur with invalid amount {invalidAmountDecimal} should throw ArgumentException");

            // Assert - Verify GetEurToRonRate was NEVER called
            mockConverter.Verify(m => m.GetEurToRonRate(), Times.Never(), $"GetEurToRonRate should NOT be called for invalid amount {invalidAmountDecimal}");

            // Assert - Verify exception message mentions positive or amount
            Assert.That(exception.Message, Does.Contain("positive").IgnoreCase, "Exception message should mention amount must be positive");

            // Success message
            Assert.Pass($"T3.11: Invalid amount {invalidAmountDecimal} prevented converter call (Times.Never)");
        }
    }
}
