using ClearBank.DeveloperTest.Factory;
using ClearBank.DeveloperTest.Strategy.Interface;
using ClearBank.DeveloperTest.Types;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace ClearBank.DeveloperTest.Tests.Factory
{
    public class PaymentFactoryUnitTests
    {
        private Mock<IEnumerable<IPaymentStrategy>> _paymentStrategiesMock;
        private PaymentFactory _paymentFactory;

        [SetUp]
        public void Setup()
        {
            _paymentStrategiesMock = new Mock<IEnumerable<IPaymentStrategy>>();
            _paymentFactory = new PaymentFactory(_paymentStrategiesMock.Object);
        }

        [Test]
        public void GetStrategy_Returns_Null_When_No_Strategy_Found()
        {  
            var paymentScheme = PaymentScheme.Bacs;

            _paymentStrategiesMock.Setup(paymentStrategies => paymentStrategies.GetEnumerator()).Returns(new List<IPaymentStrategy>
            {
                new Mock<IPaymentStrategy>().Object
            }.GetEnumerator());
            
            var result = _paymentFactory.GetStrategy(paymentScheme);
                       
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void GetStrategy_Returns_Expected_Strategy_When_Found()
        {
            var paymentScheme = PaymentScheme.Bacs;

            var expectedStrategyMock = new Mock<IPaymentStrategy>();
            
            expectedStrategyMock.Setup(paymentScheme => paymentScheme.paymentScheme).Returns(paymentScheme);
            
            _paymentStrategiesMock.Setup(paymentStrategies => paymentStrategies.GetEnumerator()).Returns(new List<IPaymentStrategy>
            {
                expectedStrategyMock.Object
            }.GetEnumerator());
            
            var result = _paymentFactory.GetStrategy(paymentScheme);
            
            Assert.That(result, Is.EqualTo(expectedStrategyMock.Object));
        }
    }
}
