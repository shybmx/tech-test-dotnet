using ClearBank.DeveloperTest.Factory.Interface;
using ClearBank.DeveloperTest.Strategy.Interface;
using ClearBank.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;

namespace ClearBank.DeveloperTest.Factory
{
    public class PaymentFactory : IPaymentFactory
    {
        private readonly IEnumerable<IPaymentStrategy> _strategies;
        public PaymentFactory(IEnumerable<IPaymentStrategy> strategies)
        {
            _strategies = strategies;
        }

        public IPaymentStrategy GetStrategy(PaymentScheme paymentScheme)
        {
            return _strategies.SingleOrDefault(paymentSchemes => paymentSchemes.paymentScheme == paymentScheme);
        }
    }
}
