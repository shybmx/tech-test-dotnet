using ClearBank.DeveloperTest.Strategy;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Strategy
{
    public class ChapsStrategyUnitTests
    {
        private ChapsStrategy _chapsStrategy;
        private Account _account;
        private MakePaymentRequest _makePaymentRequest;

        [SetUp]
        public void SetUp()
        {
            _chapsStrategy = new ChapsStrategy();
            _makePaymentRequest = new MakePaymentRequest();
            _account = new Account();

            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
        }

        [TestCase(AllowedPaymentSchemes.Bacs, false)]
        [TestCase(AllowedPaymentSchemes.Chaps, true)]
        [TestCase(AllowedPaymentSchemes.FasterPayments, false)]
        public void DoesPaymentConditionMatch_Should_Return_Expected_Value_Given_Payment_Type(AllowedPaymentSchemes allowedPaymentSchemes, bool expected)
        {
            _account.AllowedPaymentSchemes = allowedPaymentSchemes;

            var result = _chapsStrategy.DoesPaymentConditionMatch(_makePaymentRequest, _account);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(AccountStatus.Live, true)]
        [TestCase(AccountStatus.InboundPaymentsOnly, false)]
        [TestCase(AccountStatus.Disabled, false)]
        public void DoesPaymentConditionMatch_Should_Return_Expected_Value_On_Account_Status(AccountStatus accountStatus, bool expected)
        {            
            _account.Status = accountStatus;

            var result = _chapsStrategy.DoesPaymentConditionMatch(_makePaymentRequest, _account);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
