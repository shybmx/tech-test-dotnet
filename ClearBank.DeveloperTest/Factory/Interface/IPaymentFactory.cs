using ClearBank.DeveloperTest.Strategy.Interface;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Factory.Interface
{
    public interface IPaymentFactory
    {
        IPaymentStrategy GetStrategy(PaymentScheme paymentScheme);
    }
}
