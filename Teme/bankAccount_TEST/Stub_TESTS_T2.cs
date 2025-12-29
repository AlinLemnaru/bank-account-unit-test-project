using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//--------------------------------------------------------------
//-----------------------Stub TESTS T2--------------------------
//--------------------------------------------------------------

namespace bankAccount
{
    [TestFixture]
    [Category("Stub")]
    [Category("Stub tests - Currency converter(Tema_2)")]
    public class Stub_TESTS_T2
    {
        //------------------Variables-------------------

        private BankAccount? ronAccount;
        private BankAccount? eurAccount;
        private CurrencyConverterStub? stubConverter;

        //-------------Setup and teardown---------------

        [SetUp]
        public void Setup_Preconditions()
        {
            // Clear accounts before each test
            ronAccount = null;
            eurAccount = null;
            stubConverter = null;
        }

        [TearDown]
        public void Teardown_Postconditions()
        {
            // Clear accounts before each test
            ronAccount = null;
            eurAccount = null;
            stubConverter = null;
        }

        //-------------------Tests----------------------

        /// <summary>
        /// T2.1 - STUB: ConvertRonToEur with stub rate 5M
        /// Create account with Stub(5M), ConvertRonToEur(100M)
        /// Expected: Returns 100M / 5M = 20M EUR
        /// </summary>
        [Test]
        [Category("Stub")]
        [Description("ConvertRonToEur with stub rate 5M should calculate correctly")]
        public void T2_1_ConvertRonToEur_WithStub_Rate5_ShouldCalculateCorrectly()
        {
            // Arrange
            stubConverter = new CurrencyConverterStub(5M);
            ronAccount = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);

            decimal amountRon = 100M;
            decimal stubRate = 5M;
            decimal expectedEur = amountRon / stubRate; // 100 / 5 = 20 EUR

            // Act
            decimal result = ronAccount.ConvertRonToEur(amountRon);

            // Assert
            Assert.That(result, Is.EqualTo(expectedEur), $"Converting {amountRon}M RON with stub rate {stubRate} should return {expectedEur}M EUR");
            Assert.That(stubConverter.GetEurToRonRate(), Is.EqualTo(stubRate), "Stub should return configured rate");

            // Success message
            Assert.Pass($"T2.1: {amountRon}M RON / {stubRate}M = {result}M EUR");
        }

        /// <summary>
        /// T2.2 - STUB: ConvertRonToEur with stub rate 4.5M
        /// Create account with Stub(4.5M), ConvertRonToEur(90M)
        /// Expected: Returns 90M / 4.5M = 20M EUR
        /// </summary>
        [Test]
        [Category("Stub")]
        [Description("ConvertRonToEur with stub rate 4.5M should calculate correctly")]
        public void T2_2_ConvertRonToEur_WithStub_Rate4Point5_ShouldCalculateCorrectly()
        {
            // Arrange
            stubConverter = new CurrencyConverterStub(4.5M);
            ronAccount = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);

            decimal amountRon = 90M;
            decimal stubRate = 4.5M;
            decimal expectedEur = amountRon / stubRate; // 90 / 4.5 = 20 EUR

            // Act
            decimal result = ronAccount.ConvertRonToEur(amountRon);

            // Assert
            Assert.That(result, Is.EqualTo(expectedEur), $"Converting {amountRon}M RON with stub rate {stubRate} should return {expectedEur}M EUR");
            Assert.That(stubConverter.GetEurToRonRate(), Is.EqualTo(stubRate), "Stub should return configured rate");

