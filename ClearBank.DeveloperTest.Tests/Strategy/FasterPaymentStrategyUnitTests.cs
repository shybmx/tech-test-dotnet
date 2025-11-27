using ClearBank.DeveloperTest.Strategy;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Strategy
{
    public class FasterPaymentStrategyUnitTests
    {
        private FasterPaymentStrategy _fasterPaymentStrategy;
        private Account _account;
        private MakePaymentRequest _makePaymentRequest;

        [SetUp]
        public void SetUp()
        {
            _fasterPaymentStrategy = new FasterPaymentStrategy();
            _makePaymentRequest = new MakePaymentRequest();
            _account = new Account();

            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
        }

        [TestCase(AllowedPaymentSchemes.Bacs, false)]
        [TestCase(AllowedPaymentSchemes.Chaps, false)]
        [TestCase(AllowedPaymentSchemes.FasterPayments, true)]
        public void DoesPaymentConditionMatch_Should_Return_Expected_Value_Given_Payment_Type(AllowedPaymentSchemes allowedPaymentSchemes, bool expected)
        {
            _account.AllowedPaymentSchemes = allowedPaymentSchemes;

            var result = _fasterPaymentStrategy.DoesPaymentConditionMatch(_makePaymentRequest, _account);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void DoesPaymentConditionMatch_Should_Return_False_When_Insufficient_Balance()
        {
            _account.Balance = 100m;
            _makePaymentRequest.Amount = 200m;

            var result = _fasterPaymentStrategy.DoesPaymentConditionMatch(_makePaymentRequest, _account);
            
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void DoesPaymentConditionMatch_Should_Return_True_When_Balance_And_Amount_Is_The_Same()
        {
            _account.Balance = 100m;
            _makePaymentRequest.Amount = 100m;

            var result = _fasterPaymentStrategy.DoesPaymentConditionMatch(_makePaymentRequest, _account);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void DoesPaymentConditionMatch_Should_Return_True_When_Sufficient_Balance()
        {            
            _account.Balance = 300m;
            _makePaymentRequest.Amount = 200m;

            var result = _fasterPaymentStrategy.DoesPaymentConditionMatch(_makePaymentRequest, _account);
            
            Assert.That(result, Is.EqualTo(true));
        }
    }
}
