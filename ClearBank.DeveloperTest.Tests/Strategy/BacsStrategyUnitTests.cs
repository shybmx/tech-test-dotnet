using ClearBank.DeveloperTest.Strategy;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Strategy
{
    public class BacsStrategyUnitTests
    {
        private BacsStrategy _bacsStrategy;
        private Account _account;
        private MakePaymentRequest _makePaymentRequest;

        [SetUp]
        public void Setup()
        {
            _bacsStrategy = new BacsStrategy();
            _makePaymentRequest = new MakePaymentRequest();
            _account = new Account();
        }

        [TestCase(AllowedPaymentSchemes.Bacs, true)]
        [TestCase(AllowedPaymentSchemes.Chaps, false)]
        [TestCase(AllowedPaymentSchemes.FasterPayments, false)]
        public void DoesPaymentConditionMatch_Should_Return_Expected_Value_Given_Payment_Type(AllowedPaymentSchemes allowedPaymentSchemes, bool expected)
        {
            _account.AllowedPaymentSchemes = allowedPaymentSchemes;

            var result = _bacsStrategy.DoesPaymentConditionMatch(_makePaymentRequest, _account);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