            // Success message
            Assert.Pass($"T2.2: {amountRon}M RON / {stubRate}M = {result}M EUR");
        }

        /// <summary>
        /// T2.3 - STUB: ConvertRonToEur with realistic BNR rate 4.97M
        /// Create account with Stub(4.97M), ConvertRonToEur(497M)
        /// Expected: Returns 497M / 4.97M = 100M EUR
        /// </summary>
        [Test]
        [Category("Stub")]
        [Description("ConvertRonToEur with realistic BNR stub rate 4.97M")]
        public void T2_3_ConvertRonToEur_WithStub_Rate4Point97_RealisticBnrRate()
        {
            // Arrange
            stubConverter = new CurrencyConverterStub(4.97M);
            ronAccount = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);

            decimal amountRon = 497M;
            decimal stubRate = 4.97M;
            decimal expectedEur = amountRon / stubRate; // 497 / 4.97 = 100 EUR

            // Act
            decimal result = ronAccount.ConvertRonToEur(amountRon);

            // Assert
            Assert.That(result, Is.EqualTo(expectedEur), $"Converting {amountRon}M RON with stub rate {stubRate} should return {expectedEur}M EUR");

            // Success message
            Assert.Pass($"T2.3: {amountRon}M RON / {stubRate}M = {result}M EUR (realistic BNR rate)");
        }

        /// <summary>
        /// T2.4 - STUB: ConvertRonToEur parametrized test with different rates
        /// Test with [TestCase(5M, 100M, 20M), TestCase(4M, 100M, 25M)]
        /// Expected: Parametrized test validates conversion formula
        /// </summary>
        [Test]
        [Category("Stub")]
        [Description("ConvertRonToEur with different stub rates - parametrized")]
        [TestCase(5, 100, 20)]
        [TestCase(4, 100, 25)]
        public void T2_4_ConvertRonToEur_WithStub_DifferentRates_ShouldProduceDifferentResults(decimal rate, decimal amountRon, decimal expectedEur)
        {
            // Arrange
            stubConverter = new CurrencyConverterStub(rate);
            ronAccount = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);

            // Act
            decimal result = ronAccount.ConvertRonToEur(amountRon);

            // Assert
            Assert.That(result, Is.EqualTo(expectedEur), $"Converting {amountRon}M RON with stub rate {rate} should return {expectedEur}M EUR");

            // Success message
            Assert.Pass($"T2.4: {amountRon}M RON / {rate}M = {result}M EUR");
        }

        /// <summary>
        /// T2.5 - STUB: ConvertEurToRon with stub rate 5M
        /// Create account with Stub(5M), ConvertEurToRon(20M)
        /// Expected: Returns 20M × 5M = 100M RON
        /// </summary>
        [Test]
        [Category("Stub")]
        [Description("ConvertEurToRon with stub rate 5M should calculate correctly")]
        public void T2_5_ConvertEurToRon_WithStub_Rate5_ShouldCalculateCorrectly()
        {
            // Arrange
            stubConverter = new CurrencyConverterStub(5M);
            eurAccount = new BankAccount(200M, stubConverter, 0.02M, Currency.EUR);

            decimal amountEur = 20M;
            decimal stubRate = 5M;
            decimal expectedRon = amountEur * stubRate; // 20 × 5 = 100 RON

            // Act
            decimal result = eurAccount.ConvertEurToRon(amountEur);

            // Assert
            Assert.That(result, Is.EqualTo(expectedRon), $"Converting {amountEur}M EUR with stub rate {stubRate} should return {expectedRon}M RON");
            Assert.That(stubConverter.GetEurToRonRate(), Is.EqualTo(stubRate), "Stub should return configured rate");

            // Success message
            Assert.Pass($"T2.5: {amountEur}M EUR × {stubRate}M = {result}M RON");
        }

        /// <summary>
        /// T2.6 - STUB: ConvertEurToRon with stub rate 4.5M
        /// Create account with Stub(4.5M), ConvertEurToRon(20M)
        /// Expected: Returns 20M × 4.5M = 90M RON
        /// </summary>
        [Test]
        [Category("Stub")]
        [Description("ConvertEurToRon with stub rate 4.5M should calculate correctly")]
        public void T2_6_ConvertEurToRon_WithStub_Rate4Point5_ShouldCalculateCorrectly()
        {
            // Arrange
            stubConverter = new CurrencyConverterStub(4.5M);
            eurAccount = new BankAccount(200M, stubConverter, 0.02M, Currency.EUR);

            decimal amountEur = 20M;
            decimal stubRate = 4.5M;
            decimal expectedRon = amountEur * stubRate; // 20 × 4.5 = 90 RON

            // Act
            decimal result = eurAccount.ConvertEurToRon(amountEur);

            // Assert
            Assert.That(result, Is.EqualTo(expectedRon), $"Converting {amountEur}M EUR with stub rate {stubRate} should return {expectedRon}M RON");

            // Success message
            Assert.Pass($"T2.6: {amountEur}M EUR × {stubRate}M = {result}M RON");
        }

        /// <summary>
        /// T2.7 - STUB: ConvertEurToRon with realistic BNR rate 4.97M
        /// Create account with Stub(4.97M), ConvertEurToRon(100M)
        /// Expected: Returns 100M × 4.97M = 497M RON
        /// </summary>
        [Test]
        [Category("Stub")]
        [Description("ConvertEurToRon with realistic BNR stub rate 4.97M")]
        public void T2_7_ConvertEurToRon_WithStub_Rate4Point97_RealisticBnrRate()
        {
            // Arrange
            stubConverter = new CurrencyConverterStub(4.97M);
            eurAccount = new BankAccount(200M, stubConverter, 0.02M, Currency.EUR);

            decimal amountEur = 100M;
            decimal stubRate = 4.97M;
            decimal expectedRon = amountEur * stubRate; // 100 × 4.97 = 497 RON

            // Act
            decimal result = eurAccount.ConvertEurToRon(amountEur);

            // Assert
            Assert.That(result, Is.EqualTo(expectedRon), $"Converting {amountEur}M EUR with stub rate {stubRate} should return {expectedRon}M RON");

            // Success message
            Assert.Pass($"T2.7: {amountEur}M EUR × {stubRate}M = {result}M RON (realistic BNR rate)");
        }

        /// <summary>
        /// T2.8 - STUB: ConvertEurToRon parametrized test with different rates
        /// Test with [TestCase(5M, 20M, 100M), TestCase(4M, 25M, 100M)]
        /// Expected: Parametrized test validates conversion formula
        /// </summary>
        [Test]
        [Category("Stub")]
        [Description("ConvertEurToRon with different stub rates - parametrized")]
        [TestCase(5, 20, 100)]
        [TestCase(4, 25, 100)]
        public void T2_8_ConvertEurToRon_WithStub_DifferentRates_ShouldProduceDifferentResults(decimal rate, decimal amountEur, decimal expectedRon)
        {
            // Arrange
            stubConverter = new CurrencyConverterStub(rate);
            eurAccount = new BankAccount(200M, stubConverter, 0.02M, Currency.EUR);

            // Act
            decimal result = eurAccount.ConvertEurToRon(amountEur);

            // Assert
            Assert.That(result, Is.EqualTo(expectedRon), $"Converting {amountEur}M EUR with stub rate {rate} should return {expectedRon}M RON");

            // Success message
            Assert.Pass($"T2.8: {amountEur}M EUR × {rate}M = {result}M RON");
        }

        /// <summary>
        /// T2.9 - STUB: Transfer same currency with stub should NOT use converter
        /// Source(RON, 1000M).Transfer(Dest(RON, 0M), 100M) with Stub
        /// Expected: Source = 900M, Dest = 100M, no conversion
        /// </summary>
        [Test]
        [Category("Stub")]
        [Description("Transfer same currency (RON->RON) should not use converter")]
        public void T2_9_Transfer_SameCurrency_WithStub_ShouldNotUseConverter()
        {
            // Arrange
            stubConverter = new CurrencyConverterStub(5M);
            var sourceRon = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);
            var destRon = new BankAccount(0M, stubConverter, 0.02M, Currency.RON);

            decimal transferAmount = 100M;

            // Act
            sourceRon.TransferFunds(destRon, transferAmount);

            // Assert - No conversion, direct transfer
            Assert.That(sourceRon.Balance, Is.EqualTo(900M), "Source RON should have 900M after transferring 100M");
            Assert.That(destRon.Balance, Is.EqualTo(100M), "Dest RON should have 100M (no conversion)");

            // Success message
            Assert.Pass($"T2.9: Same currency transfer - Source: {sourceRon.Balance}M RON, Dest: {destRon.Balance}M RON (no conversion)");
        }

        /// <summary>
        /// T2.10 - STUB: Transfer different currency with stub rate 5M should convert correctly
        /// Source(RON, 1000M).Transfer(Dest(EUR, 0M), 100M) with Stub(5M)
        /// Expected: Source = 900M RON, Dest = 20M EUR (100/5)
        /// </summary>
        [Test]
        [Category("Stub")]
        [Description("Transfer RON->EUR with stub rate 5M should convert correctly")]
        public void T2_10_Transfer_DifferentCurrency_WithStub_Rate5_ShouldConvertCorrectly()
        {
            // Arrange
            stubConverter = new CurrencyConverterStub(5M);
            var sourceRon = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);
            var destEur = new BankAccount(0M, stubConverter, 0.02M, Currency.EUR);

            decimal transferRon = 100M;
            decimal stubRate = 5M;
            decimal expectedEur = transferRon / stubRate; // 100 / 5 = 20 EUR

            // Act
            sourceRon.TransferRonToEur(destEur, transferRon);

            // Assert
            Assert.That(sourceRon.Balance, Is.EqualTo(900M), "Source RON should have 900M after transfer");
            Assert.That(destEur.Balance, Is.EqualTo(expectedEur), $"Dest EUR should have {expectedEur}M after conversion (100 RON / 5)");

            // Success message
            Assert.Pass($"T2.10: Transfer RON->EUR - Source: {sourceRon.Balance}M RON, Dest: {destEur.Balance}M EUR (100/5)");
        }

        /// <summary>
        /// T2.11 - STUB: Transfer different currency with realistic rate 4.97M
        /// Source(RON, 498M).Transfer(Dest(EUR, 0M), 497M) with Stub(4.97M)
        /// Expected: Source = 1M RON, Dest = 100M EUR (497/4.97)
        /// </summary>
        [Test]
        [Category("Stub")]
        [Description("Transfer RON->EUR with realistic stub rate 4.97M")]
        public void T2_11_Transfer_DifferentCurrency_WithStub_Rate4Point97_ShouldConvertCorrectly()
        {
            // Arrange
            stubConverter = new CurrencyConverterStub(4.97M);
            var sourceRon = new BankAccount(498M, stubConverter, 0.02M, Currency.RON);
            var destEur = new BankAccount(0M, stubConverter, 0.02M, Currency.EUR);

            decimal transferRon = 497M;
            decimal stubRate = 4.97M;
            decimal expectedEur = transferRon / stubRate; // 497 / 4.97 = 100 EUR

            // Act
            sourceRon.TransferRonToEur(destEur, transferRon);

            // Assert
            Assert.That(sourceRon.Balance, Is.EqualTo(1M), "Source RON should have 0M after transferring all");
            Assert.That(destEur.Balance, Is.EqualTo(expectedEur), $"Dest EUR should have {expectedEur}M after conversion (497/4.97)");

            // Success message
            Assert.Pass($"T2.11: Transfer RON->EUR - Source: {sourceRon.Balance}M RON, Dest: {destEur.Balance}M EUR (497/4.97)");
        }

        /// <summary>
        /// T2.12 - STUB: Transfer EUR->RON with stub rate 5M should convert correctly
        /// Source(EUR, 100M).Transfer(Dest(RON, 0M), 20M) with Stub(5M)
        /// Expected: Source = 80M EUR, Dest = 100M RON (20×5)
        /// </summary>
        [Test]
        [Category("Stub")]
        [Description("Transfer EUR->RON with stub rate 5M should convert correctly")]
        public void T2_12_Transfer_EurToRon_WithStub_Rate5_ShouldConvertCorrectly()
        {
            // Arrange
            stubConverter = new CurrencyConverterStub(5M);
            var sourceEur = new BankAccount(100M, stubConverter, 0.02M, Currency.EUR);
            var destRon = new BankAccount(0M, stubConverter, 0.02M, Currency.RON);

            decimal transferEur = 20M;
            decimal stubRate = 5M;
            decimal expectedRon = transferEur * stubRate; // 20 × 5 = 100 RON

            // Act
            sourceEur.TransferEurToRon(destRon, transferEur);

            // Assert
            Assert.That(sourceEur.Balance, Is.EqualTo(80M), "Source EUR should have 80M after transferring 20M");
            Assert.That(destRon.Balance, Is.EqualTo(expectedRon), $"Dest RON should have {expectedRon}M after conversion (20×5)");

            // Success message
            Assert.Pass($"T2.12: Transfer EUR->RON - Source: {sourceEur.Balance}M EUR, Dest: {destRon.Balance}M RON (20×5)");
        }

        //optional negative tests

        /// <summary>
        /// T2.13 - STUB NEGATIVE: ConvertRonToEur with rate below valid range (2M < 4M)
        /// Stub rate 2M is below realistic range (normally 4-6)
        /// Expected: Should throw ArgumentException or return invalid result
        /// </summary>
        [Test]
        [Category("Stub-Negative")]
        [Description("ConvertRonToEur with stub rate below valid range (2M)")]
        [TestCase(3.99)]
        [TestCase(6.01)]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(7)]
        public void T2_13_ConvertRonToEur_WithStub_RateBelowRange_ShouldThrowException(decimal rateValue)
        {
            // Arrange
            stubConverter = new CurrencyConverterStub((decimal)rateValue); // Below 4-6 range
            ronAccount = new BankAccount(1000M, stubConverter, 0.02M, Currency.RON);

            decimal amountRon = 100M;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                ronAccount.ConvertRonToEur(amountRon), "ConvertRonToEur with rate below valid range should throw ArgumentException");

            // Assert - Verify exception message
            Assert.That(exception.Message, Does.Contain("rate").IgnoreCase, "Exception should mention invalid rate or range");

            // Success message
            Assert.Pass($"T2.13: Rate {rateValue} below range correctly threw ArgumentException");
        }
    }
}
