using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Factory.Interface;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Strategy.Interface;
using ClearBank.DeveloperTest.Types;
using Moq;
using NUnit.Framework;
using System.Configuration;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceUnitTests
    {
        private Mock<IAccountDataStore> _accountDataStoreMock;
        private Mock<IAccountDataStore> _backupAccountDataStoreMock;
        private Mock<IPaymentFactory> _paymentFactoryMock;
        private Mock<IPaymentStrategy> _paymentStrategyMock;

        private PaymentService _paymentService;
        private MakePaymentRequest _makePaymentRequest;

        [SetUp]
        public void Setup()
        {
            ConfigurationManager.AppSettings["DataStoreType"] = "Notbackup";

            _accountDataStoreMock = new Mock<IAccountDataStore>();
            _backupAccountDataStoreMock = new Mock<IAccountDataStore>();
            _paymentFactoryMock = new Mock<IPaymentFactory>();
            _paymentStrategyMock = new Mock<IPaymentStrategy>();

            _accountDataStoreMock.Setup(account => account.GetAccount(It.IsAny<string>())).Returns(new Account());
            _paymentStrategyMock.Setup(paymentCondition => paymentCondition.DoesPaymentConditionMatch(It.IsAny<MakePaymentRequest>(), It.IsAny<Account>())).Returns(true);
            _paymentFactoryMock.Setup(strategy => strategy.GetStrategy(It.IsAny<PaymentScheme>())).Returns(_paymentStrategyMock.Object);

            _paymentService = new PaymentService(_accountDataStoreMock.Object, _backupAccountDataStoreMock.Object, _paymentFactoryMock.Object);

            _makePaymentRequest = new MakePaymentRequest();
        }

        [TestCase("Backup")]
        [TestCase("backup")]
        [TestCase("backUp")]
        [TestCase("BackUp")]
        public void MakePayment_Calls_Backup_Account(string dataStoreType)
        {
            ConfigurationManager.AppSettings["DataStoreType"] = dataStoreType;

            _backupAccountDataStoreMock.Setup(account => account.GetAccount(It.IsAny<string>())).Returns(new Account());

            var result = _paymentService.MakePayment(_makePaymentRequest);

            _accountDataStoreMock.Verify(account => account.GetAccount(It.IsAny<string>()), Times.Never);
            _accountDataStoreMock.Verify(account => account.UpdateAccount(It.IsAny<Account>()), Times.Never);

            _backupAccountDataStoreMock.Verify(account => account.GetAccount(It.IsAny<string>()), Times.Once);
            _backupAccountDataStoreMock.Verify(account => account.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [Test]
        public void MakePayment_Calls_Default_Account()
        {
            var result = _paymentService.MakePayment(_makePaymentRequest);

            _accountDataStoreMock.Verify(account => account.GetAccount(It.IsAny<string>()), Times.Once);
            _accountDataStoreMock.Verify(account => account.UpdateAccount(It.IsAny<Account>()), Times.Once);

            _backupAccountDataStoreMock.Verify(account => account.GetAccount(It.IsAny<string>()), Times.Never);
            _backupAccountDataStoreMock.Verify(account => account.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void MakePayment_Returns_False_When_Account_Not_Found()
        {
            _accountDataStoreMock.Setup(account => account.GetAccount(It.IsAny<string>())).Returns((Account)null);

            var result = _paymentService.MakePayment(_makePaymentRequest);

            Assert.That(result.Success, Is.EqualTo(false));
        }

        [Test]
        public void MakePayment_Returns_False_When_Strategy_Not_Found()
        {
            _paymentFactoryMock.Setup(strategy => strategy.GetStrategy(It.IsAny<PaymentScheme>())).Returns((IPaymentStrategy)null);

            var result = _paymentService.MakePayment(_makePaymentRequest);

            Assert.That(result.Success, Is.EqualTo(false));
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public void MakePayment_Returns_Correct_Value_When_Strategy_Conditions_Match(bool conditionMatch, bool expectedResult)
        { 
            _paymentStrategyMock.Setup(paymentCondition => paymentCondition.DoesPaymentConditionMatch(It.IsAny<MakePaymentRequest>(), It.IsAny<Account>())).Returns(conditionMatch);

            var result = _paymentService.MakePayment(_makePaymentRequest);

            Assert.That(result.Success, Is.EqualTo(expectedResult));
        }

        [Test]
        public void MakePayment_Updates_Account_Balance_When_Payment_Succeeds()
        {
            var initialBalance = 1000m;
            var paymentAmount = 200m;

            _makePaymentRequest.Amount = paymentAmount;

            var account = new Account
            {
                Balance = initialBalance
            };

            _accountDataStoreMock.Setup(account => account.GetAccount(It.IsAny<string>())).Returns(account);
            _paymentStrategyMock.Setup(paymentCondition => paymentCondition.DoesPaymentConditionMatch(It.IsAny<MakePaymentRequest>(), It.IsAny<Account>())).Returns(true);

            var result = _paymentService.MakePayment(_makePaymentRequest);

            Assert.That(result.Success, Is.EqualTo(true));
            Assert.That(account.Balance, Is.EqualTo(initialBalance - paymentAmount));

            _accountDataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [Test]
        public void MakePayment_Does_Not_Update_Account_Balance_When_Payment_Fails()
        {
            var initialBalance = 1000m;
            var paymentAmount = 200m;

            _makePaymentRequest.Amount = paymentAmount;

            var account = new Account
            {
                Balance = initialBalance
            };

            _accountDataStoreMock.Setup(account => account.GetAccount(It.IsAny<string>())).Returns(account);
            _paymentStrategyMock.Setup(paymentCondition => paymentCondition.DoesPaymentConditionMatch(It.IsAny<MakePaymentRequest>(), It.IsAny<Account>())).Returns(false);

            var result = _paymentService.MakePayment(_makePaymentRequest);

            Assert.That(result.Success, Is.EqualTo(false));
            Assert.That(account.Balance, Is.EqualTo(initialBalance));
            _accountDataStoreMock.Verify(account => account.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }
    }
}
